using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities;
using System.Threading.Tasks;

namespace EZEtl
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=MELIKHOV-THINK\sqlexpress; Initial Catalog=TestAssignment; Integrated Security = True";
            string query = @"

   SELECT --TOP 10000
       [year]
      ,[StatePostalCode]
      ,[StateFipsCode]
      ,[CountyFipsCode]
      ,[Registry]
      ,[RaceId]
      ,[OriginId]
      ,[SexId]
      ,[AgeGroupId]
      ,[Population]
FROM [dbo].[Census]";


            try
            {
                string logFileTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH_mm_ss");

                string fqLogFilePath = String.Format(@"C:\Temp\EzEtl\ExEtl_{0}.log", logFileTimeStamp);
                SimpleLogEventType maxLoggingVerbosity = SimpleLogEventType.Debug;
                SimpleLog.SetUpLog(fqLogFilePath, maxLoggingVerbosity, true);

                PipeIn.SqlClientInput rdr = new PipeIn.SqlClientInput(1024, connectionString, query, 500);

                PipeOut.FileCsvOutput wrt = new PipeOut.FileCsvOutput(rdr, @"C:\Temp\EzEtl\PipeOut.csv", @",", @"""");

               /* Task task = Task.Run(() => */ wrt.ExecuteAsync()/*)*/;
              /*  Task.WaitAll(task); */
                wrt.Close();
            }
            catch ( Exception ex)
            {

                System.Console.WriteLine(ex.Message);
                SimpleLog.ToLog(ex.Message, SimpleLogEventType.Error);
                SimpleLog.ToLog(ex.StackTrace, SimpleLogEventType.Error);

                if(ex.InnerException != null && ex.InnerException != ex)
                {
                    System.Console.WriteLine(ex.InnerException.Message);
                    SimpleLog.ToLog(ex.InnerException.Message, SimpleLogEventType.Error);
                    SimpleLog.ToLog(ex.InnerException.StackTrace, SimpleLogEventType.Error);
                }
     
            }


            //DataTable bt = rdr.ReadBatch();
            //int gotCount = bt.Rows.Count;
            //bt = rdr.ReadBatch();
            //gotCount = bt.Rows.Count;

        }
    }
}
