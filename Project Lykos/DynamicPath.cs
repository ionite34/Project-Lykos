namespace Project_Lykos;

public class DynamicPath
{
    public string Path { get; private set; } = "";
    public string ShortPath { get; private set; } = "";
    public string FileName { get; private set; } = "";

    public DynamicPath()
    {
        SetPath("");
    }

    public DynamicPath(string path)
    {
        SetPath(path);
        if (path.Length > 0)
        {
            FileName = Path[(Path.LastIndexOf("\\", StringComparison.Ordinal) + 1)..];
        }
    }

    public bool Exists()
    {
        return string.IsNullOrEmpty(Path);
    }

    public void Clear()
    {
        Path = "";
        ShortPath = "";
        FileName = "";
    }

    public void SetPath(string path)
    {
        SetShortPath(path);
        SetFileName(path);
        Path = path;
    }

    private void SetShortPath(string FullPath)
    {
        if (FullPath.Length <= 0) return;
        var pathTemp = FullPath[(FullPath.LastIndexOf("\\", StringComparison.Ordinal) + 1)..];
        pathTemp = @".\" + pathTemp + @"\";
        ShortPath = pathTemp;
    }

    private void SetFileName(string fullPath)
    {
        if (fullPath.Length <= 0) return;
        var index = (fullPath.LastIndexOf("\\", StringComparison.Ordinal) + 1);
        var pathTemp = fullPath[index..];
        FileName = pathTemp;
    }
}