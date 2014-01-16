using EZEtl.Source;
using System.Data;
using System.Threading.Tasks;
using Utilities;

namespace EZEtl.Destination
{
    public abstract class DestinationBase : IDestination
    {
        ISource _inputTask;
        protected ISource InputTask { get { return _inputTask; } }

        public DestinationBase(ISource inputTask)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);


            if (inputTask == null)
                throw new System.ArgumentNullException("inputTask");

            _inputTask = inputTask;
        }

        public /*async Task*/ void ExecuteAsync()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);


            DataTable inTbl = _inputTask.ReadBatch();
        
            SimpleLog.ToLog("inTbl.Rows.Count=" + inTbl.Rows.Count.ToString(), SimpleLogEventType.Debug);

            while ( inTbl != null && inTbl.Rows.Count > 0 )
            {

                DataTable processingTbl = inTbl;
                /*  Task chunkOutputTask = Task.Run(() => */
                WriteTableChunk(processingTbl)/*)*/;
                inTbl = _inputTask.ReadBatch();

               // await chunkOutputTask;
            }

        }

        public abstract void WriteTableChunk(DataTable tableChunk);
        public abstract void Dispose();

    }
}
