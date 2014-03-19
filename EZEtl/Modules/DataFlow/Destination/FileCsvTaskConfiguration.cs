using EZEtl.Configuration;
using EZEtl.Configuration.Settings;

namespace EZEtl.Destination
{
    public static class FileCsvTaskConfiguration 
    {
        public static void Configure ( TaskConfiguration task )
        {
            FileTaskConfiguration.Configure(task);

            task.AddSetting(new Setting<string>(task, SettingNameEnum.Delimiter, SettingTypeEnum.Int32, false));
            task.AddSetting(new Setting<string>(task, SettingNameEnum.TextQualifier, SettingTypeEnum.Int32, false));

            task.SetConstructor(typeof(FileCsv));
        }
    }
}
