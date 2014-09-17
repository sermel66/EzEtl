using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ciloci.Flee;

namespace EZEtl.Expressions
{
    public static class Flee
    {
        static ExpressionContext context = new ExpressionContext();
        static Dictionary<string, IDynamicExpression> compiliedExpression = new Dictionary<string, IDynamicExpression>();
 
        static Flee()
        {

            context.Imports.AddType(typeof(DateTime), "DateTime");     
        }


        public static object Variable(string variableId)
        {
            return context.Variables[variableId];
        }

        public static void SetVariable(string variableId, object value )
        {
            context.Variables[variableId] = value;
        }

        public static bool CompileDynamicExpression(string expression, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentNullException("expression must not be NULL or empty");

            errorMessage = string.Empty;

            if (compiliedExpression.ContainsKey(expression))
            {
                return true;
            }

            IDynamicExpression compiledExp=null;
            try
            {
                compiledExp =  context.CompileDynamic(expression);        
            }
            catch (Exception ex)
            {
                errorMessage = "Flee parse error: " + ex.Message + " in expression [" + expression + "]";
                return false;
            }
            compiliedExpression.Add(expression, compiledExp);
            return true;
        }

        public static object EvaluateExpresion (string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentNullException("expression must not be NULL or empty");

            if (compiliedExpression.ContainsKey(expression))
                return compiliedExpression[expression].Evaluate();

            throw new ArgumentException("expression [" + expression + "] has not been compiled, can not evaluate");
        }
    }
}
