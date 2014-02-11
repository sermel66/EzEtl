namespace EZEtl.Configuration
{
    public interface IVariable
    {
        string Name { get; }
        object Value { get; set; }
        SupportedVariableTypeEnum VariableTypeName { get; }
        System.Type VariableType { get; }
        bool IsImmutable { get; }
    }
}
