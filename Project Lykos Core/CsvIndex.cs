using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    struct CsvIndex
    {
        public string fullPath;
        public string subFolder;
        public string fileName;
        public string text;
        public Dictionary<string, string> DictionaryPathText = new();
        
        public CsvIndex(string fullPath, string subFolder, string fileName, string text)
        {
            this.fullPath = fullPath;
            this.subFolder = subFolder;
            this.fileName = fileName;
            this.text = text;
        }
    }
}
