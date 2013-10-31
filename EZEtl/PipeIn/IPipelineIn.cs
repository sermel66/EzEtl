using System;
using System.Data;

namespace EZEtl.PipeIn
{
    public interface IPipelineIn : IDisposable
    {
        DataTable ReadBatch();
    }
}
