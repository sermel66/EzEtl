using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Configuration.Destination
{
    public static class DestinationTaskFactory
    {
        public static Task ConfigureTask(TaskCategoryEnum taskCategory, XElement item)
        {
            string taskTypeString = item.Attribute(AttributeNameEnum.type.ToString()).Value;
            if (String.IsNullOrWhiteSpace(taskTypeString))
                throw new ConfigurationException("Task type undefined in the Source item [" + item.ToString().Substring(0, 50) + "...]");

            DestinationTaskTypeEnum taskType;
            if (!Enum.TryParse<DestinationTaskTypeEnum>(taskTypeString, out taskType))
                throw new ConfigurationException("Unexpected Source Task Type [" + taskTypeString + "]");


            switch (taskType)
            {
                case DestinationTaskTypeEnum.FILE:
                    return new FileDestination(taskCategory, item);
                case DestinationTaskTypeEnum.SQLBULK:
                    return new SqlBulkDestination(taskCategory, item);
                default:
                    throw new ConfigurationException("Unsupported Destination Task Type [" + taskType.ToString() + "]");
            }

        }
    }
}
