using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FastDynamic;
using System.Data;
using System.Globalization;
using System.Text;
using ABI.Windows.Media.Playback;
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
        /// <param name="headersToRecord"></param>
        /// <param name="progress"></param>
        /// <param name="progressLines"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DataTable> ReadCsvAsync(string filePath, List<DataColumn> headersToRecord, IProgress<(double current, double total)> progress, IProgress<int>? progressLines = null)
        {
            DataTable dt = new();
            foreach (var header in headersToRecord)
            {
                dt.Columns.Add(header);
            }
            
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };

            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, configuration);

            // var readTask = reader.ReadToEndAsync();

            var readTask = Task.Run(async () =>
            {
                // This will do another tas
                await csv.ReadAsync();
                csv.ReadHeader();
                var linesRead = 0;
                while (csv.Read())
                {
                    var row = dt.NewRow();
                    foreach (var (dataColumn, index) in headersToRecord.Select((column, index) => (column, index)))
                    {
                        var headerName = dataColumn.ColumnName;
                        var T = dataColumn.DataType;

                        if (T == typeof(string))
                        {
                            // If we're in the 'out_path' column, convert all forward slashes to backslashes
                            if (headerName == "out_path")
                            {
                                row[headerName] = csv.GetField<string>(headerName).Replace('/', '\\');
                            }
                            else
                            {
                                row[headerName] = csv.GetField<string>(headerName);
                            }
                        }
                        else if (T == typeof(int))
                        {
                            row[index] = csv.GetField<int>(headerName);
                        }
                        else if (T == typeof(double))
                        {
                            row[index] = csv.GetField<double>(headerName);
                        }
                        else if (T == typeof(DateTime))
                        {
                            row[index] = csv.GetField<DateTime>(headerName);
                        }
                        else
                        {
                            throw new Exception("Unsupported data type");
                        }
                    }
                    
                    // foreach (var header in headersToRecord)
                    // {
                    //     var T = header.DataType;
                    //
                    //     if (T == typeof(string))
                    //     {
                    //         dt.Rows.Add(csv.GetField<string>(header.ColumnName));
                    //     }
                    //     else if (T == typeof(int))
                    //     {
                    //         dt.Rows.Add(csv.GetField<int>(header.ColumnName));
                    //     }
                    //     else if (T == typeof(double))
                    //     {
                    //         dt.Rows.Add(csv.GetField<double>(header.ColumnName));
                    //     }
                    //     else if (T == typeof(DateTime))
                    //     {
                    //         dt.Rows.Add(csv.GetField<DateTime>(header.ColumnName));
                    //     }
                    //     else
                    //     {
                    //         throw new Exception("Unsupported data type");
                    //     }
                    // }
                    
                    dt.Rows.Add(row);
                    linesRead++;
                    progressLines?.Report(linesRead);
                }
                return dt;
            });

            var progressTask = Task.Run(async () =>
            {
                while (stream.Position < stream.Length)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    progress.Report((stream.Position, stream.Length));
                }
            });

            await Task.WhenAll(readTask, progressTask);
            return readTask.Result;
        }

        /// <summary>
        /// Reads CSV Asynchronously with a provided list of headers provided as DataColumns, returns DataTable.
        /// v2 -> uses new native datatable method
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="headersToRecord"></param>
        /// <param name="progress"></param>
        /// <param name="progressLines"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DataTable> ReadCsvAsyncV2(string filePath, List<DataColumn> headersToRecord, IProgress<(double current, double total)> progress)
        {
            DataTable dt = new();
            foreach (var header in headersToRecord)
            {
                dt.Columns.Add(header);
            }
            
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };



            var readTask = Task.Run(async () =>
            {
                await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var csvReader = new CsvReader(reader, configuration);
                var records = csvReader.EnumerateDynamicRecordsAsync();
                await foreach (var line in records)
                {
                    var row = dt.NewRow();
                    foreach (var (dataColumn, index) in headersToRecord.Select((column, index) => (column, index)))
                    {
                        row[index] = line[dataColumn.ColumnName];
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            });
            
            // var progressTask = Task.Run(async () =>
            // {
            //     while ((stream != null) && (stream.Position < stream.Length))
            //     {
            //         await Task.Delay(TimeSpan.FromMilliseconds(100));
            //         progress.Report((stream.Position, stream.Length));
            //     }
            // });

            // await Task.WhenAll(readTask, progressTask);
            await readTask;
            return readTask.Result;
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
            if (!System.IO.File.Exists(filepath))
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
                List<string> header_items = new();
                header_items = lines[0].Split(delimiterChar).ToList();
                if (header_items.Count < 2)
                {
                    throw new Exception("The file does not contain enough columns.");
                }
                // If the required headers of "text" and "out_path" are not found, return the invalid message
                if (!header_items.Contains("text") || !header_items.Contains("out_path"))
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
