using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration.Setting
{
    public class DummyValidator : ISettingValidator
    {
        public bool Validate() { return true; }
        public string ErrorMessage { get { return string.Empty; } } 
    }
}
