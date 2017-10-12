namespace VNXTLP {
    partial class Search {
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
            this.LblLocalizar = new System.Windows.Forms.Label();
            this.txtContentToFind = new System.Windows.Forms.TextBox();
            this.LblRep = new System.Windows.Forms.Label();
            this.txtContentToRep = new System.Windows.Forms.TextBox();
            this.GBAdvOpt = new System.Windows.Forms.GroupBox();
            this.lblDirec = new System.Windows.Forms.Label();
            this.radioBack = new System.Windows.Forms.RadioButton();
            this.radioUp = new System.Windows.Forms.RadioButton();
            this.ckCircle = new System.Windows.Forms.CheckBox();
            this.ckFindNonDiag = new System.Windows.Forms.CheckBox();
            this.ckWithContains = new System.Windows.Forms.CheckBox();
            this.ckCaseSensentive = new System.Windows.Forms.CheckBox();
            this.ckRepFullDiag = new System.Windows.Forms.CheckBox();
            this.bntLocNext = new System.Windows.Forms.Button();
            this.BntRepNext = new System.Windows.Forms.Button();
            this.BntRepAll = new System.Windows.Forms.Button();
            this.bntDirFind = new System.Windows.Forms.Button();
            this.GBAdvOpt.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblLocalizar
            // 
            this.LblLocalizar.AutoSize = true;
            this.LblLocalizar.Location = new System.Drawing.Point(5, 12);
            this.LblLocalizar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblLocalizar.Name = "LblLocalizar";
            this.LblLocalizar.Size = new System.Drawing.Size(52, 13);
            this.LblLocalizar.TabIndex = 0;
            this.LblLocalizar.Text = "Localizar:";
            // 
            // txtContentToFind
            // 
            this.txtContentToFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentToFind.Location = new System.Drawing.Point(62, 10);
            this.txtContentToFind.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtContentToFind.Name = "txtContentToFind";
            this.txtContentToFind.Size = new System.Drawing.Size(402, 20);
            this.txtContentToFind.TabIndex = 1;
            // 
            // LblRep
            // 
            this.LblRep.AutoSize = true;
            this.LblRep.Location = new System.Drawing.Point(5, 41);
            this.LblRep.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblRep.Name = "LblRep";
            this.LblRep.Size = new System.Drawing.Size(53, 13);
            this.LblRep.TabIndex = 2;
            this.LblRep.Text = "Substituir:";
            // 
            // txtContentToRep
            // 
            this.txtContentToRep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentToRep.Location = new System.Drawing.Point(62, 39);
            this.txtContentToRep.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtContentToRep.Name = "txtContentToRep";
            this.txtContentToRep.Size = new System.Drawing.Size(402, 20);
            this.txtContentToRep.TabIndex = 3;
            // 
            // GBAdvOpt
            // 
            this.GBAdvOpt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBAdvOpt.Controls.Add(this.lblDirec);
            this.GBAdvOpt.Controls.Add(this.radioBack);
            this.GBAdvOpt.Controls.Add(this.radioUp);
            this.GBAdvOpt.Controls.Add(this.ckCircle);
            this.GBAdvOpt.Controls.Add(this.ckFindNonDiag);
            this.GBAdvOpt.Controls.Add(this.ckWithContains);
            this.GBAdvOpt.Controls.Add(this.ckCaseSensentive);
            this.GBAdvOpt.Controls.Add(this.ckRepFullDiag);
            this.GBAdvOpt.Location = new System.Drawing.Point(8, 66);
            this.GBAdvOpt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GBAdvOpt.Name = "GBAdvOpt";
            this.GBAdvOpt.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GBAdvOpt.Size = new System.Drawing.Size(336, 96);
            this.GBAdvOpt.TabIndex = 4;
            this.GBAdvOpt.TabStop = false;
            this.GBAdvOpt.Text = "Opções Avançadas";
            // 
            // lblDirec
            // 
            this.lblDirec.AutoSize = true;
            this.lblDirec.Location = new System.Drawing.Point(162, 62);
            this.lblDirec.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDirec.Name = "lblDirec";
            this.lblDirec.Size = new System.Drawing.Size(47, 13);
            this.lblDirec.TabIndex = 6;
            this.lblDirec.Text = "Direção:";
            // 
            // radioBack
            // 
            this.radioBack.AutoSize = true;
            this.radioBack.Location = new System.Drawing.Point(269, 60);
            this.radioBack.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioBack.Name = "radioBack";
            this.radioBack.Size = new System.Drawing.Size(46, 17);
            this.radioBack.TabIndex = 5;
            this.radioBack.Text = "Trás";
            this.radioBack.UseVisualStyleBackColor = true;
            // 
            // radioUp
            // 
            this.radioUp.AutoSize = true;
            this.radioUp.Checked = true;
            this.radioUp.Location = new System.Drawing.Point(212, 60);
            this.radioUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioUp.Name = "radioUp";
            this.radioUp.Size = new System.Drawing.Size(55, 17);
            this.radioUp.TabIndex = 4;
            this.radioUp.TabStop = true;
            this.radioUp.Text = "Frente";
            this.radioUp.UseVisualStyleBackColor = true;
            // 
            // ckCircle
            // 
            this.ckCircle.AutoSize = true;
            this.ckCircle.Checked = true;
            this.ckCircle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCircle.Enabled = false;
            this.ckCircle.Location = new System.Drawing.Point(164, 17);
            this.ckCircle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckCircle.Name = "ckCircle";
            this.ckCircle.Size = new System.Drawing.Size(109, 17);
            this.ckCircle.TabIndex = 3;
            this.ckCircle.Text = "Pesquisa Círcular";
            this.ckCircle.UseVisualStyleBackColor = true;
            // 
            // ckFindNonDiag
            // 
            this.ckFindNonDiag.AutoSize = true;
            this.ckFindNonDiag.Location = new System.Drawing.Point(4, 61);
            this.ckFindNonDiag.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckFindNonDiag.Name = "ckFindNonDiag";
            this.ckFindNonDiag.Size = new System.Drawing.Size(143, 17);
            this.ckFindNonDiag.TabIndex = 2;
            this.ckFindNonDiag.Text = "Considerar Não-Dialógos";
            this.ckFindNonDiag.UseVisualStyleBackColor = true;
            // 
            // ckWithContains
            // 
            this.ckWithContains.AutoSize = true;
            this.ckWithContains.Location = new System.Drawing.Point(4, 39);
            this.ckWithContains.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckWithContains.Name = "ckWithContains";
            this.ckWithContains.Size = new System.Drawing.Size(97, 17);
            this.ckWithContains.TabIndex = 1;
            this.ckWithContains.Text = "Diálogos Exato\r\n";
            this.ckWithContains.UseVisualStyleBackColor = true;
            // 
            // ckCaseSensentive
            // 
            this.ckCaseSensentive.AutoSize = true;
            this.ckCaseSensentive.Checked = true;
            this.ckCaseSensentive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCaseSensentive.Location = new System.Drawing.Point(4, 17);
            this.ckCaseSensentive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckCaseSensentive.Name = "ckCaseSensentive";
            this.ckCaseSensentive.Size = new System.Drawing.Size(106, 17);
            this.ckCaseSensentive.TabIndex = 0;
            this.ckCaseSensentive.Text = "Case Sensentive";
            this.ckCaseSensentive.UseVisualStyleBackColor = true;
            // 
            // ckRepFullDiag
            // 
            this.ckRepFullDiag.AutoSize = true;
            this.ckRepFullDiag.Location = new System.Drawing.Point(164, 39);
            this.ckRepFullDiag.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckRepFullDiag.Name = "ckRepFullDiag";
            this.ckRepFullDiag.Size = new System.Drawing.Size(141, 17);
            this.ckRepFullDiag.TabIndex = 0;
            this.ckRepFullDiag.Text = "Substituir todo o Diálogo";
            this.ckRepFullDiag.UseVisualStyleBackColor = true;
            // 
            // bntLocNext
            // 
            this.bntLocNext.Location = new System.Drawing.Point(348, 72);
            this.bntLocNext.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntLocNext.Name = "bntLocNext";
            this.bntLocNext.Size = new System.Drawing.Size(114, 19);
            this.bntLocNext.TabIndex = 5;
            this.bntLocNext.Text = "Localizar Próximo";
            this.bntLocNext.UseVisualStyleBackColor = true;
            this.bntLocNext.Click += new System.EventHandler(this.bntLocNext_Click);
            // 
            // BntRepNext
            // 
            this.BntRepNext.Location = new System.Drawing.Point(348, 96);
            this.BntRepNext.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BntRepNext.Name = "BntRepNext";
            this.BntRepNext.Size = new System.Drawing.Size(114, 19);
            this.BntRepNext.TabIndex = 6;
            this.BntRepNext.Text = "Substituir";
            this.BntRepNext.UseVisualStyleBackColor = true;
            this.BntRepNext.Click += new System.EventHandler(this.BntRepNext_Click);
            // 
            // BntRepAll
            // 
            this.BntRepAll.Location = new System.Drawing.Point(348, 119);
            this.BntRepAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BntRepAll.Name = "BntRepAll";
            this.BntRepAll.Size = new System.Drawing.Size(114, 19);
            this.BntRepAll.TabIndex = 7;
            this.BntRepAll.Text = "Substituir Todos";
            this.BntRepAll.UseVisualStyleBackColor = true;
            this.BntRepAll.Click += new System.EventHandler(this.BntRepAll_Click);
            // 
            // bntDirFind
            // 
            this.bntDirFind.Location = new System.Drawing.Point(348, 143);
            this.bntDirFind.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntDirFind.Name = "bntDirFind";
            this.bntDirFind.Size = new System.Drawing.Size(114, 19);
            this.bntDirFind.TabIndex = 8;
            this.bntDirFind.Text = "Localizar em Pasta";
            this.bntDirFind.UseVisualStyleBackColor = true;
            this.bntDirFind.Click += new System.EventHandler(this.bntDirFind_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 172);
            this.Controls.Add(this.bntDirFind);
            this.Controls.Add(this.BntRepAll);
            this.Controls.Add(this.BntRepNext);
            this.Controls.Add(this.bntLocNext);
            this.Controls.Add(this.GBAdvOpt);
            this.Controls.Add(this.txtContentToRep);
            this.Controls.Add(this.LblRep);
            this.Controls.Add(this.txtContentToFind);
            this.Controls.Add(this.LblLocalizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Search";
            this.Text = "Pesquisar";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Search_FormClosing);
            this.Shown += new System.EventHandler(this.Search_Shown);
            this.GBAdvOpt.ResumeLayout(false);
            this.GBAdvOpt.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblLocalizar;
        private System.Windows.Forms.TextBox txtContentToFind;
        private System.Windows.Forms.Label LblRep;
        private System.Windows.Forms.TextBox txtContentToRep;
        private System.Windows.Forms.GroupBox GBAdvOpt;
        private System.Windows.Forms.Label lblDirec;
        private System.Windows.Forms.RadioButton radioBack;
        private System.Windows.Forms.RadioButton radioUp;
        private System.Windows.Forms.CheckBox ckCircle;
        private System.Windows.Forms.CheckBox ckFindNonDiag;
        private System.Windows.Forms.CheckBox ckWithContains;
        private System.Windows.Forms.CheckBox ckCaseSensentive;
        private System.Windows.Forms.CheckBox ckRepFullDiag;
        private System.Windows.Forms.Button bntLocNext;
        private System.Windows.Forms.Button BntRepNext;
        private System.Windows.Forms.Button BntRepAll;
        private System.Windows.Forms.Button bntDirFind;
    }
}