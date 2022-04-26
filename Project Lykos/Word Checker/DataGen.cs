using System.Data;

namespace Project_Lykos.Word_Checker;

public class DataGen
{
    // Word Frequency datatable
    public DataTable WordFreq { get; } = new DataTable();

    // Word Usage datatable
    public DataTable WordUsage { get; }  = new DataTable();
    
    // List of words to skip from displaying
    public List<string> SkipWords { get; set; } = new List<string>();

    // Reference to dict reader
    private DictReader dictReader;
    
    // Constructor
    public DataGen(DictReader dictReader)
    {
        this.dictReader = dictReader;
        var col = WordFreq.Columns;
        // Create Word Frequency table
        col.Add("Word", typeof(string));
        col.Add("Freq", typeof(int));
        col.Add("Length", typeof(int));
        col.Add("In Dict", typeof(string));
        col.Add("Enabled", typeof(bool));
        col.Add("Skipped", typeof(bool));

        // Create Word Usage table
        WordUsage.Columns.Add("Length", typeof(int));
        WordUsage.Columns.Add("Line", typeof(string));
    }
    
    // Load Word Frequency Data Table using list of sentences
    public void LoadFreq(List<string> lines)
    {
        // Clear Word Frequency table
        WordFreq.Clear();
        // Get dictionary of words and their frequencies
        var dict = FreqCounter.Count(lines);
        // Loop through dictionary and add to table
        foreach (var item in dict)
        {
            // Get the word
            var word = item.Key;
            var freq = item.Value;
            // Build the row
            var row = WordFreq.NewRow();
            row["Word"] = word;
            row["Freq"] = freq;
            row["Length"] = word.Length;
            var inDict = dictReader.IsWord(word);
            row["In Dict"] = inDict;
            var enabledState = inDict && dictReader.IsWordEnabled(word);
            row["Enabled"] = enabledState;
            row["Skipped"] = SkipWords.Contains(word);
            // Add row to table
            WordFreq.Rows.Add(row);
        }
    }
    
}