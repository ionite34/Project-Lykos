﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    // Most file indexing operations are performed here.
    public class IndexControl
    {
        public DataTable IndexData { get; set; }
        public bool Indexed { get; private set; }
        private readonly string pattern;

        public IndexControl(string searchPattern)
        {
            pattern = searchPattern;
            IndexData = new DataTable();
            Indexed = false;
        }

        // Method that indexes files and sets the datatable.
        public void BackgroundIndexFiles(string directory)
        {
            IndexData = IndexToDataTable(directory, pattern);
            Indexed = true;
        }

        // Indexes a directory to a datatable.
        // Column 1 -> Relative path to file from the selected directory
        // Column 2 -> Full path of file
        private static DataTable IndexToDataTable(string directory, string searchPattern)
        {
            const SearchOption searchOption = SearchOption.AllDirectories;
            DataTable dt = new();
            dt.Columns.Add("RelativePath", typeof(string));
            dt.Columns.Add("FullPath", typeof(string));
            // Set the first column as the primary key.
            dt.PrimaryKey = new[] { dt.Columns[0] };
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
            {
                var row = dt.NewRow();
                var fileName = Path.GetFileName(file);
                var originalDirectory = Path.GetFileName(Path.GetDirectoryName(file));
                var relativePath = Path.Join(originalDirectory, fileName);
                row[0] = relativePath;
                row[1] = file;
                // Add row to table.
                dt.Rows.Add(row);
            }
            return dt;
        }
        
        // Method that checks to ensure the provided directory is at most 1 layer deep.
        // Meaning that it can contain subfolders, but not sub-subfolders.
        // We will scan the target directory recursively 
        public static bool DirectoryLayerCheck(string directoryPath)
        {
            var dirInfo = new DirectoryInfo(directoryPath);

            var directories = dirInfo.EnumerateDirectories("*", new EnumerationOptions
                { RecurseSubdirectories = true });

            // Return false if any subdirectories are found within the subdirectories.
            return directories.All(dir => dir.GetDirectories().Length <= 0);
        }
    }
}
