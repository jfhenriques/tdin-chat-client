﻿using System;
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

        public IPUser(User user, InternalIPAddress address)
            : base(user)
        {
            if (address != null)
                this._address = new InternalIPAddress(address.IP, address.PORT);
        }

        public IPUser(User user)
            : base(user)
        {
        }


        public InternalIPAddress IPAddress
        {
            get { return this._address; }
            set { this._address = value; }
        }
    }
}
