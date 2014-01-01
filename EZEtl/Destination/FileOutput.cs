using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using EZEtl.Source;
using System.Threading.Tasks;
using Utilities;

namespace EZEtl.Destination
{
    public abstract class FileOutput : PipelineDestination
    {
        string _fqTempFile;
        string _fqTargetFile;

        Stream _outputStream;
        protected Stream OutputStream { get {return _outputStream;}}

        int _bufferSize = 1024 * 1024;

        public FileOutput(ISource inputModule, string fqTargetFileName)
            : base(inputModule)
        {
            if (string.IsNullOrWhiteSpace(fqTargetFileName))
                throw new ArgumentNullException("fqTargetFileName");

            _fqTargetFile = fqTargetFileName;
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
