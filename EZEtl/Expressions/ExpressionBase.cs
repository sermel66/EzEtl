using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Expressions
{
    public delegate void ArithOper();
        
    public abstract class ExpressionBase : EZEtl.Workflow.OperatorBase
    {
        protected ArithOper _evaluator;
        public void Evaluate()
        {
            _evaluator();
        }

        protected Dictionary<OperandEnum,IVariable> _operandDict = new Dictionary<OperandEnum,IVariable>();

        protected IVariable _result;
        public IVariable Result { get { return _result; } }

        public ExpressionBase(ConfigurationFile scope, IConfigurationParent parent, string childType, string qualifierID)
            : base(scope, parent, childType, qualifierID)
        {

        }
    }
}
