using System.Data;
using System.Text;

namespace Project_Lykos
{
    using static FileOps;

    public class LykosController
    {
        // Indexer
        private readonly IndexControl sourceIndexer = new("*.wav");
        private readonly IndexControl outputIndexer = new("*.lip");
        
        // Dynamic Paths
        public DynamicPath DynPathSource = new();
        public DynamicPath DynPathOutput = new();
        public DynamicPath DynPathCSV = new();

        // Configs
        public string Delimiter { get; set; } = "";
        public bool SkipExisting { get; set; } = true;

        // Datasets
        private DataTable? CsvData { get; set; }

        // Process Controller
        public ProcessControl CtProcessControl { get; } = new();

        // Constructor
        public LykosController()
        {
            //_pc = new ProcessControl();
        }
        
        // Checks if the CSV is loaded by checking the length of the DataTable
        public bool IsCsvLoaded()
        {
            return CsvData != null && CsvData.Rows.Count > 0;
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
        public void SetFilepath_Source(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("File not found.");
            // Check Directory layers
            if (!IndexControl.DirectoryLayerCheck(filepath))
            {
                throw new Exception("Invalid folder structure. Target cannot have more than 1 layer of subfolders.");
            }
            DynPathSource.SetPath(filepath);
            sourceIndexer.BackgroundIndexFiles(filepath);
        }
        
        // Set the Filepath_Output, if valid path, else return false
        public void SetFilepath_Output(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("File not found.");
            DynPathOutput.SetPath(filepath);
            outputIndexer.BackgroundIndexFiles(filepath);
        }
        
        /*
         * Checks if the CSV file exists, then set the Filepath_Csv
         * Returns "0" if valid, or a String of invalid message
         */
        public async Task SetFilepathCsvAsync(string filepath)
        {
            Delimiter = await ValidateCSV(filepath);
            DynPathCSV.SetPath(filepath);
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
        public string IndexSource(IProgress<(int current, int total)> progress)
        {
            if (!DynPathSource.Exists()) throw new Exception("Source Path not loaded.");
            // if (!DynPathOutput.Exists()) throw new Exception("Output Path not loaded.");
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
                var relativePath = row["relative_path"].ToString();
                var file = Path.GetFileName(relativePath); 
                var text = row["text"].ToString();
                if (relativePath == null || text == null || file == null) 
                    throw new Exception("CSV Path column invalid on row: " + row.Table.Rows.IndexOf(row));

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
                    if (userOverrideOn && lastUserOverrideFile == file && lastUserChoiceUseOld)
                    {
                        // For last user chose to use the new text, use the new text
                        csvDict[relativePath] = text;
                        continue;
                    }
                    if (userOverrideOn && lastUserOverrideFile != file)
                    {
                        userOverrideOn = false;
                    }
                    // Grab the audio path by looking for a match in relative path
                    var pathAudio = sourceIndexer.IndexData.Rows.Find(relativePath)?["FullPath"].ToString();
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
            var filesSkipped = 0;

            var currentRow = 0;
            var maxRow = sourceIndexer.IndexData.Rows.Count;
            var lastReportTime = DateTime.Now;
            
            
            // If skip existing is on, convert the target index relative path to a list of strings
            List<string> filesExisting = new();
            if (SkipExisting)
            {
                // Convert to list using linq
                var list = outputIndexer.IndexData.AsEnumerable().Select(row => row[0].ToString()).ToList();
                if (list.Count > 0)
                {
                    // Confirm that no element is null
                    if (list.Any(x => x == null)) throw new Exception("Directory contains null values. Try again without skip existing option.");
                    filesExisting = list!;
                }
            }

            // Loop through the index data
            // Use the dictionary to find the text
            // Create a ProcessTask object and add to the ProcessControl's CurrentTasksBatch list
            foreach (DataRow row in sourceIndexer.IndexData.Rows)
            {
                // For skipping existing
                if (SkipExisting)
                {
                    // Check that filesExisting contains row[0]
                    var relPath = row[0].ToString();
                    // Remove extension
                    var relFolder = Path.GetDirectoryName(relPath);
                    var fileName = Path.GetFileNameWithoutExtension(relPath) + ".lip";
                    var joined = Path.Join(relFolder, fileName);
                    var outputPath = Path.Join(DynPathOutput.Path, joined);
                    if (File.Exists(outputPath))
                    {
                        filesSkipped++;
                        // Progress Report
                        currentRow++;
                        // Report progress if time elapsed more than 95ms
                        if (DateTime.Now.Subtract(lastReportTime).TotalMilliseconds < 19) continue;
                        progress.Report((currentRow, maxRow));
                        lastReportTime = DateTime.Now;
                        continue;
                    }
                }
                
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
                var lipRelativePath = Path.ChangeExtension(relativePath, ".lip");
                var lipOutputPath = Path.Combine(DynPathOutput.Path, lipRelativePath);
                // Add new task to PC
                CtProcessControl.Enqueue(new ProcessTask(fullPath, lipOutputPath, text));
                // Progress Report
                currentRow++;
                // Report progress if time elapsed more than 95ms
                if (DateTime.Now.Subtract(lastReportTime).TotalMilliseconds < 19) continue;
                progress.Report((currentRow, maxRow));
                lastReportTime = DateTime.Now;
            }
            // Get number of files, and number of files with text match, return a list of strings
            var reportNumTasks = CtProcessControl.Count;
            var reportNumFilesWithoutTextMatch = filesWithoutTextMatch.Count;
            StringBuilder result = new();
            result.AppendLine($"{reportNumTasks} generation tasks created.");
            result.AppendLine($"{reportNumFilesWithoutTextMatch} files without text match.");
            result.AppendLine($"{filesSkipped} existing files skipped.");
            return result.ToString();
        }

        // Method to load the CSV file into the DataTable
        // We do this asynchronously with progress reporting for the progress bar
        public async Task LoadCsvAsync(string filepath, IProgress<(double current, double total)> progress)
        {
            CsvData = await FileOps.ReadCsvAsync(filepath, "out_path", "text", progress).ConfigureAwait(false);
        }
    }
}
