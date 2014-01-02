using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Configuration.Setting;

namespace Configuration.Destination
{
    public class File : Destination
    {
        public File(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            foreach (FileSettingEnum settingName in Enum.GetValues(typeof(FileSettingEnum)))
            {
                ISetting setting = new Setting<string>(settingName.ToString(),false);
                this.AddSetting(setting);
            }

            this.Parse();
        }
    }
}
