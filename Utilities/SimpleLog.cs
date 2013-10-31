using System;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Utilities
{
    public static class SimpleLog
    {
        const string BackupExtension = ".bak";
        const string LogFormat = "{0}:{1}:{2}";
        const bool CopyToConsole = true;

        static bool _withThreading = false;

        static string _fqLogFilePath = string.Empty;
        public static bool IsInitialized { get { return !string.IsNullOrWhiteSpace(_fqLogFilePath); } }
        static SimpleLogEventType _maxLoggingVerbosity = SimpleLogEventType.Information; // also the default verbosity if one is not provided

        public static void SetUpLog(string fqLogFilePath, SimpleLogEventType maxLoggingVerbosity, bool withThreading)
        {
            if (string.IsNullOrWhiteSpace(fqLogFilePath))
                throw new ArgumentNullException("fqLogFilePath");

            _fqLogFilePath = fqLogFilePath;
            _maxLoggingVerbosity = maxLoggingVerbosity;
            _withThreading = withThreading;

            if (File.Exists(_fqLogFilePath))
            {
                string backFilePath = _fqLogFilePath + BackupExtension;
                if (File.Exists(backFilePath))
                {
                    File.Delete(backFilePath);
                }
                File.Move(_fqLogFilePath, backFilePath);
            }
        }

        public static void ToLog(string message, SimpleLogEventType entryType)
        {
            if (!IsInitialized)
                return;

            if (entryType > _maxLoggingVerbosity)
                return;

            string fqPath = _fqLogFilePath + (_withThreading ? "." + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() : string.Empty);

            using (StreamWriter sw = File.AppendText(fqPath))
            {
                sw.WriteLine(
                    string.Format(LogFormat,
                     DateTime.UtcNow.ToString("o"),
                     entryType,
                     message)
                );

                sw.Flush();

            }
        }



    }
}
