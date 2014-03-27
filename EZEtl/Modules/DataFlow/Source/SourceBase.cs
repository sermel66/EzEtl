using EZEtl.Configuration.Setting;
using EZEtl.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using Utilities;
using EZEtl.Configuration;

namespace EZEtl.Source
{
    public abstract class SourceBase : ISource
    {
        private int _batchSizeRows = 1024;
        private IDataReader _reader;
        protected IDataReader Reader { get { return _reader; } set { _reader = value; } }

        private DataTable _boilerPlateDataTable = new DataTable();
        protected DataTable NewDataTable { get { return _boilerPlateDataTable.Clone(); } }
   //     protected Dictionary<ExpansionAttributeEnum, string> _expansionAttribute;

        public SourceBase(ITaskConfiguration task)
        {
            _batchSizeRows = (int)(task.GetSetting(SettingNameEnum.BatchSizeRows).Value);
        }

        public DataTable ReadBatch()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);
            
            DataTable batchTable = NewDataTable;
            int rowCount = 0;
            batchTable.BeginLoadData();
            while (_reader.Read() && rowCount < _batchSizeRows )
            {
                Object[] values = new object[_reader.FieldCount];
                int fieldCount = Reader.GetValues(values);
                batchTable.LoadDataRow(values, true);
                rowCount++;
            }
            batchTable.EndLoadData();

            return batchTable;
        }

        protected virtual void GetMetadata()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            DataTable metaDataTable = _reader.GetSchemaTable();

            foreach (DataRow row in metaDataTable.Rows)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = row[SchemaTableColumnsEnum.ColumnName.ToString()].ToString();
             //   string colTypeName = row[SchemaTableColumnsEnum.DataTypeName.ToString()].ToString();
                col.DataType = (Type) row[SchemaTableColumnsEnum.DataType.ToString()];
                string colTypeName = col.DataType.ToString();
                if (colTypeName.Contains( "String" ))
                    col.MaxLength = (int) row[SchemaTableColumnsEnum.ColumnSize.ToString()];

                _boilerPlateDataTable.Columns.Add(col);
            }
        }

        protected virtual bool TryExpansion(string stringToExpand, out string expandedString)
        {
            bool succceeded = false;

            expandedString = stringToExpand;
            return succceeded;
        }

        public virtual void Dispose()
        {
            if (_reader == null) return;

            if (! _reader.IsClosed ) _reader.Close();
            _reader.Dispose();
        }
    }
}
