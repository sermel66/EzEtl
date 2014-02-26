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

            XAttribute variableIdAttribute = item.Attribute(AttributeNameEnum.ID.ToString());
            XAttribute variableTypeNameAttribute = item.Attribute(AttributeNameEnum.type.ToString());
            XAttribute variableValueAttribute = item.Attribute(AttributeNameEnum.value.ToString());

            if (variableIdAttribute == null || String.IsNullOrWhiteSpace(variableIdAttribute.Value))
                errorMessage += "Attribute " + AttributeNameEnum.ID.ToString() + " is missing or empty; ";

            if (variableValueAttribute == null )
                errorMessage += "Attribute " + AttributeNameEnum.value.ToString() + " is missing; ";

            SupportedVariableTypeEnum variableType;
            if (variableTypeNameAttribute != null)
            {
                if (!Enum.TryParse<SupportedVariableTypeEnum>(variableTypeNameAttribute.Value, out variableType))
                {
                    errorMessage += "Unsupported variable type [" + variableTypeNameAttribute.Value + "]; ";
                }
            }
            else
            {
                variableType = SupportedVariableTypeEnum.String;
            }

            if (errorMessage.Length > 0)
                return null;

            switch(variableType)
            {
                case SupportedVariableTypeEnum.String:
                    return new Variable<string>(variableIdAttribute.Value, variableType, variableValueAttribute.Value);

                case SupportedVariableTypeEnum.Int32:
                    int valueInt;
                    if (!Int32.TryParse(variableValueAttribute.Value, out valueInt))
                    {
                        errorMessage += "Could not convert value [" + variableValueAttribute.Value + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<Int32>(variableIdAttribute.Value, variableType, valueInt);

                case SupportedVariableTypeEnum.DateTime:
                    DateTime valueDateTime;
                    if (!DateTime.TryParse(variableValueAttribute.Value, out valueDateTime))
                    {
                        errorMessage += "Could not convert value [" + variableValueAttribute.Value + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<DateTime>(variableIdAttribute.Value, variableType, valueDateTime);
                default:
                    throw new Exception("Unexpected variableType [" + variableType.ToString() + "]");
            }


        }
    }
}
