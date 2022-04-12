using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Project_Lykos
{
    internal class CSVReader
    {
        // Hold the LykosController reference
        private LykosController _ct;
        public CSVReader(LykosController controller)
        {
            _ct = controller;
        }
        
        /// <summary>
        /// Reads CSV Asynchronously with custom headers 'text' and 'out_path'
        /// Index is out_path, value is text
        /// Starts collision prompt if we detect collisions
        /// </summary>
        public async Task<Dictionary<string, string>> FastReadCSVAsync(string filePath, string pathHeader, string textHeader, IProgress<(double current, double total)> progress)
        {
            // Check that the CT has already loaded the IndexData table
            /*if (_ct.IndexData == null)
            {
                _ct.IndexData = new Dictionary<string, string>();
            }*/
            // Create dictionary
            var dict = new Dictionary<string, string>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };

            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, configuration);
            await csv.ReadAsync();
            csv.ReadHeader();
            var lastReportTime = DateTime.Now;
            while (csv.Read())
            {
                var path = csv.GetField<string>(pathHeader).Replace('/', '\\');
                var text = csv.GetField<string>(textHeader);
                // Check for existing key matches (path) and prompt user
                /*if (dict.ContainsKey(path))
                {
                    // Grab the existing text of the path
                    var existingText = dict[path];
                    var pathAudio = row["FullPath"].ToString();
                    var dialog = new IndexCollisionDialog(pathAudio, matchingRow);
                    dialog.ShowDialog();
                    if (dialog.DialogResult == DialogResult.Abort) throw new Exception("Index operation aborted. No files were processed.");
                    indexToUse = dialog.ReturnedSelectedIndex;
                }
                else
                {
                    dict.Add(path, text);
                }*/
                
                if (DateTime.Now.Subtract(lastReportTime).TotalMilliseconds > 370)
                {
                    progress.Report((stream.Position, stream.Length));
                    lastReportTime = DateTime.Now;
                }
            }
            return dict;
        }
    }
}
