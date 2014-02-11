//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Xml.Linq;

//namespace EZEtl.Configuration
//{
//    public class Module
//    {
//        string _message = string.Empty;
//        public string Message { get { return _message; } }

//        string _name;
//        public string Name { get { return _name; } }
       
      
//        public Module(XElement xmlItemModule)
//        {
//            if (xmlItemModule == null || string.IsNullOrWhiteSpace(xmlItemModule.ToString()))
//                throw new ArgumentNullException("xmlItemModule");

//            _name = xmlItemModule.Attribute(AttributeNameEnum.name.ToString()).Value;
//            if (String.IsNullOrWhiteSpace(_name))
//                throw new ConfigurationException("name attribute undefined in the module ["
//                    + xmlItemModule.ToString().Substring(1, Constant.XmlQuoteLength) + "]");
           
          

//           Dictionary<DataFlowStepEnum, int> taskCategoryInstanceCounter = new Dictionary<DataFlowStepEnum, int>();
//            foreach (XElement item in xmlItemModule.Elements())
//            {
//                string taskCategoryString = item.Name.LocalName;
//                DataFlowStepEnum  taskCategory;

//                if (!Enum.TryParse<DataFlowStepEnum>(taskCategoryString, out taskCategory))
//                    throw new ConfigurationException(
//                        String.Format("Unexpected Task Category <{0}> in Module {1}", taskCategoryString, _name)
//                        );

//                //if (_taskDict.ContainsKey(taskCategory))
//                //{
//                //    if (taskCategoryInstanceCounter.ContainsKey(taskCategory))
//                //        taskCategoryInstanceCounter[taskCategory]++;
//                //    else
//                //    {
//                //        _message +=
//                //            String.Format("Multiple instances of Task Category <{0}> encountered in the Module {1} "
//                //            , taskCategoryString, _name) + Constant.UserMessageSentenceDelimiter;

//                //        taskCategoryInstanceCounter.Add(taskCategory, 2);
//                //    }
//                //}
//                //else
//                //{
//                //    Task task = TaskFactory.ConfigureTask(taskCategory, item);
//                //    _taskDict.Add(taskCategory, task);
//                //}

//            }


//        }
//    }
//}
