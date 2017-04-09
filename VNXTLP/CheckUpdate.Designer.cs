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
            this.WaitLBL = new Tema.Tema_Label();
            this.LOGO = new Tema.Tema_Label();
            this.ZLoad = new Tema.Tema_ProgressIndicator();
            this.SuspendLayout();
            // 
            // WaitLBL
            // 
            this.WaitLBL.BackColor = System.Drawing.Color.Transparent;
            this.WaitLBL.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.WaitLBL.ForeColor = System.Drawing.Color.White;
            this.WaitLBL.Location = new System.Drawing.Point(2, 199);
            this.WaitLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WaitLBL.Name = "WaitLBL";
            this.WaitLBL.Size = new System.Drawing.Size(298, 30);
            this.WaitLBL.TabIndex = 2;
            this.WaitLBL.Text = "Procurando Atualizações...";
            this.WaitLBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LOGO
            // 
            this.LOGO.AutoSize = true;
            this.LOGO.BackColor = System.Drawing.Color.Transparent;
            this.LOGO.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.LOGO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(233)))));
            this.LOGO.Location = new System.Drawing.Point(96, 82);
            this.LOGO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LOGO.Name = "LOGO";
            this.LOGO.Size = new System.Drawing.Size(109, 46);
            this.LOGO.TabIndex = 1;
            this.LOGO.Text = "VNX+";
            this.LOGO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZLoad
            // 
            this.ZLoad.Location = new System.Drawing.Point(43, 7);
            this.ZLoad.Margin = new System.Windows.Forms.Padding(4);
            this.ZLoad.MinimumSize = new System.Drawing.Size(107, 98);
            this.ZLoad.Name = "ZLoad";
            this.ZLoad.P_AnimationColor = System.Drawing.Color.DimGray;
            this.ZLoad.P_AnimationSpeed = 100;
            this.ZLoad.P_BaseColor = System.Drawing.Color.DarkGray;
            this.ZLoad.P_CircleSize = new System.Drawing.Size(18, 18);
            this.ZLoad.Size = new System.Drawing.Size(191, 191);
            this.ZLoad.TabIndex = 0;
            this.ZLoad.Text = "Wait";
            // 
            // CheckUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(301, 244);
            this.Controls.Add(this.WaitLBL);
            this.Controls.Add(this.LOGO);
            this.Controls.Add(this.ZLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckUpdate";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VNTLP - VNX+";
            this.Shown += new System.EventHandler(this.Ready);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tema.Tema_ProgressIndicator ZLoad;
        private Tema.Tema_Label LOGO;
        private Tema.Tema_Label WaitLBL;
    }
}