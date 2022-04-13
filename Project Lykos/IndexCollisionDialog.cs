using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Lykos
{
    public partial class IndexCollisionDialog : Form
    {
        private readonly string pathAudio;
        public int ReturnedSelectedIndex { get; private set; }
        public bool ReturnOverrideState { get; private set; }

        public IndexCollisionDialog(string pathAudio, List<string> selections)
        {
            InitializeComponent();
            // Configure Dialog buttons
            button_exit.DialogResult = DialogResult.Abort;
            button_exit.Enabled = true;
            button_continue.DialogResult = DialogResult.Continue;
            button_continue.Enabled = false;
            // Set combo to display options
            combo_textChoices.DataSource = selections;
            this.pathAudio = pathAudio;
            linkLabel1.Text = pathAudio;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                using var player = new System.Media.SoundPlayer(pathAudio);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: Could not open audio file. " + ex.Message, @"Error playing audio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Combo_textChoices_SelectedValueChanged(object sender, EventArgs e)
        {
            button_continue.Enabled = true;
            ReturnedSelectedIndex = combo_textChoices.SelectedIndex;
        }

        private void IndexCollisionDialog_Load(object sender, EventArgs e)
        {
            // Play error sound
            SystemSounds.Exclamation.Play();
        }

        private void checkBox_override_CheckedChanged(object sender, EventArgs e)
        {
            ReturnOverrideState = checkBox_override.Checked;
        }
    }
}
