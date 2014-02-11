using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration
{
    public class Variable<T> : IVariable
    {
        string _name;
        public string Name { get { return _name; } }

        Type _type;
        public Type VariableType { get { return _type; } }

        SupportedVariableTypeEnum _variableTypeName;
        public SupportedVariableTypeEnum VariableTypeName { get { return _variableTypeName; } }

        T _value;
        public T TypedValue { get { return _value; } set { _value = value; } }
        public object Value { get { return _value; } set { _value = (T)value; } }

        bool _isImmutable = false;
        public bool IsImmutable { get { return _isImmutable; } }

        public Variable ( string name, SupportedVariableTypeEnum variableTypeName,T value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _name = name;
            _variableTypeName = variableTypeName;
            _value = value;
            _type = typeof(T);
        }        
    }
}
