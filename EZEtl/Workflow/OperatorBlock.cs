using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

using EZEtl.Configuration.Misc;

namespace EZEtl.Workflow
{
    public class OperatorBlock : ConfigurationParentBase, IConfigurationParent, IDiagnosable
    {

        List<IOperator> _operatorSequence = new List<IOperator>();

        public void OutputDiagnostics()
        {
            foreach (IOperator op in _operatorSequence)
                op.OutputDiagnostics();
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

        public OperatorBlock(IConfigurationParent parent, string blockName)
            :base (parent,blockName)
        {

        }
        
        public void Parse ( XElement workflowBlock )
        {
           foreach(XElement element in workflowBlock.Descendants())
           {


           }

        }





    }
}
