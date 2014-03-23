//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Data.SqlClient;
//using System.Data;
//using Utilities;
//using EZEtl.Configuration;
//using EZEtl.Configuration.Settings;
//using EZEtl;

//namespace EZEtl.Source
//{
//    public class SqlClient : SourceBase
//    {
    
//        SqlConnection _connection;

//        public SqlClient(ITaskConfiguration task) : base(task)
//        {
//            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

//            string connectionStringName = task.GetSetting(SettingNameEnum.ConnectionStringName).Value.ToString();
//            System.Configuration.ConnectionStringSettings connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
//            if ( connectionString == null)
//                throw new EZEtlException ("Connection string [" + connectionStringName + "] not found in app.config" );

//            string sqlCommand = task.GetSetting(SettingNameEnum.Query).Value.ToString();

//            _connection = new SqlConnection(connectionString.ConnectionString);
//            SqlCommand cmd = _connection.CreateCommand();
//            cmd.CommandText = sqlCommand.Trim();
//            if (cmd.CommandText.ToUpperInvariant().StartsWith("SELECT"))
//                cmd.CommandType = CommandType.Text;
//            else
//                cmd.CommandType = CommandType.StoredProcedure;

//            _connection.Open();
//            base.Reader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
//            base.GetMetadata();
//        }

//        override public void Dispose()
//        {
//            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

//            base.Dispose();
//            if (_connection == null)
//                return;

//            _connection.Close();
//            _connection.Dispose();
//        }
//    }

//}
