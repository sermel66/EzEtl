using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;

namespace EZEtl.Workflow
{
    public abstract class OperatorBase : ConfigurationParentBase
    {
        protected string _errorMessage = string.Empty;

        public void OutputDiagnostics()
        {
            if (_errorMessage.Length > 0)
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
        }

        public virtual bool IsValid { get { return string.IsNullOrWhiteSpace(_errorMessage); } }

        public OperatorBase(  IConfigurationParent parent, OperatorEnum operatorType, string qualifierID )
            : base(parent, operatorType.ToString(), qualifierID)
        { }
    }
}
