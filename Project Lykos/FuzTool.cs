namespace Project_Lykos;

public class FuzTool
{
    /// <summary>
    /// Uses a xwm and lip stream to create a fuz stream
    /// </summary>
    /// <param name="fuzStream"></param>
    /// <param name="xwmStream"></param>
    /// <param name="lipStream"></param>
    /// <exception cref="Exception"></exception>
    public static void CreateFuz(Stream fuzStream, Stream xwmStream, Stream lipStream)
    {
        var uLipLength = (uint)lipStream.Length;
        try
        {
            using var binaryWriter = new BinaryWriter(fuzStream);
            // Required header
            binaryWriter.Write(1163547974U);
            binaryWriter.Write(1U);
            binaryWriter.Write(uLipLength);
            // Copy lip then xwm
            lipStream.CopyTo(fuzStream);
            xwmStream.CopyTo(fuzStream);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create fuz file: " + ex.Message);
        }
    }
    
    /// <summary>
    /// Uses a xwm stream to create a fuz stream (no lip)
    /// </summary>
    /// <param name="fuzStream"></param>
    /// <param name="xwmStream"></param>
    /// <exception cref="Exception"></exception>
    public static void CreateFuz(Stream fuzStream, Stream xwmStream)
    {
        try
        {
            using var binaryWriter = new BinaryWriter(fuzStream);
            // Required header
            binaryWriter.Write(1163547974U);
            binaryWriter.Write(1U);
            binaryWriter.Write(0U);
            // Copy xwm
            xwmStream.CopyTo(fuzStream);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create fuz file: " + ex.Message);
        }
    }
    
    /// <summary>
    /// Uses a xwm and lip file to create a fuz file
    /// </summary>
    /// <param name="fuzPath"></param>
    /// <param name="xwmPath"></param>
    /// <param name="lipPath"></param>
    public static void CreateFuz(string fuzPath, string xwmPath, string lipPath)
    {
        if (string.IsNullOrEmpty(lipPath)) throw new ArgumentNullException(nameof(lipPath));
        if (string.IsNullOrEmpty(fuzPath)) throw new ArgumentNullException(nameof(fuzPath));
        if (string.IsNullOrEmpty(xwmPath)) throw new ArgumentNullException(nameof(xwmPath));
        
        // Check that the fuz directory exists
        var fuzDir = Path.GetDirectoryName(fuzPath);
        if (string.IsNullOrEmpty(fuzDir))
        {
            throw new ArgumentException(@"Failed to get directory from fuz path");
        }
        if (!Directory.Exists(fuzDir)) Directory.CreateDirectory(fuzDir);

        try
        {
            using var fuzStream = new FileStream(fuzPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            using var xwmStream = new FileStream(xwmPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var lipStream = new FileStream(lipPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            CreateFuz((Stream) fuzStream, (Stream) xwmStream, (Stream) lipStream);
        }
        catch (IOException ex)
        {
            throw new Exception("Failed to write fuz file: " + ex.Message);
        }
    }
}