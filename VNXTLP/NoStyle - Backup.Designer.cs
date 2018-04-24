namespace VNXTLP {
    partial class NoStyleBackup {
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
            this.components = new System.ComponentModel.Container();
            this.BackupList = new System.Windows.Forms.ListBox();
            this.ItemMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.explorarUsuáriosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackupList
            // 
            this.BackupList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupList.ContextMenuStrip = this.ItemMenu;
            this.BackupList.FormattingEnabled = true;
            this.BackupList.Location = new System.Drawing.Point(9, 10);
            this.BackupList.Margin = new System.Windows.Forms.Padding(2);
            this.BackupList.Name = "BackupList";
            this.BackupList.Size = new System.Drawing.Size(312, 264);
            this.BackupList.TabIndex = 0;
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            // 
            // ItemMenu
            // 
            this.ItemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.deletarToolStripMenuItem,
            this.explorarUsuáriosToolStripMenuItem});
            this.ItemMenu.Name = "ItemMenu";
            this.ItemMenu.Size = new System.Drawing.Size(165, 92);
            this.ItemMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ItemMenu_Opening);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // deletarToolStripMenuItem
            // 
            this.deletarToolStripMenuItem.Name = "deletarToolStripMenuItem";
            this.deletarToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deletarToolStripMenuItem.Text = "Deletar";
            this.deletarToolStripMenuItem.Click += new System.EventHandler(this.deletarToolStripMenuItem_Click);
            // 
            // explorarUsuáriosToolStripMenuItem
            // 
            this.explorarUsuáriosToolStripMenuItem.Name = "explorarUsuáriosToolStripMenuItem";
            this.explorarUsuáriosToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.explorarUsuáriosToolStripMenuItem.Text = "Explorar Usuários";
            this.explorarUsuáriosToolStripMenuItem.Click += new System.EventHandler(this.explorarUsuáriosToolStripMenuItem_Click);
            // 
            // NoStyleBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 282);
            this.Controls.Add(this.BackupList);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.Name = "NoStyleBackup";
            this.Text = "Carregando Backups...";
            this.ItemMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox BackupList;
        private System.Windows.Forms.ContextMenuStrip ItemMenu;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorarUsuáriosToolStripMenuItem;
    }
}