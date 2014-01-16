using EZEtl.Source;
using System;
using System.Data;
using System.IO;
using System.Text;
using Utilities;


namespace EZEtl.Destination
{
    public class FileCsv : FileDestination
    {

        string _delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        string _textQualifier = @"""";
        private string _textQualifierSubstitute;

        bool _isHeaderOutput = false;
        StreamWriter _sw;
        System.Text.Encoding _outputEncoding = Encoding.Default;
              

        public FileCsv (
             ISource source
            ,Configuration.Task task
            )
            : base(source, task)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);
            
            if (string.IsNullOrEmpty((string)task.Setting( Configuration.Destination.FileSettingEnum.Delimiter.ToString() ).Value))
                _delimiter = (string)task.Setting(Configuration.Destination.FileSettingEnum.Delimiter.ToString()).Value;
            
            if (string.IsNullOrEmpty((string)task.Setting(Configuration.Destination.FileSettingEnum.TextQualifier.ToString()).Value))
                _textQualifier = (string)task.Setting(Configuration.Destination.FileSettingEnum.TextQualifier.ToString()).Value;
             
            _textQualifierSubstitute = _textQualifier + _textQualifier;

            _sw = new StreamWriter(base.OutputStream, _outputEncoding);

        }

        public override void WriteTableChunk(DataTable tableChunk)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            if (tableChunk == null)
                throw new ArgumentNullException("tableChunk");

            SimpleLog.ToLog("Row count=" + tableChunk.Rows.Count.ToString() , SimpleLogEventType.Debug);

            if ( ! _isHeaderOutput )
            {
                bool isFirstColumn = true;
                foreach (DataColumn col in tableChunk.Columns)
                {
                   _sw.Write(FieldFormat(col.ColumnName, isFirstColumn));
                   isFirstColumn = false;
                }
                _sw.WriteLine();
                _isHeaderOutput = true;
            }

            foreach (DataRow row in tableChunk.Rows)
            {
                bool isFirstColumn = true;
                foreach (DataColumn col in tableChunk.Columns)
                {
                    _sw.Write(FieldFormat(row[col].ToString(), isFirstColumn));
                    isFirstColumn = false;
                }
                _sw.WriteLine();
            }
        }

        private string FieldFormat(string field, bool isFirst)
        {
            bool needToWrap = field.Contains(_delimiter) || char.IsWhiteSpace(field[0]) || char.IsWhiteSpace(field[field.Length - 1]);

            return
                 (isFirst ? string.Empty : _delimiter)
               + (needToWrap ? _textQualifier : string.Empty)
               + (needToWrap ? field.Replace(_textQualifier, _textQualifierSubstitute) : field )
               + (needToWrap ? _textQualifier : string.Empty)
               ;
                
        }

    }
}
