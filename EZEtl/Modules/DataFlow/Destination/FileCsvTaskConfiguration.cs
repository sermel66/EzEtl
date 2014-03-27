using EZEtl.Configuration;
using EZEtl.Configuration.Setting;

namespace EZEtl.Destination
{
    public static class FileCsvTaskConfiguration 
    {
        public static void Configure ( TaskConfiguration task )
        {
            FileTaskConfiguration.Configure(task);

            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.Delimiter, SettingTypeEnum.Int32, false));
            task.AddSetting(new SimpleSetting<string>(task, SettingNameEnum.TextQualifier, SettingTypeEnum.Int32, false));

            task.SetConstructor(typeof(FileCsv));
        }
    }
}
