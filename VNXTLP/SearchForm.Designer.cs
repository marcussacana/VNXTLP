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
            this.LblLocalizar.Location = new System.Drawing.Point(7, 15);
            this.LblLocalizar.Name = "LblLocalizar";
            this.LblLocalizar.Size = new System.Drawing.Size(69, 17);
            this.LblLocalizar.TabIndex = 0;
            this.LblLocalizar.Text = "Localizar:";
            // 
            // txtContentToFind
            // 
            this.txtContentToFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentToFind.Location = new System.Drawing.Point(82, 12);
            this.txtContentToFind.Name = "txtContentToFind";
            this.txtContentToFind.Size = new System.Drawing.Size(534, 22);
            this.txtContentToFind.TabIndex = 1;
            // 
            // LblRep
            // 
            this.LblRep.AutoSize = true;
            this.LblRep.Location = new System.Drawing.Point(7, 51);
            this.LblRep.Name = "LblRep";
            this.LblRep.Size = new System.Drawing.Size(71, 17);
            this.LblRep.TabIndex = 2;
            this.LblRep.Text = "Substituir:";
            // 
            // txtContentToRep
            // 
            this.txtContentToRep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentToRep.Location = new System.Drawing.Point(82, 48);
            this.txtContentToRep.Name = "txtContentToRep";
            this.txtContentToRep.Size = new System.Drawing.Size(534, 22);
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
            this.GBAdvOpt.Location = new System.Drawing.Point(10, 81);
            this.GBAdvOpt.Name = "GBAdvOpt";
            this.GBAdvOpt.Size = new System.Drawing.Size(448, 118);
            this.GBAdvOpt.TabIndex = 4;
            this.GBAdvOpt.TabStop = false;
            this.GBAdvOpt.Text = "Opções Avançadas";
            // 
            // lblDirec
            // 
            this.lblDirec.AutoSize = true;
            this.lblDirec.Location = new System.Drawing.Point(216, 76);
            this.lblDirec.Name = "lblDirec";
            this.lblDirec.Size = new System.Drawing.Size(61, 17);
            this.lblDirec.TabIndex = 6;
            this.lblDirec.Text = "Direção:";
            // 
            // radioBack
            // 
            this.radioBack.AutoSize = true;
            this.radioBack.Location = new System.Drawing.Point(359, 74);
            this.radioBack.Name = "radioBack";
            this.radioBack.Size = new System.Drawing.Size(58, 21);
            this.radioBack.TabIndex = 5;
            this.radioBack.Text = "Trás";
            this.radioBack.UseVisualStyleBackColor = true;
            // 
            // radioUp
            // 
            this.radioUp.AutoSize = true;
            this.radioUp.Checked = true;
            this.radioUp.Location = new System.Drawing.Point(283, 74);
            this.radioUp.Name = "radioUp";
            this.radioUp.Size = new System.Drawing.Size(70, 21);
            this.radioUp.TabIndex = 4;
            this.radioUp.TabStop = true;
            this.radioUp.Text = "Frente";
            this.radioUp.UseVisualStyleBackColor = true;
            // 
            // ckCircle
            // 
            this.ckCircle.AutoSize = true;
            this.ckCircle.Location = new System.Drawing.Point(219, 21);
            this.ckCircle.Name = "ckCircle";
            this.ckCircle.Size = new System.Drawing.Size(140, 21);
            this.ckCircle.TabIndex = 3;
            this.ckCircle.Text = "Pesquisa Círcular";
            this.ckCircle.UseVisualStyleBackColor = true;
            // 
            // ckFindNonDiag
            // 
            this.ckFindNonDiag.AutoSize = true;
            this.ckFindNonDiag.Location = new System.Drawing.Point(6, 75);
            this.ckFindNonDiag.Name = "ckFindNonDiag";
            this.ckFindNonDiag.Size = new System.Drawing.Size(189, 21);
            this.ckFindNonDiag.TabIndex = 2;
            this.ckFindNonDiag.Text = "Considerar Não-Dialógos";
            this.ckFindNonDiag.UseVisualStyleBackColor = true;
            // 
            // ckWithContains
            // 
            this.ckWithContains.AutoSize = true;
            this.ckWithContains.Location = new System.Drawing.Point(6, 48);
            this.ckWithContains.Name = "ckWithContains";
            this.ckWithContains.Size = new System.Drawing.Size(124, 21);
            this.ckWithContains.TabIndex = 1;
            this.ckWithContains.Text = "Diálogos Exato\r\n";
            this.ckWithContains.UseVisualStyleBackColor = true;
            // 
            // ckCaseSensentive
            // 
            this.ckCaseSensentive.AutoSize = true;
            this.ckCaseSensentive.Checked = true;
            this.ckCaseSensentive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCaseSensentive.Location = new System.Drawing.Point(6, 21);
            this.ckCaseSensentive.Name = "ckCaseSensentive";
            this.ckCaseSensentive.Size = new System.Drawing.Size(136, 21);
            this.ckCaseSensentive.TabIndex = 0;
            this.ckCaseSensentive.Text = "Case Sensentive";
            this.ckCaseSensentive.UseVisualStyleBackColor = true;
            // 
            // ckRepFullDiag
            // 
            this.ckRepFullDiag.AutoSize = true;
            this.ckRepFullDiag.Location = new System.Drawing.Point(219, 48);
            this.ckRepFullDiag.Name = "ckRepFullDiag";
            this.ckRepFullDiag.Size = new System.Drawing.Size(185, 21);
            this.ckRepFullDiag.TabIndex = 0;
            this.ckRepFullDiag.Text = "Substituir todo o Diálogo";
            this.ckRepFullDiag.UseVisualStyleBackColor = true;
            // 
            // bntLocNext
            // 
            this.bntLocNext.Location = new System.Drawing.Point(464, 89);
            this.bntLocNext.Name = "bntLocNext";
            this.bntLocNext.Size = new System.Drawing.Size(152, 23);
            this.bntLocNext.TabIndex = 5;
            this.bntLocNext.Text = "Localizar Próximo";
            this.bntLocNext.UseVisualStyleBackColor = true;
            this.bntLocNext.Click += new System.EventHandler(this.bntLocNext_Click);
            // 
            // BntRepNext
            // 
            this.BntRepNext.Location = new System.Drawing.Point(464, 118);
            this.BntRepNext.Name = "BntRepNext";
            this.BntRepNext.Size = new System.Drawing.Size(152, 23);
            this.BntRepNext.TabIndex = 6;
            this.BntRepNext.Text = "Substituir";
            this.BntRepNext.UseVisualStyleBackColor = true;
            this.BntRepNext.Click += new System.EventHandler(this.BntRepNext_Click);
            // 
            // BntRepAll
            // 
            this.BntRepAll.Location = new System.Drawing.Point(464, 147);
            this.BntRepAll.Name = "BntRepAll";
            this.BntRepAll.Size = new System.Drawing.Size(152, 23);
            this.BntRepAll.TabIndex = 7;
            this.BntRepAll.Text = "Substituir Todos";
            this.BntRepAll.UseVisualStyleBackColor = true;
            this.BntRepAll.Click += new System.EventHandler(this.BntRepAll_Click);
            // 
            // bntDirFind
            // 
            this.bntDirFind.Location = new System.Drawing.Point(464, 176);
            this.bntDirFind.Name = "bntDirFind";
            this.bntDirFind.Size = new System.Drawing.Size(152, 23);
            this.bntDirFind.TabIndex = 8;
            this.bntDirFind.Text = "Localizar em Pasta";
            this.bntDirFind.UseVisualStyleBackColor = true;
            this.bntDirFind.Click += new System.EventHandler(this.bntDirFind_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 212);
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