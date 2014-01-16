using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Configuration.ConnectionString
{
    public static class ConnectionString
    {
       static ConnectionStringSettingsCollection _settings = null;

        public static ConnectionStringSettings GetConnectionString(string connectionStringName)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (_settings != null)
            {
                foreach (ConnectionStringSettings cs in _settings)
                {
                    if (cs.Name == connectionStringName)
                        return cs;
                }
            }

            return null;
        }
    }
}
