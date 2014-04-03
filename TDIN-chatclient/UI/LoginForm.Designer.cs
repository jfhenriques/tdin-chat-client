namespace TDIN_chatclient
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectButton = new System.Windows.Forms.Button();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.serverPortLabel = new System.Windows.Forms.Label();
            this.serverHost = new System.Windows.Forms.TextBox();
            this.serverHostLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.radioButtonLogin = new System.Windows.Forms.RadioButton();
            this.radioButtonRegisto = new System.Windows.Forms.RadioButton();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.nome = new System.Windows.Forms.TextBox();
            this.nomeLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.passwordConf = new System.Windows.Forms.TextBox();
            this.passwordConfLabel = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.connectButton.Location = new System.Drawing.Point(73, 252);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(199, 23);
            this.connectButton.TabIndex = 50;
            this.connectButton.Text = "Conectar";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // serverPort
            // 
            this.serverPort.Location = new System.Drawing.Point(73, 79);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(90, 20);
            this.serverPort.TabIndex = 5;
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.AutoSize = true;
            this.serverPortLabel.Location = new System.Drawing.Point(12, 82);
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(32, 13);
            this.serverPortLabel.TabIndex = 4;
            this.serverPortLabel.Text = "Porta";
            // 
            // serverHost
            // 
            this.serverHost.Location = new System.Drawing.Point(73, 53);
            this.serverHost.Name = "serverHost";
            this.serverHost.Size = new System.Drawing.Size(199, 20);
            this.serverHost.TabIndex = 3;
            // 
            // serverHostLabel
            // 
            this.serverHostLabel.AutoSize = true;
            this.serverHostLabel.Location = new System.Drawing.Point(12, 56);
            this.serverHostLabel.Name = "serverHostLabel";
            this.serverHostLabel.Size = new System.Drawing.Size(46, 13);
            this.serverHostLabel.TabIndex = 2;
            this.serverHostLabel.Text = "Servidor";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(10, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(175, 25);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "TDIN-Chat Login";
            // 
            // radioButtonLogin
            // 
            this.radioButtonLogin.AutoSize = true;
            this.radioButtonLogin.Checked = true;
            this.radioButtonLogin.Location = new System.Drawing.Point(73, 116);
            this.radioButtonLogin.Name = "radioButtonLogin";
            this.radioButtonLogin.Size = new System.Drawing.Size(51, 17);
            this.radioButtonLogin.TabIndex = 6;
            this.radioButtonLogin.TabStop = true;
            this.radioButtonLogin.Text = "Login";
            this.radioButtonLogin.UseVisualStyleBackColor = true;
            this.radioButtonLogin.CheckedChanged += new System.EventHandler(this.radioButtonLogin_CheckedChanged);
            // 
            // radioButtonRegisto
            // 
            this.radioButtonRegisto.AutoSize = true;
            this.radioButtonRegisto.Location = new System.Drawing.Point(130, 116);
            this.radioButtonRegisto.Name = "radioButtonRegisto";
            this.radioButtonRegisto.Size = new System.Drawing.Size(61, 17);
            this.radioButtonRegisto.TabIndex = 7;
            this.radioButtonRegisto.TabStop = true;
            this.radioButtonRegisto.Text = "Registo";
            this.radioButtonRegisto.UseVisualStyleBackColor = true;
            this.radioButtonRegisto.CheckedChanged += new System.EventHandler(this.radioButtonRegisto_CheckedChanged);
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 147);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(55, 13);
            this.usernameLabel.TabIndex = 8;
            this.usernameLabel.Text = "Username";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(73, 144);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(199, 20);
            this.username.TabIndex = 9;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(12, 171);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(73, 168);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(199, 20);
            this.password.TabIndex = 11;
            // 
            // nome
            // 
            this.nome.Location = new System.Drawing.Point(73, 220);
            this.nome.Name = "nome";
            this.nome.Size = new System.Drawing.Size(199, 20);
            this.nome.TabIndex = 15;
            // 
            // nomeLabel
            // 
            this.nomeLabel.AutoSize = true;
            this.nomeLabel.Location = new System.Drawing.Point(12, 223);
            this.nomeLabel.Name = "nomeLabel";
            this.nomeLabel.Size = new System.Drawing.Size(35, 13);
            this.nomeLabel.TabIndex = 14;
            this.nomeLabel.Text = "Nome";
            // 
            // statusStrip
            // 
            this.statusStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 286);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(284, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 51;
            // 
            // statusLabel
            // 
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // passwordConf
            // 
            this.passwordConf.Location = new System.Drawing.Point(73, 194);
            this.passwordConf.Name = "passwordConf";
            this.passwordConf.PasswordChar = '*';
            this.passwordConf.Size = new System.Drawing.Size(199, 20);
            this.passwordConf.TabIndex = 13;
            // 
            // passwordConfLabel
            // 
            this.passwordConfLabel.AutoSize = true;
            this.passwordConfLabel.Location = new System.Drawing.Point(12, 197);
            this.passwordConfLabel.Name = "passwordConfLabel";
            this.passwordConfLabel.Size = new System.Drawing.Size(61, 13);
            this.passwordConfLabel.TabIndex = 12;
            this.passwordConfLabel.Text = "Conf. Pass.";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.connectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 308);
            this.Controls.Add(this.passwordConf);
            this.Controls.Add(this.passwordConfLabel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.nomeLabel);
            this.Controls.Add(this.nome);
            this.Controls.Add(this.password);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.username);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.radioButtonRegisto);
            this.Controls.Add(this.radioButtonLogin);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.serverPort);
            this.Controls.Add(this.serverPortLabel);
            this.Controls.Add(this.serverHost);
            this.Controls.Add(this.serverHostLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "Chat Login";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.Label serverPortLabel;
        private System.Windows.Forms.TextBox serverHost;
        private System.Windows.Forms.Label serverHostLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.RadioButton radioButtonLogin;
        private System.Windows.Forms.RadioButton radioButtonRegisto;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox nome;
        private System.Windows.Forms.Label nomeLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TextBox passwordConf;
        private System.Windows.Forms.Label passwordConfLabel;
    }
}