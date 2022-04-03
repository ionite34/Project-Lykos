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
    }
}
