using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.ComponentModel;
using EZEtl.Configuration.Misc;


namespace EZEtl.Configuration.Setting
{
    public abstract class SettingListBase : SettingBase
    {

       List<ISetting> _settings = new List<ISetting>();

       public override object Value { get { return _settings; } }
       public IEnumerable<SettingNameEnum> SettingNameList
       {
           get
           {
               return new List<SettingNameEnum> {this.SettingName};
           }
       }

       public override bool IsValid
       {
           get
           {
               if (!base.IsValid)
                   return false;

               foreach (ISetting entry in _settings)
               {
                   if (!entry.IsValid)
                       return false;
               }
               return true;
           }
       }

       public override void OutputDiagnostics()
       {
           base.OutputDiagnostics();

           foreach (ISetting entry in _settings)
           {
               entry.OutputDiagnostics();
           }
       }


       public override XElement RawValue
       {
            get { return _rawValue; } 
            set {

                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    this.SetError("Empty value not allowed in Nested Setting");
                    return;
                }

                if (_rawValue == null)
                    _rawValue = value;
                else
                _rawValue.Add(value);  // for consistency and debugging....

                ISetting newSettingInstance = this.NewSetting();

                newSettingInstance.RawValue = value;
                _isPresent = true;
                _isValid = true; // only indicates that the list has been initialized, real validity check is done on each element

                _settings.Add(newSettingInstance);
             }
        }

        public abstract ISetting NewSetting();
     


       public SettingListBase(IConfigurationParent parent, SettingNameEnum settingName, int minOccurence, int maxOccurrence)
           : base(parent, settingName, SettingTypeEnum.NestedSetting, minOccurence == 0 )
       {
       }

    }
}
