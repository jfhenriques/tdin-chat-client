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
    public partial class LoginForm : Form
    {
        private bool isRegisto = false;
        private ChatController chatController;
        private int INIT_HEIGHT;

        public LoginForm(ChatController chatController)
        {
            this.chatController = chatController;
            InitializeComponent();
            this.serverPort.Text = TDIN_chatlib.Constants.DEFAULT_SERVER_PORT.ToString();

            INIT_HEIGHT = this.Height;

            setRegisto(false);
        }



        private void setRegisto(bool state)
        {
            this.isRegisto = state;
            this.nomeLabel.Visible = this.nome.Visible = this.confPassLabel.Visible = this.confPass.Visible = state;

            if (this.isRegisto)
                this.Height = INIT_HEIGHT;
            else
                this.Height -= 50;
        }


        private void radioButtonLogin_CheckedChanged(object sender, EventArgs e)
        {
            setRegisto(false);
        }

        private void radioButtonRegisto_CheckedChanged(object sender, EventArgs e)
        {
            setRegisto(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.statusLabel.Text = "Connecting to server...";
            this.Enabled = false;
            this.Refresh();

            try
            {
                chatController.registerWithServer(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                this.Enabled = true;
                this.statusLabel.Text = "Error connecting to server!";
            }


        }


    }
}
