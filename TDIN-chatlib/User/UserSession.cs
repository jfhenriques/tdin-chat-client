using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    public class UserSession : User
    {
        private string _sessionHash = null;


        public UserSession(string user, string name, string sessionHash)
            : base(user, name)
        {
            this._sessionHash = sessionHash;
        }


        public UserSession(string user, string name)
            : this(user, name, null)
        {
        }


        public string SessionHash
        {
            get { return this._sessionHash; }
            set { this._sessionHash = value; }
        }
    }
}
