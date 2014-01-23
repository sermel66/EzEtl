using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using Configuration.Setting;

namespace Configuration.Destination
{
    class SqlBulkDestination : DatabaseDestination
    {

        public SqlBulkDestination(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            _destinationTaskType = DestinationTaskTypeEnum.SQLBULK;

            foreach (SqlBulkSettingEnum settingName in Enum.GetValues(typeof(SqlBulkSettingEnum)))
            {
                ISetting setting = new Setting<string>(settingName.ToString(), false);
                this.AddSetting(setting);
            }

            this.Parse();
        }
    }
}
