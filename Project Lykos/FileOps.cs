using CsvHelper;
using CsvHelper.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using UtfUnknown;

namespace Project_Lykos
{
    public static class FileOps
    {
        /// <summary>
        /// This is the same default buffer size as
        /// <see cref="StreamReader"/> and <see cref="FileStream"/>.
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        /// <summary>
        /// Reads a number of lines from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="linesToRead"></param>
        /// <returns> List of strings of lines read </returns>
        private static async Task<List<string>> ReadLinesAsync(string path, int linesToRead = -1)
        {
            return await ReadLinesAsync(path, Encoding.UTF8, linesToRead);
        }

        /// <summary>
        /// Reads a number of lines from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="linesToRead"></param>
        /// <returns> List of strings of lines read </returns>
        private static async Task<List<string>> ReadLinesAsync(string path, Encoding encoding, int linesToRead = -1)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions);
            using var reader = new StreamReader(stream, encoding);
            
            // If linesToRead is -1 (Default), read the whole file
            var currentLine = await reader.ReadLineAsync();
            while (currentLine != null && (linesToRead == -1 || lines.Count < linesToRead))
            {
                lines.Add(currentLine);                
            }

            return lines;
        }


        /// <summary>
        /// Reads CSV Asynchronously with a provided list of headers provided as DataColumns, returns DataTable.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="pathHeader"></param>
        /// <param name="textHeader"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DataTable> ReadCsvAsync(string filePath, string pathHeader, string textHeader, IProgress<(double current, double total)> progress)
        {
            // Start a timer with System.Diagnostics
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            DataTable dt = new();
            dt.Columns.Add(@"path", typeof(string));
            dt.Columns.Add(@"relative_path", typeof(string));
            dt.Columns.Add(@"text", typeof(string));
            
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };
            
            // Count lines in file
            var fileTotalLines = InfoDetector.FastCountLines(filePath);
            
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, configuration);
            await csv.ReadAsync().ConfigureAwait(false); // Starts Asynchronous Read
            csv.ReadHeader(); // Reads header only
            var lastReportTime = DateTime.Now; // Last progress report time
            var lastLine = 0; // Current line number
            // ReSharper disable once MethodHasAsyncOverload
            while (csv.Read())
            {
                lastLine++;
                var row = dt.NewRow();
                var path = csv.GetField<string>(pathHeader).Replace('/', '\\');
                var folder = Path.GetFileName(Path.GetDirectoryName(path)); // Relative Path
                if (folder == null) throw new Exception($"Folder is null for path: {path}");
                var file = Path.GetFileName(path);
                row[0] = path; // Full Path
                row[1] = Path.Combine(folder, file); // Relative Path
                row[2] = csv.GetField<string>(textHeader); // Text
                dt.Rows.Add(row);
                // Report progress if time elapsed more than 95ms
                if (DateTime.Now.Subtract(lastReportTime).TotalMilliseconds < 19) continue;
                progress.Report((lastLine, fileTotalLines));
                lastReportTime = DateTime.Now;
            }
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            return dt;
        }
        
        /// <summary>
        /// Detect the validity of a csv file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<string> ValidateCSV(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new Exception("Filepath cannot be null or empty");

            // Check if the filepath is invalid
            if (!File.Exists(filepath))
                throw new Exception("The file does not exist.");

            // Check if the file is not a CSV file
            if (!filepath.EndsWith(".csv"))
                throw new Exception("The file is not a CSV file.");

            // Check file is not empty
            if (new FileInfo(filepath).Length == 0)
                throw new Exception("The file is empty.");
            
            // Check that the file is UTF-8 or ASCII (subset of UTF-8)
            var result = CharsetDetector.DetectFromFile(filepath);
            if (result.Detected == null) throw new Exception("No valid encoding was found.");
            var resultDetected = result.Detected;
            var encodingName = resultDetected.EncodingName;
            var encoding = resultDetected.Encoding;
            var confidenceLevel = resultDetected.Confidence;
            var percent = confidenceLevel.ToString("0%");

            if (!Equals(encoding, Encoding.ASCII) && !Equals(encoding, Encoding.UTF8) && confidenceLevel > 0.5)
            {
                var text = @"The selected CSV file was detected to be in a " + encodingName.ToUpper() +
                           " encoded format with " + percent + " confidence. " +
                           "Only the UTF-8 and ASCII formats are officially supported. " +
                           "You may choose to ignore this warning and attempt to read the file anyway, but the extracted text lines may be incorrect or corrupt. " +
                           "Select Yes to continue reading the file";
                const string title = @"Encoding Format Warning";
                var dialogResult = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.No) return "0";
            }

            // Check if the file is readable by reading (up to) 15 lines
            // Create array list of string for lines read
            List<string> lines;
            try
            {
                // Using FileOps
                lines = await ReadLinesAsync(filepath, 15);
            }
            catch (Exception)
            {
                throw new Exception("The file is not readable");
            }

            // Check that we have at least 2 lines
            if (lines.Count < 2)
            {
                throw new Exception("The file does not contain enough lines.");
            }

            // Predict the delimiter
            var delimiterChar = InfoDetector.DetectSeparator(lines);

            if (delimiterChar == '\0')
            {
                throw new Exception("Unable to detect a valid delimiter in the csv file.");
            }

            // Read the first line of lines into a list of header item strings
            try
            {
                var headerItems = lines[0].Split(delimiterChar).ToList();
                if (headerItems.Count < 2)
                {
                    throw new Exception("The file does not contain enough columns.");
                }
                // If the required headers of "text" and "out_path" are not found, return the invalid message
                if (!headerItems.Contains("text") || !headerItems.Contains("out_path"))
                {
                    throw new Exception("The file does not contain the required columns: 'text' and 'out_path'.");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error attempting to read header, please check the file format.");
            }
            return delimiterChar.ToString();
        } 
    }
}
