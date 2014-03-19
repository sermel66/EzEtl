
using EZEtl.Configuration;
using EZEtl.Configuration.Misc;
namespace EZEtl.Expressions
{
    public interface IExpression : IDiagnosable, IConfigurationParent
    {
        void Parse(string expressionText);
        void Evaluate();
        IVariable Result { get; }
    }
}
