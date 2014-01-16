using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration;

namespace EZEtl
{
    public static class Workflow
    {

        public static void ProcessWorkflow()
        {
          Module module = Configuration.Configuration.Module(Configuration.Configuration.EntryPointModuleName);

           ProcessModule pm = new ProcessModule(module);
           pm.DestinationTask.ExecuteAsync();
        }
    }
}
