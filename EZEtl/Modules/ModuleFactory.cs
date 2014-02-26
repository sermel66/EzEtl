using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public static class ModuleFactory
    {
        private static List<ModuleTypeEnum> CompositeModuleList = new List<ModuleTypeEnum> { ModuleTypeEnum.DataFlow };

        public static IModule Create(IConfigurationParent parent,  XElement xmlItemModule, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (xmlItemModule == null || string.IsNullOrWhiteSpace(xmlItemModule.ToString()))
                throw new ArgumentNullException("xmlItemModule");      

            XAttribute idAttribute = xmlItemModule.Attribute(AttributeNameEnum.ID.ToString());
            if (idAttribute == null || string.IsNullOrWhiteSpace( idAttribute.Value) )
            {
                errorMessage += "Attribute '" + AttributeNameEnum.ID.ToString() + "' is missing or empty; ";
                // in the module ["
                //    + xmlItemModule.ToString().Substring(1, Constant.XmlQuoteLength) + "]; ";
            }

            string moduleTypeString = xmlItemModule.Name.ToString();
            ModuleTypeEnum moduleType;
            if (!Enum.TryParse<ModuleTypeEnum>(moduleTypeString, out moduleType))
            {
                errorMessage += "Unexpected Module Type '" + moduleTypeString + "' encountered; ";
            }

            if (errorMessage.Length > 0)
                return null;

           switch(moduleType)
           {
               case ModuleTypeEnum.DataFlow:
                   return new DataFlow.DataFlowModule(parent, moduleType, idAttribute.Value, xmlItemModule);

               case ModuleTypeEnum.SqlExec:
                   return new SqlExec.SqlExecModule(parent, moduleType, idAttribute.Value, xmlItemModule);

               default:
                   throw new EZEtlException("Unexpected moduleType " + moduleType.ToString());

           }
   

            //Dictionary<TaskTypeEnum, int> taskCategoryInstanceCounter = new Dictionary<TaskTypeEnum, int>();
            //foreach (XElement item in xmlItemModule.Elements())
            //{
            //    string taskTypeString = item.Name.LocalName;
            //    TaskTypeEnum  taskCategory;

            //    if (!Enum.TryParse<TaskTypeEnum>(taskTypeString, out taskCategory))
            //        throw new ConfigurationException(
            //            String.Format("Unexpected Task Category <{0}> in Module {1}", taskTypeString, _name)
            //            );

                //if (_taskDict.ContainsKey(taskCategory))
                //{
                //    if (taskCategoryInstanceCounter.ContainsKey(taskCategory))
                //        taskCategoryInstanceCounter[taskCategory]++;
                //    else
                //    {
                //        _message +=
                //            String.Format("Multiple instances of Task Category <{0}> encountered in the Module {1} "
                //            , taskCategoryString, _name) + Constant.UserMessageSentenceDelimiter;

                //        taskCategoryInstanceCounter.Add(taskCategory, 2);
                //    }
                //}
                //else
                //{
                //    Task task = TaskFactory.ConfigureTask(taskCategory, item);
                //    _taskDict.Add(taskCategory, task);
                //}

            }



        }
    }

