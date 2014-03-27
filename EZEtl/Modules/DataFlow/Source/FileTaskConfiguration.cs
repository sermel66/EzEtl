using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Setting;

namespace EZEtl.Source
{
    public static class FileTaskConfiguration
    {
       
        public static void Configure ( TaskConfiguration task )
        {
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.FilePath, SettingTypeEnum.String, false));
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.SchemaIniTemplate, SettingTypeEnum.String, true, string.Empty));
            task.SetConstructor(typeof(File));
        }

    }
}
