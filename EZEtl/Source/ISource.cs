using System;
using System.Data;

namespace EZEtl.Source
{
    public interface ISource : IDisposable
    {
        DataTable ReadBatch();
    }
}
