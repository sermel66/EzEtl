﻿using System;
using System.Collections.Generic;
using EZEtl.PipeIn;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Utilities;


namespace EZEtl.PipeOut
{
    public class FileCsvOutput : FileOutput
    {
    
        string _delimiter;

        string _textQualifier;
        private string _textQualifierSubstitute;

        bool _isHeaderOutput = false;
        StreamWriter _sw;
        System.Text.Encoding _outputEncoding = Encoding.Default;
              

        public FileCsvOutput (
             IPipelineIn inputModule
            ,string fqOutputPath
            ,string delimiter
            ,string textQualifier
            ) : base ( inputModule, fqOutputPath )
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);
            
            if (string.IsNullOrEmpty(delimiter))
                throw new System.ArgumentNullException("delimiter");
         
            _delimiter = delimiter;
            _textQualifier = textQualifier;
            _textQualifierSubstitute = textQualifier + textQualifier;

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

        //public override void Dispose()
        //{
          

        //    base.Dispose();
        //}
    }
}
