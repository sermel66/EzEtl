using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;
using System.Data;

namespace EZEtl.Configuration.Setting
{
    public class StoredProcedureArgumentSettingList: SettingListBase
    {
        public override ISetting NewSetting()
        {
            NestedSetting argumentSetting = new NestedSetting(this, SettingNameEnum.Argument, false);
            argumentSetting.AddSetting(new SimpleSetting<string>(argumentSetting, SettingNameEnum.Name, SettingTypeEnum.String, false), 1);
            argumentSetting.AddSetting(new SimpleSetting<ParameterDirection>(argumentSetting, SettingNameEnum.Direction, SettingTypeEnum.ParameterDirection, false), 1);
            argumentSetting.AddSetting(new SimpleSetting<string>(argumentSetting, SettingNameEnum.Value, SettingTypeEnum.String, false), 1);

            return argumentSetting;
        }

        public StoredProcedureArgumentSettingList(IConfigurationParent parent, SettingNameEnum settingName, int minOccurence, int maxOccurrence)
            : base(parent, settingName, minOccurence, maxOccurrence)
        {
        }
    }
}
