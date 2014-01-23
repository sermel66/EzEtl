﻿using Configuration.Setting;
using EZEtl.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using Utilities;
using Configuration.Source;

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

        public SourceBase(Configuration.Task task)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            _batchSizeRows = (int)task.Setting(SourceSettingEnum.BatchSizeRows.ToString()).Value;

            //string batchSizeVariableValue = Configuration.Configuration.VariableValue(Configuration.ReservedVariableEnum.BatchSizeRows);
            //if (!string.IsNullOrWhiteSpace(batchSizeVariableValue))
            //{
            //    bool parseResult = Int32.TryParse(batchSizeVariableValue, out _batchSizeRows);
                
            //     if (!parseResult || _batchSizeRows < 1)
            //     {
            //        throw new Configuration.ConfigurationException(
            //                    "Value of the reserved variable " 
            //                    + Configuration.ReservedVariableEnum.BatchSizeRows.ToString() 
            //                    + " must be a positive integer"
            //                    );
            //     }
            //}

            //ISetting expansionSetting = (ISetting)task.Setting(Configuration.Source.SourceSettingEnum.Expansion.ToString()).Value;
            //if (expansionSetting != null)
            //{
            //    _expansionAttribute = (Dictionary<ExpansionAttributeEnum, string>)expansionSetting.Value;
            //}
        }

        public DataTable ReadBatch()
        {
         //   SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);
            
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