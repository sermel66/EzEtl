using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;


namespace EZEtl.Misc
{
    public static class Terminate
    {
        public static void FatalError(string message)
        {
            SimpleLog.ToLog(message, SimpleLogEventType.Error);

            Environment.Exit(EZEtl.Configuration.Constant.rcError);
        }
    }
}
