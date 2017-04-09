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
            this.BackupList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // BackupList
            // 
            this.BackupList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupList.FormattingEnabled = true;
            this.BackupList.Location = new System.Drawing.Point(9, 10);
            this.BackupList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BackupList.Name = "BackupList";
            this.BackupList.Size = new System.Drawing.Size(312, 264);
            this.BackupList.TabIndex = 0;
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            // 
            // NoStyleBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 282);
            this.Controls.Add(this.BackupList);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimizeBox = false;
            this.Name = "NoStyleBackup";
#if DEBUG
            this.Text = "Carregando Backups...";
#else
            this.Text = Engine.LoadTranslation(0);
#endif
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox BackupList;
    }
}