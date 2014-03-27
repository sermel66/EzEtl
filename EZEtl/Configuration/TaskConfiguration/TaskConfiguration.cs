using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EZEtl.Configuration.Setting;
using EZEtl.Configuration.Misc;
using System.Linq.Expressions;


namespace EZEtl.Configuration
{
    public class TaskConfiguration : ConfigurationParentBase, ITaskConfiguration
    {
       
        string _errorMessage = string.Empty;
        List<string> _unexpectedSettings = new List<string>();
        Dictionary<string, int> _settingCount = new Dictionary<string, int>();

        DataFlowStepEnum _dataFlowStep;
        public DataFlowStepEnum DataFlowStep { get { return _dataFlowStep; } }

        string _taskID;
        public string TaskID { get { return _taskID; } }

        Func<ITaskConfiguration, object> _instantiator;
        bool _needsInstantiator = true;
        public bool NeedsInstantiator { get { return _needsInstantiator; } set { _needsInstantiator = value; } }

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
                if (_needsInstantiator && _instantiator == null)
                    return false;

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
            {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
            }

            if (_needsInstantiator && _instantiator == null)
            {
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, "Instantiator not set");
            }
        }

        public TaskConfiguration(IConfigurationParent parent, DataFlowStepEnum dataFlowStep, string taskID)
            : base ( parent, taskID + "(" + dataFlowStep.ToString() + ")")
        {
            if (string.IsNullOrWhiteSpace(taskID))
                throw new ArgumentNullException("taskName");

            _dataFlowStep = dataFlowStep;
            _taskID = taskID;
        }

        public virtual void Parse(XElement body)
        {

            if (string.IsNullOrWhiteSpace(body.ToString()))
                throw new ArgumentNullException("body");

            foreach (XElement item in body.Elements() ) //  Descendants())
            {
                string itemName = item.Name.LocalName;

                SettingNameEnum settingName;
                if (Enum.TryParse<SettingNameEnum>(itemName, out settingName))
                {
                    if (_settings.ContainsKey(settingName))
                    {
                        _settings[settingName].RawValue = item;

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

        public void SetConstructor(Type typeToInstantiate)
        {
           
            Expression<Func<ITaskConfiguration, object>> lambda;
           
                ParameterExpression param = Expression.Parameter(typeof(ITaskConfiguration), "taskConfiguration");
                System.Reflection.ConstructorInfo constructor = typeToInstantiate.GetConstructor(new[] { typeof(ITaskConfiguration) });
                if (constructor == null)
                    throw new Exception("No matching constructor found for type " + typeToInstantiate.ToString());

                lambda = Expression.Lambda<Func<ITaskConfiguration, object>>(System.Linq.Expressions.Expression.New(constructor, param), param);
         
   
           _instantiator = lambda.Compile();
         }

        public object Instantiate()
        {
            if (_instantiator == null)
                throw new EZEtlException("Instantiate() called with uninitialized _instantiator");

            return _instantiator(this);
      
        }
    }
}
