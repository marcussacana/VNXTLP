using VNXTLP;

namespace VNXTLP.BernStyle
{
    partial class StyleLogin
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
            this.ZSKN = new CTema.CTema_ThemeContainer();
            this.ZRegistrar = new CTema.CTema_LinkLabel();
            this.ZEnt = new CTema.CTema_Button_2();
            this.LB2 = new CTema.CTema_Label();
            this.LB1 = new CTema.CTema_Label();
            this.PassTB = new CTema.CTema_TextBox_Small();
            this.LoginTB = new CTema.CTema_TextBox_Small();
            this.thema_ControlBox1 = new CTema.Thema_ControlBox();
            this.ZSKN.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZSKN
            // 
            this.ZSKN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ZSKN.Controls.Add(this.thema_ControlBox1);
            this.ZSKN.Controls.Add(this.ZRegistrar);
            this.ZSKN.Controls.Add(this.ZEnt);
            this.ZSKN.Controls.Add(this.LB2);
            this.ZSKN.Controls.Add(this.LB1);
            this.ZSKN.Controls.Add(this.PassTB);
            this.ZSKN.Controls.Add(this.LoginTB);
            this.ZSKN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZSKN.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ZSKN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.ZSKN.Location = new System.Drawing.Point(0, 0);
            this.ZSKN.Name = "ZSKN";
            this.ZSKN.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.ZSKN.Sizable = false;
            this.ZSKN.Size = new System.Drawing.Size(557, 275);
            this.ZSKN.SmartBounds = false;
            this.ZSKN.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.ZSKN.TabIndex = 1;
            this.ZSKN.Text = "Minha Conta - VNX+";
            // 
            // ZRegistrar
            // 
            this.ZRegistrar.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(202)))));
            this.ZRegistrar.AutoSize = true;
            this.ZRegistrar.BackColor = System.Drawing.Color.Transparent;
            this.ZRegistrar.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ZRegistrar.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ZRegistrar.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(225)))));
            this.ZRegistrar.Location = new System.Drawing.Point(243, 227);
            this.ZRegistrar.Name = "ZRegistrar";
            this.ZRegistrar.Size = new System.Drawing.Size(72, 13);
            this.ZRegistrar.TabIndex = 3;
            this.ZRegistrar.TabStop = true;
            this.ZRegistrar.Text = "Registrar-me";
            this.ZRegistrar.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(202)))));
            this.ZRegistrar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ZRegistrar_LinkClicked);
            // 
            // ZEnt
            // 
            this.ZEnt.BackColor = System.Drawing.Color.Transparent;
            this.ZEnt.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ZEnt.ForeColor = System.Drawing.Color.White;
            this.ZEnt.Image = null;
            this.ZEnt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ZEnt.Location = new System.Drawing.Point(139, 178);
            this.ZEnt.Name = "ZEnt";
            this.ZEnt.Size = new System.Drawing.Size(277, 40);
            this.ZEnt.TabIndex = 2;
            this.ZEnt.Text = "Fazer Login";
            this.ZEnt.TextAlignment = System.Drawing.StringAlignment.Center;
            this.ZEnt.Click += new System.EventHandler(this.ZEnt_Click);
            // 
            // LB2
            // 
            this.LB2.AutoSize = true;
            this.LB2.BackColor = System.Drawing.Color.Transparent;
            this.LB2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LB2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.LB2.Location = new System.Drawing.Point(257, 119);
            this.LB2.Name = "LB2";
            this.LB2.Size = new System.Drawing.Size(39, 13);
            this.LB2.TabIndex = 4;
            this.LB2.Text = "Senha";
            // 
            // LB1
            // 
            this.LB1.AutoSize = true;
            this.LB1.BackColor = System.Drawing.Color.Transparent;
            this.LB1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LB1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.LB1.Location = new System.Drawing.Point(253, 66);
            this.LB1.Name = "LB1";
            this.LB1.Size = new System.Drawing.Size(47, 13);
            this.LB1.TabIndex = 3;
            this.LB1.Text = "Usuário";
            // 
            // PassTB
            // 
            this.PassTB.BackColor = System.Drawing.Color.Transparent;
            this.PassTB.Font = new System.Drawing.Font("Tahoma", 11F);
            this.PassTB.ForeColor = System.Drawing.Color.DimGray;
            this.PassTB.Location = new System.Drawing.Point(80, 134);
            this.PassTB.MaxLength = 32767;
            this.PassTB.Multiline = false;
            this.PassTB.Name = "PassTB";
            this.PassTB.ReadOnly = false;
            this.PassTB.Size = new System.Drawing.Size(397, 28);
            this.PassTB.TabIndex = 1;
            this.PassTB.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.PassTB.UseSystemPasswordChar = true;
            this.PassTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyLoginPress);
            // 
            // LoginTB
            // 
            this.LoginTB.BackColor = System.Drawing.Color.Transparent;
            this.LoginTB.Font = new System.Drawing.Font("Tahoma", 11F);
            this.LoginTB.ForeColor = System.Drawing.Color.DimGray;
            this.LoginTB.Location = new System.Drawing.Point(78, 82);
            this.LoginTB.MaxLength = 32767;
            this.LoginTB.Multiline = false;
            this.LoginTB.Name = "LoginTB";
            this.LoginTB.ReadOnly = false;
            this.LoginTB.Size = new System.Drawing.Size(397, 28);
            this.LoginTB.TabIndex = 0;
            this.LoginTB.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.LoginTB.UseSystemPasswordChar = false;
            // 
            // thema_ControlBox1
            // 
            this.thema_ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thema_ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.thema_ControlBox1.EnableMaximize = false;
            this.thema_ControlBox1.EnableMinimize = false;
            this.thema_ControlBox1.Font = new System.Drawing.Font("Marlett", 7F);
            this.thema_ControlBox1.Location = new System.Drawing.Point(485, 0);
            this.thema_ControlBox1.Name = "thema_ControlBox1";
            this.thema_ControlBox1.Size = new System.Drawing.Size(72, 24);
            this.thema_ControlBox1.TabIndex = 5;
            this.thema_ControlBox1.Text = "thema_ControlBox1";
            // 
            // StyleLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 275);
            this.Controls.Add(this.ZSKN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(126, 39);
            this.Name = "StyleLogin";
            this.Text = "Minha Conta - VNX+";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ZSKN.ResumeLayout(false);
            this.ZSKN.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion

        private CTema.CTema_ThemeContainer ZSKN;
        private CTema.CTema_Button_2 ZEnt;
        private CTema.CTema_Label LB2;
        private CTema.CTema_Label LB1;
        private CTema.CTema_TextBox_Small PassTB;
        private CTema.CTema_TextBox_Small LoginTB;
        private CTema.CTema_LinkLabel ZRegistrar;
        private CTema.Thema_ControlBox thema_ControlBox1;
    }
}