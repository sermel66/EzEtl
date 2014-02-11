using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Modules;

using EZEtl.Configuration.Misc;
using EZEtl.Configuration;


namespace EZEtl.Workflow
{
    public class ExecuteModule:  OperatorBase, IOperator
    {
        IModule _module;

        // <ExecuteModule ID="GetStartDate" />
        public ExecuteModule(IConfigurationParent parent, OperatorEnum operatorType, string itemToExecuteID)
            : base(parent, operatorType, itemToExecuteID)
        {
            // locate the module with the provided ID

            _module = Configuration.Configuration.Module(itemToExecuteID);
            if (_module == null)
            {
                _errorMessage = "Module ID=" + itemToExecuteID + " not found in the Modules section";
                return;
            }

        }

        public void Execute()
        {
            _module.Execute();
        }
    }
}
