using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public abstract class ModuleBase : ConfigurationParentBase
    {
        ModuleTypeEnum _moduleType;
        public ModuleTypeEnum ModuleType { get { return _moduleType; } }

        string _moduleID;
        public string ModuleID { get { return _moduleID; } }

        protected string _errorMessage;

        public ModuleBase(IConfigurationParent parent, ModuleTypeEnum moduleType, string moduleID)
            : base(parent, moduleType.ToString(), moduleID)
        {
            if (string.IsNullOrWhiteSpace(moduleID))
                throw new ArgumentNullException("moduleID");

            _moduleID = moduleID;
            _moduleType = moduleType;
        }
    }
}
