using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EZEtl.PipeOut
{
    public interface IPipeOut : IDisposable
    {
        /*Task*/ void ExecuteAsync();
        void WriteTableChunk(DataTable tableChunk);
    }
}
