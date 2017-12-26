using System;
using System.Linq;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        internal static ToolStripMenuItem[] CustomResources(ref SpellTextBox TB) {
            ToolStripMenuItem Item = new ToolStripMenuItem(LoadTranslation(99));
            Item.Click += (a, b) => {
                OpenFileDialog FileDialog = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(99)
                };
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                string Ori = FileDialog.FileName;
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                string Trg = FileDialog.FileName;
                Genmap(Ori, Trg);
            };

            ToolStripSeparator Separator = new ToolStripSeparator();
            ToolStripMenuItem Item2 = new ToolStripMenuItem(LoadTranslation(109));
            Item2.Click += (a, b) => {
                OpenFileDialog FD = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(111),
                    Multiselect = true
                };

                if (FD.ShowDialog() != DialogResult.OK)
                    return;

                string List = LogLines(FD.FileNames, true);
                MessageBox.Show(List, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ToolStripMenuItem Item3 = new ToolStripMenuItem(LoadTranslation(110));
            Item3.Click += (a, b) => {
                OpenFileDialog FD = new OpenFileDialog() {
                    Filter = Filter,
                    Title = LoadTranslation(111),
                    Multiselect = true
                };

                if (FD.ShowDialog() != DialogResult.OK)
                    return;

                string List = LogLines(FD.FileNames, false);
                MessageBox.Show(List, "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };


            ToolStripMenuItem ItemHost = new ToolStripMenuItem(LoadTranslation(108));
            ItemHost.DropDownItems.Add(Item);
            ItemHost.DropDownItems.Add(Separator);
            ItemHost.DropDownItems.Add(Item2);
            ItemHost.DropDownItems.Add(Item3);

            return new ToolStripMenuItem[] {
                ItemHost
            };
        }

        private static string LogLines(string[] Scripts, bool Filter) {
            string LOG = LoadTranslation(112) + "\n==================";

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

            LOG += string.Format("\n==================\n" + LoadTranslation(113), Total);
            return LOG;
        }

        private static string GetStringByIndex(int i) {
            return Program.UsingTheme ? Program.StyleForm.GetStr(i) : Program.NoStyleForm.GetStr(i);
        }
        private static void SetStringByIndex(int i, string Content) {
            if (Program.UsingTheme)
                Program.StyleForm.SetStr(i, Content);
            else
                Program.NoStyleForm.SetStr(i, Content);
        }
    }
}
