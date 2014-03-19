//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace EZEtl.Destination
//{
//    public static class DestinationFactory
//    {
//        public static IDestination CreateDestination(Source.ISource source, Configuration.TaskConfiguration task)
//        {
//            IDestination result = null;

//            switch (task.TaskID)
//            {
//                case "FILE":
//                    result = new EZEtl.Destination.FileCsv(source, task);
//                    break;

//                case "SQLBULK":
//                    result = new EZEtl.Destination.SqlBulkDestination(source, task);
//                    break;

//                default:
//                    throw new Configuration.ConfigurationException("Unexpected Destination type [" + task.TaskID + "]");

//            }

//            return result;
//        }
//    }
//}
