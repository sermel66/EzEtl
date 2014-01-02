using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace Configuration.Setting
{
    public class Expansion : SettingBase, ISetting
    {
         // <Expansion name="Date" 
         //          type="date_loop"
         //          format="yyyy-MM-dd"
         //          loop_start_query="dbo.StartDate"
         //          loop_start_query_connection_string="MyLocalSql"
         ///>

      
        Dictionary<ExpansionAttributeEnum, string> _attributes = new Dictionary<ExpansionAttributeEnum, string>();
        ExpansionTypeEnum _expansionType = ExpansionTypeEnum.UNDEFINED;

        public object Value { get { return _attributes; } }

        XElement _expansionXml;
        public string RawValue { get { return _expansionXml.ToString(); } set { } }

        public Type ValueType { get { return _attributes.GetType(); } }
        
        public Expansion(ISetting setting)
        {
            if (setting == null || !setting.IsValid)
                throw new ArgumentNullException("Argument setting is NULL or invalid");

            _expansionXml = (XElement)setting.Value;
           
            ExpansionAttributeEnum attributeName;
            string unexpectedAttributes = string.Empty;

            foreach ( XAttribute attribute in _expansionXml.Attributes() )
            {
                if ( ! Enum.TryParse<ExpansionAttributeEnum>(attribute.Name.ToString(), true, out attributeName ))
                {
                    unexpectedAttributes += attribute.Name + Constant.UserMessageListDelimiter;
                    continue;
                }
               
                if ( attributeName == ExpansionAttributeEnum.type)
                {
                    if ( ! Enum.TryParse<ExpansionTypeEnum>(attribute.Value, true, out _expansionType ))
                    {
                       _message += "Unrecognized expansion type " + attribute.Value + Constant.UserMessageSentenceDelimiter;
                    }
                }
            }
         
            if (_expansionType.Equals(ExpansionTypeEnum.UNDEFINED))
            {
                _message += "Undefined expansion type" + Constant.UserMessageSentenceDelimiter;
            }

            if (!string.IsNullOrWhiteSpace(unexpectedAttributes))
                _warning += "Unexpected Attributes: " + unexpectedAttributes + Constant.UserMessageSentenceDelimiter;
        }
    }
}
