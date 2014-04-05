using EZEtl.Configuration;
using EZEtl.Configuration.Misc;
using EZEtl.Configuration.Setting;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using Utilities;

namespace EZEtl.Modules
{
/*
      	<SqlNonQuery ID="ProcessFeed">
		<ConnectionStringName>MyLocalSql</ConnectionStringName>
		<Command>
			<CommandText>dbo.AdHocLoad</CommandText>
			<Argument>
				<Name>FeedName</Name>
				<Value>AdHocTest</Value>
				<Direction>INPUT</Direction>
			</Argument>
			<Argument>
				<Name>FeedDate</Name>
				<Value>2014-03-23</Value>
				<Direction>INPUT</Direction>
			</Argument>
		</Command>
	</SqlNonQuery>
*/

    public class SqlNonQueryModule : SimpleModule
    {
        NestedSetting _commandSetting;


        public SqlNonQueryModule (IConfigurationParent parent, ModuleTypeEnum moduleType, string id, XElement xmlItemModule )
            : base (parent,moduleType,id, xmlItemModule)
        {

            // Connection
            _taskConfiguration.AddSetting(new SimpleSetting<string>(_taskConfiguration, SettingNameEnum.ConnectionStringName, SettingTypeEnum.String, false));
            _taskConfiguration.AddSetting(new SimpleSetting<int>(_taskConfiguration, SettingNameEnum.DbOperationTimeout, SettingTypeEnum.String, true, Defaults.DbOperationTimeout));

            // Command
            _commandSetting = new NestedSetting(_taskConfiguration, SettingNameEnum.Command, false);
            _commandSetting.AddSetting(new SimpleSetting<string>(_commandSetting, SettingNameEnum.CommandText, SettingTypeEnum.String, false), 1);

            SettingListBase argumentListSetting = 
                new Configuration.Setting.StoredProcedureArgumentSettingList(_commandSetting, Configuration.SettingNameEnum.Argument, 0, int.MaxValue);

            _commandSetting.AddSetting(argumentListSetting, int.MaxValue);
            _taskConfiguration.AddSetting(_commandSetting);

            _taskConfiguration.Parse(xmlItemModule);

            // Constructor
            _taskConfiguration.NeedsInstantiator = false;

        }


        public override void Execute()
        {
            string debugMessage;
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);
            Dictionary<DbParameter,string> outputVariable = new Dictionary<DbParameter,string>();

            using (DbConnection connection = EZEtl.Modules.Util.DbProviderConnection.Create(_taskConfiguration))
            {
                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = _commandSetting.GetSetting(SettingNameEnum.CommandText).Value.ToString().Trim();
                if (cmd.CommandText.ToUpperInvariant().StartsWith("SELECT"))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = (int)_taskConfiguration.GetSetting(SettingNameEnum.DbOperationTimeout).Value;

                ISetting argumentListSetting = _commandSetting.GetSetting(Configuration.SettingNameEnum.Argument);
                List<ISetting> args = (List<ISetting>)argumentListSetting.Value;

                foreach (NestedSetting argSetting in args)
                {
                    DbParameter par = cmd.CreateParameter();
                    par.DbType = (DbType) argSetting.GetSetting(SettingNameEnum.DbType).Value;

                    ISetting sizeSetting=argSetting.GetSetting(SettingNameEnum.Size);
                    if (sizeSetting.IsPresent)
                        par.Size = (int)sizeSetting.Value;

                    par.ParameterName = argSetting.GetSetting(SettingNameEnum.Name).Value.ToString();
                    par.Direction = (ParameterDirection)argSetting.GetSetting(SettingNameEnum.Direction).Value;
                    
                    ISetting inputValueSetting=argSetting.GetSetting(SettingNameEnum.InputValue);
                    if (inputValueSetting.IsPresent)
                        par.Value = inputValueSetting.Value;

                    cmd.Parameters.Add(par);

                    ISetting outputVariableSetting = argSetting.GetSetting(SettingNameEnum.OutputVariable);
                    if (outputVariableSetting.IsPresent)
                       outputVariable.Add(par, outputVariableSetting.Value.ToString());
                }

                int rc=int.MinValue;
                try
                {
                    connection.Open();
                    rc = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    Misc.Terminate.FatalError(ex.Message);
                }

                debugMessage = "SQL NonQuery [" + cmd.CommandText + "] has affected " + rc.ToString() + " rows";
                SimpleLog.ToLog(debugMessage, SimpleLogEventType.Debug);

                // Output parameters
                foreach ( DbParameter par in outputVariable.Keys )
                {
                    string variableName = outputVariable[par];

                    debugMessage = "Non-Input parameter [" + par.ParameterName + "] populates variable [" + variableName + "] with value [" + par.Value.ToString() +"]";
                    SimpleLog.ToLog(debugMessage, SimpleLogEventType.Debug);

                    IVariable variable = Program.Configuration.GetVariable(variableName);
                    variable.Value = par.Value;
                }

            } // end of connection
        }
    }
}
