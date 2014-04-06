using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    [Serializable()]
    public class LoginUser : User
    {
        private string _pass = null;


        public LoginUser(string username, string pass, string name)
            : base(username, name)
        {
            this.Pass = pass;
        }


        public LoginUser(string username, string pass)
            : this(username, pass, null)
        {
        }

        public LoginUser()
            : this(null, null, null)
        {
        }

        public LoginUser(LoginUser user)
            : this(null, null, null)
        {
            if (user != null)
            {
                this.UUID = user.UUID;
                this.Username = user.Username;
                this.Name = user.Name;
                this._pass = user.Pass;
            }
        }
        public string Pass
        {
            get { return this._pass; }
            set {
                this._pass = (value == null || value.Trim().Length == 0)
                                   ? null
                                   : TDIN_chatlib.Utils.hashBytes(System.Text.Encoding.Unicode.GetBytes(value.Trim()));
            }
        }



        public bool isValidLogin()
        {
            return this.Username != null && this.Username.Length > 0
                    && this._pass != null && this._pass.Length > 0;
        }

        public bool isValidRegister()
        {
            return isValidLogin() && this.Name != null && this.Name.Length > 0;
        }


        public bool comparePassword(LoginUser user)
        {
            return this._pass == user._pass;
        }
    }
}
