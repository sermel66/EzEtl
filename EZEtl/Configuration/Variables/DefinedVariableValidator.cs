using System;
using System.Collections.Generic;
using EZEtl.Configuration.Setting;
using System.Text;
using System.Text.RegularExpressions;

namespace EZEtl.Configuration.Variables
{
    public class DefinedVariableValidator : SettingValidatorBase
    {
        const string BoilerPlate = "Variable reference expected, but supplied value [";

        public DefinedVariableValidator(ISetting setting)
            : base(setting)
        {
            if (setting.SettingType != Configuration.SettingTypeEnum.String)
            {
                throw new EZEtlException("Variable name setting type must be String");
            }
        }

        public override bool Validate()
        {
            if (!_setting.IsPresent)
                return true;

            string settingValue = _setting.Value.ToString();
            _errorMessage = BoilerPlate + settingValue + "] " ;
            string message = string.Empty;

            if (!VariableNameCheck.IsValid(settingValue, out message))
            {
                _errorMessage += message;
                return false;
            }

            if (! VariableNameCheck.IsDefined(settingValue) )
            {
                _errorMessage += "is not defined in Variables section";
                return false;
            }

            _errorMessage = string.Empty;
            return true;
        }

    }
}
