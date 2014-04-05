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
                errorMessage = "Attribute " + attributeName.ToString() + " not found in the XML Element "
                    + Quot(item.ToString());
                return false;
            }

            attributeValue = attribute.Value;
            return true;
        }

        public static string Quot(string source)
        {
            return @"[" + source.Substring(0, Constant.XmlQuoteLength > source.Length ? source.Length : Constant.XmlQuoteLength)
                 + (source.Length > Constant.XmlQuoteLength ? @"..." : string.Empty)
                + @"]";
        }
    }
}
