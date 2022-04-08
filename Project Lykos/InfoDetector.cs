namespace Project_Lykos;

internal static class InfoDetector
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
    
    public static long FastCountLines(string filePath)  
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var lineCount = 0L;
        var byteBuffer = new byte[1024 * 1024];
        const int bytesAtTheTime = 4;
        char? detectedEOL = null;
        char? currentChar = null;

        int bytesRead;
        while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
        {
            var i = 0;
            for (; i <= bytesRead - bytesAtTheTime; i += bytesAtTheTime)
            {
                currentChar = (char)byteBuffer[i];

                if (detectedEOL != null)
                {
                    if (currentChar == detectedEOL) lineCount++;

                    currentChar = (char)byteBuffer[i + 1];
                    if (currentChar == detectedEOL) lineCount++;

                    currentChar = (char)byteBuffer[i + 2];
                    if (currentChar == detectedEOL) lineCount++;

                    currentChar = (char)byteBuffer[i + 3];
                    if (currentChar == detectedEOL) lineCount++;
                }
                else
                {
                    if (currentChar is '\n' or '\r')
                    {
                        detectedEOL = currentChar;
                        lineCount++;
                    }
                    i -= bytesAtTheTime - 1;
                }
            }

            for (; i < bytesRead; i++)
            {
                currentChar = (char)byteBuffer[i];

                if (detectedEOL != null)
                {
                    if (currentChar == detectedEOL) { lineCount++; }
                }
                else
                {
                    if (currentChar is not ('\n' or '\r')) continue;
                    detectedEOL = currentChar;
                    lineCount++;
                }
            }
        }

        if (currentChar != '\n' && currentChar != '\r' && currentChar != null)
        {
            lineCount++;
        }
        return lineCount;
    }
}