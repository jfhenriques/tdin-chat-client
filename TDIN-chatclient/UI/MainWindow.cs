using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TDIN_chatclient
{
    public partial class MainWindow : Form
    {
        private ChatController controller = null;


        public MainWindow(ChatController controller)
        {
            InitializeComponent();

            this.userList.DisplayMember = "DisplayName";
            this.userList.ValueMember = "UID";

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
                    if( user.UUID != controller.Session.UUID)
                        this.userList.Items.Add(user);
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


        private void _appendOpenError(ChatWindow chat)
        {
            
            chat.AppendMsg("* An error occured while trying to open conversation with the user", System.Drawing.Color.Red);

        }

        private void userList_DoubleClicked(object sender, EventArgs e)
        {
            if (this.userList.SelectedItem != null)
            {
                lock (controller.syncLockChat)
                {

                    TDIN_chatlib.IPUser user = (TDIN_chatlib.IPUser)this.userList.SelectedItem;

                    ChatWindow chat = controller.getChatByUUID(user.UUID);

                    if (chat != null)
                        chat.BringToFront();

                    else
                    {
                        chat = controller.createChat(user.UUID, null);

                        if (chat != null)
                        {
                            chat.Show(Program.window);
                            chat.AppendMsg("* starting conversation", System.Drawing.Color.Gray);

                            //chat.generateSessionHash();
                            //controller.putChatSession(chat.SessionHash, chat);

                            //Thread t = new Thread(() =>
                            //{
                                    
                            //    bool error = false;
                            //    try
                            //    {
                            //        string _endpoint = chat.EndPointObject.startChat(chat.SessionHash, chat.CUID, controller.Session.UUID);

                            //        if (_endpoint != chat.EndpointCUID)
                            //            error = true;

                            //    }
                            //    catch(Exception ex1)
                            //    {
                            //        Console.WriteLine(ex1);
                            //        error = true;
                            //    }

                            //    if( error )
                            //    {
                            //        controller.removeSession(chat.SessionHash, false);
                            //        _appendOpenError(chat);
                            //    }
                            //});
                            //t.SetApartmentState(ApartmentState.STA);
                            //t.Start();
                        }

                    }

                }
            }
        }



    }
}