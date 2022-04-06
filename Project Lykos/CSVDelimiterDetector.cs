namespace Project_Lykos;

internal static class CSVDelimiterDetector
{
    private static readonly char[] SeparatorChars = { ';', '|', '\t', ',' };

    public static char DetectSeparator(string csvFilePath)
    {
        var lines = File.ReadAllLines(csvFilePath);
        var linesList = lines.ToList();
        return DetectSeparator(linesList);
    }

    public static char DetectSeparator(List<string> lines)
    {
        var q = SeparatorChars.Select(sep => new
                { Separator = sep, Found = lines.GroupBy(line => line.Count(ch => ch == sep)) })
            .OrderByDescending(res => res.Found.Count(grp => grp.Key > 0))
            .ThenBy(res => res.Found.Count())
            .First();

        // Default behavior returns '\0' if no separator was found
        return q.Separator;
    }
}