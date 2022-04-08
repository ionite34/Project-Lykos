using System.ComponentModel;

namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController ct = new();
        private readonly State state;

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
            _labelTips.SetToolTip(label_multiprocess_count, "Number of processes to run in parallel");

            // Set Default state for Combo Boxes
            combo_audio_preprocessing.SelectedIndex = 2;
            combo_csvDelimiter.SelectedIndex = 0;
            combo_multiprocess_count.DataSource = Enumerable.Range(1, Environment.ProcessorCount).ToList();
            combo_multiprocess_count.SelectedIndex = 0;
            
            // Initialize state
            state = new State(this);
        }

        // Browse Source button
        private async void Button_browse_source_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            // Process
            try
            {
                await Task.Run(() => ct.SetFilepath_Source(fbd.SelectedPath));
                UpdateComboFormats();
                UpdateStatus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Browse Output button
        private async void Button_browse_output_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            // Process
            ct.SetFilepath_Output(fbd.SelectedPath);
            UpdateComboFormats();
            UpdateStatus();
            try
            {
                await Task.Run(() => ct.SetFilepath_Output(fbd.SelectedPath));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateComboFormats();
                UpdateStatus();
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
            if (ofd.ShowDialog() != DialogResult.OK) return;
            await ParseCSV(ofd.FileName).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts the index preview process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_preview_Click(object sender, EventArgs e)
        {
            state.FreezeButtons();
            await DoIndex();
            state.RestoreButtons();
        }
        
        /// <summary>
        /// Starts the batch process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void button_start_batch_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
        // Parse CSV
        private async Task ParseCSV(string filename)
        {
            try
            {
                state.FreezeButtons();
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
                state.RestoreButtons();
                ResetUIProgress();
                UpdateComboFormats();
                UpdateStatus();
            }
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
        private void UpdateStatus()
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
                var lb1 = label_progress_status1A;
                lb1.Text = @"Indexing: ";
                lb1.Visible = true;
                var progress = LinkProgressBarInt2L(this, progress_total, label_progress_value1A, label_progress_status1A, true);
                Progress1Running = true;
                result = await Task.Run(() => ct.IndexSource(progress)) ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error During File Indexing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ResetUIProgress();
            if (result.Count == 0) return;
            MessageBox.Show(result[0]);
            MessageBox.Show(result[1]);
        }
    }
}