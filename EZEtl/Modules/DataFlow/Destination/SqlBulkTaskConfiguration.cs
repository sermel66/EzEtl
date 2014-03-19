﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Settings;

namespace EZEtl.Destination
{
    public static class SqlBulkTaskConfiguration
    {
       
        public static void Configure ( TaskConfiguration task )
        {
            task.AddSetting(new Setting<string>(task, SettingNameEnum.ConnectionStringName, SettingTypeEnum.String, false));
    //        task.AddSetting(new Setting<string>(task, SettingNameEnum.Query, SettingTypeEnum.String, false));

            task.SetConstructor(typeof(SqlBulkDestination));

        }

    }
}