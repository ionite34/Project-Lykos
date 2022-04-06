using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Project_Lykos
{
    using static FileOps;
    
    public class LykosController
    {
        // Paths
        public string? Filepath_Source { get; private set; }
        public string? Filepath_Output { get; private set; }
        public string? Filepath_Csv { get; private set; }
        
        // Configs
        public string Delimiter { get; set; } = "";

        // Data
        private DataTable CsvData { get; set; } = new DataTable();

        // Checks if the CSV is loaded by checking the length of the DataTable
        public bool IsCsvLoaded()
        {
            return CsvData.Rows.Count > 0;
        }

        // Set the Filepath_Source, if valid path, else return false
        public bool SetFilepath_Source(string filepath)
        {
            // Check if the filepath is valid
            if (System.IO.Directory.Exists(filepath))
            {
                // Set the Filepath_Source
                Filepath_Source = filepath;
                return true;
            }
            else
            {
                // Return false
                return false;
            }
        }
        
        // Set the Filepath_Output, if valid path, else return false
        public bool SetFilepath_Output(string filepath)
        {
            // Check if the filepath is valid
            if (System.IO.Directory.Exists(filepath))
            {
                // Set the Filepath_Output
                Filepath_Output = filepath;
                return true;
            }
            else
            {
                // Return false
                return false;
            }
        }
        
        /*
         * Checks if the CSV file exists, then set the Filepath_Csv
         * Returns "0" if valid, or a String of invalid message
         */
        public async Task<bool> SetFilepathCsvAsync(string filepath)
        {
            var detectedDelimiter = await ValidateCSV(filepath);
            if (detectedDelimiter == "0") return false;
            Filepath_Csv = filepath;
            return true;
        }

        // Method to load the CSV file into the DataTable
        // We do this asynchronously with progress reporting for the progress bar
        public async Task LoadCsvAsync(string filepath, IProgress<(double current, double total)> progress)
        {
            List<DataColumn> headerColumns = new()
            {
                new DataColumn("text", typeof(string)),
                new DataColumn("out_path", typeof(string))
            };
            var readTask = ReadCsvAsync(filepath, headerColumns, progress);
            CsvData = await readTask;
        }
        
        // Check if the folder path exists
        private static bool CheckFolderPathExists(String path)
        {
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if the file path exists
        private static bool CheckFilePathExists(String path)
        {
            // Check if the file path exists
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
