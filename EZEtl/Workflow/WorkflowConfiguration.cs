using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

using EZEtl.Configuration.Misc;

namespace EZEtl.Workflow
{
    public class WorkflowConfiguration : IConfigurationParent, IDiagnosable
    {

        string _configurationHierarchy = "";
        public string ConfigurationHierarchy { get { return _configurationHierarchy; } }



        OperatorBlock _topOperatorBlock;

        public void OutputDiagnostics()
        {
            _topOperatorBlock.OutputDiagnostics();
        }

        public bool IsValid
        {
            get
            {
                if (_topOperatorBlock == null)
                    return false;

                return _topOperatorBlock.IsValid;
            }
        }

        public WorkflowConfiguration()
        {
            _topOperatorBlock = new OperatorBlock(this, "Workflow");
        }

        public void Parse(XElement workflowConfiguration)
        {
            _topOperatorBlock.Parse(workflowConfiguration);
        }

    }
}
