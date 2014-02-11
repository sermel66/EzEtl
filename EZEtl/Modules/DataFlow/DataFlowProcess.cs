using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Modules;

namespace EZEtl
{
    public class DataFlowProcess
    {
        const string LogFormat = "{0}.{1}:{2}";

        Source.ISource _sourceTask = null;
        Destination.IDestination _destinationTask = null;
        public Destination.IDestination DestinationTask { get { return _destinationTask; } }

        public DataFlowProcess(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

          //  bool hasConfigurationErrors = false;

            //string moduleName = module.Name;
            //Source.ISource wrappedSource= null;

            //foreach (DataFlowStepEnum step in (DataFlowStepEnum[])Enum.GetValues(typeof(DataFlowStepEnum)))
            //{

            //    TaskConfiguration task = module.GetTask(step);

            //    if (task == null)
            //        continue;

            //    if ( task.HasErrors )
            //    {
            //  //      hasConfigurationErrors = true;
            //        Utilities.SimpleLog.ToLog(
            //            String.Format(LogFormat,
            //            module,
            //            step.ToString(),
            //            task.Errors
            //        ), Utilities.SimpleLogEventType.Error);
            //    }

            //    if ( task.HasWarnings )
            //    {
            //        Utilities.SimpleLog.ToLog(
            //          String.Format(LogFormat,
            //          module,
            //          step.ToString(),
            //          task.Warnings
            //      ), Utilities.SimpleLogEventType.Warning);

            //    }

            //    if ( step != TaskCategoryEnum.Source && _sourceTask == null )
            //    {
            //        throw new ConfigurationException("Source task is required but not defined for module " + module.Name);
            //    }


            //    switch (step)
            //    {
            //        case TaskCategoryEnum.Source:
            //            _sourceTask = Source.SourceFactory.CreateSource(task);
            //            wrappedSource = _sourceTask;
            //            break;
            //        case TaskCategoryEnum.Filter1:
            //        case TaskCategoryEnum.Filter2:
            //        case TaskCategoryEnum.Filter3:
            //        case TaskCategoryEnum.Filter4:
            //        case TaskCategoryEnum.Filter5:
            //        case TaskCategoryEnum.Filter6:
            //        case TaskCategoryEnum.Filter7:
            //        case TaskCategoryEnum.Filter8:
            //        case TaskCategoryEnum.Filter9:
            //        case TaskCategoryEnum.Filter10:
            //            if ( _sourceTask == null)
            //                throw new EZEtlException("Attempt to create filter with no source created");
            //            throw new EZEtlException("Filter not implemented yet");
            //            break;
            //        case TaskCategoryEnum.Destination:
            //            _destinationTask = Destination.DestinationFactory.CreateDestination(wrappedSource, (Configuration.Destination.DestinationTask) task);
            //            break;
            //        default:
            //            throw new EZEtlException("Unexpected TaskCategory " + step.ToString());
     //}


     //           Utilities.SimpleLog.ToLog(step.ToString(), Utilities.SimpleLogEventType.Trace);

     //       }
        }
    }
}
