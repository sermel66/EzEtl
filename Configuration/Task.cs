using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Configuration.Setting;

namespace Configuration
{
    public abstract class Task
    {
        TaskCategoryEnum _taskCategory;
        public TaskCategoryEnum TaskCategory { get { return _taskCategory; } }

        protected string _warnings = string.Empty;

        public bool HasWarnings { get { return string.IsNullOrWhiteSpace(_warnings); } }
        public string Warnings { get { return _warnings; } }

        protected void GetWarnings()
        {  
                string unexpectedSettings = string.Empty;
                foreach (string setting in _unexpectedSettings)
                {
                    unexpectedSettings += setting + Constant.UserMessageListDelimiter;
                }
                if (!String.IsNullOrWhiteSpace(unexpectedSettings))
                    _warnings += "Unexpected settings encountered: " + unexpectedSettings + Constant.UserMessageSentenceDelimiter;

                string multipleSettings = string.Empty;
                foreach (string setting in _settingCount.Keys)
                {
                    if (_settingCount[setting] > 1)
                    {
                        multipleSettings += setting + Constant.UserMessageListDelimiter;
                    }

                    if ( !string.IsNullOrWhiteSpace( _settings[setting].WarningMessage ) )
                    {
                        _warnings += "Setting " + setting + ":" + _settings[setting].WarningMessage + Constant.UserMessageSentenceDelimiter;
                    }
                }
                if (!String.IsNullOrWhiteSpace(multipleSettings))
                    _warnings += "Settings defined multiple times: " + multipleSettings + Constant.UserMessageSentenceDelimiter;

        }

        int _errorCount = 0;
        protected int ErrorCount { get { return _errorCount; } set { _errorCount = value; } }

        public bool HasErrors { get { return _errorCount > 0; } }
        protected string _errors = string.Empty;
        public string Errors { get { return _errors; } }

        protected virtual void GetErrors()
        {
                if (!HasErrors)
                    return;

                string settingMessages = string.Empty;
                foreach (string setting in _settings.Keys)
                {
                    if (!String.IsNullOrWhiteSpace(_settings[setting].ErrorMessage))
                    {
                        settingMessages += String.Format("<{0}>:{1}", setting, _settings[setting].ErrorMessage) + Constant.UserMessageSentenceDelimiter;
                    }
                }

                if (!String.IsNullOrWhiteSpace(settingMessages))
                    _errors += "Setting Messages:" + Constant.UserMessageSentenceDelimiter + settingMessages;     
        }

        XElement _body;
        protected XElement Body { get { return _body; } }

        string _type;
        public string Type { get { return _type; } }

        List<string> _unexpectedSettings = new List<string>();
        Dictionary<string, int> _settingCount = new Dictionary<string, int>();

        Dictionary<string,ISetting> _settings = new Dictionary<string,ISetting>();
        public ISetting Setting(string key)
        {            
            if (_settings.ContainsKey(key))
                return _settings[key];

            return null;
        }

        public IEnumerable<string> ConfiguredSettings { get { return _settings.Keys; } }
        protected void AddSetting(ISetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            if (_settings.ContainsKey(setting.Key))
                throw new ConfigurationException("Attempt to register setting " + setting.Key + " multiple times");

            _settings.Add(setting.Key, setting);
        }

        public Task(TaskCategoryEnum taskCategory, XElement xmlItemSourceDestination)
        {
            if (string.IsNullOrWhiteSpace(xmlItemSourceDestination.ToString()))
                throw new ArgumentNullException("xmlItemSourceDestination");

            _body = xmlItemSourceDestination;
            _taskCategory = taskCategory;
            _type =  _body.Attribute( ((XName)"type") ).Value;
        }

        public virtual void Parse()
        {
            foreach (XElement item in _body.Descendants())
            {
                string itemName = item.Name.LocalName;
                string itemValue = string.Empty;

                if (item.HasAttributes || item.HasElements)
                    itemValue = item.ToString();
                else if (!item.IsEmpty)
                    itemValue = item.Value;

                if (_settings.ContainsKey(itemName))
                {
                    _settings[itemName].RawValue = itemValue;
                    if (!String.IsNullOrWhiteSpace(_settings[itemName].ErrorMessage))
                    {                     
                        _errorCount++;
                    }
                }
                else
                    if (!_unexpectedSettings.Contains(itemName))
                    {
                        _unexpectedSettings.Add(itemName);                     
                    }

                if (_settingCount.ContainsKey(itemName))
                {
                    _settingCount[itemName]++;                 
                }
                else
                    _settingCount.Add(itemName, 1);

            }

            this.GetWarnings();
        }
    }
}
