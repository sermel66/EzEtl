using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuration.Setting
{
    public abstract class SettingBase
    {
        protected string _warning = string.Empty;
        public string WarningMessage { get { return _warning; } }

        protected string _message = string.Empty;
        public string ErrorMessage { get { return _message; } }
        public bool HasMessage { get { return !string.IsNullOrWhiteSpace(_message); } }

        protected bool _isOptional = false;
        public bool IsOptional { get { return _isOptional; } }

        protected string _key;
        public string Key { get { return _key; } }

        protected bool _isPresent = false;
        public bool IsPresent { get { return _isPresent; } }

        protected bool _isValid = false;
        public bool IsValid { get { return _isPresent && _isValid; } }

    }
}
