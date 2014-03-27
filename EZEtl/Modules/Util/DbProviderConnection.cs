using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data;
using Utilities;
using EZEtl.Configuration;
using EZEtl.Configuration.Setting;
using EZEtl;

namespace EZEtl.Modules.Util
{
    public static class DbProviderConnection
    {
        public static DbConnection Create (ITaskConfiguration taskConfiguration)
        {
            if (taskConfiguration == null)
                throw new ArgumentNullException("taskConfiguration");

            DbConnection connection;

            string connectionStringName = taskConfiguration.GetSetting(SettingNameEnum.ConnectionStringName).Value.ToString();
            System.Configuration.ConnectionStringSettings connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];

            if (connectionString == null)
                throw new EZEtlException("Connection string [" + connectionStringName + "] not found in app.config");

            if (string.IsNullOrWhiteSpace(connectionString.ProviderName))
            {
                throw new EZEtlException("Connection string [" + connectionStringName + "] is missing Provider clause");
            }

            DbProviderFactory factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            connection = factory.CreateConnection();
            connection.ConnectionString = connectionString.ConnectionString;

            return connection;
        }

    }
}
