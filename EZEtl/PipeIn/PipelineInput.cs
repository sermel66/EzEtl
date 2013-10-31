using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using EZEtl.Misc;
using Utilities;

namespace EZEtl.PipeIn
{
    public abstract class PipelineInput : IPipelineIn
    {
        private int _batchSizeRows = 1024;
        private IDataReader _reader;
        protected IDataReader Reader { get { return _reader; } set { _reader = value; } }

        private DataTable _boilerPlateDataTable = new DataTable();
        protected DataTable NewDataTable { get { return _boilerPlateDataTable.Clone(); } }

        public PipelineInput(int batchSize)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            if (batchSize < 1)
                throw new ArgumentOutOfRangeException("batchSize", "must be positive");

            _batchSizeRows = batchSize;
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
                string colTypeName = row[SchemaTableColumnsEnum.DataTypeName.ToString()].ToString();
                col.DataType = (Type) row[SchemaTableColumnsEnum.DataType.ToString()];
                if (colTypeName.Contains( "char" ))
                    col.MaxLength = (int) row[SchemaTableColumnsEnum.ColumnSize.ToString()];

                _boilerPlateDataTable.Columns.Add(col);
            }
        }

        public virtual void Dispose()
        {
            if (_reader == null) return;

            if (! _reader.IsClosed ) _reader.Close();
            _reader.Dispose();
        }
    }
}
