using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

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
        /// <param name="encoding"></param>
        /// <param name="linesToRead"></param>
        /// <returns> List of strings of lines read </returns>
        public static Task<List<string>> ReadLinesAsync(string path, int linesToRead = -1)
        {
            return ReadLinesAsync(path, Encoding.UTF8, linesToRead);
        }

        /// <summary>
        /// Reads a number of lines from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="linesToRead"></param>
        /// <returns> List of strings of lines read </returns>
        public static async Task<List<string>> ReadLinesAsync(string path, Encoding encoding, int linesToRead = -1)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions);
            using var reader = new StreamReader(stream, encoding);
            
            // If linesToRead is -1 (Default), read the whole file
            string? currentLine = await reader.ReadLineAsync();
            while (currentLine != null && (linesToRead == -1 || lines.Count < linesToRead))
            {
                lines.Add(currentLine);                
            }

            return lines;
        }
        
        
        public static async Task<DataTable> ReadCsvAsync(string filePath, List<DataColumn> headersToRecord, IProgress<(double current, double total)> progress, IProgress<int> progressLines)
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
            
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, configuration);

            // var readTask = reader.ReadToEndAsync();

            var readTask = Task.Run(async () =>
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var linesRead = 0;
                while (csv.Read())
                {
                    foreach (var header in headersToRecord)
                    {
                        Type T = header.DataType;

                        if (T == typeof(string))
                        {
                            dt.Rows.Add(csv.GetField<string>(header.ColumnName));
                        }
                        else if (T == typeof(int))
                        {
                            dt.Rows.Add(csv.GetField<int>(header.ColumnName));
                        }
                        else if (T == typeof(double))
                        {
                            dt.Rows.Add(csv.GetField<double>(header.ColumnName));
                        }
                        else if (T == typeof(DateTime))
                        {
                            dt.Rows.Add(csv.GetField<DateTime>(header.ColumnName));
                        }
                        else
                        {
                            throw new Exception("Unsupported data type");
                        }
                    }
                    linesRead++;
                    progressLines.Report(linesRead);
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

            // Check if the file is readable by reading (up to) 15 lines
            // Create arraylist of string for lines read
            List<string> lines = new();
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
            char delimiter_char = CsvSeperatorDetector.DetectSeparator(lines);

            if (delimiter_char == '\0')
            {
                throw new Exception("Unable to detect a valid delimiter in the csv file.");
            }

            // Read the first line of lines into a list of header item strings
            try
            {
                List<string> header_items = new();
                header_items = lines[0].Split(delimiter_char).ToList();
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

            // Everything is okay
            return delimiter_char.ToString();
        } 
    }

    static class CsvSeperatorDetector
    {
        private static readonly char[] SeparatorChars = { ';', '|', '\t', ',' };

        public static char DetectSeparator(string csvFilePath)
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            List<string> lines_list = lines.ToList();
            return DetectSeparator(lines_list);
        }

        public static char DetectSeparator(List<string> lines)
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
