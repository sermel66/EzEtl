using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Configuration.Filter
{
    public abstract class Filter : Task
    {
        public Filter(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
        }
    }
}
