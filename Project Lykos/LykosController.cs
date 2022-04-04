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
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        // Paths
        public String Filepath_Source { get; set; } = "";
        public String Filepath_Output { get; set; } = "";
        public String Filepath_Csv { get; set; } = "";
        
        // Configs
        public String Delimiter { get; set; } = "";

        // Data
        public DataTable CsvData { get; private set; } = new DataTable();

        // Constructor
        public LykosController()
        {
            // Initialize the LykosController
            // Initialize();
        }

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
        public async Task<string> SetFilepathCsvAsync(string filepath)
        {
            try
            {
                Delimiter = await ValidateCSV(filepath);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            // If everything is okay
            Filepath_Csv = filepath;
            return "0";
        }

        // Method to load the CSV file into the datatable
        // We do this asynchronously with progress reporting for the progress bar
        public async Task LoadCsvAsync(string filepath, IProgress<(double current, double total)> progress, IProgress<int> progressLines)
        {
            List<DataColumn> headerColumns = new();
            headerColumns.Add(new DataColumn("text", typeof(string)));
            headerColumns.Add(new DataColumn("out_path", typeof(string)));
            var readTask = ReadCsvAsync(filepath, headerColumns, progress, progressLines);
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
