using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Configuration.Misc;

namespace EZEtl.Configuration.Setting
{
    public abstract class SettingBase : ConfigurationParentBase, ISetting
    {
        SettingNameEnum _settingName;
        public SettingNameEnum SettingName { get { return _settingName; } }

        SettingTypeEnum _settingType;
        public SettingTypeEnum SettingType { get { return _settingType; } }

        protected bool _isOptional = false;
        public bool IsOptional { get { return _isOptional; } }

        ISettingValidator _validator = new DummyValidator();
        public ISettingValidator Validator { set { _validator = value; } }

        public virtual object Value { get { throw new NotImplementedException(); } }
        public virtual XElement RawValue { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

        public SettingBase(IConfigurationParent parent, SettingNameEnum settingName, SettingTypeEnum settingType, bool isOptional   )
            : base ( parent, settingName.ToString())
        {
            _settingName = settingName;
            _settingType = settingType;
            _isOptional  = isOptional;
        }

       // protected string _warning = string.Empty;
        List<string> _errorMessageList = new List<string>();
        protected XElement _rawValue;

        public virtual void OutputDiagnostics()
        {
            if ( ! (_isOptional || _isPresent))
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, "Required setting is not provided");

           foreach(string errorMessage in _errorMessageList)
           {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, errorMessage);
            }

            if (!_validator.Validate())
            {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _validator.ErrorMessage);
            }
        }

        protected bool _isPresent = false;
        public bool IsPresent { get { return _isPresent; } }

        protected bool _isValid = false;
        public virtual bool IsValid
        {
            get
            {
                return
                    (_isOptional || _isPresent)
                    && _isValid
                    && _validator.Validate();
        } }

        protected void SetBadConversionMessage(string toType)
        {
            _errorMessageList.Add( @"Invalid value of the setting " + this.SettingName.ToString()
                        + @": Can not convert value " + XmlUtil.Quot(_rawValue.Value) +" to type " + toType);

            _isValid = false;
        }

        protected void SetError(string errorMessage)
        {
            _errorMessageList.Add( @"Error """ + errorMessage + @""" in setting " + this.SettingName.ToString()
                        + XmlUtil.Quot(_rawValue.ToString())
                        );

            _isValid = false;
        }
    }
}
