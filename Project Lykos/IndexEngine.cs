using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    internal class IndexEngine
    {
        // Most file indexing operations are performed here.


        // Method that checks to ensure the provided directory is at most 1 layer deep.
        // Meaning that it can contain subfolders, but not sub-subfolders.
        // We will scan the target directory recursively 
        public static bool IsValidDirectory(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);

            var dirs = dirInfo.EnumerateDirectories("*", new EnumerationOptions
                { RecurseSubdirectories = true });

            foreach (var name in dirs)
            {
                Console.WriteLine(name);
            }
            return true;
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
                row[1] = file.Substring(path.Length);
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
