using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    // Manages static operations relating to building and destroying the temp directory
    public static class Cache
    {
        // The path to the temp directory
        public static readonly string TempDir = Path.GetTempPath();
        public static string FullTempDir => Path.Join(TempDir, "Lykos_Temp");
        public static string WrapDir => Path.Join(FullTempDir, "Wrapper");
        public static string WrapPath => Path.Join(WrapDir, "FaceFXWrapper.exe");
        public static string AudioDir => Path.Join(FullTempDir, "AudioSource");
        public static string AudioResamplingDir => Path.Join(FullTempDir, "ResampleCache");

        // Creates the temp directory
        public static void Create()
        {
            if (Directory.Exists(FullTempDir)) return;
            Directory.CreateDirectory(FullTempDir);
            Directory.CreateDirectory(Path.Join(FullTempDir, "Wrapper"));
            Directory.CreateDirectory(Path.Join(FullTempDir, "Source_Audio"));
        }

        // Deletes the temp directory
        public static void Destroy()
        {
            if (!Directory.Exists(FullTempDir)) return;
            Directory.Delete(FullTempDir, true);
        }

        public static void DeployFaceFX(string relativeTempPath = "Wrapper")
        {
            Create();
            var wrapDir = Path.Join(FullTempDir, relativeTempPath);
            File.WriteAllBytes(wrapDir, Properties.Resources.FaceFXWrapper);
        }

    }
}
