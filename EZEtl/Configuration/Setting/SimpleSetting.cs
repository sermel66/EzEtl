using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.ComponentModel;
using EZEtl.Configuration.Misc;


namespace EZEtl.Configuration.Setting
{
    public class SimpleSetting<T> : SettingBase, ISetting
    {
       Type _type;
       public Type ValueType { get { return _type; } }
       TypeConverter _typeConverter;

       T _value;
       T _initValue;  // autoinitialized

       public object Value { get { return _value; } }
      
       public XElement RawValue 
       {
            get { return _rawValue; } 
            set {

                _rawValue = value;
                _isPresent = true;
                this.Parse();
            }
        }

       void Parse()
       {
           if (string.IsNullOrWhiteSpace(_rawValue.Value))
               _value = _initValue;
           else
           {
               try
               {
                   _value = (T)_typeConverter.ConvertFromString(_rawValue.Value);
                   _isValid = true;
               }
               catch
               {
                   this.SetBadConversionMessage(_type.ToString());
                   return;
               }
           }

           if (_rawValue.HasAttributes || _rawValue.HasElements)
               this.SetError("Simple settings can not have Attributes or Elements");
       }


       public SimpleSetting( IConfigurationParent parent,  SettingNameEnum settingName, SettingTypeEnum settingType, bool isOptional, params T[] defaultValue)
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
