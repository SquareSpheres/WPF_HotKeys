using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HotKey
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
