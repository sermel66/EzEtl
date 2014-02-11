
using EZEtl.Configuration.Misc;

namespace EZEtl.Modules
{
    public interface IModule: IDiagnosable, IConfigurationParent
    {
        string ModuleID { get; }
        void Execute();
    }
}
