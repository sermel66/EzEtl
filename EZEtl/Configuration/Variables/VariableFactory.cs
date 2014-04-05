using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using EZEtl.Configuration.Misc;

namespace EZEtl.Configuration
{
    public static class VariableFactory
    {
        public static IVariable Create(XElement item, out string errorMessage)
        {
            errorMessage=String.Empty;
            string attrNotFoundErrorMessage;
            const string VariableItemName="Variable";

            if (item == null)
                throw new ArgumentNullException("item");

            if (!item.Name.ToString().Equals(VariableItemName))
                throw new ConfigurationException("Expected item name [" + VariableItemName + "], but encountered ["
                    + item.Name.ToString() + "]");

            string variableId;// = item.Attribute(AttributeNameEnum.ID.ToString());
            string variableTypeName;// = item.Attribute(AttributeNameEnum.type.ToString());
            string variableValue;// = item.Attribute(AttributeNameEnum.value.ToString());

            if (!XmlUtil.TryGetAttribute(item, AttributeNameEnum.ID, out variableId, out attrNotFoundErrorMessage))
            { errorMessage += attrNotFoundErrorMessage + Constant.UserMessageSentenceDelimiter; }

            if (!XmlUtil.TryGetAttribute(item, AttributeNameEnum.value, out variableValue, out attrNotFoundErrorMessage))
            { errorMessage += attrNotFoundErrorMessage + Constant.UserMessageSentenceDelimiter; }
                    
            SupportedVariableTypeEnum variableType;

            if (XmlUtil.TryGetAttribute(item, AttributeNameEnum.type, out variableTypeName, out attrNotFoundErrorMessage))
            {
                if (!Enum.TryParse<SupportedVariableTypeEnum>(variableTypeName, out variableType))
                {
                    errorMessage += "Unsupported variable type [" + variableTypeName + "]; ";
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
                    return new Variable<string>(variableId, variableType, variableValue);

                case SupportedVariableTypeEnum.Int32:
                    int valueInt;
                    if (!Int32.TryParse(variableValue, out valueInt))
                    {
                        errorMessage += "Could not convert value [" + variableValue + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<Int32>(variableId, variableType, valueInt);

                case SupportedVariableTypeEnum.DateTime:
                    DateTime valueDateTime;
                    if (!DateTime.TryParse(variableValue, out valueDateTime))
                    {
                        errorMessage += "Could not convert value [" + variableValue + "] to " + variableType.ToString();
                        return null;
                    }
                    return new Variable<DateTime>(variableId, variableType, valueDateTime);
                default:
                    throw new Exception("Unexpected variableType [" + variableType.ToString() + "]");
            }


        }
    }
}
