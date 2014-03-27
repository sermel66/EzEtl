﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Setting;

namespace EZEtl.Source
{
    public static class SqlTaskConfiguration
    {
       
        public static void Configure ( TaskConfiguration task )
        {
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.ConnectionStringName, SettingTypeEnum.String, false));
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.Query, SettingTypeEnum.String, false));
            task.AddSetting(new SimpleSetting<Int32>(task, SettingNameEnum.DbOperationTimeout, SettingTypeEnum.Int32, true, EZEtl.Configuration.Misc.Defaults.DbOperationTimeout));

            task.SetConstructor(typeof(SqlClient));

        }

    }
}
