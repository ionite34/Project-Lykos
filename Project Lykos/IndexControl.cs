using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    // Most file indexing operations are performed here.
    internal class IndexControl
    {
        // Datatable holding index data.
        public DataTable IndexData { get; set; }

        // State indicator
        public bool Indexed { get; private set; }

        public IndexControl()
        {
            IndexData = new DataTable();
            Indexed = false;
        }

        // Method that indexes files and sets the datatable.
        public void BackgroundIndexFiles(string directory)
        {
            // Avoid blocking the caller and return immediately.
            // await Task.Yield();
            const string searchPattern = "*.wav";
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
            IndexData = dt;
            Indexed = true;
        }
        
        // Method that indexes a directory to the IndexControl
        // Column 1 -> Full path of file
        // Column 2 -> Relative path to file from the selected directory
        public void Index(string indexTargetDirectory)
        {
            const string searchPattern = "*.wav";
            const SearchOption searchOption = SearchOption.AllDirectories;
            
            foreach (var file in Directory.EnumerateFiles(indexTargetDirectory, searchPattern, searchOption))
            {

            }
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

        // Method that indexes a directory and returns a DataTable
        // Column 1 -> Full path of file
        // Column 2 -> Relative path to file from the selected directory
        public static DataTable IndexToDataTable(string path)
        {
            // Avoid blocking the caller for the initial enumerate call.
            // await Task.Yield();
            var searchPattern = "*.wav";
            var searchOption = SearchOption.AllDirectories;

            DataTable dt = new();
            dt.Columns.Add("FullPath", typeof(string));
            dt.Columns.Add("RelativePath", typeof(string));
            foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
            {
                var row = dt.NewRow();
                row[0] = file;
                var fileName = Path.GetFileName(file);
                var originalDirectory = Path.GetFileName(Path.GetDirectoryName(file));
                var relativePath = Path.Join(originalDirectory, fileName);
                row[1] = relativePath;
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static async Task ForEachFileAsync(string path, string searchPattern, SearchOption searchOption, Func<string, Task> doAsync)
        {
            // Avoid blocking the caller for the initial enumerate call.
            await Task.Yield();

            foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
            {
                await doAsync(file);
            }
        }
    }
}
