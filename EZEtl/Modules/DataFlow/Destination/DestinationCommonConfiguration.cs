using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Settings;
using EZEtl.Configuration.Misc;

namespace EZEtl.Destination
{
    public static class DestinationCommonConfiguration
    {
        public static void Configure(TaskConfiguration task)
        {
            task.AddSetting(new Setting<ExistingDataActionEnum>(task, SettingNameEnum.ExistingDataAction, SettingTypeEnum.Int32, true,
                Defaults.ExistingDataAction));

            task.AddSetting(new Setting<Int32>(task, SettingNameEnum.OneDebugMessagePerBatchCount, SettingTypeEnum.Int32, true,
                Defaults.OneDebugMessagePerBatchCount));
        }
    }
}
