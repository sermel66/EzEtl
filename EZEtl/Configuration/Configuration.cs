using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration
{
    public static class Configuration
    {
        private static ConfigurationFile _configurationFile;

        public static IEnumerable<string> VariableNames { get { return _configurationFile.VariableNames; } }
        public static IVariable GetVariable(string variableName) { return _configurationFile.GetVariable(variableName); }
   //     public static string VariableValue(Variable.ReservedVariableEnum reservedVariable) { return _configurationFile.VariableValue(reservedVariable.ToString()); }

        public static IEnumerable<string> ModuleIDs { get { return _configurationFile.ModuleIDs; } }
        public static EZEtl.Modules.IModule Module(string moduleName) { return _configurationFile.Module(moduleName); }
             
        public static void Create(ConfigurationFile configurationFile)
        {
            if ( configurationFile == null)
                throw new ArgumentNullException("configurationFile");

            _configurationFile = configurationFile;

        }

    }
}
