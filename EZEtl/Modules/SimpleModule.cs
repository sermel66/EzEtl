using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public abstract class SimpleModule : ModuleBase, IModule
    {
        protected TaskConfiguration _taskConfiguration;

        public bool IsValid { get { return _taskConfiguration.IsValid; } }

        public void OutputDiagnostics()
        {
            _taskConfiguration.OutputDiagnostics();
        }

        public void Execute()
        {
            // TODO
            throw new NotImplementedException();
        }
        
        public SimpleModule (IConfigurationParent parent, ModuleTypeEnum moduleType, string id, XElement xmlItemModule )
            : base (parent, moduleType, id)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            if (string.IsNullOrWhiteSpace(xmlItemModule.ToString()))
                throw new ArgumentNullException("xmlItemModule");


            _taskConfiguration = new TaskConfiguration(this, DataFlowStepEnum.NonDataFlow, id);
                
        }

    }
}
