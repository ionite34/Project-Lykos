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
		public static string GetMD5Checksum(string filename)
		{
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static bool ChecksumMatchesFonixData(string filename)
        {
            var hash_str = GetMD5Checksum(filename);
            string expected = "4A9D909F712AA82AA28116EE30F42431";
            return hash_str == expected;
        }

        public static bool CheckFonixData()
        {
            string appDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(appDirectory, "FonixData.cdf");
            // Check file exists first
            if (!File.Exists(path))
            {
                return false;
            }
            // Check Checksum
            return ChecksumMatchesFonixData(path);
        }
    }
}
