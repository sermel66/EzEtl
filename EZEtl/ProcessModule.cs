using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration;

namespace EZEtl
{
    public static class Processing
    {

        public static bool ProcessModule(Configuration.Module module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            bool hasErrors = false;

            string name = module.Name;

            Source.ISource source = null;

            foreach (TaskCategoryEnum category in (TaskCategoryEnum[])Enum.GetValues(typeof(TaskCategoryEnum)))
            {

                Configuration.Task task = module.GetTask(category);

                if (task == null)
                    continue;

                //switch (category)
                //{
                //    case TaskCategoryEnum.Source:
                //        source = Source.SourceFactory.CreateSource(task);

                //}


                Utilities.SimpleLog.ToLog(category.ToString(), Utilities.SimpleLogEventType.Trace);


            }
            return hasErrors;
        }
    }
}
