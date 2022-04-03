namespace Project_Lykos
{
    public partial class MainWindow : Form
    {
        private readonly LykosController ct;

        public MainWindow()
        {
            InitializeComponent();
            // Set Default state for UI elements
            combo_audio_preprocessing.SelectedIndex = 1;
            // Create a LykosController object
            ct = new LykosController();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void Box_output_path_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button_browse_source_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // Show the dialog
            DialogResult result = fbd.ShowDialog();
            // If the user clicked OK
            if (result == DialogResult.OK)
            {
                // Check folder path exists
                LykosController.CheckFolderPathExists(fbd.SelectedPath);
                // Set the text of the textbox to the selected folder
                ct.Filepath_Source = fbd.SelectedPath;
            }
        }

        private void Button_browse_output_Click(object sender, EventArgs e)
        {

        }

        private void Button_browse_csv_Click(object sender, EventArgs e)
        {

        }
    }
}