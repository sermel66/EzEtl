using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Destination
{
    public static class DestinationFactory
    {
        public static IDestination CreateDestination(Source.ISource source, Configuration.Destination.DestinationTask task)
        {
            IDestination result = null;

            switch (task.DestinationTaskType)
            {
                case Configuration.Destination.DestinationTaskTypeEnum.FILE:
                    result = new EZEtl.Destination.FileCsv(source, task);
                    break;

                case Configuration.Destination.DestinationTaskTypeEnum.SQLBULK:
                    result = new EZEtl.Destination.SqlBulkDestination(source, task);
                    break;

                default:
                    throw new Configuration.ConfigurationException("Unexpected Destination type [" + task.Type + "] DestinationTaskTypeEnum=["
                        + task.DestinationTaskType.ToString() + "]"
                        );

            }

            return result;
        }
    }
}
