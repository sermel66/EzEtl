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
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, errorMessage);
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


            foreach (XElement element in workflowBlock.Elements())
            {
                string operatorTypeString = element.Name.ToString();
                if (!Enum.TryParse<OperatorEnum>(operatorTypeString, out operatorType))
                {
                    _errorMessages.Add("Unexpected operator " + operatorTypeString + " encountered");
                    continue;
                }
                 
                XAttribute idAttribute = element.Attribute(AttributeNameEnum.ID.ToString());
                if (idAttribute==null)
                {
                    _errorMessages.Add("Attribute " + AttributeNameEnum.ID.ToString() + " is missing in the operator " + element.ToString());
                    continue;
                }
                

                IOperator newOperator;
                switch ( operatorType)
                {
                    case OperatorEnum.ExecuteModule:
                        newOperator = new ExecuteModule(_scopeConfigurationFile, this, operatorType, idAttribute.Value);
                        break;

                    case OperatorEnum.SetVariable:
                       XAttribute valueAttribute = element.Attribute(AttributeNameEnum.value.ToString());
                        if ( valueAttribute == null)
                        {
                            _errorMessages.Add("Attribute " + AttributeNameEnum.value.ToString() + " is missing in the operator " + element.ToString());
                            continue;
                        }
                        newOperator = new SetVariable(_scopeConfigurationFile,  this, operatorType, idAttribute.Value, valueAttribute.Value);
                        break;

                    case OperatorEnum.ForLoop:
                        newOperator = new ForLoop(_scopeConfigurationFile, this, operatorType, idAttribute.Value, element);
                        break;
                    default:
                        throw new System.NotImplementedException("Operator type " + operatorType);
                }

                _operatorSequence.Add(newOperator);
            }
        }

        public void Execute(params object[] args)
        {
            // TODO
        }

    }
}
