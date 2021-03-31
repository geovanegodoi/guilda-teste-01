using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary.Exemplo_1
{
    public class AuditManager_1 : AbstractAuditBase
    {
        private readonly int _maxEntriesPerFile;
        private readonly string _directoryName;

        public AuditManager_1(int maxEntriesPerFile, string directoryName)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
            _directoryName = directoryName;
        }
        
        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            var filePaths = Directory.GetFiles(_directoryName);
            var sorted    = SortByIndex(filePaths);
            var newRecord = visitorName + ';' + timeOfVisit;
            
            if (sorted.Length == 0)
            {
                string newFile = Path.Combine(_directoryName, "audit_1.txt");
                File.WriteAllText(newFile, newRecord);
                return;
            }
            
            (int currentFileIndex, string currentFilePath) = sorted.Last();

            List<string> lines = File.ReadAllLines(currentFilePath).ToList();

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
                File.WriteAllText(newFile, newRecord);
            }
        }
    }
}
