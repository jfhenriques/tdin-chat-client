using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatclient
{
    public interface LocalClientInterface
    {
        string handshake(string sessionHash, string cuid);
        string startChat(string sessionHash, string cuid, string uuid);
        void stopChat(string sessionHash);
        void sendMessage(string sessionHash, string msg);
        bool checkAlive();
    }
}
