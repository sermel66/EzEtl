using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration;

namespace EZEtl
{
    public class ProcessModule
    {
        const string LogFormat = "{0}.{1}:{2}";

        Source.ISource _sourceTask = null;
        Destination.IDestination _destinationTask = null;
        public Destination.IDestination DestinationTask { get { return _destinationTask; } }

        public ProcessModule(Configuration.Module module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            bool hasConfigurationErrors = false;

            string moduleName = module.Name;
            Source.ISource wrappedSource= null;

            foreach (TaskCategoryEnum category in (TaskCategoryEnum[])Enum.GetValues(typeof(TaskCategoryEnum)))
            {

                Configuration.Task task = module.GetTask(category);

                if (task == null)
                    continue;

                if ( task.HasErrors )
                {
                    hasConfigurationErrors = true;
                    Utilities.SimpleLog.ToLog(
                        String.Format(LogFormat,
                        module,
                        category.ToString(),
                        task.Errors
                    ), Utilities.SimpleLogEventType.Error);
                }

                if ( task.HasWarnings )
                {
                    Utilities.SimpleLog.ToLog(
                      String.Format(LogFormat,
                      module,
                      category.ToString(),
                      task.Warnings
                  ), Utilities.SimpleLogEventType.Warning);

                }

                if ( category != TaskCategoryEnum.Source && _sourceTask == null )
                {
                    throw new ConfigurationException("Source task is required by not defined for module " + module.Name);
                }


                switch (category)
                {
                    case TaskCategoryEnum.Source:
                        _sourceTask = Source.SourceFactory.CreateSource(task);
                        wrappedSource = _sourceTask;
                        break;
                    case TaskCategoryEnum.Filter1:
                    case TaskCategoryEnum.Filter2:
                    case TaskCategoryEnum.Filter3:
                    case TaskCategoryEnum.Filter4:
                    case TaskCategoryEnum.Filter5:
                    case TaskCategoryEnum.Filter6:
                    case TaskCategoryEnum.Filter7:
                    case TaskCategoryEnum.Filter8:
                    case TaskCategoryEnum.Filter9:
                    case TaskCategoryEnum.Filter10:
                        if ( _sourceTask == null)
                            throw new EZEtlException("Attempt to create filter with no source created");
                        throw new EZEtlException("Filter not implemented yet");
                        break;
                    case TaskCategoryEnum.Destination:
                        _destinationTask = Destination.DestinationFactory.CreateDestination(wrappedSource, task);
                        break;
                    default:
                        throw new EZEtlException("Unexpected TaskCategory " + category.ToString());
                }


                Utilities.SimpleLog.ToLog(category.ToString(), Utilities.SimpleLogEventType.Trace);

            }
        }
    }
}
