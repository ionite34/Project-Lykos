using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using WinRT;

namespace Project_Lykos.Word_Checker
{
    public class WordParser
    {
        // Event for when data updated due to file change
        public event EventHandler? DataUpdated;
        
        public readonly DynamicPath CSVPath = new();
        public readonly DynamicPath DictPath = new();
        public readonly DataGen DataGen;
        private readonly CSV csv = new();
        private readonly DictReader dictReader = new();
        
        // Constructor
        public WordParser()
        {
            DataGen = new DataGen(dictReader);
        }
        
        public bool ReadyRefresh()
        {
            return CSVPath.Exists() && DictPath.Exists();
        }

        public async Task Refresh()
        {
            // Check CSV Loaded
            if (!csv.Loaded) throw new Exception("CSV not loaded");
            // Load Dict
            await Task.Run((() => dictReader.ReadDictionaries(DictPath.Path)));
            // Check Dict Loaded
            if (!dictReader.Loaded) throw new Exception("Dictionary not loaded");
            // Load Freq
            DataGen.LoadFreq(csv.Lines);
            // Start a folder watcher to watch for changes
            StartFolderWatcher();
        }
        
        // Watches for changes in the dictionary folder
        private void StartFolderWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            var watcher = new FileSystemWatcher
            {
                Path = DictPath.Path,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.json",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            // Add event handlers.
            watcher.Changed += OnChanged;
        }
        
        // Called when a file is changed
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Get the file name
            var fileName = e.FullPath;
            var fileInfo = new FileInfo(fileName);
            
            // Wait for the file to be fully written
            var waitCycles = 0;
            while (IsFileLocked(fileInfo))
            {
                if (waitCycles > 10) return;
                // Wait for the file to be fully written
                Task.Delay(100).Wait();
                waitCycles++;
            }

            // Build a new dictionary for enable state
            var dict = new ConcurrentDictionary<string, bool>();
            
            // Read json
            var jsonDict = DictReader.ReadDictFromJson(fileName);
            
            // Find the same json in the old dictionary
            var oldDict = dictReader.GetDictionary(jsonDict.Title);
            if (oldDict != null)
            {
                // If data records are not the same, call refresh
                if (oldDict.Data.Count != jsonDict.Data.Count)
                {
                    Refresh().ConfigureAwait(false);
                    return;
                }
                
            }
            else
            {
                // User deleted the dictionary?
                Refresh().ConfigureAwait(false);
                return;
            }

            // Loop through all words in the json dictionary
            Parallel.ForEach(jsonDict.Data, entry =>
            {
                var word = entry.Key;
                var enabled = entry.Value.Enabled;
                var currentState = dictReader.IsWordEnabled(word);
                if (currentState != enabled)
                {
                    // Add to dictionary
                    dict.TryAdd(word, enabled);
                }
            });
            
            // Loop through the dictionary and update the enabled state
            foreach (var (word, enabled) in dict)
            {
                // Find the entry in the datatable
                var dt = DataGen.WordFreq;
                // Find the data row
                var dataRow = dt.AsEnumerable().FirstOrDefault(r =>
                    r["Word"].ToString() == word);
                // If found, update the enabled state
                if (dataRow != null)
                {
                    dataRow["Enabled"] = enabled;
                }
                // Also update the enabled state in the dictionary
                dictReader.SetWordEnabled(word, enabled);
            }

            // Update the data table
            OnDataUpdated();
        }
        
        // Skips word
        public async Task SkipWord(string word, bool state, int rowIndex)
        {
            // Find the entry in the datatable
            var dt = DataGen.WordFreq;
            // Find the data row
            var row = dt.Rows[rowIndex];
            // If found, update the enabled state
            if (row != null)
            {
                row["Skipped"] = state;
            }
            // Also update the enabled state in the dictionary
            dictReader.SetWordSkipState(word, false);
            // Update the data table
            OnDataUpdated();
            // Save the dictionary
            await dictReader.SaveSkipState();
        }
        
        /// <summary>
        /// Checks if a file is locked
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileLocked(FileInfo file)
        {
            FileStream? stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }
        
        public async Task SetFilepath_CSV(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) throw new Exception("Directory not found.");
            CSVPath.SetPath(filePath);
            await csv.Load(filePath);
        }

        public void SetFilepath_Dict(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath)) throw new Exception("Directory not found.");
            DictPath.SetPath(dirPath);
        }
        
        // Method that returns list of sentences containing a supplied word
        public List<string> GetSentences(string word)
        {
            var result = new List<string>();
            // Loop through each line in CSV
            foreach (var line in csv.Lines)
            {
                if (line.ToLowerInvariant().Contains(word.ToLowerInvariant()))
                {
                    result.Add(line);
                }
            }
            return result;
        }

        private void OnDataUpdated()
        {
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
