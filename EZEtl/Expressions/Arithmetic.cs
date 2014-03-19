using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Workflow;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Expressions
{
    public class Arithmetic : ExpressionBase, IExpression
    {

        ArithmeticExpressionEnum _operation;

        public Arithmetic(ConfigurationFile scope
            , IConfigurationParent parent
            , string qualifierID
            , IVariable op1
            , ArithmeticExpressionEnum operation
            , IVariable op2  )
            : base(scope, parent, "Expression_Arithmetic", qualifierID)
        {
            if (op1 == null) throw new ArgumentNullException("op1");
            if (op2 == null) throw new ArgumentNullException("op2");

            _operandDict.Add(OperandEnum.Op1, op1);
            _operandDict.Add(OperandEnum.Op2, op2);
            _operation = operation;

            // TODO type checks

            switch (_operation)
            {
                case ArithmeticExpressionEnum.Add:
                    _evaluator = AddInt32;
                    break;
                case ArithmeticExpressionEnum.Multiply:
                case ArithmeticExpressionEnum.Divide:
                default:
                    throw new NotImplementedException(_operation.ToString());

            }
            
        }

        public void Parse(string expressionString)
        {
            throw new NotImplementedException();
        }

        void AddInt32()
        {
            _result.Value = (int)_operandDict[OperandEnum.Op1].Value + (int)_operandDict[OperandEnum.Op2].Value;
        }

        IVariable AddString()
        {            
            return new Variable<string>(
                "ArithmeticAddStringResult"
                , SupportedVariableTypeEnum.String
                , (string)_operandDict[OperandEnum.Op1].Value + (string)_operandDict[OperandEnum.Op2].Value
            );
        }

    }
}
