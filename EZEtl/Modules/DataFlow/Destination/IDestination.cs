using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EZEtl.Destination
{
    public interface IDestination : IDisposable
    {
        /*Task*/ void ExecuteAsync();
        void WriteTableChunk(DataTable tableChunk);
        void Close();
        void SetSource(Source.ISource source);
    }
}
