using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Settings;
using EZEtl.Configuration.Misc;

namespace EZEtl.Source
{
    public static class SourceCommonConfiguration
    {
        public static void Configure(TaskConfiguration task)
        {
            task.AddSetting(new Setting<Int32>(task, SettingNameEnum.OneDebugMessagePerBatchCount, SettingTypeEnum.Int32, true,
                Defaults.OneDebugMessagePerBatchCount));
            task.AddSetting(new Setting<Int32>(task, SettingNameEnum.BatchSizeRows, SettingTypeEnum.Int32, true, Defaults.BatchSizeRows));
        }
    }
}
