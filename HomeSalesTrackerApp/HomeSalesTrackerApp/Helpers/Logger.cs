using System;
using System.Collections.Generic;
using System.IO;

namespace HomeSalesTrackerApp.Helpers
{
    public class Logger
    {
        private readonly static string LogFileName = "HSTLog.txt";
        private FileInfo LogfileInfo { get; set; }
        private string LogFilePath { get; set; }
        private Queue<string> LogEntries = null;

        public bool IsEnabled { get; set; } = false;

        public Logger()
        {
            string rootDirName = Directory.GetCurrentDirectory();
            DirectoryInfo rootDirectory;
            rootDirectory = new DirectoryInfo(rootDirName);
            LogFilePath = Path.Combine(rootDirectory.FullName, LogFileName);
            LogfileInfo = new FileInfo(LogFilePath);
            LogEntries = new Queue<string>();
            IsEnabled = true;
        }

        public void Data(string name, string info)
        {
            string timeTick = DateTime.Now.ToString();
            string logEntry = $"{ timeTick }\t{ name }\t{ info }\n";
            LogEntries.Enqueue(logEntry);
        }

        public void Flush()
        {
            using (StreamWriter sw = File.AppendText(LogfileInfo.FullName))
            {
                while (LogEntries.Count > 0)
                {
                    sw.WriteLine(LogEntries.Dequeue());
                }
            }
        }

    }
}
