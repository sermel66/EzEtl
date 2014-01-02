using System;

namespace Configuration.Setting
{
    public interface ISetting
    {
        string Key { get; }
        string ErrorMessage { get; }
        string WarningMessage { get; }
        Type ValueType { get; }
        string RawValue { get; set; }
        object Value { get; }

        bool IsValid { get; }
    }
}
