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
            this.label1.Location = new System.Drawing.Point(7, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login:";
            // 
            // LoginUser
            // 
            this.LoginUser.Location = new System.Drawing.Point(58, 56);
            this.LoginUser.Name = "LoginUser";
            this.LoginUser.Size = new System.Drawing.Size(239, 22);
            this.LoginUser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Senha:";
            // 
            // LoginPass
            // 
            this.LoginPass.Location = new System.Drawing.Point(59, 92);
            this.LoginPass.Name = "LoginPass";
            this.LoginPass.Size = new System.Drawing.Size(239, 22);
            this.LoginPass.TabIndex = 3;
            this.LoginPass.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(1, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(306, 39);
            this.label3.TabIndex = 4;
            this.label3.Text = "VNX+ - Bem-Vindo";
            // 
            // LoginBnt
            // 
            this.LoginBnt.Location = new System.Drawing.Point(6, 122);
            this.LoginBnt.Name = "LoginBnt";
            this.LoginBnt.Size = new System.Drawing.Size(301, 25);
            this.LoginBnt.TabIndex = 5;
            this.LoginBnt.Text = "Fazer Login";
            this.LoginBnt.UseVisualStyleBackColor = true;
            this.LoginBnt.Click += new System.EventHandler(this.LoginBnt_Click);
            // 
            // ChangeLBL
            // 
            this.ChangeLBL.AutoSize = true;
            this.ChangeLBL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChangeLBL.Location = new System.Drawing.Point(238, 189);
            this.ChangeLBL.Name = "ChangeLBL";
            this.ChangeLBL.Size = new System.Drawing.Size(66, 17);
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
            this.LoginPanel.Location = new System.Drawing.Point(3, 4);
            this.LoginPanel.Name = "LoginPanel";
            this.LoginPanel.Size = new System.Drawing.Size(318, 181);
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
            this.RegisterPanel.Name = "RegisterPanel";
            this.RegisterPanel.Size = new System.Drawing.Size(321, 181);
            this.RegisterPanel.TabIndex = 6;
            this.RegisterPanel.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(15, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(275, 39);
            this.label7.TabIndex = 7;
            this.label7.Text = "VNX+ - Cadastro";
            // 
            // RegisterConfirmPass
            // 
            this.RegisterConfirmPass.Location = new System.Drawing.Point(82, 119);
            this.RegisterConfirmPass.Name = "RegisterConfirmPass";
            this.RegisterConfirmPass.Size = new System.Drawing.Size(220, 22);
            this.RegisterConfirmPass.TabIndex = 6;
            this.RegisterConfirmPass.UseSystemPasswordChar = true;
            // 
            // RegisterPass
            // 
            this.RegisterPass.Location = new System.Drawing.Point(82, 91);
            this.RegisterPass.Name = "RegisterPass";
            this.RegisterPass.Size = new System.Drawing.Size(220, 22);
            this.RegisterPass.TabIndex = 5;
            this.RegisterPass.UseSystemPasswordChar = true;
            // 
            // RegisterLogin
            // 
            this.RegisterLogin.Location = new System.Drawing.Point(82, 63);
            this.RegisterLogin.Name = "RegisterLogin";
            this.RegisterLogin.Size = new System.Drawing.Size(220, 22);
            this.RegisterLogin.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Confirmar:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Senha:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Login:";
            // 
            // RegisterBNT
            // 
            this.RegisterBNT.Location = new System.Drawing.Point(9, 150);
            this.RegisterBNT.Name = "RegisterBNT";
            this.RegisterBNT.Size = new System.Drawing.Size(293, 25);
            this.RegisterBNT.TabIndex = 0;
            this.RegisterBNT.Text = "Registrar";
            this.RegisterBNT.UseVisualStyleBackColor = true;
            this.RegisterBNT.Click += new System.EventHandler(this.button1_Click);
            // 
            // AutoLoginChkBx
            // 
            this.AutoLoginChkBx.AutoSize = true;
            this.AutoLoginChkBx.Location = new System.Drawing.Point(6, 188);
            this.AutoLoginChkBx.Name = "AutoLoginChkBx";
            this.AutoLoginChkBx.Size = new System.Drawing.Size(180, 21);
            this.AutoLoginChkBx.TabIndex = 8;
            this.AutoLoginChkBx.Text = "Logar Automáticamente";
            this.AutoLoginChkBx.UseVisualStyleBackColor = true;
            // 
            // NoStyleLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 215);
            this.Controls.Add(this.AutoLoginChkBx);
            this.Controls.Add(this.RegisterPanel);
            this.Controls.Add(this.LoginPanel);
            this.Controls.Add(this.ChangeLBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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