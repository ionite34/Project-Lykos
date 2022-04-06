namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController ct;
        private readonly ProcessControl pc;
        
        private readonly DynamicPath DynPathSource = new();
        private readonly DynamicPath DynPathOutput = new();
        private readonly DynamicPath DynPathCSV = new();

        // Tooltip
        private readonly ToolTip labelTips = new();

        // States
        private bool ready_Preview = false;

        public MainWindow()
        {
            InitializeComponent();

            // Help text
            labelTips.ToolTipIcon = ToolTipIcon.Info;
            labelTips.IsBalloon = true;
            labelTips.ShowAlways = true;
            labelTips.SetToolTip(label_multiprocess_count, "Number of processes to run in parallel");

            // Set Default state for Combo Boxes
            combo_audio_preprocessing.SelectedIndex = 2;
            combo_csvDelimiter.SelectedIndex = 0;
            combo_multiprocess_count.DataSource = Enumerable.Range(1, Environment.ProcessorCount).ToList();
            combo_multiprocess_count.SelectedIndex = 0;

            ct = new LykosController();
            pc = new ProcessControl();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void Box_output_path_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Browse Source button
        private void Button_browse_source_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Check folder path exists
                if (ct.SetFilepath_Source(fbd.SelectedPath))
                {
                    DynPathSource.SetPath(fbd.SelectedPath);
                    UpdateComboFormats();
                }
                else
                {
                    MessageBox.Show(@"The selected directory does not exist. Or cannot be reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Browse Output button
        private void Button_browse_output_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Check folder path exists
                if (ct.SetFilepath_Output(fbd.SelectedPath))
                {
                    DynPathOutput.SetPath(fbd.SelectedPath);
                    UpdateComboFormats();
                }
                else
                {
                    MessageBox.Show(@"The selected directory does not exist. Or cannot be reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        }

        // Parse CSV
        private async Task ParseCSV(string filename)
        {
            var lb1 = label_progress_status1A;
            lb1.Text = @"Progress: ";

            var testSetPath = Task.Run(async () =>
            {
                try
                {
                    await ct.SetFilepathCsvAsync(filename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error in initial reading of CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            await testSetPath;

            var loadCSV = Task.Run(async () =>
            {
                var progress = LinkProgressBar2L(progress_total, label_progress_value1A, label_progress_status1A, true);
                try
                {
                    await ct.LoadCsvAsync(filename, progress);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Error Parsing CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await ResetUIProgress(1000);
                }
            });

            await loadCSV;

            if (ct.IsCsvLoaded())
            {
                DynPathCSV.SetPath(filename);
                UpdateComboFormats();
            }

            await ResetUIProgress(1000);
        }

        // Asynchronous Method that clears the progress related elements after a certain time passed in the method
        private async Task ResetUIProgress(int delay)
        {
            // Check that the delay is valid
            if (delay is < 0 or > 5000) throw new Exception("Invalid delay UI action.");
            var resetElements = Task.Run(() =>
            {
                Task.Delay(delay);
                progress_total.BeginInvoke((MethodInvoker)delegate ()
                {
                    progress_total.Value = 0;
                    progress_total.Maximum = 100;
                    label_progress_value1A.Text = "";
                    label_progress_status1A.Text = "";
                    label_progress_value2A.Text = "";
                    label_progress_status2A.Text = "";
                });
            });
            await resetElements;
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            combo_source.Text = combo_source.Focused ? DynPathSource.Path : DynPathSource.ShortPath;
            combo_output.Text = combo_output.Focused ? DynPathOutput.Path : DynPathOutput.ShortPath;
            combo_csv.Text = combo_csv.Focused ? DynPathCSV.Path : DynPathCSV.FileName;
        }

        private void Combo_Any_Enter_Leave(object sender, EventArgs e)
        {
            UpdateComboFormats();
        }
    }
}