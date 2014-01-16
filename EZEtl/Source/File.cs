using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using Utilities;

namespace EZEtl.Source
{
    public class File : SourceBase
    {
        const string ConnectionStringTemplateName = "GenericFlatFileOleDb";
        const string SupportedProviderName = "System.Data.OleDb";
        const string FolderPlaceHolder = "[FOLDER]";

        OleDbConnection _connection;

        string _filePathPattern;
        string _schemaIniTemplate=string.Empty;
        string _tempFolder = string.Empty;
        Encoding _encoding = Encoding.Default;
        Encoding _tempFileEncoding = Encoding.ASCII;
        Configuration.Setting.ISetting _expansion = null;
        
        public File(Configuration.Task task) : base(task)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            _filePathPattern = (string)task.Setting(Configuration.Source.FileSettingEnum.FilePathPattern.ToString()).Value;
            _schemaIniTemplate = (string)task.Setting(Configuration.Source.FileSettingEnum.SchemaIniTemplate.ToString()).Value;
            _tempFolder = (string)task.Setting(Configuration.Source.FileSettingEnum.TempFolder.ToString()).Value;

            string actualFolder = string.IsNullOrWhiteSpace(_tempFolder) ? System.IO.Path.GetDirectoryName(_filePathPattern) : _tempFolder;
            string actualFileName = System.IO.Path.GetFileName(_filePathPattern); // TODO implement expansion and globs

            System.Configuration.ConnectionStringSettings cs = Configuration.ConnectionString.ConnectionString.GetConnectionString(ConnectionStringTemplateName);
            if (cs == null)
                throw new Configuration.ConfigurationException("Connection string template " + ConnectionStringTemplateName + " is not found in the application .config file");

            if (!cs.ProviderName.Equals(SupportedProviderName))
                throw new Configuration.ConfigurationException("Unexpected connection string " + ConnectionStringTemplateName + " provider: " + cs.ProviderName + "; only " + SupportedProviderName + " is supported");

            string connectionString = cs.ConnectionString.Replace(FolderPlaceHolder, actualFolder);
            Utilities.SimpleLog.ToLog("Connection string: " + connectionString, SimpleLogEventType.Debug);
            string query = "SELECT * FROM " + actualFileName; // TODO consider another option for the column list
            Utilities.SimpleLog.ToLog("Query: " + query, SimpleLogEventType.Debug);

            _connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
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
