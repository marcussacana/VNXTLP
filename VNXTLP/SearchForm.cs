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

            LblLocalizar.Text = Engine.LoadTranslation(Engine.TLID.SearchFor);
            LblRep.Text = Engine.LoadTranslation(Engine.TLID.ReplaceWith);
            GBAdvOpt.Text = Engine.LoadTranslation(Engine.TLID.AdvancedSettings);
            ckCaseSensentive.Text = Engine.LoadTranslation(Engine.TLID.CaseSensetive);
            ckWithContains.Text = Engine.LoadTranslation(Engine.TLID.SearchFullDialogue);
            ckFindNonDiag.Text = Engine.LoadTranslation(Engine.TLID.SearchNonDialogue);
            ckCircle.Text = Engine.LoadTranslation(Engine.TLID.CircleSearch);
            ckRepFullDiag.Text = Engine.LoadTranslation(Engine.TLID.ReplaceFullDialogue);
            lblDirec.Text = Engine.LoadTranslation(Engine.TLID.SearchDirection);
            radioUp.Text = Engine.LoadTranslation(Engine.TLID.Forward);
            radioBack.Text = Engine.LoadTranslation(Engine.TLID.Behind);
            bntLocNext.Text = Engine.LoadTranslation(Engine.TLID.SearchFor);
            BntRepNext.Text = Engine.LoadTranslation(Engine.TLID.ReplaceWith);
            BntRepAll.Text = Engine.LoadTranslation(Engine.TLID.ReplaceAll);
            bntDirFind.Text = Engine.LoadTranslation(Engine.TLID.SearchInFolder);
        }

        private void bntLocNext_Click(object sender, EventArgs e) {
            Engine.SearchNext(txtContentToFind.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, false);
        }

        private void BntRepNext_Click(object sender, EventArgs e) {
            if (Engine.ReplaceNext(txtContentToFind.Text, txtContentToRep.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, ckRepFullDiag.Checked, false))
                MessageBox.Show(string.Format(Engine.LoadTranslation(Engine.TLID.XResultsReplaced), 1), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BntRepAll_Click(object sender, EventArgs e) {
            uint reps = 0;
            while (Engine.ReplaceNext(txtContentToFind.Text, txtContentToRep.Text, !ckWithContains.Checked, radioUp.Checked, ckCaseSensentive.Checked, ckCircle.Checked, ckFindNonDiag.Checked, ckRepFullDiag.Checked, true))
                reps++;
            MessageBox.Show(string.Format(Engine.LoadTranslation(Engine.TLID.XResultsReplaced), reps), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoMatchFound), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FoundFiles = FoundFiles.Substring(0, FoundFiles.Length - 2);
            MessageBox.Show(string.Format(Engine.LoadTranslation(Engine.TLID.XResultsFoundAt), founds) + '\n' + FoundFiles, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
