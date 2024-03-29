﻿using System;
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
        private ChatController controller;
        private int INIT_HEIGHT;

        public LoginForm(ChatController chatController)
        {
            this.controller = chatController;
            InitializeComponent();
            this.serverPort.Text = TDIN_chatlib.Constants.DEFAULT_SERVER_PORT.ToString();
            this.serverHost.Text = "127.0.0.1";

            INIT_HEIGHT = this.Height;

            setRegisto(false);
        }



        private void setRegisto(bool state)
        {
            this.isRegisto = state;
            this.nomeLabel.Visible = this.nome.Visible = this.passwordConfLabel.Visible = this.passwordConf.Visible = state;

            if (this.isRegisto)
            {
                this.Height = INIT_HEIGHT;
                this.nome.Text = "";
                this.passwordConf.Text = "";
            }
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


        private void alertMessage(string msg)
        {
            MessageBox.Show(msg, "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (    this.serverHost.Text.Length == 0
                 || this.serverPort.Text.Length == 0 )
                alertMessage("Host e a porta do servidor não preenchidos");

            else
            if (    this.username.Text.Length == 0
                 || this.password.Text.Length == 0 )
                alertMessage("Username e password têm de estar preenchidos");

            else
            if( this.username.Text.Length < 4 )
                alertMessage("O username tem de ter no mínimo 4 caractéres");

            else
            if ( this.password.Text.Length < 4 )
                alertMessage("A password tem de ter no mínimo 4 caractéres");

            else
            if(    this.isRegisto
                && (    this.passwordConf.Text.Length == 0
                     || this.nome.Text.Length == 0 ) )
                alertMessage("Confirmação de password e nome têm de estar preenchidos");
            else
            if (    this.isRegisto
                 && this.password.Text != this.passwordConf.Text )
                alertMessage("Por favor repita correctamente a password no campo de confirmação");

            else
            {
                this.statusLabel.Text = "Connecting to server...";
                this.Enabled = false;
                this.Refresh();

                bool success = false;

                try
                {
                    TDIN_chatlib.LoginUser user = new TDIN_chatlib.LoginUser(this.username.Text, this.password.Text);
                    user.hashPassword();

                    if (this.isRegisto)
                        user.Name = this.nome.Text;

                    if (controller.registerWithServer(this.serverHost.Text, Convert.ToInt32(this.serverPort.Text), user))
                    {
                        Console.WriteLine("Sucefully registered with server. session: " + controller.Session.SessionHash);
                        this.statusLabel.Text = "Success!";

                        success = true;
                    }
                    else
                    {
                        this.statusLabel.Text = "Error registering with server!";
                    }

                }
                catch (TDIN_chatlib.ChatException ex1)
                {
                    this.statusLabel.Text = ex1.Message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    this.statusLabel.Text = "Error connecting to server!";
                }
                finally {
                   
                    if (success)
                    {
                        Thread t = new Thread(Program.LaunchListWindow);
                        t.TrySetApartmentState(ApartmentState.STA);
                        t.Start();

                        this.Dispose();
                    }
                    else
                        this.Enabled = true;
                }
  
            }
        }

    }
}
