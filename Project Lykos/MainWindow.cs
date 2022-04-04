namespace Project_Lykos
{
    using static ElementLink;
    public partial class MainWindow : Form
    {
        private readonly LykosController ct;
        private readonly ProcessControl pc;
        
        private readonly DynamicPath source = new();
        private readonly DynamicPath output = new();
        private readonly DynamicPath csv = new();

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
                    source.SetPath(fbd.SelectedPath);
                    UpdateComboFormats();
                }
                else
                {
                    MessageBox.Show("The selected directory does not exist. Or cannot be reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    output.SetPath(fbd.SelectedPath);
                    UpdateComboFormats();
                }
                else
                {
                    MessageBox.Show("The selected directory does not exist. Or cannot be reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Browse CSV button
        private async void Button_browse_csv_Click(object sender, EventArgs e)
        {
            // Create a new OpenFileDialog and show
            OpenFileDialog ofd = new()
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Select the .csv file used for voice generation",
                CheckFileExists = true,
            };
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    try
                    {
                        await ParseCSV(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Parsing CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("The selected file does not exist. Or cannot be reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Parse CSV
        private async Task ParseCSV(string filename)
        {  
            var readTask = ct.SetFilepathCsvAsync(filename);
            string msg = await readTask;
            
            if (await readTask != "0")
            {
                MessageBox.Show(msg, "Error reading csv file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lb1 = label_progress_status1A;
            var lb2 = label_progress_status2A;
            lb1.Text = "Progress: ";
            lb2.Text = "Current Lines: ";
            lb1.Visible = true;
            lb2.Visible = true;

            var progress = LinkProgressBar2L(progress_total, label_progress_value1A, label_progress_status1A, true);
            var progressLine = LinkLabel(label_progress_value2A);

            await ct.LoadCsvAsync(filename, progress, progressLine);

            if (ct.IsCsvLoaded())
            {
                csv.SetPath(filename);
                UpdateComboFormats();
                label_progress_value1A.Text = "";
                progress_total.Value = 0;
                progress_total.Maximum = 100;
            }
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            if (combo_source.Focused)
            {
                combo_source.Text = source.Path;
            }
            else
            {
                combo_source.Text = source.ShortPath;
            }
            if (combo_output.Focused)
            {
                combo_output.Text = output.Path;
            }
            else
            {
                combo_output.Text = output.ShortPath;
            }
            if (combo_csv.Focused)
            {
                combo_csv.Text = csv.Path;
            }
            else
            {
                combo_csv.Text = csv.FileName;
            }
        }

        private void Combo_Any_Enter_Leave(object sender, EventArgs e)
        {
            UpdateComboFormats();
        }
    }

    public class DynamicPath
    {
        public string Path { get; private set; } = "";
        public string ShortPath { get; private set; } = "";
        public string FileName { get; private set; } = "";

        public DynamicPath()
        {
            SetPath("");
        }

        public DynamicPath(string path)
        {
            SetPath(path);
            if (path.Length > 0)
            {
                FileName = Path[(Path.LastIndexOf("\\") + 1)..];
            }
        }

        public void SetPath(string path)
        {
            SetShortPath(path);
            SetFileName(path);
            this.Path = path;
        }

        private void SetShortPath(string FullPath)
        {
            if (FullPath.Length > 0)
            {
                string path_temp = FullPath[(FullPath.LastIndexOf("\\") + 1)..];
                path_temp = @".\" + path_temp + @"\";
                this.ShortPath = path_temp;
            }
        }

        private void SetFileName(string FullPath)
        {
            if (FullPath.Length > 0)
            {
                string path_temp = FullPath[(FullPath.LastIndexOf("\\") + 1)..];
                this.FileName = path_temp;
            }
        }
    }
}