using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        internal static ToolStripMenuItem[] CustomResources(ref SpellTextBox TB) {
            ToolStripMenuItem Item = new ToolStripMenuItem(LoadTranslation(99));
            Item.Click += (a,b) => {
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
            return new ToolStripMenuItem[] {
                Item
            };
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
