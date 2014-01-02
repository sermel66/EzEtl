using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

namespace Configuration
{
    public class Module
    {
        string _message = string.Empty;
        public string Message { get { return _message; } }

        string _name;
        public string Name { get { return _name; } }

        bool _isEntryPoint;
        public bool IsEntryPoint { get { return _isEntryPoint; } }

        Dictionary<TaskCategoryEnum,Task> _taskDict = new Dictionary<TaskCategoryEnum,Task>();
        public Task GetTask(TaskCategoryEnum taskCategory) { 
            
            if ( _taskDict.ContainsKey(taskCategory))
            return _taskDict[taskCategory];

            return null;
        }
      
        public Module(XElement xmlItemModule)
        {
            if (xmlItemModule == null || string.IsNullOrWhiteSpace(xmlItemModule.ToString()))
                throw new ArgumentNullException("xmlItemModule");

            _name = xmlItemModule.Attribute(AttributeNameEnum.name.ToString()).Value;
            if (String.IsNullOrWhiteSpace(_name))
                throw new ConfigurationException("name attribute undefined in the module ["
                    + xmlItemModule.ToString().Substring(1, Constant.XmlQuoteLength) + "]");
           
           string isEntryPointString = xmlItemModule.Attribute(AttributeNameEnum.is_entry_point.ToString()).Value;
           if (!String.IsNullOrWhiteSpace(isEntryPointString))
           {
               if (!Boolean.TryParse(isEntryPointString, out _isEntryPoint))
               {
                   _message += String.Format("Attribute value is_entry_point={0} could not be converted to Boolean", isEntryPointString);
               }
                   
           }

           Dictionary<TaskCategoryEnum, int> taskCategoryInstanceCounter = new Dictionary<TaskCategoryEnum, int>();
            foreach (XElement item in xmlItemModule.Elements())
            {
                string taskCategoryString = item.Name.LocalName;
                TaskCategoryEnum  taskCategory;

                if (!Enum.TryParse<TaskCategoryEnum>(taskCategoryString, out taskCategory))
                    throw new ConfigurationException(
                        String.Format("Unexpected Task Category <{0}> in Module {1}", taskCategoryString, _name)
                        );

                if (_taskDict.ContainsKey(taskCategory))
                {
                    if (taskCategoryInstanceCounter.ContainsKey(taskCategory))
                        taskCategoryInstanceCounter[taskCategory]++;
                    else
                    {
                        _message +=
                            String.Format("Multiple instances of Task Category <{0}> encountered in the Module {1} "
                            , taskCategoryString, _name) + Constant.UserMessageSentenceDelimiter;

                        taskCategoryInstanceCounter.Add(taskCategory, 2);
                    }
                }
                else
                {
                    Task task = TaskFactory.ConfigureTask(taskCategory, item);
                    _taskDict.Add(taskCategory, task);
                }

            }


        }
    }
}
