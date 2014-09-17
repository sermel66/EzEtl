using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Workflow
{
    public class OperatorBlock : ConfigurationParentBase, IOperator // IConfigurationParent, IDiagnosable
    {
        ConfigurationFile _scopeConfigurationFile;

        List<string> _errorMessages = new List<string>();

        List<IOperator> _operatorSequence = new List<IOperator>();

        public void OutputDiagnostics()
        {
            foreach (IOperator op in _operatorSequence)
            {
                op.OutputDiagnostics();
            }

            foreach (string errorMessage in _errorMessages)
            {
                Diagnostics.Output(this.ConfigurationHierarchy, Utilities.SimpleLogEventType.Error, errorMessage);
            }
        }

        public bool IsValid
        {
            get
            {
                if (_operatorSequence.Count < 1)
                    return false;

                foreach (IDiagnosable op in _operatorSequence)
                {
                    if (!op.IsValid)
                        return false;
                }
                return true;
            }
        }

        public OperatorBlock(ConfigurationFile scope,  IConfigurationParent parent, string blockName)
            : base(parent, blockName)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            _scopeConfigurationFile = scope;
        }

        public void Parse(XElement workflowBlock)
        {
            OperatorEnum operatorType;

            int operatorCounter = 0;
            foreach (XElement element in workflowBlock.Elements())
            {
                operatorCounter++;

                string operatorTypeString = element.Name.ToString();
                if (!Enum.TryParse<OperatorEnum>(operatorTypeString, out operatorType))
                {
                    _errorMessages.Add("Unexpected operator " + operatorTypeString + " encountered");
                    continue;
                }
                
                string ID, errorMessage;
                if ( !XmlUtil.TryGetAttribute(element,AttributeNameEnum.ID,out ID, out errorMessage) )
                {
                    _errorMessages.Add(errorMessage);
                    continue;
                }

                IOperator newOperator;
                switch ( operatorType)
                {
                    case OperatorEnum.ExecuteModule:
                        newOperator = new ExecuteModule(_scopeConfigurationFile, this, operatorType.ToString(), ID);
                        break;

                    case OperatorEnum.SetVariable:
                        string expression = element.Value;
                        if ( string.IsNullOrWhiteSpace(expression))
                        {
                            _errorMessages.Add(
                                  "Expression is missing in the operator # " 
                                + operatorCounter.ToString() + " " 
                                + XmlUtil.Quot(element.ToString())
                                );
                            continue;
                        }
                        newOperator = new SetVariable(_scopeConfigurationFile,  this, operatorType.ToString(), ID, expression);
                        break;

                    case OperatorEnum.ForLoop:
                        newOperator = new ForLoop(_scopeConfigurationFile, this, operatorType.ToString(), ID, element);
                        break;
                    default:
                        throw new System.NotImplementedException("Operator type " + operatorType);
                }

                _operatorSequence.Add(newOperator);
            }
        }

        public void Execute(params object[] args)
        {
            foreach ( IOperator op in _operatorSequence)
            {
                op.Execute();
            }

        }

    }
}
