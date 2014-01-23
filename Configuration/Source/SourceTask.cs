using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Configuration.Setting;

namespace Configuration.Source
{
    public abstract class SourceTask : Task
    {
        const int BatchSizeRowsDefault = 1024;

        ISetting _expansion;
        public ISetting Setting(SourceSettingEnum sourceSetting)
        {
            if (sourceSetting == SourceSettingEnum.Expansion)
                return _expansion;

            throw new ConfigurationException("Unsupported SourceSettingEnum value " + sourceSetting.ToString());

        }

        public SourceTask(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            ISetting setting;
            foreach (SourceSettingEnum settingName in Enum.GetValues(typeof(SourceSettingEnum)))
            {

                switch (settingName)
                {
                    case SourceSettingEnum.BatchSizeRows:
                        setting = new Setting<int>(settingName.ToString(), false, BatchSizeRowsDefault);
                        break;
                    default:
                        setting = new Setting<string>(settingName.ToString(), false);
                        break;
                }
                this.AddSetting(setting);
            }

        }

        //public override void Parse()
        //{
        //    base.Parse();

        //    if (this.ConfiguredSettings.Contains(SourceSettingEnum.Expansion.ToString()))
        //    {
        //        ISetting expansionSetting = this.Setting(SourceSettingEnum.Expansion.ToString()); // overload taking string must be used here
        //        _expansion = new Expansion(expansionSetting);

        //        this._warnings += "Expansion:" + _expansion.WarningMessage + Constant.UserMessageSentenceDelimiter;

        //        if (!_expansion.IsValid)
        //            this.ErrorCount++;
        //    }
        //}

        protected override void GetErrors()
        {
            if (_expansion.IsValid)
                return;

            _errors += _expansion.ErrorMessage;
        }

    }
}
