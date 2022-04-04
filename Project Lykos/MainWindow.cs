namespace Project_Lykos
{
    public partial class MainWindow : Form
    {
        private readonly LykosController ct;
        private readonly ProcessControl pc;
        
        private string source_path_full = "";
        private string source_path_short = "";
        private string output_path_full = "";
        private string output_path_short = "";
        private string csv_path_full = "";
        private string csv_path_short = "";

        // States
        private bool ready_Preview = false;

        public MainWindow()
        {
            InitializeComponent();
            // Set Default state for Combo Boxes
            combo_audio_preprocessing.SelectedIndex = 2;
            combo_csvDelimiter.SelectedIndex = 0;
            combo_multiprocess_count.DataSource = Enumerable.Range(1, Environment.ProcessorCount).ToList();
            combo_multiprocess_count.SelectedIndex = 0;

            // Bind states to buttons
            button_preview.DataBindings.Add("Enabled", this, "ready_Preview");

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
                    source_path_full = fbd.SelectedPath;
                    source_path_short = fbd.SelectedPath.Substring(fbd.SelectedPath.LastIndexOf("\\") + 1);
                    source_path_short = @".\" + source_path_short + @"\";
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
                    output_path_full = fbd.SelectedPath;
                    output_path_short = fbd.SelectedPath.Substring(fbd.SelectedPath.LastIndexOf("\\") + 1);
                    output_path_short = @".\" + output_path_short + @"\";
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
            int currentLine = 0;
            
            var readTask = ct.SetFilepathCsvAsync(filename);
            string msg = await readTask;
            
            if (await readTask != "0")
            {
                MessageBox.Show(msg, "Error reading csv file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var progress = new Progress<(double current, double total)>(update =>
            {
                progress_total.Maximum = (int)Math.Round(update.total);
                progress_total.Value = (int) Math.Round(update.current);
                var formatted = currentLine.ToString("N0"); // Format the line count with commas
                label_lineCount.Text = "Reading lines: " + formatted;
            });
            var progressLine = new Progress<int>(lineNum =>
            {
                currentLine = lineNum;
            });
            await ct.LoadCsvAsync(filename, progress, progressLine);

            if (ct.IsCsvLoaded())
            {
                csv_path_full = filename;
                csv_path_short = csv_path_full.Substring(csv_path_full.LastIndexOf("\\") + 1);
                UpdateComboFormats();
                label_lineCount.Text = "";
                progress_total.Value = 0;
                progress_total.Maximum = 100;
            }
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            if (combo_source.Focused)
            {
                combo_source.Text = source_path_full;
            }
            else
            {
                combo_source.Text = source_path_short;
            }
            if (combo_output.Focused)
            {
                combo_output.Text = output_path_full;
            }
            else
            {
                combo_output.Text = output_path_short;
            }
            if (combo_csv.Focused)
            {
                combo_csv.Text = csv_path_full;
            }
            else
            {
                combo_csv.Text = csv_path_short;
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
        
        public DynamicPath(string path)
        {
            SetPath(path);
            FileName = Path[(Path.LastIndexOf("\\") + 1)..];
        }

        public void SetPath(string path)
        {
            SetShortPath(path);
            this.Path = path;
        }

        private void SetShortPath(string FullPath)
        {
            string path_temp = FullPath[(FullPath.LastIndexOf("\\") + 1)..];
            path_temp = @".\" + path_temp + @"\";
            this.ShortPath = path_temp;
        }
    }
}