using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Configuration.Source
{
    public class SQL : SourceTask
    {
        public SQL(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
        }
    }
}
