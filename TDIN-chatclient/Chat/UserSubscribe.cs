using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TDIN_chatclient
{
    class UserSubscribe: MarshalByRefObject, TDIN_chatlib.UserSubscribeInterface
    {
        private static ChatController controller = null;



        public void clientListUpdated(long count)
        {
            if (controller == null)
                controller = ChatController.getController();

            Console.WriteLine("* New client list is ready, event is: " + count);

            Thread t = new Thread(() => controller.updateClientList(count));
            t.TrySetApartmentState(ApartmentState.STA);
            t.Start();
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
