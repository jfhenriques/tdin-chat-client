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
    public partial class ChatWindow : Form
    {
        delegate void APDelegate(string text);


        private TDIN_chatlib.IPUser user = null;
        private LocalClientInterface endPoint = null;
        private string _chatUID = null;
        private string _sessionHash = null;
        private string _endpointChatUID = null;
        private bool _showCalled = false;


        public ChatWindow(TDIN_chatlib.IPUser user)
            : this(user, null)
        {
        }

        public ChatWindow(TDIN_chatlib.IPUser user, string forceIP)
        {
            this.user = user;
            this.Text = user.DisplayName;

            InitializeComponent();

            this.Text = user.DisplayName;

            createCOMMChannel(forceIP);

            this._chatUID = Guid.NewGuid().ToString();

            this.textBox.Focus();

        }



        public string EndpointCUID
        {
            get { return this._endpointChatUID; }
            set { this._endpointChatUID = value; }
        }
        public string CUID
        {
            get { return this._chatUID; }
        }
        public string SessionHash
        {
            get { return this._sessionHash; }
            set { this._sessionHash = value; }
        }

        public TDIN_chatlib.IPUser User
        {
            get { return this.user; }
        }
        public LocalClientInterface EndPointObject
        {
            get { return this.endPoint; }
        }


        public void _safeShow(bool bring)
        {
            if (!_showCalled)
            {
                _showCalled = true;

                //if (InvokeRequired)
                    Program.window.Invoke((System.Windows.Forms.MethodInvoker)delegate() { this.Show(); });

                //else
                //    this.Show(Program.window);
            }
            
            if( bring )
                this.BringToFront();
        }

        public void generateSessionHash()
        {
            this._sessionHash = TDIN_chatlib.Utils.generateRandomHash();
        }

        public void createCOMMChannel(string forceIP)
        {
            try
            {
                string serverURL = "tcp://" + (forceIP != null ? forceIP : user.IPAddress.IP)
                                            + ":" + user.IPAddress.PORT + "/" + ChatController.LOCAL_CHAT_SERVICE;

                this.Refresh();

                // Create an instance of the remote object
                endPoint = (LocalClientInterface)Activator.GetObject(typeof(LocalClientInterface), serverURL);
            }
            catch (Exception e)
            {
                AppendMsg("* Error creating communication channel", Color.Red);
            }
        }



        public void AppendMsg(string msg, Color color)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { AppendMsg(msg, color); });  //Invoke using an anonymous delegate calling AppendMsg and passing parameters
            else
                this.chatbox.AppendText(msg, color, true);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ChatController controller = ChatController.getController();

            if (this._sessionHash != null)
            {
                try
                {
                    EndPointObject.stopChat(this._sessionHash);
                }
                catch (Exception ex)
                {
                }
            }

            controller.removeUUID(this.user.UUID, false);
            
            base.OnFormClosing(e);
        }

        private void send_Click(object sender, EventArgs e)
        {
            if( this.textBox.Text.Length > 0)
            {
                string textToSend = this.textBox.Text;
                this.textBox.Text = "";
                ChatController controller = ChatController.getController();

                Thread t = new Thread(() =>
                {
                    bool error = false;
                    try
                    {
                        if (this.SessionHash == null)
                        {
                            this.generateSessionHash();
                            controller.putChatSession(this.SessionHash, this);

                            string _endpoint = EndPointObject.startChat(this.SessionHash, this.CUID, controller.Session.UUID);

                            if (_endpoint != this.EndpointCUID)
                                error = true;
                        }

                        if( !error)
                            EndPointObject.sendMessage(this._sessionHash, textToSend);
                    }
                    catch (Exception ex)
                    {
                        error = true;
                    }

                    if (error)
                        AppendMsg("Error sending message: " + textToSend, Color.Red);
                    else
                        AppendMsg(controller.Session.Username + ": " + textToSend, System.Drawing.Color.Black);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            
            this.textBox.Focus();
        }
    }



    /* Allows extending a Framework class without derivation. *
     * In this case we are extending RichTextBox with an      *
     * overload of method AppendText(string text).            */
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, bool newLine = false)
        {
            if (newLine)
                text += Environment.NewLine;

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }
    }
}
