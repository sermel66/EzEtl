using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;
using EZEtl.Configuration;

namespace EZEtl.Workflow
{
    public abstract class OperatorBase : ConfigurationParentBase
    {
        protected string _errorMessage = string.Empty;

        protected ConfigurationFile _scope;

        public virtual void OutputDiagnostics()
        {
            if (_errorMessage.Length > 0)
                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
        }

        public virtual bool IsValid { get { return string.IsNullOrWhiteSpace(_errorMessage); } }

        public OperatorBase(ConfigurationFile scope, IConfigurationParent parent, OperatorEnum operatorType, string qualifierID )
            : base(parent, operatorType.ToString(), qualifierID)
        {

            if (scope == null)
                throw new ArgumentNullException("scope");

            _scope = scope;
        }
    }
}
