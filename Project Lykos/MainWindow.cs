namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController _ct;

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

            _ct = new LykosController();
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
                await _ct.SetFilepath_Source(fbd.SelectedPath);
                UpdateComboFormats();
                UpdateStatus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Browse Output button
        private void Button_browse_output_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog();
            if (result != DialogResult.OK) return;
            // Process
            _ct.SetFilepath_Output(fbd.SelectedPath);
            UpdateComboFormats();
            UpdateStatus();
            try
            {
                _ct.SetFilepath_Output(fbd.SelectedPath);
                UpdateComboFormats();
                UpdateStatus();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var result = ofd.ShowDialog();
            if (result != DialogResult.OK) return;
            if (!(File.Exists(ofd.FileName)))
            {
                MessageBox.Show(@"The selected file does not exist. Or cannot be reached.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await ParseCSV(ofd.FileName);
            UpdateStatus();
        }

        private async void button_preview_Click(object sender, EventArgs e)
        {
            button_preview.Enabled = false;
            // Start file indexing
            await DoIndex();
        }
        
        // Parse CSV
        private async Task ParseCSV(string filename)
        {
            try
            {
                var csvCheckResult = await _ct.SetFilepathCsvAsync(filename);
                if (!csvCheckResult) return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error in initial reading of CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var progress = LinkProgressBar2L(progress_total, label_progress_value1A, label_progress_status1A, true);

            try
            {
                var lb1 = label_progress_status1A;
                lb1.Text = @"Progress: ";
                lb1.Visible = true;
                await _ct.LoadCsvAsync(filename, progress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error Parsing CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Run(ResetUIProgress);
            }

            if (_ct.IsCsvLoaded())
            {
                _ct.DynPathCSV.SetPath(filename);
                UpdateComboFormats();
            }

            await Task.Run(ResetUIProgress);
        }

        // Asynchronous Method that clears the progress related elements after a certain time passed in the method
        // We do this because IProgress is also async 
        private void ResetUIProgress()
        {
            var task = (MethodInvoker) delegate
            {
                progress_total.Value = 0;
                progress_total.Maximum = 100;
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
            button_preview.BeginInvoke((MethodInvoker)delegate
            {
                button_preview.Enabled = _ct.ReadyIndex();
                button_start_batch.Enabled = _ct.ReadyBatch();
            });
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            combo_source.Text = combo_source.Focused ? _ct.DynPathSource.Path : _ct.DynPathSource.ShortPath;
            combo_output.Text = combo_output.Focused ? _ct.DynPathOutput.Path : _ct.DynPathOutput.ShortPath;
            combo_csv.Text = combo_csv.Focused ? _ct.DynPathCSV.Path : _ct.DynPathCSV.FileName;
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
                lb1.Text = @"Progress: ";
                lb1.Visible = true;
                var progress = LinkProgressBarInt2L(progress_total, label_progress_value1A, label_progress_status1A, true);
                //await Task.Run(() => _ct.IndexSource(progress));
                result = await Task.Run(() => _ct.IndexSource(progress)) ;
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

        private async void button_start_batch_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}