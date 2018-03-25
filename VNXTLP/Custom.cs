using System;
using System.Linq;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {

        static ToolStripMenuItem ItemHost = new ToolStripMenuItem();
        internal static ToolStripMenuItem[] CustomResources(ref SpellTextBox TB) {
            ToolStripMenuItem Item = new ToolStripMenuItem(LoadTranslation(TLID.RemapNow));
            Item.Click += (a, b) => {
                OpenFileDialog FileDialog = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(TLID.RemapNow)
                };
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                string Ori = FileDialog.FileName;
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                string Trg = FileDialog.FileName;
                Genmap(Ori, Trg);
            };
            
            ToolStripMenuItem Item2 = new ToolStripMenuItem(LoadTranslation(TLID.DialoguesCount));
            Item2.Click += (a, b) => {
                OpenFileDialog FD = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(TLID.SelectFilesToCount),
                    Multiselect = true
                };

                if (FD.ShowDialog() != DialogResult.OK)
                    return;

                string List = LogLines(FD.FileNames, true);
                MessageBox.Show(List, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ToolStripMenuItem Item3 = new ToolStripMenuItem(LoadTranslation(TLID.LinesCount));
            Item3.Click += (a, b) => {
                OpenFileDialog FD = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(TLID.SelectFilesToCount),
                    Multiselect = true
                };

                if (FD.ShowDialog() != DialogResult.OK)
                    return;

                string List = LogLines(FD.FileNames, false);
                MessageBox.Show(List, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };


            ToolStripMenuItem Item4 = new ToolStripMenuItem(LoadTranslation(TLID.GenerateToken));
            Item4.Click += (a, b) => {
                RegKey Key = new RegKey();
                if (Key.ShowDialog() != DialogResult.OK)
                    return;

                MessageBox.Show(LoadTranslation(TLID.AuthToken) + GetRefSeed(Key.Key), "VNXTLP - DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ToolStripMenuItem Item5 = new ToolStripMenuItem(LoadTranslation(TLID.DecryptFiles));
            Item5.Click += (a, b) => {
                OpenFileDialog FD = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(TLID.DecryptFiles) + ":",
                    Multiselect = true
                };

                if (FD.ShowDialog() != DialogResult.OK)
                    return;

                foreach (string Script in FD.FileNames) {
                    byte[] Result = Decrypt(System.IO.File.ReadAllBytes(Script));
                    System.IO.File.Delete(Script);
                    System.IO.File.WriteAllBytes(Script, Result);
                }

                MessageBox.Show(LoadTranslation(TLID.OperationClear), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ItemHost.Text = LoadTranslation(TLID.Tools);
            ItemHost.DropDownItems.Add(Item);
            ItemHost.DropDownItems.Add(new ToolStripSeparator());
            ItemHost.DropDownItems.Add(Item2);
            ItemHost.DropDownItems.Add(Item3);
            
            if (DebugMode) {
                ItemHost.DropDownItems.Add(new ToolStripSeparator());
                ItemHost.DropDownItems.Add(Item4);
                ItemHost.DropDownItems.Add(Item5);
            }

            return new ToolStripMenuItem[] {
                ItemHost
            };
        }

        private static string LogLines(string[] Scripts, bool Filter) {
            string LOG = LoadTranslation(TLID.PressCtrlCToCopy) + "\n==================";

            Array.Sort(Scripts);

            bool Asian = GetConfig("VNXTLP", "SelMode", false) == "1";

            ulong Total = 0;

            foreach (string File in Scripts) {
                string[] Lines = Open(File, true);
                if (Filter)
                    Lines = (from x in Lines where IsDialogue(x.ToLower(), Asian) select x).ToArray();

                if (Lines.LongLength == 0)
                    continue;

                Total += (ulong)Lines.LongLength;

                LOG += $"\n{Lines.LongLength:D5}: {System.IO.Path.GetFileName(File)}";
            }

            LOG += string.Format("\n==================\n" + LoadTranslation(TLID.WithTotalOf), Total);
            return LOG;
        }
        
    }
}
