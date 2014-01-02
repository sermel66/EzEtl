using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Configuration.Filter
{
    public class ColumnMap : Filter
    {
        public ColumnMap(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
        }
    }
}
