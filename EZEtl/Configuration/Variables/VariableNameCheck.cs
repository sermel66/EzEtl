using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EZEtl.Configuration.Variables
{
    public static class VariableNameCheck
    {
        static Regex nameValidatorRegEx = new Regex(Constant.VariableNamePattern,RegexOptions.ECMAScript);

        public static bool IsValid(string variableName, out string errorMessage)
        {
            if ( string.IsNullOrEmpty(variableName))
            {
                errorMessage = "is empty";
                return false;
            }

            if ( ! nameValidatorRegEx.IsMatch(variableName) )
            {
                errorMessage = "is invalid";
                return false;
            }

            errorMessage = string.Empty;
            return true;
         
        }

        public static bool IsDefined(string variableName)
        {
            return Program.Configuration.VariableNames.Contains(variableName);
        }
    }
}
