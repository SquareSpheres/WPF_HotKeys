using System;
using System.Runtime.Serialization;

namespace HotKeysLib
{
    [Serializable]
    public class HotKeyException : Exception
    {
        public HotKeyException() : base("General HotKeyException") { }
        public HotKeyException(string message) : base(message) { }
        public HotKeyException(string message, Exception innerException) : base(message, innerException) { }
        protected HotKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

