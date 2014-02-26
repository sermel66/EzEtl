using EZEtl.Configuration.Misc;

namespace EZEtl.Workflow
{
    public interface IOperator : IDiagnosable, IConfigurationParent 
    {
        void Execute(params object[] args);
    }
}
