﻿using EZEtl.Source;
using System.Data;
using System.Threading.Tasks;
using Utilities;
using Configuration.Destination;

namespace EZEtl.Destination
{
    public abstract class DestinationBase : IDestination
    {
        ISource _inputTask;
        protected ISource InputTask { get { return _inputTask; } }

        ExistingDataActionEnum _existingDataAction = ExistingDataActionEnum.UNDEFINED;
        public ExistingDataActionEnum ExistingDataAction { get { return _existingDataAction; } }

        int _debugMessagePerNumberOfBatches = -1;
        int _batchesProcessed = 0;
        int _rowsProcessed = 0;

        public DestinationBase(ISource inputTask, Configuration.Task task)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            if (inputTask == null)
                throw new System.ArgumentNullException("inputTask");

            _inputTask = inputTask;

            _existingDataAction = 
                (ExistingDataActionEnum) task.Setting(Configuration.Destination.DestinationSettingEnum.ExistingDataAction.ToString()).Value;

            _debugMessagePerNumberOfBatches = 
                (int) task.Setting(Configuration.Destination.DestinationSettingEnum.OneDebugMessagePerBatchCount.ToString()).Value;

        }

        public /*async Task*/ void ExecuteAsync()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);

            DataTable inTbl = _inputTask.ReadBatch();
     
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

                inTbl = _inputTask.ReadBatch();

               // await chunkOutputTask;
            }

            Close();
        }

        public abstract void WriteTableChunk(DataTable tableChunk);
        public abstract void Close();
        public abstract void Dispose();

    }
}
