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

        }
    }
}
