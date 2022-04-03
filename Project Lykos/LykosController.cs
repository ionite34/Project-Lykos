using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Project_Lykos
{
    using static FileOps;
    
    public class LykosController
    {
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        // Variables
        public String Filepath_Source { get; set; } = "";
        public String Filepath_Output { get; set; } = "";
        public String Filepath_Csv { get; set; } = "";
        public String Delimiter { get; set; } = "";

        // Constructor
        public LykosController()
        {
            // Initialize the LykosController
            // Initialize();
        }

        // Set the Filepath_Source, if valid path, else return false
        public bool SetFilepath_Source(String filepath)
        {
            // Check if the filepath is valid
            if (System.IO.File.Exists(filepath))
            {
                // Set the Filepath_Source
                Filepath_Source = filepath;
                return true;
            }
            else
            {
                // Return false
                return false;
            }
        }

        // Set the Filepath_Output, if valid path, else return false
        public bool SetFilepath_Output(String filepath)
        {
            // Check if the filepath is valid
            if (System.IO.File.Exists(filepath))
            {
                // Set the Filepath_Output
                Filepath_Output = filepath;
                return true;
            }
            else
            {
                // Return false
                return false;
            }
        }

        /*
         * Checks if the CSV file exists, then set the Filepath_Csv
         * Returns "0" if valid, or a String of invalid message
         */
        public async Task<string> SetFilepathCsvAsync(string filepath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return "Filepath cannot be null or empty";
            }

            if (encoding is null)
            {
                return "Runtime Error: Encoding cannot be null";
            }
            
            // Check if the filepath is invalid
            if (!System.IO.File.Exists(filepath))
            {
                // Return the invalid message
                return "The file does not exist.";
            }

            // Check if the file is not a CSV file
            if (!filepath.EndsWith(".csv"))
            {
                // Return the invalid message
                return "The file is not a CSV file.";
            }

            // Check if the file is readable by reading (up to) 15 lines
            // Create arraylist of string for lines read
            List<string> lines = new();
            try
            {
                // Using FileOps
            }
            catch (Exception)
            {
                // Return the invalid message
                return "The file is not readable.";
            }
            

            // Check that we have at least 2 lines
            if (lines.Length < 2)
            {
                // Return the invalid message
                return "The csv file requires a minimum of 2 lines.";
            }

            // Predict the delimiter
            char delimiter_char = CsvSeperatorDetector.DetectSeparator(lines);
            
            if (delimiter_char == '\0')
            {
                // Return the invalid message
                return "Unable to detect a valid delimiter in the csv file.";
            }

            Delimiter = "" + delimiter_char;

            // If everything is okay, set the Filepath_Csv
            Filepath_Csv = filepath;
        }

        // Check if the folder path exists
        private static bool CheckFolderPathExists(String path)
        {
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if the file path exists
        private static bool CheckFilePathExists(String path)
        {
            // Check if the file path exists
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    static class CsvSeperatorDetector
    {
        private static readonly char[] SeparatorChars = { ';', '|', '\t', ',' };

        public static char DetectSeparator(string csvFilePath)
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            return DetectSeparator(lines);
        }

        public static char DetectSeparator(string[] lines)
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
}
