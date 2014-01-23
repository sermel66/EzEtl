using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Configuration.Setting;


namespace Configuration.Destination
{
   
    public abstract class DatabaseDestination : DestinationTask
    {
        
        public DatabaseDestination(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            ISetting setting;
            foreach (DatabaseSettingEnum settingName in Enum.GetValues(typeof(DatabaseSettingEnum)))
            {
                switch (settingName)
                {
                    case DatabaseSettingEnum.DbOperationTimeout:
                        setting = new Setting<int>(settingName.ToString(), false);
                        break;
                    default:
                         setting = new Setting<string>(settingName.ToString(), false);
                        break;
                }
                 this.AddSetting(setting);
            }

            this.Parse();
        }
    }
}
