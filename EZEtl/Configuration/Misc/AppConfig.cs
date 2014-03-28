using System;
using System.Configuration;
using EZEtl.Misc;

namespace EZEtl.Configuration.Misc
{
    public static class AppConfig
    {
        public static string CommonAppConfigFile = string.Empty;

        public static void Load()
        {
            string setting = ConfigurationManager.AppSettings[AppConfigSettingEnum.DbOperationTimeout.ToString()];
            if ( !string.IsNullOrWhiteSpace(setting))
            {
                if (! Int32.TryParse(setting, out Defaults.DbOperationTimeout))
                {
                    Terminate.FatalError("appSetting " + AppConfigSettingEnum.DbOperationTimeout.ToString() + " value [" + setting + "] is not an integer");
                }
            }

            setting = ConfigurationManager.AppSettings[AppConfigSettingEnum.GlobalConfigurationFile.ToString()];
            if (!string.IsNullOrWhiteSpace(setting))
                CommonAppConfigFile = setting;

        }
    }
}
