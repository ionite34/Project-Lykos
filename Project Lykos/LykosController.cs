using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Project_Lykos
{
    
    public class LykosController
    {
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
        public async String SetFilepath_Csv(String filepath)
        {
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
            string[] lines;
            try
            {
                // Read the first 15 lines, until the end of the file              
                System.IO.StreamReader file = new(filepath);
                lines = new string[15];
                for (int i = 0; i < 15; i++)
                {
                    if (!file.EndOfStream && file != null)
                    {
                        lines[i] = file.ReadLine();
                    }
                    else
                    {
                        break;
                    }
                }
                file.Close();
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

            // Detect the delimiter
            Delimiter = "" + CsvSeperatorDetector.DetectSeparator(lines);

            // Predict the delimiter of the CSV file using the first line
            // Parse the CSV for the correct headers
            // We need at least the following headers:
            // 'out_path' and 'text'

        }

        // Static method to detect the delimiter of a CSV file
        public static char DetectDelimiter(String filepath)
        {
            // Create a new StreamReader
            System.IO.StreamReader reader = new System.IO.StreamReader(filepath);

            // Read the first line
            String line = reader.ReadLine();

            // Check if the line is empty
            if (line == "")
            {
                // Return the default delimiter
                return ',';
            }

            // Check if the line contains a comma
            if (line.Contains(","))
            {
                // Return the comma delimiter
                return ',';
            }

            // Check if the line contains a semicolon
            if (line.Contains(";"))
            {
                // Return the semicolon delimiter
                return ';';
            }

            // Return the default delimiter
            return ',';
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

            return q.Separator;
        }
    }
}
