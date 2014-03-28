using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Utilities;
using EZEtl.Configuration;

namespace EZEtl.Destination
{
    public abstract class FileDestination : DestinationBase
    {
        string _fqTempFile;
        string _fqTargetFile;

        Stream _outputStream;
        protected Stream OutputStream { get {return _outputStream;}}

        int _bufferSize = 1024 * 1024;

        public FileDestination(ITaskConfiguration task)
            : base(task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            _fqTargetFile = (string)task.GetSetting(SettingNameEnum.FilePath).Value;
            string targetFileFolder = Path.GetDirectoryName(_fqTargetFile);
            _fqTempFile = System.IO.Path.Combine(targetFileFolder, Path.GetRandomFileName());

            _outputStream = new System.IO.FileStream(_fqTempFile, FileMode.Create, FileAccess.Write, FileShare.None, _bufferSize);
        }

        public override void ExistingDataAction()
        {
            throw new NotImplementedException();
        }


        public override void Close()
        {
           _outputStream.Close();

            if (File.Exists(_fqTargetFile))
            {
                SimpleLog.ToLog("Deleting pre-existing target file " + _fqTargetFile, SimpleLogEventType.Information);
                File.Delete(_fqTargetFile);
            }

            SimpleLog.ToLog("Renaming temp file to the target file " + _fqTargetFile, SimpleLogEventType.Information);
            System.IO.File.Move(_fqTempFile, _fqTargetFile);
            this.Dispose();
        }

        public override void Dispose()
        {
            if (_outputStream != null)
            {
                _outputStream.Dispose();
            }
        }
    }
}
