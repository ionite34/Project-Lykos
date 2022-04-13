using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Lykos
{
    public partial class DependencyDialog : Form
    {
        public DependencyDialog()
        {
            InitializeComponent();
        }

        private void LinkLabel_download_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            const string target = @"https://www.nexusmods.com/skyrimspecialedition/mods/40971?tab=files";

            ProcessStartInfo startInfo = new()
            {
                FileName = "cmd",
                Arguments = "/c start " + target,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process processTemp = new()
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };
            try
            {
                processTemp.Start();
            }
            catch (Exception e1)
            {
                MessageBox.Show(@"Failed to launch link." + e1.Message);
            }
        }
        

        private void Button_exit_Click(object sender, EventArgs e)
        {
            // Close the entire application
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Button_retry_Click(object sender, EventArgs e)
        {
            if (!DependencyCheck.FonixDataExists()) {
                MessageBox.Show(@"Unable to locate FonixData.cdf, please ensure it is in the same folder as this application.",
                    @"FonixData.cdf Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!DependencyCheck.FonixDataChecksumOK())
            {
                MessageBox.Show(@"A FonixData.cdf file was found but its data is not valid. Please ensure you have downloaded the correct file.",
                    @"FonixData.cdf Integrity Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
