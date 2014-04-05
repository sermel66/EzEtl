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
            argumentSetting.AddSetting(new SimpleSetting<DbType>(argumentSetting, SettingNameEnum.DbType, SettingTypeEnum.DbType, false), 1);
            argumentSetting.AddSetting(new SimpleSetting<ParameterDirection>(argumentSetting, SettingNameEnum.Direction, SettingTypeEnum.ParameterDirection, false), 1);

          // Optional    
            argumentSetting.AddSetting(new SimpleSetting<int>(argumentSetting, SettingNameEnum.Size, SettingTypeEnum.Int32, true, 1), 1);
            argumentSetting.AddSetting(new SimpleSetting<string>(argumentSetting, SettingNameEnum.InputValue, SettingTypeEnum.String, true, string.Empty), 1);

            SimpleSetting<string> varSetting = new SimpleSetting<string>(argumentSetting, SettingNameEnum.OutputVariable, SettingTypeEnum.String, true, string.Empty);
            varSetting.Validator = new EZEtl.Configuration.Variables.DefinedVariableValidator(varSetting);
            argumentSetting.AddSetting(varSetting, 1);

            return argumentSetting;
        }

        public StoredProcedureArgumentSettingList(IConfigurationParent parent, SettingNameEnum settingName, int minOccurence, int maxOccurrence)
            : base(parent, settingName, minOccurence, maxOccurrence)
        {
        }
    }
}
