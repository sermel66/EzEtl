using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration.Setting
{
    public interface ISettingValidator
    {
        bool Validate();
        string ErrorMessage { get; }
    }
}
