using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EZEtl.Configuration.Settings;
using EZEtl.Configuration.Misc;


namespace EZEtl.Configuration
{
    public class TaskConfiguration : ConfigurationParentBase, ITaskConfiguration
    {
        //string _configurationHierarchy;
        //public string ConfigurationHierarchy { get { return _configurationHierarchy; } }

        string _errorMessage = string.Empty;
        List<string> _unexpectedSettings = new List<string>();
        Dictionary<string, int> _settingCount = new Dictionary<string, int>();

        DataFlowStepEnum _dataFlowStep;
        public DataFlowStepEnum DataFlowStep { get { return _dataFlowStep; } }

        string _taskID;
        public string TaskID { get { return _taskID; } }

        public IEnumerable<SettingNameEnum> SettingNameList
        {
            get
            {
                return _settings.Keys;
            }
        }

        protected Dictionary<SettingNameEnum, ISetting> _settings = new Dictionary<SettingNameEnum, ISetting>();
        public ISetting GetSetting(SettingNameEnum settingName)
        {
            if (_settings.ContainsKey(settingName))
                return _settings[settingName];

            return null;
        }

        public void AddSetting(ISetting setting)
        {
            _settings.Add(setting.SettingName, setting);
        }

        public bool IsValid
        {
            get
            {

                foreach (KeyValuePair<SettingNameEnum, ISetting> entry in _settings)
                {
                    if (!entry.Value.IsValid)
                        return false;
                }
                return true;
            }
        }

        public void OutputDiagnostics()
        {
            foreach (KeyValuePair<SettingNameEnum, ISetting> entry in _settings)
            {
                entry.Value.OutputDiagnostics();
            }

            if (_errorMessage.Length > 0)
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);

        }

        public TaskConfiguration(IConfigurationParent parent, DataFlowStepEnum dataFlowStep, string taskID)
            : base ( parent, taskID + "(" + dataFlowStep.ToString() + ")")
        {
            if (string.IsNullOrWhiteSpace(taskID))
                throw new ArgumentNullException("taskName");

            _dataFlowStep = dataFlowStep;
            _taskID = taskID;
        }
 

        //public IEnumerable<string> ConfiguredSettings { get { return _settings.Keys; } }
        //protected void AddSetting(ISetting setting)
        //{
        //    if (setting == null)
        //        throw new ArgumentNullException("setting");

        //    if (_settings.ContainsKey(setting.Key))
        //        throw new ConfigurationException("Attempt to register setting " + setting.Key + " multiple times");

        //    _settings.Add(setting.Key, setting);
        //}



        public virtual void Parse(XElement body)
        {

            if (string.IsNullOrWhiteSpace(body.ToString()))
                throw new ArgumentNullException("body");

            foreach (XElement item in body.Descendants())
            {
                string itemName = item.Name.LocalName;

                SettingNameEnum settingName;
                if (Enum.TryParse<SettingNameEnum>(itemName, out settingName))
                {
                    string itemValue = string.Empty;

                    if (item.HasAttributes || item.HasElements)
                        itemValue = item.ToString();
                    else if (!item.IsEmpty)
                        itemValue = item.Value;

                    if (_settings.ContainsKey(settingName))
                    {
                        _settings[settingName].RawValue = itemValue;


                        if (_settingCount.ContainsKey(itemName))
                        {
                            _settingCount[itemName]++;
                        }
                        else
                            _settingCount.Add(itemName, 1);

                        continue;
                    }

                }
                else
                {
                    if (!_unexpectedSettings.Contains(itemName))
                    {
                        _unexpectedSettings.Add(itemName);
                    }

                }

            }

        }
    }
}
