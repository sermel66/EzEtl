using System;
using System.Runtime.Serialization;

namespace EZEtl
{
    [Serializable()]
    public class EZEtlException : System.Exception
    {
        public EZEtlException(string message) : base(message) { }
        public EZEtlException(string message, System.Exception ex) : base(message, ex) { }
        public EZEtlException() : base() {}
            
        protected EZEtlException(SerializationInfo serializationInfoArgument,
            StreamingContext streamingContextArgument)
            :base(serializationInfoArgument, streamingContextArgument) {}
        

    }
}
