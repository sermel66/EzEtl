﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Source;
using System.Data.SqlClient;
using System.Configuration;
using Utilities;

namespace EZEtl.Destination
{
    public class SqlBulkDestination : DestinationBase
    {
        SqlConnection   _connection;
        SqlTransaction  _transaction;
        SqlBulkCopy _sbc;

        public SqlBulkDestination(ISource source, Configuration.Task task)
            : base(source, task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            string connectionStringName = (string)task.Setting(Configuration.Destination.SqlBulkSettingEnum.ConnectionString.ToString()).Value;

            System.Configuration.ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new Configuration.ConfigurationException("Connection string [" + connectionStringName + "] is not found in the application .config file");

            _connection = new SqlConnection(cs.ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
              
            _sbc = new SqlBulkCopy(_connection,SqlBulkCopyOptions.TableLock,_transaction);
            _sbc.DestinationTableName = (string)task.Setting(Configuration.Destination.DatabaseSettingEnum.TableName.ToString()).Value;
            _sbc.BulkCopyTimeout = (int)task.Setting(Configuration.Destination.DatabaseSettingEnum.DbOperationTimeout.ToString()).Value;
        
        }


        public override void WriteTableChunk(System.Data.DataTable tableChunk)
        {
           // SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

           _sbc.WriteToServer(tableChunk);
        }


        public override void Close()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            _transaction.Commit();
            _sbc.Close();            
        }



        public override void Dispose()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            if (_transaction != null)
                _transaction.Dispose();

            if (_connection != null )
                _connection.Dispose();
        
        }
    }
}