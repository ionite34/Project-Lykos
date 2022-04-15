using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    public static class Extensions
    {
        /// <summary>
        /// Truncate a string to a maximum length, usually to avoid DB Constraint violations
        /// </summary>
        /// <param name="self">The string to truncate</param>
        /// <param name="maxChars">Maximum number of characters to allow</param>
        /// <returns></returns>
        public static string Truncate(this string self, int maxChars)
        {
            if (self.Trim().Length > maxChars)
                return self.Trim().Substring(0, maxChars);
            else
                return self.Trim();
        }
    }
}
