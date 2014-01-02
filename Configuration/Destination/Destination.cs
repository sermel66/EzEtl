using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Configuration.Destination
{
    public abstract class Destination : Task
    {
        public Destination(TaskCategoryEnum taskCategory, XElement item)
            : base(taskCategory, item)
        {
        }
    }
}
