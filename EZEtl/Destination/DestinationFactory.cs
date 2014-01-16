using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Destination
{
    public static class DestinationFactory
    {
        public static IDestination CreateDestination(Source.ISource source, Configuration.Task task)
        {
            IDestination result = null;

            switch (task.Type)
            {
                case "FILE":
                    result = new EZEtl.Destination.FileCsv(source, task);
                    break;


                default:
                    throw new Configuration.ConfigurationException("Unexpected Source type [" + task.Type + "]");

            }

            return result;
        }
    }
}
