using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDIN_chatclient
{
    public partial class MainWindow : Form
    {
        private ChatController controller = null;


        public MainWindow(ChatController controller)
        {
            InitializeComponent();

            Program.window = this;
            this.controller = controller;

            _refreshList(controller.UserList);
        }


        private void _refreshList(IList<TDIN_chatlib.IPUser> userList)
        {
            this.userList.Items.Clear();

            if (userList != null
                && userList.Count > 0)
            {
                foreach (TDIN_chatlib.IPUser user in userList)
                {
                    this.userList.Items.Add(user.Username + " (" + user.Name + ")");
                }

                this.Refresh();
            }
        }


        public void refreshCLientList(IList<TDIN_chatlib.IPUser> userList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    _refreshList(userList);
                }));
            }
            else
                _refreshList(userList);

        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            controller.informServerExit();

            base.OnFormClosing(e);
        }

    }
}
