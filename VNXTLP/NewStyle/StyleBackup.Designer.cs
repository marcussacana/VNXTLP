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
            Tema.ControlRenderer controlRenderer1 = new Tema.ControlRenderer();
            Tema.MSColorTable msColorTable1 = new Tema.MSColorTable();
            this.ZSKN = new Tema.Tema_ThemeContainer();
            this.ZControl = new Tema.Thema_ControlBox();
            this.ZOK = new Tema.Tema_Button_2();
            this.BackupList = new System.Windows.Forms.ListBox();
            this.ZItemMenu = new Tema.Tema_ContextMenuStrip();
            this.ZAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.ZDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ZS1 = new Tema.Tema_Separator();
            this.ZSKN.SuspendLayout();
            this.ZItemMenu.SuspendLayout();
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
            this.ZSKN.Name = "ZSKN";
            this.ZSKN.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.ZSKN.Sizable = true;
            this.ZSKN.Size = new System.Drawing.Size(425, 371);
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
            this.ZControl.Location = new System.Drawing.Point(384, 0);
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
            this.ZOK.Location = new System.Drawing.Point(132, 299);
            this.ZOK.Name = "ZOK";
            this.ZOK.Size = new System.Drawing.Size(166, 40);
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
            this.BackupList.Size = new System.Drawing.Size(403, 236);
            this.BackupList.TabIndex = 8;
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            // 
            // ZItemMenu
            // 
            this.ZItemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZAbrir,
            this.ZDelete});
            this.ZItemMenu.Name = "ZItemMenu";
            controlRenderer1.ColorTable = msColorTable1;
            controlRenderer1.RoundedEdges = true;
            this.ZItemMenu.Renderer = controlRenderer1;
            this.ZItemMenu.Size = new System.Drawing.Size(112, 48);
            this.ZItemMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ZOpenMenu);
            // 
            // ZAbrir
            // 
            this.ZAbrir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ZAbrir.Name = "ZAbrir";
            this.ZAbrir.Size = new System.Drawing.Size(111, 22);
            this.ZAbrir.Text = "Abrir";
            this.ZAbrir.Click += new System.EventHandler(this.ZAbrir_Click);
            // 
            // ZDelete
            // 
            this.ZDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ZDelete.Name = "ZDelete";
            this.ZDelete.Size = new System.Drawing.Size(111, 22);
            this.ZDelete.Text = "Deletar";
            this.ZDelete.Click += new System.EventHandler(this.ZDelete_Click);
            // 
            // ZS1
            // 
            this.ZS1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZS1.Location = new System.Drawing.Point(31, 285);
            this.ZS1.Name = "ZS1";
            this.ZS1.Size = new System.Drawing.Size(366, 10);
            this.ZS1.TabIndex = 9;
            this.ZS1.Text = "tema_Separator1";
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

        private Tema.Tema_ThemeContainer ZSKN;
        private System.Windows.Forms.ListBox BackupList;
        private Tema.Tema_Separator ZS1;
        private Tema.Tema_Button_2 ZOK;
        private Tema.Thema_ControlBox ZControl;
        private Tema.Tema_ContextMenuStrip ZItemMenu;
        private System.Windows.Forms.ToolStripMenuItem ZAbrir;
        private System.Windows.Forms.ToolStripMenuItem ZDelete;
    }
}