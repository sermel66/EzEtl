using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Setting;

namespace EZEtl.Destination
{
    public static class FileTaskConfiguration
    {
       
        public static void Configure ( TaskConfiguration task )
        {
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.FilePath, SettingTypeEnum.String, false));
    //        task.AddSetting(new Setting<string>(task, SettingNameEnum.Query, SettingTypeEnum.String, false));

          
        }

    }
}
