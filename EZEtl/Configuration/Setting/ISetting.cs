using EZEtl.Configuration.Misc;

namespace EZEtl.Configuration
{
    public interface ISetting : IDiagnosable, IConfigurationParent 
    {
        bool IsOptional { get; }
        bool IsPresent { get; }

        SettingNameEnum SettingName { get; }
        SettingTypeEnum SettingType { get; }
        object Value { get; }
        string RawValue { get; set; }
    }
}
