using System.Data;

namespace Project_Lykos
{
    using static FileOps;

    public class LykosController
    {
        // Indexer
        private readonly IndexControl _indexControl = new();
        
        // Dynamic Paths
        public DynamicPath DynPathSource = new();
        public DynamicPath DynPathOutput = new();
        public DynamicPath DynPathCSV = new();

        // Configs
        public string Delimiter { get; set; } = "";

        // Datasets
        private DataTable? CsvData { get; set; }

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
        public Task SetFilepath_Source(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("File not found.");
            // Check Directory layers
            if (!IndexControl.DirectoryLayerCheck(filepath))
            {
                throw new Exception("Invalid folder structure. Target cannot have more than 1 layer of subfolders.");
            }
            // Set the Filepath_Source
            DynPathSource.SetPath(filepath);
            // Calls a task to index the files
            _indexControl.BackgroundIndexFiles(filepath);
            return Task.CompletedTask;
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
        public List<string> IndexSource(IProgress<(int current, int total)> progress)
        {
            if (!DynPathSource.Exists()) throw new Exception("Source Path not loaded.");
            if (!DynPathOutput.Exists()) throw new Exception("Output Path not loaded.");
            if (!DynPathCSV.Exists() || (CsvData.Rows.Count <= 0)) throw new Exception("CSV Path not loaded.");

            // Build a dictionary with the CSV DataTable
            // The key is the relative path (subfolder/file)
            // The value is the text
            // Where the key is the path and the value is the text
            var csvDict = new Dictionary<string, string>();
            
            // Temp store the last user override choice
            var lastUserOverrideFile = "";
            var lastUserOverrideChoice = "";
            var lastUserChoiceUseOld = false;
            var userOverrideOn = false;
            
            foreach (DataRow row in CsvData.Rows)
            {
                var fullPathOriginal = row["path"].ToString();
                var subFolder = Path.GetFileName(Path.GetDirectoryName(fullPathOriginal));
                var file = Path.GetFileName(fullPathOriginal);
                var relativePath = $"{subFolder}\\{file}";
                var text = row["text"].ToString();
                if (text == null) throw new Exception("Text is null.");

                // Detect if the path is already in the dictionary
                if (csvDict.ContainsKey(relativePath))
                {
                    // If it is, do a lookup of the current value
                    var currentText = csvDict[relativePath];
                    // Check if this matches the last user override choice, if that is on
                    if (userOverrideOn && lastUserOverrideFile == file && !lastUserChoiceUseOld)
                    {
                        // For last user chose to use the old text, use the old text
                        csvDict[relativePath] = currentText;
                        continue;
                    }
                    else if (userOverrideOn && lastUserOverrideFile == file && lastUserChoiceUseOld)
                    {
                        // For last user chose to use the new text, use the new text
                        csvDict[relativePath] = text;
                        continue;
                    }
                    else if (userOverrideOn && lastUserOverrideFile != file)
                    {
                        userOverrideOn = false;
                    }
                    // Grab the audio path by looking for a match in relative path
                    var pathAudio = _indexControl.IndexData.Rows.Find(relativePath)?["FullPath"].ToString();
                    // Create a list with the current value and new value to be added
                    var list = new List<string> { text, currentText };
                    var dialog = new IndexCollisionDialog(pathAudio, list);
                    dialog.ShowDialog();
                    if (dialog.DialogResult == DialogResult.Abort) throw new Exception("Index operation aborted. No files were processed.");
                    // For case that user selects new value, overwrite, otherwise do not overwrite.
                    userOverrideOn = dialog.ReturnOverrideState;
                    if (dialog.ReturnedSelectedIndex == 0)
                    {
                        csvDict[relativePath] = text;
                        lastUserOverrideFile = file;
                        lastUserChoiceUseOld = false;
                    }
                    else
                    {
                        lastUserOverrideFile = file;
                        lastUserChoiceUseOld = true;
                    }
                }
                else
                {
                    // If it is not, add the text and relative path to the dictionary
                    csvDict.Add(relativePath, text);
                }
            }
            
            // Record reportable events
            List<string> filesWithoutTextMatch = new();

            var currentRow = 0;
            var lastPercent = -1;
            var maxRow = _indexControl.IndexData.Rows.Count;
            
            // Loop through the index data
            // Use the dictionary to find the text
            // Create a ProcessTask object and add to the ProcessControl's CurrentTasksBatch list
            foreach (DataRow row in _indexControl.IndexData.Rows)
            {
                // Get the relative path from the IndexData
                var relativePath = row[0].ToString();
                var fullPath = row[1].ToString();
                if (relativePath == null || fullPath == null) 
                    throw new Exception("IndexData has a null relative path.");
                // Lookup the text from the dictionary
                var text = csvDict.ContainsKey(relativePath) ? csvDict[relativePath] : "";

                // For no match, record the full paths
                if (text == "")
                {
                    filesWithoutTextMatch.Add(row[1].ToString() ?? string.Empty);
                    continue;
                }
                
                // Replace the extension of relativePath from .wav to .lip
                var lipOutput = Path.ChangeExtension(fullPath, ".lip");
                    
                // Build the ProcessTask object
                var task = new ProcessTask
                {
                    WavSourcePath = fullPath,
                    LipOutputPath = Path.Combine(DynPathOutput.Path, lipOutput),
                };
                
                // Add the ProcessTask to the ProcessControl's CurrentTasksBatch list
                _pc.CurrentTaskBatch.Enqueue(task);

                // Progress Report
                currentRow++;
                var percent = currentRow / maxRow * 100;
                if (percent != lastPercent)
                {
                    if (currentRow > maxRow) throw new Exception("IndexData has more rows than expected.");
                    progress.Report((currentRow, maxRow));
                    lastPercent = percent;
                }
            }
            // Get number of files, and number of files with text match, return a list of strings
            var numFiles = _pc.CurrentTaskBatch.Count;
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
            // var readTask = ReadCsvAsync(filepath, headerColumns, progress);
            var readTask = ReadCsvAsyncV2(filepath, "out_path", "text", progress);
            CsvData = await readTask;
        }
    }
}
