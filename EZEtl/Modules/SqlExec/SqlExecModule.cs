using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;
using EZEtl.Configuration.Settings;

namespace EZEtl.Modules.SqlExec
{

     //<ConnectionStringName>MyLocalSql</ConnectionStringName>
     //    <StoredProcedure ID="dbo.StartDate">
     //       <Argument ID="@FeedName" value="${FeedName}" direction="INPUT" />
     //       <Argument ID="@StartDate" value="${StartDate}" direction="OUTPUT" />
     //   </StoredProcedure>

    public class SqlExecModule : SimpleModule
    {
        public SqlExecModule (IConfigurationParent parent, ModuleTypeEnum moduleType, string id, XElement xmlItemModule )
            : base (parent,moduleType,id, xmlItemModule)
        {

            _taskConfiguration.AddSetting(new Setting<string>(parent, SettingNameEnum.ConnectionStringName, SettingTypeEnum.String, false));
        // TODO    _taskConfiguration.AddSetting(new Setting<SqlQuerySetting>(parent, SettingNameEnum.SqlNonQuery, SettingTypeEnum.SqlQuerySetting, false));


            _taskConfiguration.Parse(xmlItemModule);
        }
    }
}
