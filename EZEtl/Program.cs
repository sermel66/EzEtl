using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities;
using System.Threading.Tasks;
using EZEtl.Misc;
using EZEtl.Configuration;

namespace EZEtl
{
    class ExitCodes
    {
        public const int 
                Success = 0
              , Failure = 1;
    }

    class Program
    {

        const string CommandLineSwitchIndicator = "-";
        const string CommandLineSwitchValueSeparator = "=";
        const string LogFileTimeStampFormat = "yyyy-MM-dd_HH_mm_ss";

        static ConfigurationFile _configurationFile;
        public static ConfigurationFile Configuration { get { return _configurationFile; } }

        static int Main(string[] args)
        {
            try
            {

                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


                string configFilePath = string.Empty;
                List<string> processedConfigFilePathList = null;

                VerbosityLevel verbosityLevel = VerbosityLevel.Quiet;
                DateTime dateIteratorFixedValue = DateTime.MinValue;
                List<string> commandLineErrorMessages = new List<string>();
                Dictionary<string, string> freeFormVariable = new Dictionary<string, string>();

                for (int i = 0; i < args.Length; i++)
                {
                    string argumentString = args[i];
                    bool isSwitch = false;

                    if (argumentString.Contains(CommandLineSwitchValueSeparator))
                    {
                        string[] switchSplit = argumentString.Split(
                                CommandLineSwitchValueSeparator.ToCharArray()
                                , StringSplitOptions.None);

                        string argumentKeyword = switchSplit[0];
                        string argumentValue = switchSplit[1];

                        if (argumentKeyword.StartsWith(CommandLineSwitchIndicator))
                        {
                            isSwitch = true;
                            argumentKeyword = argumentKeyword.TrimStart(CommandLineSwitchIndicator.ToCharArray()).ToUpperInvariant();
                        }

                        if (argumentKeyword.Length < 1)
                        {
                            commandLineErrorMessages.Add(
                                    String.Format("Emply keyword in the argument [{0}]", argumentString)
                                    );
                            continue;
                        }

                        if (isSwitch)
                        {
                            switch (argumentKeyword)
                            {
                                case "DATE":
                                    if (!DateTime.TryParse(argumentValue, out dateIteratorFixedValue))
                                    {
                                        commandLineErrorMessages.Add(
                                            String.Format("Argument [{0}] does not represent valid date", argumentString)
                                            );
                                    }
                                    break;

                                default:
                                    commandLineErrorMessages.Add(
                                            String.Format("Unexpected switch with value encountered: [{0}] ", argumentString)
                                            );
                                    break;
                            }
                        }
                        else
                        {
                            // Free-form variables ( case-sensitive )
                            freeFormVariable[argumentKeyword] = argumentValue;
                        }

                    } // End of key-value pairs
                    else
                    {
                        // switches 
                        if (argumentString.StartsWith(CommandLineSwitchIndicator))
                        {
                            string switchKeyword = argumentString.TrimStart(CommandLineSwitchIndicator.ToCharArray()).ToUpperInvariant();
                            switch (switchKeyword)
                            {
                                case "V":
                                    verbosityLevel = VerbosityLevel.Verbose;
                                    break;
                                case "D":
                                    verbosityLevel = VerbosityLevel.Debug;
                                    break;
                                case "Q":
                                    verbosityLevel = VerbosityLevel.Quiet;
                                    break;
                                case "U":
                                    verbosityLevel = VerbosityLevel.Usage;
                                    break;
                                default:
                                    commandLineErrorMessages.Add(
                                            String.Format("Unexpected argument encountered: [{0}] ", argumentString)
                                            );
                                    break;
                            }
                        } // End of switches
                        else
                        // Config file path
                        {
                            configFilePath = argumentString;
                            if (!System.IO.File.Exists(configFilePath))
                            {
                                commandLineErrorMessages.Add(
                                          String.Format("Invalid file name or file does not exist or inaccessible: [{0}] ", configFilePath)
                                          );
                            }
                        }
                    }

                } // end of argument loop

                if (string.IsNullOrWhiteSpace(configFilePath))
                    commandLineErrorMessages.Add("Required argument <Configuration File Path> must be provided");

                if (commandLineErrorMessages.Count > 0)
                {
                    Console.WriteLine("ERROR:");
                    Console.WriteLine();

                    foreach (string errorMessage in commandLineErrorMessages)
                    {
                        Console.WriteLine(errorMessage);
                    }
                    Console.WriteLine();
                }

                if (verbosityLevel == VerbosityLevel.Usage || commandLineErrorMessages.Count > 0)
                {
                     Usage();
                     Environment.Exit(commandLineErrorMessages.Count);
                }

                // ============= End of Command Line, Begin Configuration =============

                string logName = @"c:\temp\ezetl.log"; // TODO
                Utilities.SimpleLog.SetUpLog(logName,SimpleLogEventType.Trace,false);
                switch ( verbosityLevel )
                {
                    case VerbosityLevel.Debug:
                        Utilities.SimpleLog.MaxLoggingConsoleVerbosity = SimpleLogEventType.Debug;
                        break;
                    case VerbosityLevel.Verbose:
                        Utilities.SimpleLog.MaxLoggingConsoleVerbosity = SimpleLogEventType.Information;
                            break;
                    case VerbosityLevel.Quiet:
                            Utilities.SimpleLog.MaxLoggingConsoleVerbosity = SimpleLogEventType.None;
                            break;
                    default:
                            throw new NotImplementedException("VerbosityLevel = " + verbosityLevel.ToString());
                }

                Utilities.SimpleLog.ToLog("EzETL Version " + version, SimpleLogEventType.Information);

                EZEtl.Configuration.Misc.AppConfig.Load();

                _configurationFile = new Configuration.ConfigurationFile(configFilePath, processedConfigFilePathList);
                if ( _configurationFile.IsValid)
                {
                    Utilities.SimpleLog.ToLog("Configration Loaded", SimpleLogEventType.Trace);               
                }
                else
                {
                    Utilities.SimpleLog.ToLog("Configration is invalid", SimpleLogEventType.Trace);
                    _configurationFile.OutputDiagnostics();
                    return ExitCodes.Failure;
                }

                Utilities.SimpleLog.ToLog("Begin workflow execution", SimpleLogEventType.Trace);
                _configurationFile.OuterWorkflowOperatorBlock.Execute();

                return ExitCodes.Success;

            }
            catch (Exception ex)
            {
                SimpleLog.ToLog(ex.ToString(), SimpleLogEventType.Error);         

                if (ex.InnerException != null && ex.InnerException != ex)
                {
                    SimpleLog.ToLog(ex.InnerException.ToString(), SimpleLogEventType.Error);
                }

                return ExitCodes.Failure;
            }
        }

        static void Usage()
        {
            System.Console.WriteLine("Usage:");
            System.Console.WriteLine();

            System.Console.WriteLine(
                System.AppDomain.CurrentDomain.FriendlyName
            + " <Configuration File Path>");

            System.Console.WriteLine("      [-d|-v|-q|-u]");
            System.Console.WriteLine("      [-DATE=<date>]");
            System.Console.WriteLine("      [{<Variable>=<Value>}]");
        }
    }
}
