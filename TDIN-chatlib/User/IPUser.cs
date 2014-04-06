using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    public class IPUser : User
    {
        private InternalIPAddress _address = null;


        public IPUser(string username, string name, InternalIPAddress address)
            : base(username, name)
        {
            this._address = address;
        }


        public IPUser(string username, string name)
            : this(username, name, null)
        {
        }


        public InternalIPAddress IPAddress
        {
            get { return this._address; }
        }
    }
}
