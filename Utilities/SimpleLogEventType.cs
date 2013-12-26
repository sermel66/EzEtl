namespace Utilities
{
    public enum SimpleLogEventType
    {
        // Summary:
        //     An error event. This indicates a significant problem the user should know
        //     about; usually a loss of functionality or data.
        Error = 1,
        //
        // Summary:
        //     A warning event. This indicates a problem that is not immediately significant,
        //     but that may signify conditions that could cause future problems.
        Warning = 2,
        //
        // Summary:
        //     An information event. This indicates a significant, successful operation.
        Information = 4,
        //
        // Summary:
        //     A trace event. This indicates hitting certain place in the source code
        //     For example, entering a method body.
        Trace = 8,
        //
        // Summary:
        //     A debug event. This can be used to e.g. output values of variables
        Debug = 16,


        None = 32
    }
}
