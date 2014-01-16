using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Utilities;

namespace EZEtl.Destination
{
    public abstract class FileDestination : DestinationBase
    {
        string _fqTempFile;
        string _fqTargetFile;

        Stream _outputStream;
        protected Stream OutputStream { get {return _outputStream;}}

        int _bufferSize = 1024 * 1024;

        public FileDestination(Source.ISource source, Configuration.Task task)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (task == null)
                throw new ArgumentNullException("task");

            _fqTargetFile = (string)task.Setting( Configuration.Destination.FileSettingEnum.PathName.ToString() ).Value;
            string targetFileFolder = Path.GetDirectoryName(_fqTargetFile);
            _fqTempFile = System.IO.Path.Combine(targetFileFolder, Path.GetRandomFileName());

            _outputStream = new System.IO.FileStream(_fqTempFile, FileMode.Create, FileAccess.Write, FileShare.None, _bufferSize);
        }
        
        public virtual void Close()
        {
            _outputStream.Close();

            if (File.Exists(_fqTargetFile))
            {
                SimpleLog.ToLog("Deleting pre-existing target file " + _fqTargetFile, SimpleLogEventType.Information);
                File.Delete(_fqTargetFile);
            }

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
