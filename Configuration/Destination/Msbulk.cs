using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace Configuration.Destination
{
    class Msbulk : Destination
    {
        public Msbulk(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            //foreach (FileSettingEnum settingName in Enum.GetValues(typeof(FileSettingEnum)))
            //{
            //    ISetting setting = new Setting<string>(settingName.ToString(),false);
            //    this.AddSetting(setting);
            //}

            this.Parse();
        }
    }
}
