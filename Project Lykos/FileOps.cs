using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FastDynamic;
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
            var sw = System.Diagnostics.Stopwatch.StartNew();
            DataTable dt = new();
            dt.Columns.Add(@"path", typeof(string));
            dt.Columns.Add(@"sub_folder", typeof(string));
            dt.Columns.Add(@"file_name", typeof(string));
            dt.Columns.Add(@"text", typeof(string));
            
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };

            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, configuration);
            await csv.ReadAsync(); // Starts Asynchronous Read
            csv.ReadHeader(); // Reads header only
            var lastReportTime = DateTime.Now; // Last progress report time
            // ReSharper disable once MethodHasAsyncOverload
            while (csv.Read())
            {
                var row = dt.NewRow();
                var path = csv.GetField<string>(pathHeader);
                row[0] = path.Replace('/', '\\');
                row[1] = Path.GetFileName(Path.GetDirectoryName(path));
                row[2] = Path.GetFileName(path);
                row[3] = csv.GetField<string>(textHeader);
                dt.Rows.Add(row);
                // Report progress if time elapsed more than 370ms
                if (!(DateTime.Now.Subtract(lastReportTime).TotalMilliseconds > 370)) continue;
                progress.Report((stream.Position, stream.Length));
                lastReportTime = DateTime.Now;
            }
            sw.Stop();
            var elapsed = sw.Elapsed.TotalMilliseconds;
            Console.WriteLine(@"[v1] Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);
            return dt;
        }

        // Detects if a target CSV file is valid, is so, returns the delimiter
        // Otherwise throw an exception
        public static async Task<string> ValidateCSV(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                throw new Exception("Filepath cannot be null or empty");
            }

            // Check if the filepath is invalid
            if (!File.Exists(filepath))
            {
                throw new Exception("The file does not exist.");
            }

            // Check if the file is not a CSV file
            if (!filepath.EndsWith(".csv"))
            {
                throw new Exception("The file is not a CSV file.");
            }

            // Check that the file is UTF-8 or ASCII (subset of UTF-8)
            var result = CharsetDetector.DetectFromFile(filepath);
            var resultDetected = result.Detected;
            var encodingName = resultDetected.EncodingName;
            var encoding = resultDetected.Encoding;
            var confidenceLevel = resultDetected.Confidence;

            // Convert float to percentage
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
            var delimiterChar = CSVDelimiterDetector.DetectSeparator(lines);

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
