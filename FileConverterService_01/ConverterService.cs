﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace FileConverterService_01
{
    public class ConverterService
    {
        private FileSystemWatcher _watcher;
        private static readonly LogWriter _log= HostLogger.Get<ConverterService>();

        public bool Start()
        {
            _watcher = new FileSystemWatcher(@"C:\temp\Converter", "*.txt");
            _watcher.Created += FileCreated;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            return true;
        }
        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            _log.InfoFormat("Starting conversion of '{0'}",e.FullPath);
            if (e.FullPath.Contains("bad_in"))
            {
                throw new NotSupportedException("Cannot convert...");
            }

            string content = File.ReadAllText(e.FullPath);
            string upperContent = content.ToUpperInvariant();
            var dir = Path.GetDirectoryName(e.FullPath);
            var convertedFileNmae = Path.GetFileName(e.FullPath) + ".converted";
            var convertedPath = Path.Combine(dir, convertedFileNmae);
            File.WriteAllText(convertedPath, upperContent);
        }
        public void CustomCommand(int commandNumber)
        {
            _log.InfoFormat("Hey I got the command number '{0}'", commandNumber);

        }
        public bool Stop()
        {
            _watcher.Dispose();
            return true;
        }
        public bool Pause()
        {
            _watcher.EnableRaisingEvents = false;
            return true;
        }
        public bool Continue()
        {
            _watcher.EnableRaisingEvents = true;
            return true;
        }
    }
}
