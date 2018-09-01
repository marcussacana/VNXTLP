using System;
using System.Collections.Generic;
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
                    Multiselect = true,
                    Title = LoadTranslation(TLID.RemapNow)
                };
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                var Oris = FileDialog.FileNames;
                if (DialogResult.OK != FileDialog.ShowDialog())
                    return;
                var Trgs = FileDialog.FileNames;
                foreach (string Ori in Oris) {
                    string OFN = System.IO.Path.GetFileName(Ori);
                    foreach (string Trg in Trgs) {
                        string TFN = System.IO.Path.GetFileName(Trg);
                        if (TFN.ToLower() != OFN.ToLower())
                            continue;

                        if (!Genmap(Ori, Trg))
                            MessageBox.Show(LoadTranslation(TLID.FailedScriptsNotEqual) + string.Format("\n{0} to {1}", OFN, TFN), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    }
                }
                MessageBox.Show(LoadTranslation(TLID.RemapGenerated), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            ToolStripMenuItem Item6 = null;
            try {
                if (Program.OfflineMode)
                    throw new Exception();

                string Lang = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                //Lang = "EN";
                Item6 = new ToolStripMenuItem(TLIB.Google.Translate("Switch to my Language", "EN", Lang));
                Item6.Click += (a, e) => {

                    string Path = AppDomain.CurrentDomain.BaseDirectory + "Translation.ini";
                    string[] Entry = System.IO.File.ReadAllLines(Path);


                    List<string> Strings = new List<string>();
                    for (int i = 0; i < Entry.Length; i++) {
                        string Line = Entry[i];
                        if (Line.StartsWith("//") || Line.StartsWith("[") || Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                            continue;

                        string Content = Line.Split('=')[1];
                        Strings.Add(Content);
                    }

                    string[] Translated = TLIB.Google.Translate(Strings.ToArray(), "pt", Lang);
                    for (int i = 0, x = 0; i < Entry.Length; i++) {
                        string Line = Entry[i];
                        if (Line.StartsWith("//") || Line.StartsWith("[") || Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                            continue;
                        string Content = Line.Split('=')[1];

                        string Result = Translated[x++];
                        Result = Result.Replace("{ ", "{");
                        Result = Result.Replace(" }", "}");
                        Result = Result.Replace("\\ n", "\\n");
                        Result = Result.Replace(" .", ".");

                        if (char.IsUpper(Content.First()))
                            Result = Result.First().ToString().ToUpper() + Result.Substring(1);

                        Entry[i] = Line.Split('=')[0] + '=' + Result;
                    }

                    System.IO.File.WriteAllLines(Path, Entry);
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                };
            } catch { }

            ItemHost.Text = LoadTranslation(TLID.Tools);
            ItemHost.DropDownItems.Add(Item);
            ItemHost.DropDownItems.Add(new ToolStripSeparator());
            ItemHost.DropDownItems.Add(Item2);
            ItemHost.DropDownItems.Add(Item3);
            if (Item6 != null)
                ItemHost.DropDownItems.Add(Item6);
            
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
