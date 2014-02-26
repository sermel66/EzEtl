using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;

namespace EZEtl.Workflow
{
    public static class WorkflowProcess
    {
        static ConfigurationFile _configuration;
        public static ConfigurationFile Configuration { get { return _configuration; } }

        public static void Execute(params object[] args)
        {
            


          //Module module = Module(guration.EntryPointModuleName);

          // DataFlowProcess pm = new DataFlowProcess(module);
          // pm.DestinationTask.ExecuteAsync();
        }
    }
}
