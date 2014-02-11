using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public class CompositeModule : ModuleBase,  IModule
    {


        public bool IsValid { get { return false; } } // TODO

        public void OutputDiagnostics()
        {
           // TODO _taskConfiguration.OutputDiagnostics();
        }

        public void Execute()
        {
            // TODO
        }


        public CompositeModule(IConfigurationParent parent, ModuleTypeEnum moduleType, string id, XElement xmlItemModule)
            : base (parent, moduleType, id)
        {
          //  _taskConfiguration = new TaskConfiguration();
        
        
        }

    }
}
