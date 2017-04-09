using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class Search : Form {
        internal Search() {
            InitializeComponent();

            LblLocalizar.Text = Engine.LoadTranslation(59);
            LblRep.Text = Engine.LoadTranslation(60);
            GBAdvOpt.Text = Engine.LoadTranslation(73);
            ckCaseSensentive.Text = Engine.LoadTranslation(74);
            ckWithContains.Text = Engine.LoadTranslation(75);
            ckFindNonDiag.Text = Engine.LoadTranslation(76);
            ckCircle.Text = Engine.LoadTranslation(77);
            ckRepFullDiag.Text = Engine.LoadTranslation(78);
            lblDirec.Text = Engine.LoadTranslation(79);
            radioUp.Text = Engine.LoadTranslation(80);
            radioBack.Text = Engine.LoadTranslation(81);
            bntLocNext.Text = Engine.LoadTranslation(82);
            BntRepNext.Text = Engine.LoadTranslation(83);
            BntRepAll.Text = Engine.LoadTranslation(84);
            bntDirFind.Text = Engine.LoadTranslation(86);
        }

        private void bntLocNext_Click(object sender, EventArgs e) {
            Engine.SearchNext(txtContentToFind.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, false);
        }

        private void BntRepNext_Click(object sender, EventArgs e) {
            if (Engine.ReplaceNext(txtContentToFind.Text, txtContentToRep.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, ckRepFullDiag.Checked, false))
                MessageBox.Show(string.Format(Engine.LoadTranslation(72), 1), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BntRepAll_Click(object sender, EventArgs e) {
            uint reps = 0;
            while (Engine.ReplaceNext(txtContentToFind.Text, txtContentToRep.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, ckRepFullDiag.Checked, true))
                reps++;
            MessageBox.Show(string.Format(Engine.LoadTranslation(72), reps), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Search_FormClosing(object sender, FormClosingEventArgs e) {
            Program.SearchOpen = false;
        }

        private void Search_Shown(object sender, EventArgs e) {
            Program.SearchOpen = true;
        }

        private string TempScript = AppDomain.CurrentDomain.BaseDirectory + "TmpFile.tmp";
        private void bntDirFind_Click(object sender, EventArgs e) {
            string Dir = GetDir();
            if (string.IsNullOrEmpty(Dir))
                return;
            string[] Content = new string[Engine.StrList.Items.Count];
            for (int i = 0; i < Engine.StrList.Items.Count; i++)
                Content[i] = Engine.StrList.Items[i].ToString();

            string Filter = Engine.Filter.Split('|')[1];
            string[] Files = System.IO.Directory.GetFiles(Dir, Filter);
            string FoundFiles = string.Empty;
            int founds = 0;
            foreach (string File in Files) {
                Content = Engine.Open(File, true);
                bool Found = Engine.Search(txtContentToFind.Text, Content, 0, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked) > -1;
                if (Found) {
                    FoundFiles += System.IO.Path.GetFileName(File) + ", ";
                    founds++;
                }
            }
            if (founds == 0) {
                MessageBox.Show(Engine.LoadTranslation(61), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FoundFiles = FoundFiles.Substring(0, FoundFiles.Length - 2);
            MessageBox.Show(string.Format(Engine.LoadTranslation(85), founds) + '\n' + FoundFiles, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetDir() {
            FolderBrowserDialog BD = new FolderBrowserDialog();
            if (BD.ShowDialog() == DialogResult.OK)
                return BD.SelectedPath;
            else
                return null;
        }
    }
}
