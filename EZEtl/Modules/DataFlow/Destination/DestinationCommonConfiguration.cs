﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Setting;
using EZEtl.Configuration.Misc;

namespace EZEtl.Destination
{
    public static class DestinationCommonConfiguration
    {
        public static void Configure(TaskConfiguration task)
        {
            task.AddSetting(new SimpleSetting<ExistingDataActionEnum>(task, SettingNameEnum.ExistingDataAction, SettingTypeEnum.Int32, true,
                Defaults.ExistingDataAction));

            task.AddSetting(new SimpleSetting<Int32>(task, SettingNameEnum.OneDebugMessagePerBatchCount, SettingTypeEnum.Int32, true,
                Defaults.OneDebugMessagePerBatchCount));
        }
    }
}
