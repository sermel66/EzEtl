using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration
{
    public static class Configuration
    {
        private static ConfigurationFile _configurationFile;

        public static IEnumerable<string> VariableNames { get { return _configurationFile.VariableNames; } }
        public static string VariableValue(string variableName) { return _configurationFile.VariableValue(variableName); }
        public static string VariableValue(ReservedVariableEnum reservedVariable) { return _configurationFile.VariableValue(reservedVariable.ToString()); }

        public static IEnumerable<string> ModuleNames { get { return _configurationFile.ModuleNames; } }
        public static Module Module(string moduleName) { return _configurationFile.Module(moduleName); }

        public static string EntryPointModuleName { get { return _configurationFile.EntryPointModuleName; } }

        
        public static void Create(ConfigurationFile configurationFile)
        {
            if ( configurationFile == null)
                throw new ArgumentNullException("configurationFile");

            _configurationFile = configurationFile;

        }

    }
}
