using System.Diagnostics;

namespace Project_Lykos.Word_Checker
{
    public struct WordState
    {
        public bool Enabled { get; set; }
        public bool Skipped { get; set; }
        public string DictName { get; set; }
    }
    
    // Reads dictionary json files
    public class DictReader
    {
        // List of loaded json dictionaries
        private readonly List<JsonDictionary> loadedJsons = new();
        // List of loaded dictionary paths
        private readonly List<string> loadedJsonPaths = new();
        // Dictionary of words and their states
        private readonly Dictionary<string, WordState> wordDictState = new();
        // Dictionary mapping dict names to dictionary paths
        private readonly Dictionary<string, string> dictNameToPath = new();
        // Path of the dictionary folder
        private string? dictPath;
        
        // State
        public bool Loaded { get; private set; } = false;
        
        // Check if the word is in the main dictionary
        public bool IsWord(string word)
        {
            return wordDictState.ContainsKey(word);
        }
        
        // Check if the word is enabled
        public bool IsWordEnabled(string word)
        {
            return wordDictState[word].Enabled;
        }
        
        // Set the state of the word enabled
        public void SetWordEnabled(string word, bool enabled)
        {
            // Try to find the word
            if (!wordDictState.ContainsKey(word)) return;
            var state = wordDictState[word];
            state.Enabled = enabled;
            wordDictState[word] = state;
        }
        
        // Gets a JsonDictionary by title
        public JsonDictionary? GetDictionary(string title)
        {
            return loadedJsons.Find(x => x.Title == title);
        }

        // Read all dictionaries from the given path
        public void ReadDictionaries(string path)
        {
            dictPath = path;
            Unload();

            // Index the path and get all json files
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in dirInfo.GetFiles("*.json"))
            {
                // Get path
                var filePath = file.FullName;
                // Add path to loaded json paths
                loadedJsonPaths.Add(filePath);
                // Read json file
                var jsonDict = ReadDictFromJson(file.FullName);
                // Get the title
                var dictTitle = jsonDict.Title;
                // Add to dictionary
                dictNameToPath.Add(dictTitle, filePath);
                
                // Add json dictionary to loaded json dictionaries
                loadedJsons.Add(jsonDict);
                
                // Loop through all words in the json dictionary
                foreach (var word in jsonDict.Data)
                {
                    // Get infos
                    var wordString = word.Key;
                    var state = word.Value.Enabled;
                    
                    // Check if word is already in the dictionary, if so skip
                    if (wordDictState.ContainsKey(wordString)) continue;
                    
                    // Create WordState
                    var wordState = new WordState
                    {
                        Enabled = state,
                        DictName = dictTitle
                    };
                    
                    // Add word to dictionary
                    wordDictState.Add(wordString, wordState);
                }
            }
            
            // Loop through the 
            Loaded = true;
        }
        
        // Unload data
        private void Unload()
        {
            // Clear the loaded dictionaries
            loadedJsons.Clear();
            wordDictState.Clear();
            loadedJsonPaths.Clear();
            dictNameToPath.Clear();
            
            // Set state
            Loaded = false;
        }
        
        /// <summary>
        /// Read a dictionary json file and add it to the list of loaded dictionaries
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static JsonDictionary ReadDictFromJson(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            // Open a stream to the file with share mode read
            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            // Read the json file
            using var reader = new StreamReader(stream);
            // Read the json file to string
            var jsonString = reader.ReadToEnd();
            // Deserialize the json string to JsonDictionary
            return JsonDictionary.FromJson(jsonString);
        }
        
        /// <summary>
        /// Write a dictionary json file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="jsonDictionary"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void WriteDictToJson(string fileName, JsonDictionary jsonDictionary)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (jsonDictionary == null) throw new ArgumentNullException(nameof(jsonDictionary));
            // Serialize the dictionary to json string
            var jsonString = jsonDictionary.ToJson();
            // Write the json string to the file
            System.IO.File.WriteAllText(fileName, jsonString);
        }

        public void SetWordSkipState(string word, bool state)
        {
            if (!wordDictState.ContainsKey(word)) return;
            var wordState = wordDictState[word];
            wordState.Enabled = state;
            wordDictState[word] = wordState;
        }

        // We will save the skip states to text file by line
        public async Task SaveSkipState()
        {
            // Skip if path null
            if (dictPath == null) return;
            
            // Define file path as "SkipWords.res" in the dictionary folder
            var filePath = Path.Combine(dictPath, "SkipWords.res");
            
            // Create the file if it doesn't exist
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);
            }
            
            // Create a list of lines to write
            var lines = (from word in wordDictState 
                where !word.Value.Skipped select word.Key).ToList();
            
            // Write the lines to the file
            await File.WriteAllLinesAsync(filePath, lines);
            
            // Loop through the dictionary
        }

        private void LoadSkipState()
        {
            
        }
    }   
}
