using EZEtl.Configuration.Misc;
using EZEtl.Modules;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EZEtl.Configuration
{
    public class ConfigurationFile : IConfigurationParent, IDiagnosable
    {
        string _configurationHierarchy;
        public string ConfigurationHierarchy { get { return _configurationHierarchy; } }

        const int MaximumInheritance = 1;

        List<string> _knownConfigFilePathList;
        XDocument _userConfigDocument;

        Dictionary<string, IVariable> _variables = new Dictionary<string, IVariable>();
        public IEnumerable<string> VariableNames { get { return _variables.Keys; } }
        public IVariable GetVariable(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new System.ArgumentNullException("variableName");

            if (_variables.ContainsKey(variableName))
                return _variables[variableName];

            return null;
        }

        Dictionary<string, IModule> _modules = new Dictionary<string, IModule>();
        public IEnumerable<string> ModuleIDs { get { return _modules.Keys; } }
        public IModule Module(string moduleID)
        {
            if (string.IsNullOrWhiteSpace(moduleID))
                throw new System.ArgumentNullException("moduleID");

            if (_modules.ContainsKey(moduleID))
                return _modules[moduleID];

            throw new System.ArgumentOutOfRangeException("moduleID", moduleID, "Not found");
        }

        Workflow.OperatorBlock _workFlowOperatorBlock;

        List<string> _warningMessages = new List<string>();
        List<string> _errorMessages = new List<string>();
        public bool IsValid
        {
            get
            {

                if (_errorMessages.Count != 0) return false;

                foreach (IDiagnosable module in _modules.Values)
                {
                    if (!module.IsValid)
                        return false;
                }

                if (_workFlowOperatorBlock == null)
                    return false;

                return _workFlowOperatorBlock.IsValid;
            }
        }

        public void OutputDiagnostics()
        {
            foreach (string errorMessage in _errorMessages)
            {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, errorMessage);
            }

            foreach ( IDiagnosable module in _modules.Values)
            {
                module.OutputDiagnostics();
            }

            if (_workFlowOperatorBlock != null)
                _workFlowOperatorBlock.OutputDiagnostics();

        }

        public ConfigurationFile(string configFilePath, List<string> processedConfigFilePathList)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentNullException("configFilePath");

            if (processedConfigFilePathList == null)
                _knownConfigFilePathList = new List<string>();
            else
                _knownConfigFilePathList = processedConfigFilePathList;

            _configurationHierarchy = configFilePath;
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

                ConfigurationFile baseFile = new ConfigurationFile(baseFilePath, _knownConfigFilePathList);

                // import base variables
                foreach (string variableName in baseFile.VariableNames)
                {
                    _variables.Add(variableName, baseFile.GetVariable(variableName));
                }

                // import base modules
                foreach (string moduleID in baseFile.ModuleIDs)
                {
                    _modules.Add(moduleID, baseFile.Module(moduleID));
                }

                // TODO import workflow

            }

            // Loop through the remaining top level elements
            List<string> unexpectedTopLevelElements = new List<string>();
            Dictionary<TopLevelItemEnum, XElement> presentToplevelItems = new Dictionary<TopLevelItemEnum, XElement>();
            string errorMessage;

            foreach (XElement item in _userConfigDocument.Descendants())
            {

                // try to match the item to the expected top level elements
                TopLevelItemEnum topLevelItem = TopLevelItemEnum.UNKNOWN;
                if (!Enum.TryParse<TopLevelItemEnum>(item.Name.ToString(), out topLevelItem))
                {
                    unexpectedTopLevelElements.Add(item.Name.ToString());
                    continue;
                }
                presentToplevelItems.Add(topLevelItem, item);
            }

            if (presentToplevelItems.ContainsKey(TopLevelItemEnum.Variables))
            {
                foreach (XElement variableItem in presentToplevelItems[TopLevelItemEnum.Variables].Elements())
                {
                    IVariable newVariable = VariableFactory.Create(variableItem, out errorMessage);
                    if (newVariable == null)
                    {
                        if (string.IsNullOrWhiteSpace(errorMessage))
                        {
                            throw new Exception("VariableFactory.Create returned null object with empty error message");
                        }
                        _errorMessages.Add(errorMessage);
                    }
                    else if (_variables.ContainsKey(newVariable.Name))
                    {
                        errorMessage = "Variable [" + newVariable.Name + "] already defined in this or included file(s)";
                        _errorMessages.Add(errorMessage);
                    }
                    else
                    {
                        _variables.Add(newVariable.Name, newVariable);
                    }
                }
            }

            if (presentToplevelItems.ContainsKey(TopLevelItemEnum.Modules))
            {
                foreach (XElement moduleItem in presentToplevelItems[TopLevelItemEnum.Modules].Elements() )
                {
                    IModule newModule = ModuleFactory.Create(this, moduleItem, out errorMessage);
                    if (newModule == null)
                    {
                        _errorMessages.Add(errorMessage);
                        continue;
                    }

                    if (_modules.ContainsKey(newModule.ModuleID))
                    {
                        _errorMessages.Add("Module ID " + newModule.ModuleID + " already defined in this or included file(s)");
                        continue;
                    }

                    _modules.Add(newModule.ModuleID, newModule);
                }

            }

            if (presentToplevelItems.ContainsKey(TopLevelItemEnum.Workflow))
            {
                _workFlowOperatorBlock = new Workflow.OperatorBlock(this, this, TopLevelItemEnum.Workflow.ToString());
                _workFlowOperatorBlock.Parse(presentToplevelItems[TopLevelItemEnum.Workflow]);

            }
        }
    }
}
