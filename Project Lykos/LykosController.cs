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
        // Indexer
        private IndexEngine indexer = new();
        // Dynamic Paths
        public DynamicPath DynPathSource = new();
        public DynamicPath DynPathOutput = new();
        public DynamicPath DynPathCSV = new();

        // Configs
        public string Delimiter { get; set; } = "";

        // Data from CSV
        private DataTable CsvData { get; set; } = new();

        // Combined Data
        private DataTable CombinedData { get; set; } = new();

        // Data from user provided directories
        

        // Process Controller
        private readonly ProcessControl _pc;

        // Constructor
        public LykosController()
        {
            _pc = new ProcessControl();
        }
        
        // Checks if the CSV is loaded by checking the length of the DataTable
        public bool IsCsvLoaded()
        {
            return CsvData.Rows.Count > 0;
        }

        public bool ReadyIndex()
        { 
            return DynPathSource.Exists() && DynPathCSV.Exists();
        }

        public bool ReadyBatch()
        {
            return DynPathSource.Exists() && DynPathCSV.Exists() && DynPathOutput.Exists();
        }

        // Set the Filepath_Source, if valid path, else return false
        public bool SetFilepath_Source(string filepath)
        {
            // Check if the filepath is valid
            if (System.IO.Directory.Exists(filepath))
            {
                // Set the Filepath_Source
                DynPathSource.SetPath(filepath);
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
                DynPathOutput.SetPath(filepath);
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
            DynPathCSV.SetPath(filepath);
            return true;
        }


        /// <summary>
        /// IndexSource - Indexes the Source Directory and sets the CombinedData DataTable if successful
        /// </summary>
        /// <param name="progress"></param>
        /// <returns>
        /// Returns a list of strings of indexed results
        /// </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<List<string>> IndexSource(IProgress<(int current, int total)> progress)
        {
            if (!DynPathSource.Exists()) throw new Exception("Source Path not loaded.");
            if (!DynPathCSV.Exists() || (CsvData.Rows.Count <= 0)) throw new Exception("CSV Path not loaded.");
            var path = DynPathSource.Path;
            var indexData = IndexEngine.IndexToDataTable(path);

            // Check length of the new index DataTable and the CSV DataTable, if they are not the same, throw an exception
            var indexCount = indexData.Rows.Count;
            var csvCount = CsvData.Rows.Count;
            // if (indexCount != csvCount) throw new Exception($"Source file count ({indexCount}) and CSV file count ({csvCount}) are not the same size.");

            // Build a new DataTable with the CSV DataTable and the new index DataTable
            var dtNew = new DataTable();
            dtNew.Columns.Add("text", typeof(string));
            dtNew.Columns.Add("absolute_path", typeof(string));
            dtNew.Columns.Add("relative_path", typeof(string));
            dtNew.Columns.Add("output_path", typeof(string));            

            // Record reportable events
            List<string> filesWithoutTextMatch = new();

            // Loop for each row in the index DataTable (indexDataTable), we will take the 'RelativePath' entry
            // we will search for a match of this entry in another DataTable called CSVData under the 'out_path' column
            // if a match is found we will add the respective 'text' match in that other CSVData table to the new DataTable.
            var indexOperation = Task.Run(() =>
            {
                var currentRow = 0;
                foreach (DataRow row in indexData.Rows)
                {
                    var indexToUse = 0;
                    var relativePath = row["RelativePath"].ToString();
                    // Find if any row within the CSVData 'out_path' column contains a part of the relativePath
                    var matchingRow = CsvData.Select($"out_path LIKE '%{relativePath}%'");
                    // For no match:
                    if (matchingRow.Length <= 0)
                    {
                        // Record the filepath
                        if (relativePath != null) filesWithoutTextMatch.Add(relativePath);
                        continue;
                    }
                    // For multiple matches on the same relativePath
                    if (matchingRow.Length > 1)
                    {
                        if (row.IsNull("FullPath"))
                            throw new NullReferenceException("An indexed audio file was read as null.");
                        var pathAudio = row["FullPath"].ToString();
                        var dialog = new IndexCollisionDialog(pathAudio, matchingRow);
                        dialog.ShowDialog();
                        if (dialog.DialogResult == DialogResult.Abort) throw new TaskCanceledException("Index operation aborted. No files were processed.");
                        if (indexToUse < 0 || indexToUse > matchingRow.Length) throw new IndexOutOfRangeException("Dialog returned choice index out of bounds.");
                        indexToUse = dialog.ReturnedSelectedIndex;
                    }
                    // Continue for match -> add to new DataTable
                    var text = matchingRow[indexToUse]["text"].ToString();
                    var absPath = row["FullPath"].ToString();
                    var relPath = row["RelativePath"].ToString();
                    // outPath is the user selected output path + the 'RelativePath' from indexData
                    var outPath = Path.Join(DynPathOutput.Path, row["RelativePath"].ToString());
                    // Add to new DataTable
                    dtNew.Rows.Add(text, absPath, relPath, outPath);

                    // Progress Report
                    currentRow++;
                    progress.Report((currentRow, indexData.Rows.Count));
                }
            });

            await indexOperation;

            // Get number of files, and number of files with text match, return a list of strings
            var numFiles = dtNew.Rows.Count;
            var numFilesWithoutTextMatch = filesWithoutTextMatch.Count;
            List<string> result = new()
            {
                $"{numFiles} files indexed.",
                $"{numFilesWithoutTextMatch} files without text match."
            };
            return result;
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
    }
}
