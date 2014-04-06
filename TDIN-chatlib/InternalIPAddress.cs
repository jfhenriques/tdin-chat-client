using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    public class InternalIPAddress : MarshalByRefObject
    {
        private string _ip = null;
        private int _port = -1;

        public InternalIPAddress(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }

        public string IP
        {
            get { return this._ip; }
            set { this._ip = value; }
        }

        public int PORT
        {
            get { return this._port; }
            set { this._port = value; }
        }
        
    }
}
