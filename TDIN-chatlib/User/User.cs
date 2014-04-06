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
            this.Username = username;
            this.Name = name;
        }
        public User(User user)
        {
            if (user != null)
            {
                this._user = user.Username;
                this._name = user.Name;
                this._uuid = user.UUID;
            }
        }




        public string Username
        {
            get { return this._user; }
            set { this._user = value == null ? null : value.Trim(); ; }
        }


        public string Name
        {
            get { return this._name; }
            set { this._name = value == null ? null : value.Trim(); }
        }

        public string DisplayName
        {
            get { return this.Username + " (" + this.Name + ")"; }
        }

        public string UUID
        {
            get { return this._uuid; }
            set { this._uuid = value == null ? null : value.Trim(); }
        }

        public void generateUID()
        {
            this.UUID = Guid.NewGuid().ToString();
        }

    }
}
