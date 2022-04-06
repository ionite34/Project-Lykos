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
        public static readonly string FullTempDir = Path.Join(Path.GetTempPath(), "Lykos_Temp");

        // Creates the temp directory
        public static void Create()
        {
            if (!Directory.Exists(FullTempDir))
            {
                Directory.CreateDirectory(FullTempDir);
            }
        }

        // Deletes the temp directory
        public static void Destroy()
        {
            if (Directory.Exists(FullTempDir))
            {
                Directory.Delete(FullTempDir, true);
            }
        }

        public static void DeployFaceFX(string relativeTempPath = "Wrapper")
        {
            if (!Directory.Exists(FullTempDir))
            {
                Directory.CreateDirectory(FullTempDir);
            }
            
            var wrapDir = Path.Join(FullTempDir, relativeTempPath);
            // Extract the exe file from the resource
            File.WriteAllBytes(wrapDir, Properties.Resources.FaceFXWrapper);
        }

    }
}
