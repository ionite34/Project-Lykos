using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Lykos.Word_Checker
{
    public partial class WordChecker : Form
    {
        // Word Parser
        private readonly WordParser wp = new();

        public WordChecker()
        {
            InitializeComponent();
        }

        private void Button_refresh_Click(object sender, EventArgs e)
        {

        }

        private void Button_browse_csv_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = ofd.FileName;
            try
            {
                wp.SetFilepath_CSV(path);
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

        private void Button_browse_dict_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new();
            var result = fbd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = fbd.SelectedPath;
            try
            {
                wp.SetFilepath_Dict(path);
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

        // Update buttons
        private void UpdateButtonState()
        {
            button_refresh.Enabled = wp.ReadyRefresh();
        }

        // Update the combo boxes depending on focus, shows short paths when out of focus, shows full paths when in focus
        private void UpdateComboFormats()
        {
            combo_csv.Text = combo_csv.Focused ? wp.CSVPath.Path : wp.CSVPath.ShortPath;
            combo_dict.Text = combo_dict.Focused ? wp.DictPath.Path : wp.DictPath.ShortPath;
        }

        private void Combo_Any_Enter_Leave(object sender, EventArgs e)
        {
            UpdateComboFormats();
        }
    }
}
