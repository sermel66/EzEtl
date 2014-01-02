using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration.Setting;

namespace Configuration
{
    public class Section
    {
      //  const string  

        Dictionary<string, ISetting> _settingObjectDict = new Dictionary<string, ISetting>();

        bool _isLoaded = false;
        public bool IsLoaded { get { return _isLoaded; } }

        bool _isValid = false;
        public bool IsValid { get { return _isValid; } }

        public void Add(string key, ISetting setting)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingObjectDict.Add(key, setting);
        }

        public bool ContainsKey(string key)
        {
            return _settingObjectDict.ContainsKey(key);
        }

        public ISetting Setting(string key)
        {
            if (_settingObjectDict.ContainsKey(key))
                return _settingObjectDict[key];

            return null;
        }

        public void Parse(string keyValueListString)
        {
            if ( string.IsNullOrWhiteSpace(keyValueListString) )
                throw new ArgumentNullException("keyValueListString");

        }

    }
}
