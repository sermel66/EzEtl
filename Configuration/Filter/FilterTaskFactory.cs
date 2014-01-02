using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration.Filter
{
    public static class FilterTaskFactory 
    {

        public static Task ConfigureTask(TaskCategoryEnum taskCategory, XElement item)
        {
            string taskTypeString = item.Attribute(AttributeNameEnum.type.ToString()).Value;
            if ( String.IsNullOrWhiteSpace(taskTypeString))
                throw new ConfigurationException("Task type undefined in the Filter item [" + item.ToString().Substring(0,50) + "...]");

            FilterTaskTypeEnum taskType;
            if (!Enum.TryParse<FilterTaskTypeEnum>(taskTypeString, out taskType))
                throw new ConfigurationException("Unexpected Filter Task Type [" + taskTypeString + "]");


            switch (taskType)
            {
                case FilterTaskTypeEnum.ColumnMap:
                    return new ColumnMap(taskCategory, item);
                default:
                    throw new ConfigurationException("Unsupported Task Type [" + taskType.ToString() + "]");
            }
            
        }
    }
}
