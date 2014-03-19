using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;

using EZEtl.Configuration.Misc;
using EZEtl.Configuration;

namespace EZEtl.Workflow
{
   
    /*
        <ForLoop ID="Loop1"
				InitExpression="${FeedDate}=${StartDate}"
				EvalExpression="${FeedDate}&lt;=${EndDate}"
				AssignExpression="${FeedDate}=DATEADD(day,1,${FeedDate})">
				
			<ExecuteModule ID="Upload"/>
			<ExecuteModule ID="ProcessFeed"/>
		</ForLoop>
     */
     
    public class ForLoop : OperatorBase, IOperator
    {
        enum ForLoopAttribute
        {
            InitExpression,
            EvalExpression,
            AssignExpression
        }

        Dictionary<ForLoopAttribute, string> _expression = new Dictionary<ForLoopAttribute, string>();
        OperatorBlock _operatorBlock;

        public override bool IsValid
        {
            get
            {
                if (_operatorBlock == null)
                    return false;
                
                return base.IsValid && _operatorBlock.IsValid;
            }
        }
        public override void OutputDiagnostics()
        {
            base.OutputDiagnostics();
            if (_operatorBlock != null && !_operatorBlock.IsValid)
            {
                _operatorBlock.OutputDiagnostics();
            }
        }
                
        public ForLoop (ConfigurationFile scope, IConfigurationParent parent, string operatorType, string blockID, XElement element )
            : base(scope, parent, operatorType, blockID)
        {

            List<ForLoopAttribute> missingAttributeList = new List<ForLoopAttribute>();

            foreach( ForLoopAttribute requiredAttributeName in Enum.GetValues(typeof(ForLoopAttribute)).Cast<ForLoopAttribute>() )
            {
                XAttribute forLoopAttribute = element.Attribute(requiredAttributeName.ToString());
                if (forLoopAttribute == null)
                {
                    missingAttributeList.Add(requiredAttributeName);
                    continue;
                }

                _expression.Add(requiredAttributeName, forLoopAttribute.Value);

            }
           
            if ( missingAttributeList.Count > 0 )
            {
                _errorMessage = "Required attribute(s) missing: ";
                foreach ( ForLoopAttribute missingAttributeName in missingAttributeList)
                {
                    _errorMessage += missingAttributeName.ToString() + ", ";
                }

                _errorMessage = _errorMessage.Substring(1, _errorMessage.Length - 2);
                return;
            }

            _operatorBlock = new OperatorBlock(scope, parent, blockID);
            _operatorBlock.Parse(element);

        }

        public void Execute(params object[] args)
        {
            //for (
                
                
            //    )
            //{
            //    _operatorBlock.Execute();
            //}


           // throw new System.NotImplementedException(); // TODO
        }
    }
}
