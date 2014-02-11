using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;
using EZEtl.Configuration;


namespace EZEtl.Modules.SqlExec
{
    public class SqlQuerySetting : EZEtl.Configuration.Settings.SettingBase, ISetting
    {
        string _sqlCommand;

        public object Value { get {return null;}} // TODO

        public string RawValue
        {
            get { return _rawValue; }
            set
            {
                _rawValue = value;
                _isPresent = true;

                try
                {
                   // _value = (T)_typeConverter.ConvertFromString(_rawValue);
                    _isValid = true;
                }
                catch
                {
                    this.SetBadConversionMessage("SqlQuerySetting");
                }
            }
        }

        public SqlQuerySetting(IConfigurationParent parent, SettingNameEnum settingName, SettingTypeEnum settingType, bool isOptional)
            : base(parent,settingName,settingType,isOptional)
        {
            //TODO
        }


    }
}
