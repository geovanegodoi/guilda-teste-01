using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary.Exemplo_1
{
    public interface IFileSystem
    {
        string[] GetFiles(string directoryName);
        void WriteAllText(string filePath, string content);
        List<string> ReadAllLines(string filePath);
    }

    public class AuditManager_2 : AbstractAuditBase
    {
        private readonly int _maxEntriesPerFile;
        private readonly string _directoryName;
        private readonly IFileSystem _fileSystem;

        public AuditManager_2(int maxEntriesPerFile, string directoryName, IFileSystem fileSystem)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
            _directoryName = directoryName;
            _fileSystem = fileSystem;
        }

        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            var filePaths = _fileSystem.GetFiles(_directoryName);
            var sorted    = SortByIndex(filePaths);
            var newRecord = visitorName + ';' + timeOfVisit;
            
            if (sorted.Length == 0)
            {
                string newFile = Path.Combine(_directoryName, "audit_1.txt");
                File.WriteAllText(newFile, newRecord);
                return;
            }
            
            (int currentFileIndex, string currentFilePath) = sorted.Last();

            List<string> lines = _fileSystem.ReadAllLines(currentFilePath).ToList();

            if (lines.Count < _maxEntriesPerFile)
            {
                lines.Add(newRecord);
                string newContent = string.Join("\r\n", lines);
                File.WriteAllText(currentFilePath, newContent);
            }
            else
            {
                int newIndex = currentFileIndex + 1;
                string newName = $"audit_{newIndex}.txt";
                string newFile = Path.Combine(_directoryName, newName);
                _fileSystem.WriteAllText(newFile, newRecord);
            }
        }
    }
}
