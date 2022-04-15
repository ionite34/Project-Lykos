using System.ComponentModel;
using System.Diagnostics;

namespace Project_Lykos.Master
{
    using static ElementLink;
    /// <summary>
    /// Clone from <see cref="Project_Lykos.MainWindow"/> but this queues records for distributed processing
    /// </summary>
    public partial class MainWindow : Form, IMainWindow
    {
        private readonly LykosController ct = new();
        private readonly State state;

        // Dictionary of buttons and states
        private readonly Dictionary<Control, bool> buttonStates = new();
        
        // Cancellation token for the background worker
        private readonly CancellationTokenSource cts1 = new();
        
        // Timer
        private System.Windows.Forms.Timer progressTimer = new();
        
        public bool Progress1Running { get; set; } = false;

        // Background Workers
        private readonly BackgroundWorker worker = new()
        {
            WorkerSupportsCancellation = false,
            WorkerReportsProgress = true,
        };
        
        // Tooltip
        private readonly ToolTip _labelTips = new();

        public MainWindow()
        {
            InitializeComponent();

            // Help text
            _labelTips.ToolTipIcon = ToolTipIcon.Info;
            _labelTips.IsBalloon = true;
            _labelTips.ShowAlways = true;

            // Set Default state for Combo Boxes
            combo_audio_preprocessing.SelectedIndex = 2;
            combo_csvDelimiter.SelectedIndex = 0;
            combo_outputActions.SelectedIndex = 0;
            
            // Initialize state with all buttons
            state = new State(this);
            state.Buttons.Add(button_browse_csv);
            state.Buttons.Add(button_browse_output);
            state.Buttons.Add(button_browse_source);
            state.Buttons.Add(button_preview);
            state.Buttons.Add(button_start_batch);
            state.Buttons.Add(button_stop_batch);
            
            progressTimer.Tick += Batch_ProgressChanged;
            progressTimer.Interval = 370;
        }
        
        private void MainWindow_Load(object sender, EventArgs e)
        {
            var result = Cache.KillProcesses();
            if (!result)
            {
                MessageBox.Show(@"Error in clearing temp directory.", @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        // Browse Source button
        private async void Button_browse_source_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = fbd.SelectedPath;
            // Process
            try
            {
                await Task.Run(() => ct.SetFilepath_Source(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error setting path", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateComboFormats();
                UpdateButtonState();
            }
        }

        // Browse Output button
        private async void Button_browse_output_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = fbd.SelectedPath;
            // Process
            try
            {
                await Task.Run(() => ct.SetFilepath_Output(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error setting path", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateComboFormats();
                UpdateButtonState();
            }
        }

        // Browse CSV button
        private async void Button_browse_csv_Click(object sender, EventArgs e)
        {
            // Create a new OpenFileDialog and show
            OpenFileDialog ofd = new()
            {
                Filter = @"CSV Files (*.csv)|*.csv",
                Title = @"Select the .csv file used for voice generation",
                CheckFileExists = true,
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            await ParseCSV(ofd.FileName).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts the index preview process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_preview_Click(object sender, EventArgs e)
        {
            await DoIndex();
        }
        
        /// <summary>
        /// Starts the batch process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void button_start_batch_Click(object sender, EventArgs e)
        {
            await StartBatch().ConfigureAwait(false);
        }
        

        private async Task StartBatch()
        {
            // First check if indexing is done, if not, call indexing
            // We do this by checking if any tasks are queued
            if (ct.CtProcessControl.Count == 0)
            {
                // If there are no tasks, we need to index first
                await DoIndex();
            }
            // Now we can start the batch
            try
            {
                await Task.Run(state.FreezeButtons);
                button_stop_batch.Enabled = true;
                // Set the progress bar
                progress_total.Style = ProgressBarStyle.Marquee;
                label_progress_status1A.Text = @"Starting batch processing...";
                label_progress_status1A.Visible = true;
                Progress1Running = true;
                
                progressTimer.Start();
                await ct.CtProcessControl.Start(1, 200, false, cts1);
            }
            catch (TaskCanceledException cancelException)
            {
                MessageBox.Show(cancelException.Message, @"Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error processing batch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressTimer.Stop();
                Cache.KillProcesses();
                ResetUIProgress();
                await Task.Run(state.RestoreButtons);
                // Messagebox with average times, if any were recorded
                if (Cache.ProcessingTimes.Count > 0)
                {
                    var avg = Cache.AverageProcessingTime;
                    var msg = $@"Average processing time: {avg:0.0} ms";
                    MessageBox.Show(msg, @"Batch processing stopped", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
            }
        }
        
        private async void button_stop_batch_Click(object sender, EventArgs e)
        {
            button_stop_batch.Enabled = false;
            Progress1Running = false;
            progress_total.Style = ProgressBarStyle.Marquee;
            label_progress_status1A.Text = @"Cancelling, please wait. Do not close this application.";
            label_progress_status1A.Visible = true;
            label_progress_value1A.Visible = false;
            cts1.Cancel();
        }
        
        // Parse CSV
        private async Task ParseCSV(string filename)
        {
            try
            {
                await Task.Run(() => state.FreezeButtons());
                combo_csv.Text = "";
                label_progress_status1A.Text = @"Checking file format...";
                label_progress_status1A.Visible = true;
                progress_total.Style = ProgressBarStyle.Marquee;
                progress_total.MarqueeAnimationSpeed = 5;
                await Task.Run(() => ct.SetFilepathCsvAsync(filename));
                var progress = LinkProgressBar2L(this, progress_total, label_progress_value1A, label_progress_status1A, @"Parsing CSV lines: ", false, true);
                Progress1Running = true;
                await Task.Run(() => ct.LoadCsvAsync(filename, progress));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error Parsing CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await Task.Run(() => state.RestoreButtons());
                ResetUIProgress();
                UpdateComboFormats();
                UpdateButtonState();
            }
        }
        
        // Method to set the progress bar to a percentage event
        private void Batch_ProgressChanged(object sender, EventArgs e)
        {
            if (!Progress1Running) return;
            if (ct.CtProcessControl.processedCount == 0) return;
            progress_total.BeginInvoke((MethodInvoker)delegate ()
            {
                int progress = ct.CtProcessControl.processedCount;
                int batchesDone = ct.CtProcessControl.CurrentBatch;
                label_progress_status1A.Text = @"Current batch progress: ";
                label_progress_value1A.Text = $@"{progress}/{ct.CtProcessControl.BatchSize}";
                label_progress_value1A.Visible = true;
                progress_total.Style = ProgressBarStyle.Continuous;
                progress_total.Value = progress;
                progress_total.Maximum = ct.CtProcessControl.BatchSize;
                // batch progress portion
                label_batch_status.Text = @"Total batches: ";
                label_batch_value.Text = $@"{batchesDone}/{ct.CtProcessControl.TotalBatches}";
                label_batch_value.Visible = true;
                label_batch_status.Visible = true;
                progress_batch.Maximum = ct.CtProcessControl.TotalBatches;
                progress_batch.Visible = true;
                progress_batch.Value = batchesDone;
            });
        }
        
        // Asynchronous Method that clears the progress related elements after a certain time passed in the method
        // We do this because IProgress is also async 
        private void ResetUIProgress()
        {
            Progress1Running = false;
            var task = (MethodInvoker) delegate
            {
                progress_total.Value = 0;
                progress_total.Maximum = 100;
                progress_total.Style = ProgressBarStyle.Continuous;
                label_progress_value1A.Text = "";
                label_progress_value1A.Visible = false;
                label_progress_status1A.Text = "";
                label_progress_status1A.Visible = false;
                label_progress_value2A.Text = "";
                label_progress_value2A.Visible = false;
                label_progress_status2A.Text = "";
                label_progress_value2A.Visible = false;
            };
            if (this.InvokeRequired)
                this.BeginInvoke(task);
            else
                task();
        }

        // Update buttons
        private void UpdateButtonState()
        {
            var task = (MethodInvoker) delegate
            {
                button_preview.Enabled = ct.ReadyIndex();
                button_start_batch.Enabled = ct.ReadyBatch();
            };
            if (this.InvokeRequired)
                this.BeginInvoke(task);
            else
                task();
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            combo_source.Text = combo_source.Focused ? ct.DynPathSource.Path : ct.DynPathSource.ShortPath;
            combo_output.Text = combo_output.Focused ? ct.DynPathOutput.Path : ct.DynPathOutput.ShortPath;
            combo_csv.Text = combo_csv.Focused ? ct.DynPathCSV.Path : ct.DynPathCSV.FileName;
        }

        private void Combo_Any_Enter_Leave(object sender, EventArgs e)
        {
            UpdateComboFormats();
        }

        private async Task DoIndex()
        {
            List<string> result = new();
            try
            {
                //await Task.Run(() => state.FreezeButtons());
                await Task.Run(state.FreezeButtons);
                var lb1 = label_progress_status1A;
                lb1.Text = @"Indexing: ";
                lb1.Visible = true;
                var progress = LinkProgressBarInt2L(this, progress_total, label_progress_value1A,
                    label_progress_status1A, true);
                Progress1Running = true;
                result = await Task.Run(() => ct.IndexSource(progress));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error During File Indexing", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally
            {
                await Task.Run(state.RestoreButtons);
                // await Task.Run(() => state.RestoreButtons());
                ResetUIProgress();
            }
            if (result.Count < 2) return;
            var displayResult = result.Aggregate((a, b) => a + Environment.NewLine + b);
            // Asynchronously populate the ListView at this point
            MessageBox.Show(displayResult, @"Indexing Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // await Task.Run(PopulateWorkersList);
        }

        private void PopulateWorkersList()
        {
            // Create listview items
            listView1.BeginInvoke((MethodInvoker)delegate ()
            {
                listView1.Clear();
                listView1.Columns.Add("Subfolder", 150, HorizontalAlignment.Left);
                listView1.Columns.Add("File", 150, HorizontalAlignment.Left);
                listView1.Columns.Add("Text", 150, HorizontalAlignment.Left);
                listView1.BeginUpdate();
            });

            // Create new listview item collection
            var items = new ListView.ListViewItemCollection(listView1);
            
            // Create an array of listview items
            var itemsOut = ct.CtProcessControl.Select(x => new ListViewItem(new[]
            {
                Path.GetFileName(Path.GetDirectoryName(x.WavSourcePath)), // Subfolder
                Path.GetFileName(x.WavSourcePath), // File
                x.Text // Text
            })).ToArray();
            
            items.AddRange(itemsOut);

            listView1.BeginInvoke((MethodInvoker)delegate ()
            {
                listView1.EndUpdate();
            });
        }

        /// <summary>
        /// Populates the ListView with the results of the indexing
        /// </summary>
        private void PopulateListView_Full()
        {
            // Create listview items
            listView1.BeginInvoke((MethodInvoker)delegate ()
            {
                progress_total.Style = ProgressBarStyle.Marquee;
                progress_total.MarqueeAnimationSpeed = 50;
                label_progress_status1A.Text = @"Populating list view...";
                label_progress_status1A.Visible = true;
                listView1.Clear();
                listView1.Columns.Add("Subfolder", 150, HorizontalAlignment.Left);
                listView1.Columns.Add("File", 150, HorizontalAlignment.Left);
                listView1.Columns.Add("Text", 150, HorizontalAlignment.Left);
            });

            // Create new listview item collection
            var items = new ListView.ListViewItemCollection(listView1);
            
            // Create an array of listview items
            var itemsOut = ct.CtProcessControl.Select(x => new ListViewItem(new[]
            {
                Path.GetFileName(Path.GetDirectoryName(x.WavSourcePath)), // Subfolder
                Path.GetFileName(x.WavSourcePath), // File
                x.Text // Text
            })).ToArray();
            
            items.AddRange(itemsOut);

            listView1.BeginInvoke((MethodInvoker)delegate ()
            {
                // listView1.BeginUpdate();
                // listView1.Items.AddRange(itemsOut);
                // listView1.EndUpdate();
                label_progress_status1A.Text = "";
                label_progress_status1A.Visible = false;
                progress_total.Style = ProgressBarStyle.Continuous;
            });
        }
    }
}