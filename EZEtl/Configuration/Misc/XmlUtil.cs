using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace EZEtl.Configuration.Misc
{
    public static class XmlUtil
    {
        public static bool TryGetAttribute (XElement item, AttributeNameEnum attributeName, out string attributeValue, out string errorMessage )
        {
            attributeValue = string.Empty;
            errorMessage = string.Empty;

            XAttribute attribute = item.Attribute(attributeName.ToString());
            if ( attribute == null)
            {
                errorMessage = "Attribute " + attributeName.ToString() + " not found in the Xml Element ["
                     + item.ToString().Substring(1, Constant.XmlQuoteLength);
                return false;
            }

            attributeValue = attribute.Value;
            return true;
        }
    }
}
