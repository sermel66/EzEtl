using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public abstract class CompositeModule : ModuleBase, IModule
    {

        public abstract List<TaskConfiguration> TaskConfigurationList { get; }

        public bool IsValid
        {
            get
            {
                if ( TaskConfigurationList.Count() < 1)
                    return false;

                if (!string.IsNullOrWhiteSpace(_errorMessage))
                    return false;

                foreach (TaskConfiguration tc in TaskConfigurationList )
                {
                    if (!tc.IsValid)
                        return false;
                }

                return true;
            }
        }

        public void OutputDiagnostics()
        {
            if ( TaskConfigurationList.Count < 1 )
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, "No tasks configured in the module");

            if ( !string.IsNullOrWhiteSpace( _errorMessage) )
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);

            foreach (TaskConfiguration tc in TaskConfigurationList)
            {
                tc.OutputDiagnostics();
            }
        }

        public abstract void Execute();
     

        public CompositeModule(IConfigurationParent parent, ModuleTypeEnum moduleType, string id, XElement xmlItemModule)
            : base(parent, moduleType, id)
        {
            //  _taskConfiguration = new TaskConfiguration();
            int i = 0;

        }

    }
}
