using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class ConfigurationSetting
    {
        bool    _isOptional = false;
        string  _key;
        bool    _isPresent = false;
        string  _rawValue;

        public ConfigurationSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            _key = key;
        }

         

    }
}
