using System.Windows.Forms;

#region BuildImport
#if Eushully
using VNX.EushullyEditor;
#endif
#if KiriKiri
using KrKrSceneManager;
#endif
#if SteinsGate
using SGFilter;
#endif
#endregion

namespace VNXTLP {
    internal static partial class Engine {
        internal static ToolStripMenuItem[] CustomResources(ref SpellTextBox TB) {
#if VNX
            #region Preview

            ToolStripMenuItem ShowPreviewMenuItem = new ToolStripMenuItem("Preview");
            ShowPreviewMenuItem.Click += (obj, e) => {
                PV = new PreviewForm();
                PV.Show();
            };
            Append(ref TB.TextChanged, () => {
                try {
                    int i = Index;
#if Kamidori
                    if (Editor.Strings[i].EndLine)
                        PV.ChangeText(GetStringByIndex(i), GetStringByIndex(i + 1));
                    else if (i > 0 && Editor.Strings[i - 1].EndLine)
                        PV.ChangeText(GetStringByIndex(i - 1), GetStringByIndex(i));
                    else
                        PV.ChangeText(GetStringByIndex(i), "");
#endif
#if KNR
                    string Str = string.Empty;
                    if (Editor.Strings[i].IsString) {
                        int Start = i;
                        for (; Start > 0; Start--)
                            if (Editor.Strings[Start].IsString && !Editor.Strings[Start].EndText)
                                break;
                        int Ends = Start;
                        for (; Ends < Editor.Strings.Length; Ends++)
                            if (Editor.Strings[Ends].IsString && Editor.Strings[Ends].EndText)
                                break;
                        string Links = string.Empty;
                        bool Furigana = false;
                        for (int j = Start; j <= Ends; j++) {
                            if (Furigana) {
                                Furigana = false;
                                continue;
                            }
                            Furigana = Editor.Strings[j].Furigana;
                            Str += GetStringByIndex(j);
                        }
                    }
                    else
                        Str += GetStringByIndex(i) + string.Format("\n({0})", LoadTranslation(69));
                    PV.ChangeText(Str);
                                 
#endif
#if Sankai
                    PV.ChangeText(TextBox.Text);
#endif
                }
                catch { }
            });
            ToolStripMenuItem MergeFuriganas = new ToolStripMenuItem("Fusionar Furiganas");
            MergeFuriganas.Click += (s, e) => {
                try {
                    int i = Index;
                    if (Editor.Strings[i].IsString) {
                        int Start = i;
                        for (; Start > 0; Start--)
                            if (!Editor.Strings[Start].IsString || Editor.Strings[Start - 1].EndText || Editor.Strings[Start - 1].EndLine)
                                break;
                        int Ends = Start;
                        for (; Ends < Editor.Strings.Length; Ends++)
                            if (!Editor.Strings[Ends].IsString || Editor.Strings[Ends].EndText || Editor.Strings[Ends].EndLine)
                                break;
                        string Result = string.Empty;
                        bool Furigana = false;
                        for (int j = Start; j <= Ends; j++) {
                            if (Furigana) {
                                SetStringByIndex(j, "");
                                Furigana = false;
                                continue;
                            }
                            Furigana = Editor.Strings[j].Furigana;
                            Result += GetStringByIndex(j);
                            SetStringByIndex(j, "");
                        }
                        SetStringByIndex(Start, Result);
                    }
                }
                catch { }
            };
            return new ToolStripMenuItem[] { ShowPreviewMenuItem
#if KNR
                , MergeFuriganas
#endif
            };
            #endregion
#else
#if Umineko
            ToolStripMenuItem Main = new ToolStripMenuItem("Partição");
            ToolStripMenuItem Split = new ToolStripMenuItem("Particionar");
            ToolStripMenuItem Merge = new ToolStripMenuItem("Mesclar");
            Main.DropDown.Items.Add(Split);
            Main.DropDown.Items.Add(Merge);
            Split.Click += Split_Click;
            Merge.Click += Merge_Click;
            return new ToolStripMenuItem[] { Main };
#else
            return new ToolStripMenuItem[0];
#endif
#endif
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

#if Umineko
        private static void Merge_Click(object sender, System.EventArgs e) {
            Input Dlg = new Input();
            Dlg.Type = new Input.TestInput((args) => {
                if (System.IO.File.Exists(args + "\\Merge.lst"))
                    return args;
                else
                    throw new System.Exception();
            });
            Dlg.Text = "Insira o caminho da pasta com arquivos para mesclar.";
            Dlg.ShowDialog();
            if (Dlg.Value == null)
                return;

            SaveFileDialog FD = new SaveFileDialog();
            FD.Filter = "All Utf Files|*.utf";
            DialogResult dr = FD.ShowDialog();
            if (dr != DialogResult.OK)
                return;
            System.IO.TextReader Lst = System.IO.File.OpenText(Dlg.Value + "\\Merge.lst");
            System.Collections.Generic.List<string> Files = new System.Collections.Generic.List<string>();
            while (Lst.Peek() != -1) {
                string Line = Lst.ReadLine();
                if (string.IsNullOrWhiteSpace(Line))
                    continue;
                Files.Add(Dlg.Value + "\\" + Line);
            }
            Lst.Close();

            string[] Sorted = Files.ToArray();
            using (System.IO.TextWriter Writer = System.IO.File.CreateText(FD.FileName))
                foreach (string File in Sorted)
                    using (System.IO.TextReader Reader = System.IO.File.OpenText(File))
                        while (Reader.Peek() != -1)
                            Writer.WriteLine(Reader.ReadLine());

            MessageBox.Show("Script Mesclado.", "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static void Split_Click(object sender, System.EventArgs e) {
            System.Collections.Generic.List<string> Strs = new System.Collections.Generic.List<string>();
            foreach (string line in StrList.Items)
                Strs.Add(line);

            string TempPath = System.IO.Path.GetTempFileName();
            string Path = ScriptPath;

            Save(TempPath, Strs.ToArray());
            ScriptPath = Path;
            Strs = new System.Collections.Generic.List<string>();
            string WD = System.IO.Path.GetDirectoryName(ScriptPath) + "\\Splited\\";
            if (!System.IO.Directory.Exists(WD))
                System.IO.Directory.CreateDirectory(WD);

            string FMask =  "Umineko - EP{0} CP{1}.utf";
            System.Collections.Generic.List<string> Labels = new System.Collections.Generic.List<string>(new string[] { "*umi1_opning", "*umi2_opning", "*umi3_opning", "*umi4_opning", "*umi1_1", "*umi1_2", "*umi1_3", "*umi1_4", "*umi1_5", "*umi1_6", "*umi1_7", "*umi1_8", "*umi1_9", "*umi1_10", "*umi1_11", "*umi1_12", "*umi1_13", "*umi1_14", "*umi1_15", "*umi1_16", "*umi1_17", "*umi2_1", "*umi2_2", "*umi2_3", "*umi2_4", "*umi2_5", "*umi2_6", "*umi2_7", "*umi2_8", "*umi2_9", "*umi2_10", "*umi2_11", "*umi2_12", "*umi2_13", "*umi2_14", "*umi2_15", "*umi2_16", "*umi2_17", "*umi2_18", "*umi3_1", "*umi3_2", "*umi3_3", "*umi3_4", "*umi3_5", "*umi3_6", "*umi3_7", "*umi3_8", "*umi3_9", "*umi3_10", "*umi3_11", "*umi3_12", "*umi3_13", "*umi3_14", "*umi3_15", "*umi3_16", "*umi3_17", "*umi3_18", "*umi4_1", "*umi4_2", "*umi4_3", "*umi4_4", "*umi4_5", "*umi4_6", "*umi4_7", "*umi4_8", "*umi4_9", "*umi4_10", "*umi4_11", "*umi4_12", "*umi4_13", "*umi4_14", "*umi4_15", "*umi4_16", "*umi4_17", "*umi4_18", "*umi4_19" });
            System.IO.TextWriter LOG = System.IO.File.CreateText(WD + "Merge.lst");
            using (System.IO.TextReader Reader = System.IO.File.OpenText(TempPath)) {
                int Count = 0, FC = 1;
                LOG.WriteLine("Prefix.utf");
                System.IO.TextWriter Writer = System.IO.File.CreateText(WD + "Prefix.utf");
                while (Reader.Peek() != -1) {
                    string Line = Reader.ReadLine();
                    if (Labels.Contains(Line.ToLower())) {
                        FC++;
                        Writer.Close();
                        int[] R = SplitLabel(Line);
                        string FN = string.Format(FMask, R[0], R[1]);
                        LOG.WriteLine(FN);
                        Writer = System.IO.File.CreateText(WD + FN);
                    }
                    Count++;
                    Writer.WriteLine(Line);
                }
            }
            LOG.Close();
            MessageBox.Show("Script particionado com sucesso.", "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static int[] SplitLabel(string lbl) {
            int split = lbl.IndexOf("_");
            if (lbl.EndsWith("_opning"))
                return new int[] { int.Parse(lbl.Substring(split - 1, 1)), 0 };
            else
                return new int[] { int.Parse(lbl.Substring(split - 1, 1)), int.Parse(lbl.Substring(split + 1, lbl.Length - (split + 1))) };
        }

#endif

#if VNX
#if Kamidori
        private static PreviewForm PV;
        private class PreviewForm : Form {
            private PictureBox PB = new PictureBox();
            internal PreviewForm() {
                Text = "Kamidori Text Preview";
                PB.SizeMode = PictureBoxSizeMode.StretchImage;
                PB.Dock = DockStyle.Fill;
                PB.Image = Kamidori.TextPreview.GetPreview("", "");
                System.Drawing.Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
                int titleHeight = screenRectangle.Top - Top;
                Size = new System.Drawing.Size(PB.Image.Size.Width, PB.Image.Size.Height + titleHeight);
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                MaximizeBox = false;
                MinimizeBox = false;
                Controls.Add(PB);
                ShowInTaskbar = false;
            }
            internal void ChangeText(string FirstLine, string SecondLine) {
                PB.Image = Kamidori.TextPreview.GetPreview(FirstLine, SecondLine);
            }
        }
#endif
#if KNR
        private static PreviewForm PV;
        private class PreviewForm : Form {
            private PictureBox PB = new PictureBox();
            internal PreviewForm() {
                Text = "Kami No Rhapsody Text Preview";
                PB.SizeMode = PictureBoxSizeMode.StretchImage;
                PB.Dock = DockStyle.Fill;
                PB.Image = KnR.TextPreview.GetPreview(string.Empty);
                System.Drawing.Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
                int titleHeight = screenRectangle.Top - Top;
                Size = new Size(PB.Image.Size.Width, PB.Image.Size.Height + titleHeight);
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                MaximizeBox = false;
                MinimizeBox = false;
                Controls.Add(PB);
                ShowInTaskbar = false;
            }
            internal void ChangeText(string Text) {
                PB.Image = KnR.TextPreview.GetPreview(Text);
            }
        }
#endif
#if Sankai
        private static PreviewForm PV;
        private class PreviewForm : Form {
            private PictureBox PB = new PictureBox();
            internal PreviewForm() {
                Text = "Sankai Ou no Yubiwa Text Preview";
                PB.SizeMode = PictureBoxSizeMode.StretchImage;
                PB.Dock = DockStyle.Fill;
                PB.Image = Sankai.TextPreview.GetPreview(string.Empty);
                System.Drawing.Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
                int titleHeight = screenRectangle.Top - Top;
                Size = new System.Drawing.Size(PB.Image.Size.Width, PB.Image.Size.Height + titleHeight);
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                MaximizeBox = false;
                MinimizeBox = false;
                Controls.Add(PB);
                ShowInTaskbar = false;
            }
            internal void ChangeText(string Text) {
                PB.Image = Sankai.TextPreview.GetPreview(Text);
            }
        }
#endif
#endif
    }
}
