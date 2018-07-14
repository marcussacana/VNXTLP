namespace VNXTLP.BernStyle
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
            CTema.ControlRenderer controlRenderer1 = new CTema.ControlRenderer();
            CTema.MSColorTable msColorTable1 = new CTema.MSColorTable();
            this.ZSKN = new CTema.CTema_ThemeContainer();
            this.thema_ControlBox1 = new CTema.Thema_ControlBox();
            this.ZOK = new CTema.CTema_Button_2();
            this.BackupList = new System.Windows.Forms.ListBox();
            this.ZItemMenu = new CTema.CTema_ContextMenuStrip();
            this.ZAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.ZDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ZExplorar = new System.Windows.Forms.ToolStripMenuItem();
            this.ZSKN.SuspendLayout();
            this.ZItemMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZSKN
            // 
            this.ZSKN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ZSKN.Controls.Add(this.thema_ControlBox1);
            this.ZSKN.Controls.Add(this.ZOK);
            this.ZSKN.Controls.Add(this.BackupList);
            this.ZSKN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZSKN.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ZSKN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.ZSKN.Location = new System.Drawing.Point(0, 0);
            this.ZSKN.Name = "ZSKN";
            this.ZSKN.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.ZSKN.Sizable = true;
            this.ZSKN.Size = new System.Drawing.Size(425, 371);
            this.ZSKN.SmartBounds = true;
            this.ZSKN.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.ZSKN.TabIndex = 2;
            this.ZSKN.Text = "Carregando Backups...";
            // 
            // thema_ControlBox1
            // 
            this.thema_ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thema_ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.thema_ControlBox1.EnableMaximize = false;
            this.thema_ControlBox1.EnableMinimize = false;
            this.thema_ControlBox1.Font = new System.Drawing.Font("Marlett", 7F);
            this.thema_ControlBox1.Location = new System.Drawing.Point(353, 0);
            this.thema_ControlBox1.Name = "thema_ControlBox1";
            this.thema_ControlBox1.Size = new System.Drawing.Size(72, 24);
            this.thema_ControlBox1.TabIndex = 11;
            this.thema_ControlBox1.Text = "thema_ControlBox1";
            // 
            // ZOK
            // 
            this.ZOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ZOK.BackColor = System.Drawing.Color.Transparent;
            this.ZOK.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ZOK.ForeColor = System.Drawing.Color.White;
            this.ZOK.Image = null;
            this.ZOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ZOK.Location = new System.Drawing.Point(11, 299);
            this.ZOK.Name = "ZOK";
            this.ZOK.Size = new System.Drawing.Size(402, 40);
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
            this.BackupList.ContextMenuStrip = this.ZItemMenu;
            this.BackupList.FormattingEnabled = true;
            this.BackupList.Location = new System.Drawing.Point(11, 33);
            this.BackupList.Margin = new System.Windows.Forms.Padding(2);
            this.BackupList.Name = "BackupList";
            this.BackupList.Size = new System.Drawing.Size(403, 262);
            this.BackupList.TabIndex = 8;
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            // 
            // ZItemMenu
            // 
            this.ZItemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZAbrir,
            this.ZDelete,
            this.ZExplorar});
            this.ZItemMenu.Name = "ZItemMenu";
            controlRenderer1.ColorTable = msColorTable1;
            controlRenderer1.RoundedEdges = true;
            this.ZItemMenu.Renderer = controlRenderer1;
            this.ZItemMenu.Size = new System.Drawing.Size(160, 70);
            this.ZItemMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ZOpenMenu);
            // 
            // ZAbrir
            // 
            this.ZAbrir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ZAbrir.Name = "ZAbrir";
            this.ZAbrir.Size = new System.Drawing.Size(159, 22);
            this.ZAbrir.Text = "Abrir";
            this.ZAbrir.Click += new System.EventHandler(this.ZAbrir_Click);
            // 
            // ZDelete
            // 
            this.ZDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ZDelete.Name = "ZDelete";
            this.ZDelete.Size = new System.Drawing.Size(159, 22);
            this.ZDelete.Text = "Deletar";
            this.ZDelete.Click += new System.EventHandler(this.ZDelete_Click);
            // 
            // ZExplorar
            // 
            this.ZExplorar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ZExplorar.Name = "ZExplorar";
            this.ZExplorar.Size = new System.Drawing.Size(159, 22);
            this.ZExplorar.Text = "Explorar Usuário";
            this.ZExplorar.Click += new System.EventHandler(this.ZExplorar_Click);
            // 
            // StyleBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 371);
            this.Controls.Add(this.ZSKN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(425, 371);
            this.Name = "StyleBackup";
            this.Text = "Carregando Backups...";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ZSKN.ResumeLayout(false);
            this.ZItemMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

#endregion

        private  CTema.CTema_ThemeContainer ZSKN;
        private System.Windows.Forms.ListBox BackupList;
        private  CTema.CTema_Button_2 ZOK;
        private  CTema.CTema_ContextMenuStrip ZItemMenu;
        private System.Windows.Forms.ToolStripMenuItem ZAbrir;
        private System.Windows.Forms.ToolStripMenuItem ZDelete;
        private System.Windows.Forms.ToolStripMenuItem ZExplorar;
        private CTema.Thema_ControlBox thema_ControlBox1;
    }
}