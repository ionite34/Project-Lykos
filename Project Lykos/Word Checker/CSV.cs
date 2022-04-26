using System.Data;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Project_Lykos.Word_Checker;

public class CSV
{
    // List of unique text lines in the CSV file
    public List<string> Lines { get; } = new List<string>();

    // State
    public bool Loaded { get; private set; } = false;
    
    // Method to load from CSV file to a unique list of text lines
    public async Task Load(string filePath, string textHeader = "text")
    {
        // Clear the list of lines, set loaded to false
        Lines.Clear();
        Loaded = false;
        
        // Create a new CSV reader and load the file
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            DetectDelimiter = true
        };
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var csv = new CsvReader(reader, configuration);
        await csv.ReadAsync().ConfigureAwait(false); // Starts Asynchronous Read
        csv.ReadHeader(); // Reads header only
        // ReSharper disable once MethodHasAsyncOverload
        while (csv.Read())
        {
            var text = csv.GetField<string>(textHeader); // Text field
            
            // Checks if the text is not null or empty
            if (text == null) continue;
            var line = text.Trim();
            if (string.IsNullOrEmpty(line)) continue;
            
            // Add to list only if not already in the list
            var lineLower = line.ToLower();
            // Check if lowered line matches any of the lines in the list as lower case
            if (Lines.Any(x => x.ToLower() == lineLower)) continue;
            Lines.Add(line);
        }
        
        // Set loaded if the list is not empty
        Loaded = (Lines.Count > 0);
    }
}