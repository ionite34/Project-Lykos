using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Project_Lykos
{
    public static class DependencyCheck
    {
        private const string DataName = "FonixData.cdf";
        private const string DataChecksum = "4A9D909F712AA82AA28116EE30F42431";
        private static string GetMD5Checksum(string filename)
		{
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static bool FonixDataChecksumOK()
        {
            var path = GetFonixDataPath();
            var hashStr = GetMD5Checksum(path);
            return hashStr == DataChecksum;
        }

        public static bool FonixDataExists()
        {
            return File.Exists(GetFonixDataPath());
        }

        public static bool CheckFonixData()
        {
            return FonixDataExists() && FonixDataChecksumOK();
        }

        public static string GetFonixDataFolder()
        {
            return Directory.GetCurrentDirectory();
        }
        public static string GetFonixDataPath()
        {
            return Path.Combine(GetFonixDataFolder(), DataName);
        }
    }
}
