using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController ct = new();
        private readonly ProcessControl pc;
        private readonly State state;

        // Dictionary of buttons and states
        private readonly Dictionary<Control, bool> buttonStates = new();
        
        // Cancellation token for the background worker
        private CancellationTokenSource cts1 = new();
        
        // Timers
        private readonly System.Windows.Forms.Timer progressTimer = new();
        private TimeEstimate? etcCalc;

        public bool Progress1Running { get; set; } = false;
        private int lastTimeEstimateCounter = 0;

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
            
            // Objects
            pc = ct.CtProcessControl;
            
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
            if (pc.Count == 0)
            {
                // If there are no tasks, we need to index first
                await DoIndex();
            }
            // Now we can start the batch
            try
            {
                // First, create a new cancellation token source
                cts1 = new CancellationTokenSource();
                
                await Task.Run(state.FreezeButtons);
                
                // Enable stop button
                button_stop_batch.Enabled = true;
                
                // ct.CtProcessControl.ProgressChanged += Batch_ProgressChanged;
                pc.Report += AddListViewItem;

                // Start processing
                progressTimer.Start();
                etcCalc = new TimeEstimate(pc.Count);
                var procNum = combo_multiprocess_count.SelectedIndex + 1;
                await pc.Start(procNum, 128, false, cts1);
                
                // If pc count is not 0, run until pc count is 0
                while (true)
                {
                    // Reset Index
                    var sourcePath = ct.DynPathSource.Path;
                    var targetPath = ct.DynPathOutput.Path;
                    await Task.Run(() => ct.SetFilepath_Source(sourcePath));
                    await Task.Run(() => ct.SetFilepath_Output(targetPath));
                    // Run index again
                    await DoIndex(true);
                    // Check if pc count is 0
                    if (pc.Count == 0) break;
                    // If not, run again
                    await pc.Start(procNum, 128, false, cts1);
                    // Wait 100ms
                    await Task.Delay(100);
                }
                
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
                if (etcCalc != null) etcCalc.TimeInitialized = false;
                ResetUIProgress(); // Reset the progress bar and labels
                
                // Clear temp
                await TryStopClearTemp();
                
                // Show some results via message box
                MessageBox.Show($@"Total files output: {pc.TotalProcessed}/{pc.TotalFiles}", 
                    @"Batch processing completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        
        // Do indexing
        private async Task DoIndex(bool quiet = false)
        {
            var result = "";
            try
            {
                await Task.Run(state.FreezeButtons);
                var lb1 = label_progress_status1A;
                lb1.Text = @"Indexing: ";
                lb1.Visible = true;
                label_progress_value1A.Visible = true;
                var progress = LinkProgressBarInt2L(this, progress_total, label_progress_value1A,
                    label_progress_status1A, false, true);
                Progress1Running = true;
                progress_total.Style = ProgressBarStyle.Marquee; 
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
                ResetUIProgress();
            }
            if (quiet) return;
            MessageBox.Show(result, @"Indexing Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        
        // Method to add new entry to listview based on report
        private void AddListViewItem(string report)
        {
            listView1.BeginInvoke((MethodInvoker) delegate()
            {
                if (listView1.Columns.Count == 0)
                {
                    listView1.Columns.Insert(0, "Time");
                    listView1.Columns.Insert(1, "Report");
                    var totalWidth = listView1.Width;
                    listView1.Columns[0].Width = (int) (totalWidth * 0.2);
                    listView1.Columns[1].Width = (int) (totalWidth * 0.8);
                    // Set auto resize
                    listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
                    listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.None);
                }
                var lvi = new ListViewItem(report);
                var now = DateTime.Now;
                lvi.SubItems.Add(report);
                lvi.SubItems.Add($"{now:HH:mm:ss}");
                listView1.Items.Add(lvi);
                listView1.EnsureVisible(listView1.Items.Count - 1);
            });
        }
        
        
        // Method to set the progress bar on batch updates
        private void Batch_ProgressChanged(object? sender, EventArgs e)
        {
            progress_total.BeginInvoke((MethodInvoker)delegate ()
            {
                // Check if process count is 0, show marquee
                if (pc.ProcessedCount == 0)
                {
                    if (progress_total.Style == ProgressBarStyle.Marquee) return;
                    progress_total.Style = ProgressBarStyle.Marquee;
                    label_progress_status1A.Text = @"Starting batch...";
                    progress_total.MarqueeAnimationSpeed = 25;
                    return;
                }
                
                // Create etcCalc if null
                etcCalc ??= new TimeEstimate(pc.TotalFiles);
                // If this is the first update, set the etc timer start time
                if (pc.ProcessedCount > 0 && !etcCalc.TimeInitialized)
                {
                    etcCalc.SetStartTime(DateTime.Now);
                }
                
                // Calculate estimated time remaining every 10 cycles
                lastTimeEstimateCounter++;
                if (lastTimeEstimateCounter > 10)
                {
                    var estComplete = etcCalc.GetEtc(pc.TotalProcessed);
                    var estDuration = estComplete.Subtract(DateTime.Now);
                    var strDuration = estDuration.ToString(@"hh\:mm");
                    // Get processing rate
                    var procRate = etcCalc.GetItemsPerSecond(pc.TotalProcessed);
                    // Convert procRate to 2 significant figures
                    var strProcRate = Math.Round(procRate, 2).ToString(CultureInfo.CurrentCulture);
                    // Update the label
                    label_progress_status2A.Text = $@"Time remaining: {strDuration}";
                    label_progress_value2A.Text = $@"Process rate: {strProcRate} items/sec";
                    label_progress_status2A.Visible = true;
                    label_progress_value2A.Visible = true;
                    // Reset counter
                    lastTimeEstimateCounter = 0;
                }

                int progress = ct.CtProcessControl.ProcessedCount;
                int batchesDone = ct.CtProcessControl.CurrentBatch;
                label_progress_status1A.Text = @"Current batch progress: ";
                label_progress_status1A.Visible = true;
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

        private void button_additional_Click(object sender, EventArgs e)
        {
            // Open Dialog of AdditionalOptions
            AdditionalOptions form = new();
            form.ShowDialog();
        }
    }
}