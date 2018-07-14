namespace VNXTLP
{
    partial class CheckUpdate
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
            this.LOGO = new System.Windows.Forms.PictureBox();
            this.WaitLBL = new Tema.Tema_Label();
            this.ZLoad = new Tema.Tema_ProgressIndicator();
            ((System.ComponentModel.ISupportInitialize)(this.LOGO)).BeginInit();
            this.SuspendLayout();
            // 
            // LOGO
            // 
            this.LOGO.Image = global::VNXTLP.Properties.Resources.Logo;
            this.LOGO.Location = new System.Drawing.Point(77, 51);
            this.LOGO.Name = "LOGO";
            this.LOGO.Size = new System.Drawing.Size(70, 70);
            this.LOGO.TabIndex = 3;
            this.LOGO.TabStop = false;
            // 
            // WaitLBL
            // 
            this.WaitLBL.BackColor = System.Drawing.Color.Transparent;
            this.WaitLBL.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.WaitLBL.ForeColor = System.Drawing.Color.White;
            this.WaitLBL.Location = new System.Drawing.Point(2, 162);
            this.WaitLBL.Name = "WaitLBL";
            this.WaitLBL.Size = new System.Drawing.Size(224, 24);
            this.WaitLBL.TabIndex = 2;
            this.WaitLBL.Text = "Procurando Atualizações...";
            this.WaitLBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ZLoad
            // 
            this.ZLoad.Location = new System.Drawing.Point(32, 6);
            this.ZLoad.MinimumSize = new System.Drawing.Size(80, 80);
            this.ZLoad.Name = "ZLoad";
            this.ZLoad.P_AnimationColor = System.Drawing.Color.DarkGray;
            this.ZLoad.P_AnimationSpeed = 100;
            this.ZLoad.P_BaseColor = System.Drawing.Color.White;
            this.ZLoad.P_CircleSize = new System.Drawing.Size(18, 18);
            this.ZLoad.Size = new System.Drawing.Size(155, 155);
            this.ZLoad.TabIndex = 0;
            this.ZLoad.Text = "Wait";
            // 
            // CheckUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(226, 198);
            this.Controls.Add(this.LOGO);
            this.Controls.Add(this.WaitLBL);
            this.Controls.Add(this.ZLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckUpdate";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VNTLP - VNX+";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.Ready);
            ((System.ComponentModel.ISupportInitialize)(this.LOGO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Tema.Tema_ProgressIndicator ZLoad;
        private Tema.Tema_Label WaitLBL;
        private System.Windows.Forms.PictureBox LOGO;
    }
}