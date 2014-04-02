using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatclient
{
    class LocalClient : MarshalByRefObject, LocalClientInterface
    {

        public LocalClient()
        {



        }





        public bool startChat(TDIN_chatlib.IPAddress address)
        {



            return false;
        }

        public bool stopChat()
        {



            return false;
        }


        public bool sendMessage(string msg)
        {



            return false;
        }
        
    }
}
