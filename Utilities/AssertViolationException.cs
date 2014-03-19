using System;
using System.Runtime.Serialization;

namespace Utilities
{
    [Serializable()]
    public class AssertViolationException : System.Exception
    {
        public AssertViolationException(string message) : base(message) { }
        public AssertViolationException(string message, System.Exception ex) : base(message, ex) { }
        public AssertViolationException() : base() {}
            
        protected AssertViolationException(SerializationInfo serializationInfoArgument,
            StreamingContext streamingContextArgument)
            :base(serializationInfoArgument, streamingContextArgument) {}
        

    }
}
