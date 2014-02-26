using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Modules;

using EZEtl.Configuration.Misc;
using EZEtl.Configuration;


namespace EZEtl.Workflow
{
    public class SetVariable:  OperatorBase, IOperator
    {
        IVariable _variable;
        string _expression = string.Empty;


       //  <SetVariable ID="FeedName" value="AdHoc" />
        public SetVariable(ConfigurationFile scope,  IConfigurationParent parent, OperatorEnum operatorType, string variableId, string expression)
            : base(scope, parent, operatorType, variableId)
        {
         
            _expression = expression;

            _variable = _scope.GetVariable(variableId);
            if (_variable == null)
            {
                _errorMessage = "Variable [" + variableId + "] not found in the Variables section";
                return;
            }

        }

        public void Execute(params object[] args)
        {
          _variable.Value = _expression; // TODO expression evaluation
        }
    }
}
