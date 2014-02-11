using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace EZEtl.Configuration
{
    public static class VariableFactory
    {
        public static IVariable Create(XElement item, out string errorMessage)
        {
            errorMessage=String.Empty;

            const string VariableItemName="Variable";

            if (item == null)
                throw new ArgumentNullException("item");

            if (!item.Name.ToString().Equals(VariableItemName))
                throw new ConfigurationException("Expected item name [" + VariableItemName + "], but encountered ["
                    + item.Name.ToString() + "]");

            string variableName = item.Attribute(AttributeNameEnum.name.ToString()).Value;
            string variableTypeName = item.Attribute(AttributeNameEnum.type.ToString()).Value;
            string variableValue = item.Attribute(AttributeNameEnum.value.ToString()).Value;

            if (string.IsNullOrWhiteSpace(variableName))
                errorMessage += "Attribute " + AttributeNameEnum.name.ToString() + " is missing;";

            if (string.IsNullOrWhiteSpace(variableTypeName))
                errorMessage += "Attribute " + AttributeNameEnum.type.ToString() + " is missing;";

            SupportedVariableTypeEnum variableType;
            if ( ! Enum.TryParse<SupportedVariableTypeEnum>(variableTypeName, out variableType))
                errorMessage += "Unsupported variable type " + variableTypeName +";";

            if (errorMessage.Length > 0)
                return null;

            switch(variableType)
            {
                case SupportedVariableTypeEnum.String:
                    return new Variable<string>(variableName, variableType, variableValue);

                case SupportedVariableTypeEnum.Int32:
                    int valueInt; 
                    if (! Int32.TryParse(variableValue, out valueInt))
                    {
                        errorMessage += "Could not convert value [" + variableValue + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<Int32>(variableName, variableType, valueInt);

                case SupportedVariableTypeEnum.DateTime:
                    DateTime valueDateTime;
                    if (!DateTime.TryParse(variableValue, out valueDateTime))
                    {
                        errorMessage += "Could not convert value [" + variableValue + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<DateTime>(variableName, variableType, valueDateTime);
                default:
                    throw new Exception("Unexpected variableType [" + variableType.ToString() + "]");
            }


        }
    }
}
