using EZEtl.PipeIn;
using System.Data;
using System.Threading.Tasks;
using Utilities;

namespace EZEtl.PipeOut
{
    public abstract class PipelineOutput : IPipeOut
    {
        IPipelineIn _inputModule;
        protected IPipelineIn InputModule { get { return _inputModule; } }

        public PipelineOutput(IPipelineIn inputModule)
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);


            if (inputModule == null)
                throw new System.ArgumentNullException("inputModule");

            _inputModule = inputModule;
        }

        public async Task ExecuteAsync()
        {
            SimpleLog.ToLog(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, SimpleLogEventType.Trace);


            DataTable inTbl = _inputModule.ReadBatch();
        
            SimpleLog.ToLog("inTbl.Rows.Count=" + inTbl.Rows.Count.ToString(), SimpleLogEventType.Debug);

            while ( inTbl != null && inTbl.Rows.Count > 0 )
            {

                DataTable processingTbl = inTbl;
                Task chunkOutputTask = Task.Run(() => WriteTableChunk(processingTbl));
                inTbl = _inputModule.ReadBatch();

                await chunkOutputTask;
            }

        }

        public abstract void WriteTableChunk(DataTable tableChunk);
        public abstract void Dispose();

    }
}
