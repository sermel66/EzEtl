using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Configuration
{
    public static class TaskFactory
    {
        public static Task ConfigureTask(TaskCategoryEnum category, XElement item)
        {
            if (String.IsNullOrWhiteSpace(item.ToString()))
                throw new ArgumentNullException("item");


            switch (category)
            {
                case TaskCategoryEnum.Source:
                    return Source.SourceTaskFactory.ConfigureTask(category, item);

                case TaskCategoryEnum.Destination:
                    return Destination.DestinationTaskFactory.ConfigureTask(category, item);

                case TaskCategoryEnum.Filter1:
                case TaskCategoryEnum.Filter2:
                case TaskCategoryEnum.Filter3:
                case TaskCategoryEnum.Filter4:
                case TaskCategoryEnum.Filter5:
                case TaskCategoryEnum.Filter6:
                case TaskCategoryEnum.Filter7:
                case TaskCategoryEnum.Filter8:
                case TaskCategoryEnum.Filter9:
                case TaskCategoryEnum.Filter10:
                    int filterNumber = Int32.Parse(category.ToString().Substring(6, 10));
                    return Filter.FilterTaskFactory.ConfigureTask(category, item);

                default:
                    throw new ConfigurationException("Unexpected Task Category [" + category.ToString() + "]");

            }

        }
    }
}
