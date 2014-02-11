using System;

namespace EZEtl.Configuration.Misc
{
    public static class Diagnostics
    {
        public static void Output(string sourceName, MessageSeverityEnum severity, string message)
        {
            Console.WriteLine("<{0}>:{1}:{2}", severity, sourceName, message);
        }

        public static string HierarchyConcat(string parentHierarchy, string childName)
        {
            return parentHierarchy + "." + childName;
        }
    }
}
