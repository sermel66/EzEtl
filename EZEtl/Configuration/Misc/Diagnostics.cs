﻿using System;
using Utilities;

namespace EZEtl.Configuration.Misc
{
    public static class Diagnostics
    {
        public static void Output(string sourceName, SimpleLogEventType severity, string message)
        {
            string formattedMessage = string.Format("<{0}>:{1}:{2}", severity, sourceName, message);
             SimpleLog.ToLog(formattedMessage, severity);
        }

        public static string HierarchyConcat(string parentHierarchy, string childName)
        {
            return parentHierarchy + "." + childName;
        }
    }
}
