using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Utilities;

namespace EZEtl.Source
{
    public class SqlClient : Source
    {
        SqlConnection _connection;

        public SqlClient(
              int batchSize
            , string connectionString
            , string command
            , int commandTimeout
            )
            : base()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentNullException("command");

            _connection = new SqlConnection(connectionString);
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = command.Trim();
            if (cmd.CommandText.ToUpperInvariant().StartsWith("SELECT"))
                cmd.CommandType = CommandType.Text;
            else
                cmd.CommandType = CommandType.StoredProcedure;

            _connection.Open();
            base.Reader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
            base.GetMetadata();
        }

        override public void Dispose()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            base.Dispose();
            if (_connection == null)
                return;

            _connection.Close();
            _connection.Dispose();
        }
    }

}
