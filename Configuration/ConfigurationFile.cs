using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Configuration
{
    public class ConfigurationFile
    {
        const int MaximumInheritance = 1;

        List<string> _knownConfigFilePathList;
        XDocument _userConfigDocument;
        ConfigurationFile _base;
        
        Dictionary<string, string> _variables = new Dictionary<string, string>();
        public IEnumerable<string> VariableNames { get { return _variables.Keys; } }
        public string VariableValue(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new System.ArgumentNullException("variableName");

            if (_modules.ContainsKey(variableName))
                return _variables[variableName];

            return string.Empty;
        }

        Dictionary<string, Module> _modules = new Dictionary<string, Module>();
        public IEnumerable<string> ModuleNames { get { return _modules.Keys; } }
        public Module Module(string moduleName) { 
            if ( string.IsNullOrWhiteSpace(moduleName))
                throw new System.ArgumentNullException("moduleName");
            
            if (_modules.ContainsKey(moduleName))
                return _modules[moduleName];

            throw new System.ArgumentOutOfRangeException("moduleName",moduleName,"Not found");
        }

        string _entryPointModuleName;
        public string EntryPointModuleName { get { return _entryPointModuleName; } }
                
        protected ConfigurationFile() {} // Blank configuration for internal use

        public ConfigurationFile(string configFilePath,  List<string> processedConfigFilePathList)
        {           
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentNullException("configFilePath");

            if (processedConfigFilePathList == null)
                _knownConfigFilePathList = new List<string>();
            else
                _knownConfigFilePathList = processedConfigFilePathList;

            _knownConfigFilePathList.Add(configFilePath);
            _userConfigDocument = XDocument.Load(configFilePath); 
            // TODO schema validation
            
            // recursive call of the base
            int baseItemsEncountered = 0;
            foreach (XElement item in _userConfigDocument.Descendants(TopLevelItemEnum.Base.ToString()))
            {
                baseItemsEncountered++;
                if (baseItemsEncountered > MaximumInheritance)
                    throw new ConfigurationException(String.Format("More than {0} Base nodes in the config file {1}", MaximumInheritance, configFilePath));

                string baseFilePath = item.Attribute(AttributeNameEnum.file.ToString()).Value;

                _base = new ConfigurationFile(baseFilePath, _knownConfigFilePathList);
            }
            if (_base == null)
                _base = new ConfigurationFile();
            
            // Variables
            foreach( 
                XElement item in _userConfigDocument.Descendants(TopLevelItemEnum.Variables.ToString()).Descendants("Variables")
                )
            {
                string variableName  = item.Attribute(AttributeNameEnum.name.ToString()).Value;
                string variableValue = item.Attribute(AttributeNameEnum.value.ToString()).Value;

                // TODO resolve nested variables 
                _variables.Add(variableName, variableValue);
            }

            // Modules
            foreach (
              XElement item in _userConfigDocument.Descendants(TopLevelItemEnum.Modules.ToString()).Descendants("Module")
              )
            {
                Module module = new Module(item);
                if (module.IsEntryPoint)
                {
                    if (string.IsNullOrWhiteSpace(_entryPointModuleName))
                        _entryPointModuleName = module.Name;
                    else
                        throw new ConfigurationException("Multiple modules declared entry points");
                }

                if (_modules.ContainsKey(module.Name))
                    throw new ConfigurationException(String.Format("Module {0} declared multiple times", module.Name));
                
                _modules.Add(module.Name, module);
                
            }

        }
    }
}
