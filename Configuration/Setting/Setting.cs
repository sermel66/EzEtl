﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace  Configuration.Setting
{
    public class Setting<T> : SettingBase, ISetting
    {
       public const int MaxQuotedValueLengh = 20;
       Type _type;
       public Type ValueType { get { return _type; } }
       TypeConverter _typeConverter;

       T _value;
       public object Value { get { return _value; } }
      
       string  _rawValue;
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
                    _message = @"Invalid value of the key " + _key
                        + @": Can not convert value [" + _rawValue.Substring(1, MaxQuotedValueLengh)
                        + (_rawValue.Length > MaxQuotedValueLengh ? "..." : string.Empty)
                        + @"] to type "
                        + _type.ToString();

                    _isValid = false;
                }           
            } 
        }

  
        public Setting(string key, /*Type type,*/ bool isOptional, params T[] defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            //if ( type == null)
            //    throw new ArgumentNullException("type");

            _key = key;
        //    _type = type;
            _type = typeof(T);
            _isOptional = isOptional;

            if ( defaultValue != null && defaultValue.Length > 0 )
                _value = defaultValue[0];
          
           _typeConverter =TypeDescriptor.GetConverter(_type);
          
        }
        
    }
}
