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
        public ExecuteModule(ConfigurationFile scope, IConfigurationParent parent, OperatorEnum operatorType, string itemToExecuteID)
            : base(scope, parent, operatorType, itemToExecuteID)
        {
            // locate the module with the provided ID

            if (  ! new List<string>(_scope.ModuleIDs).Contains(itemToExecuteID) )            {
                _errorMessage = "Module ID=" + itemToExecuteID + " not found in the Modules section";
                return;
            }
            
            _module = _scope.Module(itemToExecuteID);

        }

        public void Execute(params object[] args)
        {
            _module.Execute();
        }
    }
}
