using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration.Source
{
    public static class SourceTaskFactory 
    {

        public static Task ConfigureTask(TaskCategoryEnum taskCategory, XElement item)
        {
            string taskTypeString = item.Attribute(AttributeNameEnum.type.ToString()).Value;
            if ( String.IsNullOrWhiteSpace(taskTypeString))
                throw new ConfigurationException("Task type undefined in the Source item [" + item.ToString().Substring(0,50) + "...]");

            SourceTaskTypeEnum taskType;
            if (!Enum.TryParse<SourceTaskTypeEnum>(taskTypeString, out taskType))
                throw new ConfigurationException("Unexpected Source Task Type [" + taskTypeString + "]");

            Task task;
            switch (taskType)
            {
                case SourceTaskTypeEnum.FILE:
                   task = new File(taskCategory, item);
                   break;
                case SourceTaskTypeEnum.SQL:
                    task = new SQL(taskCategory, item);
                    break;
                default:
                    throw new ConfigurationException("Unsupported Source Task Type [" + taskType.ToString() + "]");
            }

            task.Parse();
            return task;
        }
    }
}
