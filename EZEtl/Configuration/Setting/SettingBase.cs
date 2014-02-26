using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;

namespace EZEtl.Configuration.Settings
{
    public abstract class SettingBase : ConfigurationParentBase
    {
        SettingNameEnum _settingName;
        public SettingNameEnum SettingName { get { return _settingName; } }

        SettingTypeEnum _settingType;
        public SettingTypeEnum SettingType { get { return _settingType; } }

        protected bool _isOptional = false;
        public bool IsOptional { get { return _isOptional; } }

        public SettingBase(IConfigurationParent parent, SettingNameEnum settingName, SettingTypeEnum settingType, bool isOptional   )
            : base ( parent, settingName.ToString())
        {
            _settingName = settingName;
            _settingType = settingType;
            _isOptional  = isOptional;
        }

        protected string _warning = string.Empty;
        protected string _errorMessage = string.Empty;
        protected string _rawValue;

        public void OutputDiagnostics()
        {
            if (!string.IsNullOrWhiteSpace(_errorMessage))
            {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
            }
        }

        protected bool _isPresent = false;
        public bool IsPresent { get { return _isPresent; } }

        protected bool _isValid = false;
        public bool IsValid { get { return (_isOptional || _isPresent ) && _isValid; } }

        protected void SetBadConversionMessage(string toType)
        {
            _errorMessage = @"Invalid value of the setting " + this.SettingName.ToString()
                        + @": Can not convert value [" + _rawValue.Substring(1, Constant.XmlQuoteLength)
                        + (_rawValue.Length > Constant.XmlQuoteLength ? "..." : string.Empty)
                        + @"] to type "
                        + toType;

            _isValid = false;
        }

    }
}
