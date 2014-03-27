using EZEtl.Configuration.Misc;
using System.Xml.Linq;

namespace EZEtl.Configuration.Setting
{
    public interface ISetting : IDiagnosable, IConfigurationParent 
    {
        bool IsOptional { get; }
        bool IsPresent { get; }

        SettingNameEnum SettingName { get; }
        SettingTypeEnum SettingType { get; }
        object Value { get; }
        XElement RawValue { get; set; }

    }
}
