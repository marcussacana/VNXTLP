namespace VNXTLP.NewStyle
{
    partial class StyleBackup
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
            this.ZSKN = new Tema.Tema_ThemeContainer();
            this.ZControl = new Tema.Thema_ControlBox();
            this.ZOK = new Tema.Tema_Button_2();
            this.BackupList = new System.Windows.Forms.ListBox();
            this.ZS1 = new Tema.Tema_Separator();
            this.ZSKN.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZSKN
            // 
            this.ZSKN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ZSKN.Controls.Add(this.ZControl);
            this.ZSKN.Controls.Add(this.ZOK);
            this.ZSKN.Controls.Add(this.BackupList);
            this.ZSKN.Controls.Add(this.ZS1);
            this.ZSKN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZSKN.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ZSKN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.ZSKN.Location = new System.Drawing.Point(0, 0);
            this.ZSKN.Margin = new System.Windows.Forms.Padding(4);
            this.ZSKN.Name = "ZSKN";
            this.ZSKN.Padding = new System.Windows.Forms.Padding(4, 34, 4, 34);
            this.ZSKN.Sizable = true;
            this.ZSKN.Size = new System.Drawing.Size(567, 457);
            this.ZSKN.SmartBounds = true;
            this.ZSKN.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.ZSKN.TabIndex = 2;
            this.ZSKN.Text = "Carregando Backups...";
            // 
            // ZControl
            // 
            this.ZControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZControl.BackColor = System.Drawing.Color.Transparent;
            this.ZControl.EnableMaximize = true;
            this.ZControl.EnableMinimize = false;
            this.ZControl.Font = new System.Drawing.Font("Marlett", 7F);
            this.ZControl.Location = new System.Drawing.Point(512, 0);
            this.ZControl.Margin = new System.Windows.Forms.Padding(4);
            this.ZControl.Name = "ZControl";
            this.ZControl.Size = new System.Drawing.Size(64, 22);
            this.ZControl.TabIndex = 12;
            this.ZControl.Text = "ambiance_ControlBox1";
            // 
            // ZOK
            // 
            this.ZOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ZOK.BackColor = System.Drawing.Color.Transparent;
            this.ZOK.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ZOK.ForeColor = System.Drawing.Color.White;
            this.ZOK.Image = null;
            this.ZOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ZOK.Location = new System.Drawing.Point(176, 368);
            this.ZOK.Margin = new System.Windows.Forms.Padding(4);
            this.ZOK.Name = "ZOK";
            this.ZOK.Size = new System.Drawing.Size(221, 49);
            this.ZOK.TabIndex = 10;
            this.ZOK.Text = "Fechar";
            this.ZOK.TextAlignment = System.Drawing.StringAlignment.Center;
            this.ZOK.Click += new System.EventHandler(this.ZOK_Click);
            // 
            // BackupList
            // 
            this.BackupList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.BackupList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackupList.FormattingEnabled = true;
            this.BackupList.ItemHeight = 17;
            this.BackupList.Location = new System.Drawing.Point(15, 41);
            this.BackupList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BackupList.Name = "BackupList";
            this.BackupList.Size = new System.Drawing.Size(537, 291);
            this.BackupList.TabIndex = 8;
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            // 
            // ZS1
            // 
            this.ZS1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZS1.Location = new System.Drawing.Point(41, 351);
            this.ZS1.Margin = new System.Windows.Forms.Padding(4);
            this.ZS1.Name = "ZS1";
            this.ZS1.Size = new System.Drawing.Size(488, 12);
            this.ZS1.TabIndex = 9;
            this.ZS1.Text = "tema_Separator1";
            // 
            // StyleBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 457);
            this.Controls.Add(this.ZSKN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(567, 457);
            this.Name = "StyleBackup";
            this.Text = "Carregando Backups...";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ZSKN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

#endregion

        private Tema.Tema_ThemeContainer ZSKN;
        private System.Windows.Forms.ListBox BackupList;
        private Tema.Tema_Separator ZS1;
        private Tema.Tema_Button_2 ZOK;
        private Tema.Thema_ControlBox ZControl;
    }
}