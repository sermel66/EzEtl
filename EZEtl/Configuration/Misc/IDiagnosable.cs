namespace EZEtl.Configuration.Misc
{
    public interface IDiagnosable
    {
        void OutputDiagnostics();
        bool IsValid { get; }
    }
}
