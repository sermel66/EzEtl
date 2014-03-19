//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace EZEtl.Configuration.Misc
//{
//    public abstract class DiagnosableBase
//    {
//        protected string _errorMessage = string.Empty;

//        protected ConfigurationFile _scope;

//        public virtual void OutputDiagnostics()
//        {
//            if (_errorMessage.Length > 0)
//                Diagnostics.Output(this.ConfigurationHierarchy, MessageSeverityEnum.Error, _errorMessage);
//        }

//        public virtual bool IsValid { get { return string.IsNullOrWhiteSpace(_errorMessage); } }
//    }
//}
