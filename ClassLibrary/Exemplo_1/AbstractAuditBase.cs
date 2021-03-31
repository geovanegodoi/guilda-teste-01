using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.Exemplo_1
{
    public abstract class AbstractAuditBase
    {
        protected (int, string)[] SortByIndex(string[] filePaths)
        {
            var sortedList = new List<(int, string)>();

            for (int i = 0; i < filePaths.Length; i++)
            {
                var currentFilePath = filePaths[i];
                var index = GetFileIndex(currentFilePath);
                sortedList.Add((index, currentFilePath));
            }
            return sortedList.OrderBy(o => o.Item1).ToArray();
        }

        protected (int, FileContent)[] SortByIndex(FileContent[] filePaths)
        {
            var sortedList = new List<(int, FileContent)>();

            for (int i = 0; i < filePaths.Length; i++)
            {
                var currentFile = filePaths[i];
                var index = GetFileIndex(currentFile.FileName);
                sortedList.Add((index, currentFile));
            }
            return sortedList.OrderBy(o => o.Item1).ToArray();
        }

        protected int GetFileIndex(string filePath)
        {
            var fileName = filePath.Split('\\').LastOrDefault();
            var startIndex = 6;
            var length = fileName.IndexOf('.') - startIndex;
            return Convert.ToInt32(fileName.Substring(startIndex, length));
        }
    }
}
