using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void DependencyDialog_Load(object sender, EventArgs e)
        {

        }

        private void LinkLabel_download_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = "https://www.nexusmods.com/skyrimspecialedition/mods/40971?tab=files";
            System.Diagnostics.Process.Start(target);
        }

        private void Button_exit_Click(object sender, EventArgs e)
        {
            // Close the entire application
            button_exit.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Button_retry_Click(object sender, EventArgs e)
        {
            // Recheck dependency
            if (DependencyCheck.CheckFonixData()) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Unable to locate FonixData.cdf, please ensure it is in the same folder as this application.", "FonixData.cdf Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
