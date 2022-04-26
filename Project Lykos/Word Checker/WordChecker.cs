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
            // link data views
            // dataview_freq.DataSource = wp.DataGen.WordFreq;
            
            dataview_usage.DataSource = wp.DataGen.WordUsage;
            dataview_usage.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataview_usage.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataview_usage.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataview_usage.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            dataview_freq.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
            
            // Subscribe to the WordParser's DataUpdated event to refresh the data views
            wp.DataUpdated += (sender, e) =>
            {
                // Use thread safe calls to update the data views
                dataview_freq.Invoke((MethodInvoker)delegate
                {
                    LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
                });
            };
        }

        // Method to load the data view based on data table and filters
        private void LoadDataView(bool hideDict, bool hideIgnores)
        {
            DataView dv = new(wp.DataGen.WordFreq);
            var filter = "";
            if (hideDict)
            {
                filter += "NOT Enabled = true";
            }
            if (hideIgnores && filter != "")
            {
                filter += " AND ";
            }
            if (hideIgnores)
            {
                filter += "NOT Skipped = true";
            }
            if (filter != "")
            {
                dv.RowFilter = filter;
            }
            // Change column names
            dv.Sort = "Length DESC, Freq DESC";
            dataview_freq.DataSource = dv;
            dataview_freq.Columns["Length"]!.HeaderText = @"Len";
            dataview_freq.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        
        // Method to load word usage to view
        private async Task LoadUsageView(string word)
        {
            var usage = new List<string>();
            await Task.Run(() =>
            {
                usage = wp.GetSentences(word);
            });
            // Add to data view
            wp.DataGen.WordUsage.Clear();
            foreach (var s in usage)
            {
                var length = s.Length;
                wp.DataGen.WordUsage.Rows.Add(length, s);
            }
            wp.DataGen.WordUsage.DefaultView.Sort = "Length DESC";
        }

        private async void Button_refresh_Click(object sender, EventArgs e)
        {
            await wp.Refresh();
            LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
        }

        private async void Button_browse_csv_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = @"CSV Files (*.csv)|*.csv",
                Title = @"Select the .csv file used for voice generation",
                CheckFileExists = true,
            };
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK) return;
            var path = ofd.FileName;
            try
            {
                await wp.SetFilepath_CSV(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error parsing CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateComboFormats();
                await UpdateButtonState().ConfigureAwait(false);
            }
        }

        private async void Button_browse_dict_Click(object sender, EventArgs e)
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
                await UpdateButtonState().ConfigureAwait(false);
            }
        }

        // Update buttons
        private async Task UpdateButtonState()
        {
            button_refresh.Enabled = wp.ReadyRefresh();
            if (wp.ReadyRefresh())
            {
                await wp.Refresh();
                LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
            }
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

        private void Check_Any_Changed(object sender, EventArgs e)
        {
            LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
        }

        // Event when a cell is clicked
        private async void dataview_freq_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check that the cell clicked is a row and not a header
            if (e.RowIndex < 0) return;
            
            // Get the row that was clicked
            var row = dataview_freq.Rows[e.RowIndex];
            var word = row.Cells[0].Value.ToString();
            
            // Check if the cell clicked was a skip button cell
            if (e.ColumnIndex == 5)
            {
                // Skip the word
                // await wp.SkipWord(word);
                // Update the dataview
                LoadDataView(!check_ShowDict.Checked, !check_ShowIgnore.Checked);
            }
            

            if (string.IsNullOrWhiteSpace(word)) return;
            await LoadUsageView(word);
        }
    }
}
