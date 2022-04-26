using System.Text.RegularExpressions;

namespace Project_Lykos.Word_Checker;

public class FreqCounter
{
    // Count frequency of words in a list of sentences
    public static Dictionary<string, int> Count(List<string> sentences)
    {
        var wordFreq = new Dictionary<string, int>();
        foreach (var sentence in sentences)
        {
            // Using a regex to split the sentence into words
            var wordPattern = new Regex(@"\w+");

            // Split the sentence into words
            foreach (Match match in wordPattern.Matches(sentence))
            {
                wordFreq.TryGetValue(match.Value, out var currentCount);
                wordFreq[match.Value] = currentCount + 1;
            }
        }
        return wordFreq;
    }
}