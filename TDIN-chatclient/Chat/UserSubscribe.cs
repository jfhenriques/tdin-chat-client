using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatclient
{
    class UserSubscribe: MarshalByRefObject, TDIN_chatlib.UserSubscribeInterface
    {
        public void clientListUpdated()
        {
            Console.WriteLine("* Received subscribed event: update Client list");

        }

        public string handshake(string sessionHash)
        {
            ChatController controller = ChatController.getController();

            string uid = controller.UID;

            if (controller.SessionHash == null)
            {
                controller.SessionHash = sessionHash;

                Console.WriteLine("* Handshake uid: " + uid + ", hash: " + sessionHash);

                return uid;
            }

            return null;
        }
    }
}
