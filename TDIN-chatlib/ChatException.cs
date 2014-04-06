using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TDIN_chatlib
{
    [Serializable()]
    public class ChatException: System.Exception
    {

        public ChatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ChatException(string message)
            : base(message)
        {
        }

    }
}
