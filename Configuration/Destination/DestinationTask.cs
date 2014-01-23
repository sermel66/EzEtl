using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Configuration.Setting;

namespace Configuration.Destination
{
    public abstract class DestinationTask : Task
    {
        protected DestinationTaskTypeEnum _destinationTaskType = DestinationTaskTypeEnum.UNKNOWN;
        public DestinationTaskTypeEnum DestinationTaskType { get { return _destinationTaskType; } }

        public DestinationTask(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
            ISetting setting;
            foreach (DestinationSettingEnum settingName in Enum.GetValues(typeof(DestinationSettingEnum)))
            {
                switch (settingName)
                {
                    case DestinationSettingEnum.OneDebugMessagePerBatchCount:
                        setting = new Setting<Int32>(settingName.ToString(), false, -1);
                        break;
                    case DestinationSettingEnum.ExistingDataAction:
                        setting = new Setting<ExistingDataActionEnum>(settingName.ToString(), false, ExistingDataActionEnum.Delete);
                        break;
                    default:
                        setting = new Setting<string>(settingName.ToString(), false);
                        break;

                }
                this.AddSetting(setting);
            }

        }
    }
}
