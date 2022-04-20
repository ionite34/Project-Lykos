namespace Project_Lykos.Resampler_Tool
{
    public partial class ResamplerTool : Form
    {
        Resampler rs = new();
        private CancellationTokenSource cts = new();
        public ResamplerTool()
        {
            InitializeComponent();
            button_start.Enabled = false;
        }

        private void button_browse_source_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = fbd.SelectedPath;
            // Process
            try
            {
                rs.SetFilepath_Source(path);
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

        private void button_browse_output_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog and show
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = fbd.SelectedPath;
            // Process
            try
            {
                rs.SetFilepath_Output(path);
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

        private async void button_start_Click(object sender, EventArgs e)
        {
            if (button_start.Text == @"Cancel")
            {
                button_start.Enabled = false;
                rs.ProcessWorker.ProgressChanged -= OnProgress;
                progressBar1.Value = 0;
                label_est.Text = "";
                cts.Cancel();
                await Task.Delay(250);
                button_start.Text = @"Start";
                button_start.Enabled = true;
                return;
            }
            button_start.Enabled = false;
            rs.ProcessWorker.SampleRate = GetResamplingRate();
            rs.ProcessWorker.Channels = GetChannels();
            rs.ProcessWorker.ProgressChanged += OnProgress;
            progressBar1.Style = ProgressBarStyle.Marquee;
            try
            {
                cts = new CancellationTokenSource();
                button_start.Text = @"Cancel";
                button_start.Enabled = true;
                await rs.ProcessWorker.Start(Environment.ProcessorCount, cts);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Operation canceled", @"Canceled", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error processing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button_start.Text = @"Cancel";
                progressBar1.Value = 0;
                progressBar1.Text = "";
                MessageBox.Show(@$"Resampling has finished. {rs.ProcessWorker.Total} files processed", 
                    @"Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // On Progress update
        private void OnProgress(int current)
        {
            if (current <= 0) return;
            var max = rs.ProcessWorker.Total;
            var percent = (int)Math.Round((double)current / max * 100);
            var str = $"{current}/{max}";

            
            progressBar1.BeginInvoke((MethodInvoker)delegate ()
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Maximum = max;
                progressBar1.Value = current;
                progressBar1.Text = percent.ToString();
                label_est.Text = str;
                label_est.Visible = true;
            });
        }
        
        // Get Sampling Rate
        private int GetResamplingRate()
        {
            var choice = combo_sampling_rate.SelectedItem.ToString();
            if (choice == null) throw new Exception("Sampling rate not valid");
            return int.Parse(choice);
        }

        // Get Channels
        private int GetChannels()
        {
            var index = combo_channels.SelectedIndex;
            if (index == 0) return -1; // For automatic
            return index;
        }

        // Update buttons
        private void UpdateButtonState()
        {
            var task = (MethodInvoker)delegate
            {
                button_start.Enabled = rs.ReadyStart();
            };
            if (InvokeRequired)
                BeginInvoke(task);
            else
                task();
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            combo_source.Text = combo_source.Focused ? rs.SourcePath.Path : rs.SourcePath.ShortPath;
            combo_output.Text = combo_output.Focused ? rs.OutputPath.Path : rs.OutputPath.ShortPath;
        }

        private void Combo_Any_Enter_Leave(object sender, EventArgs e)
        {
            UpdateComboFormats();
        }
    }
}
