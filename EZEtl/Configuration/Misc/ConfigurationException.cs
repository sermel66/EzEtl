using System;
using System.Runtime.Serialization;

namespace EZEtl.Configuration
{
    [Serializable()]
    public class ConfigurationException : System.Exception
    {
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, System.Exception ex) : base(message, ex) { }
        public ConfigurationException() : base() {}
            
        protected ConfigurationException(SerializationInfo serializationInfoArgument,
            StreamingContext streamingContextArgument)
            :base(serializationInfoArgument, streamingContextArgument) {}

    }
}
