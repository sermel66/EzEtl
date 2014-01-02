using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace EZEtl.Source
{
    public class File : Source
    {
        string _filePathPattern;
        string _schemaIniTemplate=string.Empty;
        string _tempFolder = string.Empty;
        Encoding _encoding = Encoding.Default;
        Encoding _tempFileEncoding = Encoding.ASCII;
        string _expansion = string.Empty;
        

        public File(Configuration.Task task) : base()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            _filePathPattern = (string)task.Setting(Configuration.Source.FileSettingEnum.FilePathPattern.ToString()).Value;
            _schemaIniTemplate = (string)task.Setting(Configuration.Source.FileSettingEnum.SchemaIniTemplate.ToString()).Value;
            _tempFolder = (string)task.Setting(Configuration.Source.FileSettingEnum.TempFolder.ToString()).Value;
            _expansion = (string)task.Setting(Configuration.Source.SourceSettingEnum.Expansion.ToString()).Value;

           // base.Reader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
          //  base.GetMetadata();
        }

        override public void Dispose()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            base.Dispose();
            //if (_connection == null)
            //    return;

       //     _connection.Close();
       //     _connection.Dispose();
        }
    }

}
