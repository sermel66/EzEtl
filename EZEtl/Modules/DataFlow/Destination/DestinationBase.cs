using EZEtl.Source;
using System.Data;
using System.Threading.Tasks;
using Utilities;
using EZEtl.Configuration;

namespace EZEtl.Destination
{
    public abstract class DestinationBase : IDestination
    {
        ISource _sourceTask;
        protected ISource InputTask { get { return _sourceTask; } }

        ExistingDataActionEnum _existingDataAction = ExistingDataActionEnum.UNDEFINED;
        public ExistingDataActionEnum ConfiguredExistingDataAction { get { return _existingDataAction; } }

        int _debugMessagePerNumberOfBatches = -1;
        int _batchesProcessed = 0;
        int _rowsProcessed = 0;

        public DestinationBase(ITaskConfiguration task)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            _existingDataAction = (ExistingDataActionEnum) task.GetSetting(SettingNameEnum.ExistingDataAction).Value;
            _debugMessagePerNumberOfBatches =(int)task.GetSetting(SettingNameEnum.OneDebugMessagePerBatchCount).Value;

        }

        public void SetSource(ISource sourceTask)
        {
            if (sourceTask == null)
                throw new System.ArgumentNullException("sourceTask");

            _sourceTask = sourceTask;
        }

        public /*async Task*/ void ExecuteAsync()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            DataTable inTbl = _sourceTask.ReadBatch();
     
            SimpleLog.ToLog("First batch: inTbl.Rows.Count=" + inTbl.Rows.Count.ToString(), SimpleLogEventType.Debug);

            while ( inTbl != null && inTbl.Rows.Count > 0 )
            {

                DataTable processingTbl = inTbl;
                /*  Task chunkOutputTask = Task.Run(() => */
                WriteTableChunk(processingTbl)/*)*/;

                _batchesProcessed++;
                _rowsProcessed += inTbl.Rows.Count;

                if ( _debugMessagePerNumberOfBatches > 0 &&  _batchesProcessed % _debugMessagePerNumberOfBatches == 0)
                    SimpleLog.ToLog(_rowsProcessed.ToString() +" rows transferred" , SimpleLogEventType.Debug);

                inTbl = _sourceTask.ReadBatch();

               // await chunkOutputTask;
            }

            Close();
        }


        public abstract void ExistingDataAction();
        public abstract void WriteTableChunk(DataTable tableChunk);
        public abstract void Close();
        public abstract void Dispose();

    }
}
