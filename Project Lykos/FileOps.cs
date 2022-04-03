using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Task<string[]> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }
        
        // Reads a set number of lines in file asyncronously
        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, int linesToRead = -1)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                // If linesToRead is -1 (Default), read the whole file
                while ((line = await reader.ReadLineAsync()) != null && (linesToRead == -1 || lines.Count < linesToRead))
                {
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }
    }
}
