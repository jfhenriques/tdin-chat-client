using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    public class User : MarshalByRefObject
    {
        private string _user = null;
        private string _name = null;
        private string _uuid = null;


        public User(string username, string name)
        {
            this._user = username;
            this._name = name;
        }

        public string Username
        {
            get { return this._user; }
            set { this._user = value; }
        }


        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public string DisplayName
        {
            get { return this.Username + " (" + this.Name + ")"; }
        }

        public string UUID
        {
            get { return this._uuid; }
            set { this._uuid = value; }
        }

        public void generateUID()
        {
            this._uuid = Guid.NewGuid().ToString();
        }

    }
}
