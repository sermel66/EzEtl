using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Source
{
    public static class SourceFactory
    {

        public static ISource CreateSource(Configuration.TaskConfiguration task)
        {
            ISource result = null;

            switch(task.TaskID)
            {
                case "FILE":  
                    result = new EZEtl.Source.File(task);
                    break;

                case "SQL":
                    result = new EZEtl.Source.SqlClient(task);
                    break;


                default:
                    throw new Configuration.ConfigurationException("Unexpected Source implementation [" + task.TaskID + "]");

            }

            return result;
        }
    }
}
