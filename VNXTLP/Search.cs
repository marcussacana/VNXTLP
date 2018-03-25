using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        internal static bool ReplaceNext(string ContentToSearch, string ContentToReplace, bool WithContains, bool Up, bool CaseSensentive, bool Loop, bool CheckNonDialog, bool ReplaceFullDialog, bool NoMsg) {
            bool Found = SearchNext(ContentToSearch, WithContains, Up, CaseSensentive, Loop, CheckNonDialog, NoMsg);
            if (!Found)
                return false;
            if (ReplaceFullDialog)
                TextBox.Text = ContentToReplace;
            else
                TextBox.SelectedText = ContentToReplace;
            if (UseTheme())
                Program.StyleForm.TLBox_KeyDown(null, new KeyEventArgs(Keys.Enter));
            else
                Program.NoStyleForm.TLBox_KeyDown(null, new KeyEventArgs(Keys.Enter));
            return Found;
        }
        internal static int Search(string Content, string[] Script, int index, bool WithContains, bool Up, bool CaseSensentive, bool Loop) {
            int i = index;
            bool Found = false;
            bool Looped = false;
            if (!CaseSensentive)
                Content = Content.ToLower();
            if (i >= Script.Length)
                i = Script.Length - 1;
            while (Script.Length > 0) {
                string Str = Script[i];
                if (!CaseSensentive)
                    Str = Str.ToLower();
                if (WithContains ? Str.Contains(Content) : Str == Content) {
                    Found = true;
                    break;
                }

                if (Up) {
                    if (++i >= Script.Length)
                        if (Loop && !Looped) {
                            i = 0;
                            Looped = true;
                        }
                        else
                            break;
                }
                else {
                    if (--i < 0)
                        if (Loop && !Looped) {
                            i = Script.Length - 1;
                            Looped = true;
                        }
                        else
                            break;
                }
            }
            return Found ? i : -1;
        }

        internal static bool SearchNext(string Content, bool WithContains, bool Up, bool CaseSensentive, bool Loop, bool CheckNonDialog, bool NoMsg, bool NoRerty = false) {
            int Len = CheckNonDialog ? StrList.Items.Count : StrList.CheckedItems.Count;
            string[] Strings = new string[Len];
            int[] Indexs = new int[Len];
            for (int ind = 0, count = 0, diff = StrList.Items.Count; ind < StrList.Items.Count; ind++)
                if (CheckNonDialog || StrList.GetItemChecked(ind)) {
                    Strings[count] = StrList.Items[ind].ToString();
                    Indexs[count++] = ind;
                }

            int i = StrList.SelectedIndex + (Up ? 1 : -1);
            if (i < 0)
                i = 0;
            if (i >= StrList.Items.Count)
                i = StrList.Items.Count - 1;

            for (int ind = i; ind < StrList.Items.Count && ind > 0; ind += Up ? 1 : -1)
                if (CheckNonDialog || StrList.GetItemChecked(ind)) {
                    for (int j = 0; j < Indexs.Length; j++)
                        if (Indexs[j] == ind) {
                            i = j;
                            break;
                        }
                    break;
                }
            i = Search(Content, Strings, i, WithContains, Up, CaseSensentive, Loop);
            bool Found = i > -1;
            if (!Found) {
                if (!NoMsg)
                    MessageBox.Show(LoadTranslation(TLID.NoMatchFound), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                i = Indexs[i];
                StrList.SetItemChecked(i, true);
                Index = i;
                string Text = CaseSensentive ? TextBox.Text : TextBox.Text.ToLower();
                int IO = Text.IndexOf(CaseSensentive ? Content : Content.ToLower());
                if (IO < 0 && !NoRerty) {
                    System.Threading.Thread.Sleep(500);
                    return SearchNext(Content, WithContains, Up, CaseSensentive, Loop, CheckNonDialog, NoMsg, true);
                }
                TextBox.Select(IO, Content.Length);
                TextBox.Focus();
            }
            return Found;
        }


    }
}
