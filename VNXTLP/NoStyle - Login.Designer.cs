namespace VNXTLP {
    partial class NoStyleLogin {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.LoginUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LoginPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LoginBnt = new System.Windows.Forms.Button();
            this.ChangeLBL = new System.Windows.Forms.LinkLabel();
            this.LoginPanel = new System.Windows.Forms.Panel();
            this.RegisterPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.RegisterConfirmPass = new System.Windows.Forms.TextBox();
            this.RegisterPass = new System.Windows.Forms.TextBox();
            this.RegisterLogin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RegisterBNT = new System.Windows.Forms.Button();
            this.AutoLoginChkBx = new System.Windows.Forms.CheckBox();
            this.LoginPanel.SuspendLayout();
            this.RegisterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login:";
            // 
            // LoginUser
            // 
            this.LoginUser.Location = new System.Drawing.Point(44, 46);
            this.LoginUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoginUser.Name = "LoginUser";
            this.LoginUser.Size = new System.Drawing.Size(180, 20);
            this.LoginUser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Senha:";
            // 
            // LoginPass
            // 
            this.LoginPass.Location = new System.Drawing.Point(44, 75);
            this.LoginPass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoginPass.Name = "LoginPass";
            this.LoginPass.Size = new System.Drawing.Size(180, 20);
            this.LoginPass.TabIndex = 3;
            this.LoginPass.UseSystemPasswordChar = true;
            this.LoginPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LoginKeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(1, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(242, 31);
            this.label3.TabIndex = 4;
            this.label3.Text = "VNX+ - Bem-Vindo";
            // 
            // LoginBnt
            // 
            this.LoginBnt.Location = new System.Drawing.Point(4, 99);
            this.LoginBnt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoginBnt.Name = "LoginBnt";
            this.LoginBnt.Size = new System.Drawing.Size(226, 20);
            this.LoginBnt.TabIndex = 5;
            this.LoginBnt.Text = "Fazer Login";
            this.LoginBnt.UseVisualStyleBackColor = true;
            this.LoginBnt.Click += new System.EventHandler(this.LoginBnt_Click);
            // 
            // ChangeLBL
            // 
            this.ChangeLBL.AutoSize = true;
            this.ChangeLBL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChangeLBL.Location = new System.Drawing.Point(178, 154);
            this.ChangeLBL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ChangeLBL.Name = "ChangeLBL";
            this.ChangeLBL.Size = new System.Drawing.Size(49, 13);
            this.ChangeLBL.TabIndex = 6;
            this.ChangeLBL.TabStop = true;
            this.ChangeLBL.Text = "Registrar";
            this.ChangeLBL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChangeLBL_LinkClicked);
            // 
            // LoginPanel
            // 
            this.LoginPanel.Controls.Add(this.label1);
            this.LoginPanel.Controls.Add(this.label2);
            this.LoginPanel.Controls.Add(this.LoginBnt);
            this.LoginPanel.Controls.Add(this.label3);
            this.LoginPanel.Controls.Add(this.LoginPass);
            this.LoginPanel.Controls.Add(this.LoginUser);
            this.LoginPanel.Location = new System.Drawing.Point(2, 3);
            this.LoginPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoginPanel.Name = "LoginPanel";
            this.LoginPanel.Size = new System.Drawing.Size(238, 147);
            this.LoginPanel.TabIndex = 7;
            // 
            // RegisterPanel
            // 
            this.RegisterPanel.Controls.Add(this.label7);
            this.RegisterPanel.Controls.Add(this.RegisterConfirmPass);
            this.RegisterPanel.Controls.Add(this.RegisterPass);
            this.RegisterPanel.Controls.Add(this.RegisterLogin);
            this.RegisterPanel.Controls.Add(this.label6);
            this.RegisterPanel.Controls.Add(this.label5);
            this.RegisterPanel.Controls.Add(this.label4);
            this.RegisterPanel.Controls.Add(this.RegisterBNT);
            this.RegisterPanel.Location = new System.Drawing.Point(0, 0);
            this.RegisterPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RegisterPanel.Name = "RegisterPanel";
            this.RegisterPanel.Size = new System.Drawing.Size(241, 147);
            this.RegisterPanel.TabIndex = 6;
            this.RegisterPanel.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(11, 6);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(220, 31);
            this.label7.TabIndex = 7;
            this.label7.Text = "VNX+ - Cadastro";
            // 
            // RegisterConfirmPass
            // 
            this.RegisterConfirmPass.Location = new System.Drawing.Point(62, 97);
            this.RegisterConfirmPass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RegisterConfirmPass.Name = "RegisterConfirmPass";
            this.RegisterConfirmPass.Size = new System.Drawing.Size(166, 20);
            this.RegisterConfirmPass.TabIndex = 6;
            this.RegisterConfirmPass.UseSystemPasswordChar = true;
            // 
            // RegisterPass
            // 
            this.RegisterPass.Location = new System.Drawing.Point(62, 74);
            this.RegisterPass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RegisterPass.Name = "RegisterPass";
            this.RegisterPass.Size = new System.Drawing.Size(166, 20);
            this.RegisterPass.TabIndex = 5;
            this.RegisterPass.UseSystemPasswordChar = true;
            // 
            // RegisterLogin
            // 
            this.RegisterLogin.Location = new System.Drawing.Point(62, 51);
            this.RegisterLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RegisterLogin.Name = "RegisterLogin";
            this.RegisterLogin.Size = new System.Drawing.Size(166, 20);
            this.RegisterLogin.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 99);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Confirmar:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 76);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Senha:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 51);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Login:";
            // 
            // RegisterBNT
            // 
            this.RegisterBNT.Location = new System.Drawing.Point(7, 122);
            this.RegisterBNT.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RegisterBNT.Name = "RegisterBNT";
            this.RegisterBNT.Size = new System.Drawing.Size(220, 20);
            this.RegisterBNT.TabIndex = 0;
            this.RegisterBNT.Text = "Registrar";
            this.RegisterBNT.UseVisualStyleBackColor = true;
            this.RegisterBNT.Click += new System.EventHandler(this.button1_Click);
            // 
            // AutoLoginChkBx
            // 
            this.AutoLoginChkBx.AutoSize = true;
            this.AutoLoginChkBx.Location = new System.Drawing.Point(4, 153);
            this.AutoLoginChkBx.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AutoLoginChkBx.Name = "AutoLoginChkBx";
            this.AutoLoginChkBx.Size = new System.Drawing.Size(138, 17);
            this.AutoLoginChkBx.TabIndex = 8;
            this.AutoLoginChkBx.Text = "Logar Automáticamente";
            this.AutoLoginChkBx.UseVisualStyleBackColor = true;
            // 
            // NoStyleLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 175);
            this.Controls.Add(this.AutoLoginChkBx);
            this.Controls.Add(this.RegisterPanel);
            this.Controls.Add(this.LoginPanel);
            this.Controls.Add(this.ChangeLBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoStyleLogin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Minha Conta - VNX+";
            this.LoginPanel.ResumeLayout(false);
            this.LoginPanel.PerformLayout();
            this.RegisterPanel.ResumeLayout(false);
            this.RegisterPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LoginUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LoginPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button LoginBnt;
        private System.Windows.Forms.LinkLabel ChangeLBL;
        private System.Windows.Forms.Panel LoginPanel;
        private System.Windows.Forms.Panel RegisterPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox RegisterConfirmPass;
        private System.Windows.Forms.TextBox RegisterPass;
        private System.Windows.Forms.TextBox RegisterLogin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button RegisterBNT;
        private System.Windows.Forms.CheckBox AutoLoginChkBx;
    }
}