namespace VNXTLP {
    partial class RegKey {
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
            this.bntOK = new System.Windows.Forms.Button();
            this.tbKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bntOK
            // 
            this.bntOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bntOK.Location = new System.Drawing.Point(387, 6);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(75, 23);
            this.bntOK.TabIndex = 0;
            this.bntOK.Text = "OK";
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // tbKey
            // 
            this.tbKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbKey.Location = new System.Drawing.Point(12, 8);
            this.tbKey.Name = "tbKey";
            this.tbKey.Size = new System.Drawing.Size(369, 20);
            this.tbKey.TabIndex = 1;
            // 
            // RegKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 36);
            this.Controls.Add(this.tbKey);
            this.Controls.Add(this.bntOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RegKey";
            this.Text = "Autorização de Registro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.TextBox tbKey;
    }
}