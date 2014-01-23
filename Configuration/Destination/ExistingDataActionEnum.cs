using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration.Destination
{
    public enum ExistingDataActionEnum
    {
         UNDEFINED = 0
        ,Append
        ,Delete
        ,Truncate
    }
}
