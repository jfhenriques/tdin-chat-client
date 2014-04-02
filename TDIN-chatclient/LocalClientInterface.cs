using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatclient
{
    public interface LocalClientInterface
    {
        bool startChat(TDIN_chatlib.IPAddress myAddress);
        bool stopChat();
        bool sendMessage(string msg);
    }
}
