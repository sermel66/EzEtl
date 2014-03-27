using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.ComponentModel;
using EZEtl.Configuration.Misc;


namespace EZEtl.Configuration.Setting
{
    public class NestedSetting : SettingBase, ISetting
    {
       List<string> _unexpectedSettings              = new List<string>();
       Dictionary<SettingNameEnum, int> _settingCount = new Dictionary<SettingNameEnum, int>();
       Dictionary<SettingNameEnum, int> _settingMaxOccurence = new Dictionary<SettingNameEnum, int>();

       Dictionary<SettingNameEnum, ISetting> _settings = new Dictionary<SettingNameEnum, ISetting>();
       public ISetting GetSetting(SettingNameEnum settingName)
       {
           if (_settings.ContainsKey(settingName))
               return _settings[settingName];

           return null;
       }

       public object Value { get { return _settings; } }

       public IEnumerable<SettingNameEnum> SettingNameList
       {
           get
           {
               return _settings.Keys;
           }
       }

       public override bool IsValid
       {
           get
           {
               if (_errorMessage.Length > 0)
                   return false;

               foreach (KeyValuePair<SettingNameEnum, ISetting> entry in _settings)
               {
                   if (!entry.Value.IsValid)
                       return false;
               }
               return true;
           }
       }

       public override void OutputDiagnostics()
       {
           foreach (KeyValuePair<SettingNameEnum, ISetting> entry in _settings)
           {
               entry.Value.OutputDiagnostics();
           }

           if (_errorMessage.Length > 0)
           {
               Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
           }
       }


       public void AddSetting(ISetting setting, int maxOccurence)
       {
           _settings.Add(setting.SettingName, setting);
           _settingMaxOccurence.Add(setting.SettingName, maxOccurence);
       }

      
       public XElement RawValue
       {
            get { return _rawValue; } 
            set {

                _rawValue = value;
                _isPresent = true;

                if (string.IsNullOrWhiteSpace(_rawValue.Value))
                {
                    this.SetError("Empty value not allowed in Nested Setting");
                    return;
                }
              
                this.Parse();
             }
        }


       public NestedSetting(IConfigurationParent parent, SettingNameEnum settingName, bool isOptional)
           : base(parent, settingName, SettingTypeEnum.NestedSetting, isOptional)
       {
       }

       protected virtual void Parse()
       {

           foreach (XElement item in _rawValue.Elements()) 
           {
               string itemName = item.Name.LocalName;

               SettingNameEnum settingName;
               if (Enum.TryParse<SettingNameEnum>(itemName, out settingName))
               {
                   if (_settings.ContainsKey(settingName))
                   {
                       _settings[settingName].RawValue = item;

                       if (_settingCount.ContainsKey(settingName))
                       {
                           _settingCount[settingName]++;
                       }
                       else
                           _settingCount.Add(settingName, 1);

                       continue;
                   }
               }
               else
               {
                   if (!_unexpectedSettings.Contains(itemName))
                   {
                       _unexpectedSettings.Add(itemName);
                   }
               }
           }
       }
    }
}
