using System.ComponentModel;
using System.Diagnostics;

namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController ct = new();
        private readonly State state;

        // Dictionary of buttons and states
        private readonly Dictionary<Control, bool> buttonStates = new();
        
        // Cancellation token for the background worker
        private readonly CancellationTokenSource cts1 = new();
        
        // Timer
        private System.Windows.Forms.Timer progressTimer = new();
        private int queueSize = 0;

        public bool Progress1Running { get; set; } = false;

        // Tooltip
        private readonly ToolTip _labelTips = new();

        public MainWindow()
        {
            InitializeComponent();
            
            // Help text
            _labelTips.ToolTipIcon = ToolTipIcon.Info;
            _labelTips.IsBalloon = true;
            _labelTips.ShowAlways = true;
            _labelTips.SetToolTip(label_multiprocess_count, "Number of processes to run in parallel");

            // Set Default state for Combo Boxes
            combo_audio_preprocessing.SelectedIndex = 2;
            combo_csvDelimiter.SelectedIndex = 0;
            combo_outputActions.SelectedIndex = 0;
            combo_multiprocess_count.DataSource = Enumerable.Range(1, Environment.ProcessorCount).ToList();
            combo_multiprocess_count.SelectedIndex = 0;
            
            // Initialize state with all buttons
            state = new State(this);
            state.Buttons.Add(button_browse_csv);
            state.Buttons.Add(button_browse_output);
            state.Buttons.Add(button_browse_source);
            state.Buttons.Add(button_preview);
            state.Buttons.Add(button_start_batch);
            state.Buttons.Add(button_stop_batch);
            
            progressTimer.Tick += Batch_ProgressChanged;
            progressTimer.Interval = 50;
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
        
        // Index button
        private async void button_preview_Click(object sender, EventArgs e)
        {
            await DoIndex();
        }
        
        // Start batch button
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
            // Capture the current size of the queue
            queueSize = ct.CtProcessControl.Count;
            // Now we can start the batch
            try
            {
                await Task.Run(state.FreezeButtons);
                button_stop_batch.Enabled = true;
                // Set the progress bar
                progress_total.Style = ProgressBarStyle.Marquee;
                label_progress_status1A.Text = @"Starting batch processing...";
                label_progress_status1A.Visible = true;
                Progress1Running = true; // Set flag for progress update
                // ct.CtProcessControl.ProgressChanged += Batch_ProgressChanged;
                var procNum = combo_multiprocess_count.SelectedIndex + 1;
                
                progressTimer.Start();
                await ct.CtProcessControl.Start(procNum, 512, false, cts1);
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
                Progress1Running = false; // Remove flag for progress update
                ResetUIProgress(); // Reset the progress bar and labels
                
                // Show some results via message box
                var outputCount = ct.CtProcessControl.TotalProcessed;
                var msg = $@"Total files output: {outputCount}/{queueSize}";
                MessageBox.Show(msg, @"Batch processing completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Restore buttons
                await Task.Run(state.RestoreButtons);
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
        
        // Try to stop the process
        private async Task TryStopClearTemp()
        {
            // Begin Kill process and remove temp files
            await ct.CtProcessControl.EStop();
            var confirmKilled = Cache.KillProcesses();
            if (confirmKilled)
            {
                // Try to remove the temp files
                try
                {
                    Cache.Destroy();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($@"Could not remove temp files: {ex.Message}", 
                        @"Error removing temp files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Show error
                MessageBox.Show(@"Could not stop subprocesses.", 
                    @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Method to set the progress bar to a percentage event
        private void Batch_ProgressChanged(object? sender, EventArgs e)
        {
            if (!Progress1Running) return; // Stop if progress report flag is false
            progress_total.BeginInvoke((MethodInvoker)delegate ()
            {
                // Check if process count is 0, show marquee
                if (ct.CtProcessControl.ProcessedCount == 0)
                {
                    if (progress_total.Style != ProgressBarStyle.Marquee)
                    {
                        progress_total.Style = ProgressBarStyle.Marquee;
                        label_progress_status1A.Text = @"Starting batch...";
                        progress_total.MarqueeAnimationSpeed = 5;
                    }
                    return;
                }
                
                var estFiles = ct.CtProcessControl.TotalProcessed - lastFilesProcessed;
                // Calculate the estimated time spent per file in seconds
                var estTimePerFile = estDuration / estFiles;
                // Calculate the remaining files
                var remainingFiles = queueSize - ct.CtProcessControl.TotalProcessed;
                // Calculate the estimated time spent for the remaining files in seconds
                var estTimeRemaining = estTimePerFile * remainingFiles;
                // Parse to hours, minutes and seconds
                var estHMS = TimeSpan.FromSeconds(estTimeRemaining).ToString(@"hh\:mm\:ss");
                // Update the label
                label_progress_status2A.Text = $@"Estimated time to completion: ";
                label_progress_value2A.Text = $@"{estHMS}";
                label_progress_status2A.Visible = true;
                label_progress_value2A.Visible = true;
                // Update the last time estimate
                lastTimeEstimate = DateTime.UtcNow;
                lastFilesProcessed = estFiles;
                
                int progress = ct.CtProcessControl.ProcessedCount;
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
                progress_batch.Value = 0;
                progress_batch.Maximum = 100;
                progress_batch.Style = ProgressBarStyle.Continuous;
                label_progress_value1A.Text = "";
                label_progress_value1A.Visible = false;
                label_progress_status1A.Text = "";
                label_progress_status1A.Visible = false;
                label_progress_value2A.Text = "";
                label_progress_value2A.Visible = false;
                label_progress_status2A.Text = "";
                label_progress_value2A.Visible = false;
                label_batch_status.Text = "";
                label_batch_value.Text = "";
                label_batch_status.Visible = false;
                label_batch_value.Visible = false;
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