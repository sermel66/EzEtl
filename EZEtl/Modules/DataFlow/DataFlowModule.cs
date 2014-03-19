using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules.DataFlow
{
    public class DataFlowModule : CompositeModule
    {

        Dictionary<DataFlowStepEnum, TaskConfiguration> _taskConfigurationDict = new Dictionary<DataFlowStepEnum, TaskConfiguration>();
        List<TaskConfiguration> _taskConfigurationList = new List<TaskConfiguration>();

        public override List<TaskConfiguration> TaskConfigurationList
        {
            get { return _taskConfigurationList; }
        }

        public DataFlowModule(IConfigurationParent parent, ModuleTypeEnum moduleType, string moduleId, XElement xmlItemModule)
            : base(parent, moduleType, moduleId, xmlItemModule)
        {
            bool hasSource = false, hasDestination = false;

            foreach (XElement item in xmlItemModule.Elements())
            {
                string stepString = item.Name.LocalName;
                DataFlowStepEnum step;

                if (!Enum.TryParse<DataFlowStepEnum>(stepString, out step))
                {
                    _errorMessage += String.Format("Unexpected Data Flow Step <{0}>", stepString) + Constant.UserMessageSentenceDelimiter;
                    continue;
                }

                if (_taskConfigurationDict.ContainsKey(step))
                {
                    _errorMessage += String.Format("Multiple instances of Step <{0}>", stepString) + Constant.UserMessageSentenceDelimiter;
                    continue;
                }

                string taskId, errorMessage;
                if (!XmlUtil.TryGetAttribute(item, AttributeNameEnum.ID, out taskId, out errorMessage))
                {
                    _errorMessage += errorMessage + Constant.UserMessageSentenceDelimiter;
                    continue;
                }


                TaskConfiguration taskConfiguration = new TaskConfiguration(this, step, taskId);
                switch (step)
                {
                    case DataFlowStepEnum.Source:
                        hasSource = true;
                        Source.SourceTaskEnum sourceTask;
                        if ( ! Enum.TryParse(taskId, out sourceTask))
                        {
                            _errorMessage += "Unexpected " + step.ToString() + " Source task ID [" + taskId + "]" + Constant.UserMessageSentenceDelimiter;
                            continue;
                        }
                        Source.SourceCommonConfiguration.Configure(taskConfiguration);
                        switch (sourceTask)
                        {
                            case Source.SourceTaskEnum.File:
                                Source.FileTaskConfiguration.Configure(taskConfiguration);
                                break;
                            case Source.SourceTaskEnum.SQL:
                                Source.SqlTaskConfiguration.Configure(taskConfiguration);
                                break;
                            default:
                                throw new NotImplementedException(sourceTask.ToString());
                        }
                        break;
                    case DataFlowStepEnum.Destination:
                        hasDestination = true;
                        Destination.DestinationTaskEnum destTask;

                        if (!Enum.TryParse(taskId, out destTask))
                        {
                            _errorMessage += "Unexpected " + step.ToString() + " Destination task ID [" + taskId + "]" + Constant.UserMessageSentenceDelimiter;
                            continue;
                        }

                        Destination.DestinationCommonConfiguration.Configure(taskConfiguration);
                        switch (destTask)
                        {
                            case Destination.DestinationTaskEnum.File:
                                Destination.FileCsvTaskConfiguration.Configure(taskConfiguration);
                                break;
                            case Destination.DestinationTaskEnum.SqlBulk:
                                Destination.SqlBulkTaskConfiguration.Configure(taskConfiguration);
                                break;
                            default:
                                throw new NotImplementedException(destTask.ToString());
                        }

                        break;

                }

                taskConfiguration.Parse(item);
                _taskConfigurationDict.Add(step, taskConfiguration);
            } // End task parse

            if (!hasSource)
                _errorMessage += "Dataflow module must have " + DataFlowStepEnum.Source.ToString() + " task defined" + Constant.UserMessageSentenceDelimiter;

            if (!hasDestination)
                _errorMessage += "Dataflow module must have " + DataFlowStepEnum.Destination.ToString() + " task defined" + Constant.UserMessageSentenceDelimiter;


            foreach (DataFlowStepEnum step in Enum.GetValues(typeof(DataFlowStepEnum)))
            {
                if (_taskConfigurationDict.ContainsKey(step))
                {
                    _taskConfigurationList.Add(_taskConfigurationDict[step]);
                }
            }

        }

        public override void Execute()
        {

            if (!this.IsValid)
                throw new Exception("Attempt to execute an invalid Module");

            // Instantiate tasks
            Source.ISource wrappedSource = null;
            Destination.IDestination destination = null;
            //foreach(ITaskConfiguration taskConfiguration in  this.TaskConfigurationList)
            for (int taskNumber = 0; taskNumber < this.TaskConfigurationList.Count; taskNumber++ )
            {
                ITaskConfiguration taskConfiguration = this.TaskConfigurationList[taskNumber];
                if ( taskNumber == this.TaskConfigurationList.Count - 1) // Destination
                {
                    if (wrappedSource == null)
                        throw new Utilities.AssertViolationException("wrappedSource == null while instantiating the destination");

                    destination = taskConfiguration.Instantiate() as Destination.IDestination;
                    if (destination == null)
                        throw new Utilities.AssertViolationException("taskConfiguration.Instantiate() failed to produce IDestination object");

                    destination.SetSource(wrappedSource);
                }
                else if ( taskNumber == 0 ) // source
                {
                    wrappedSource = taskConfiguration.Instantiate() as Source.ISource;
                    if ( wrappedSource == null)
                        throw new Utilities.AssertViolationException("taskConfiguration.Instantiate() failed to produce ISource object");
                }
            }

            destination.ExecuteAsync();
        }
    }
}