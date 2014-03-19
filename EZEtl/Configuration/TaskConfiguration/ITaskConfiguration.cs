using EZEtl.Configuration.Misc;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EZEtl.Configuration
{
    public interface ITaskConfiguration : IDiagnosable, IConfigurationParent
    {
       DataFlowStepEnum DataFlowStep {get;}
       string TaskID { get; }
       ISetting GetSetting(SettingNameEnum settingName);
       IEnumerable<SettingNameEnum> SettingNameList { get; }
       void Parse(XElement item);
       object Instantiate();
       void SetConstructor(System.Type typeToInstantiate);
    }
}
