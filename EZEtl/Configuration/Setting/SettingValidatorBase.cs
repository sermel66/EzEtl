using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration.Setting
{
    public abstract class SettingValidatorBase : ISettingValidator
    {
        protected string _errorMessage = string.Empty;
        protected ISetting _setting;

        public SettingValidatorBase(ISetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            _setting = setting;
        }

        public abstract bool Validate();

        public virtual string ErrorMessage { get { return _errorMessage; } } 
    }
}
