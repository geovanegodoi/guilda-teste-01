using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary.Exemplo_1
{
    public class FileContent
    {
        public readonly string FileName;
        public readonly string[] Lines;
        public FileContent(string fileName, string[] lines)
        {
            FileName = fileName;
            Lines = lines;
        }
    }

    public class FileUpdate
    {
        public readonly string FileName;
        public readonly string NewContent;
        public FileUpdate(string fileName, string newContent)
        {
            FileName = fileName;
            NewContent = newContent;
        }
    }

    public class AuditManager_3 : AbstractAuditBase
    {
        private readonly int _maxEntriesPerFile;

        public AuditManager_3(int maxEntriesPerFile)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
        }
        
        public FileUpdate AddRecord(FileContent[] files, string visitorName, DateTime timeOfVisit)
        {
            var sorted    = SortByIndex(files);
            var newRecord = visitorName + ';' + timeOfVisit;
            
            if (sorted.Length == 0)
            {
                return new FileUpdate("audit_1.txt", newRecord);
            }
            
            (int currentFileIndex, FileContent currentFile) = sorted.Last();

            List<string> lines = currentFile.Lines.ToList();

            if (lines.Count < _maxEntriesPerFile)
            {
                lines.Add(newRecord);
                string newContent = string.Join("\r\n", lines);
                return new FileUpdate(currentFile.FileName, newContent);
            }
            else
            {
                int newIndex = currentFileIndex + 1;
                string newName = $"audit_{newIndex}.txt";
                return new FileUpdate(newName, newRecord);
            }
        }
    }

    public class Persister
    {
        public FileContent[] ReadDirectory(string directoryName)
        {
            return Directory
            .GetFiles(directoryName)
            .Select(x => new FileContent(
            Path.GetFileName(x),
            File.ReadAllLines(x)))
            .ToArray();
        }
        public void ApplyUpdate(string directoryName, FileUpdate update)
        {
            string filePath = Path.Combine(directoryName, update.FileName);
            File.WriteAllText(filePath, update.NewContent);
        }
    }
}
