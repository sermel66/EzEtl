using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EZEtl.Configuration.Misc;


namespace EZEtl.Configuration.Settings
{
    public class Setting<T> : SettingBase, ISetting
    {
       public const int MaxQuotedValueLengh = 20;
       Type _type;
       public Type ValueType { get { return _type; } }
       TypeConverter _typeConverter;

       T _value;
       public object Value { get { return _value; } }
      
       public string RawValue 
       {
            get { return _rawValue; } 
            set {
                _rawValue = value;
                _isPresent = true;
             
                try
                {
                   _value = (T)_typeConverter.ConvertFromString(_rawValue);
                   _isValid = true;
                }
                catch
                {
                    this.SetBadConversionMessage(_type.ToString());
                }
            } 
        }


       public Setting( IConfigurationParent parent,  SettingNameEnum settingName, SettingTypeEnum settingType, bool isOptional, params T[] defaultValue)
           : base(parent, settingName, settingType, isOptional)
       {
            _type = typeof(T);
            _isOptional = isOptional;

            if (_isOptional )
            {
                if( defaultValue != null && defaultValue.Length > 0)
                {
                    _value = defaultValue[0];
                    _isValid = true;
                }
                else
                {
                    throw new EZEtlException("Setting [" + settingName.ToString() + "] is declared optional, but default value is not provided");
                
                }
            }
           _typeConverter =TypeDescriptor.GetConverter(_type);
          
        }
    }
}
