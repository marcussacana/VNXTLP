using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using VNXTLP.NewStyle;
using System.Drawing;
using PopupControl;
using System.Linq;
//Suported Engines: KiriKiri, Eushully, SteinsGate, ExHIBIT, KrKrFate, Umineko
//My Project Extesions: 
//VNX: Kamidori, Kami no Rhapsody (KNR), Sankai Ou no Yubiwa (Sankai)

namespace VNXTLP {
    internal static partial class Engine {

        #region LabelInfoEngine
        internal static void UpdateInfo(int value, ref Label InfoLbl, int BackupFreq, ref int Changes, bool TestIndex) {
            if (StrList.Items.Count == 0) {
                InfoLbl.Text = string.Empty;
                return;
            }
            if (value < 0) {
                InfoLbl.Text = LoadTranslation(TLID.NoChangeDialogueNow);
                return;
            }
            bool CanJump = true;
            if (value == StrList.Items.Count) {
                Changes = 0;
                InfoLbl.Text = LoadTranslation(TLID.Congratulation) + ": " + LoadTranslation(TLID.ScriptComplete);
                MessageBox.Show(LoadTranslation(TLID.ScriptComplete), LoadTranslation(TLID.Congratulation), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            while (CanJump && !StrList.GetItemChecked(value) && TestIndex) {
                if (StrList.SelectedIndex < value)
                    value++;
                else
                    value--;
                CanJump = value < StrList.Items.Count && value > 0;
            }

            if (!CanJump) {
                if (value == StrList.Items.Count) {
                    Changes = 0;
                    InfoLbl.Text = LoadTranslation(TLID.Congratulation) + ": " + LoadTranslation(TLID.ScriptComplete);
                    MessageBox.Show(LoadTranslation(TLID.ScriptComplete), LoadTranslation(TLID.Congratulation), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                } else
                    return;
            }

            bool Changing = StrList.SelectedIndex != value;
            StrList.SelectedIndex = value;


            //Prevent Spellcheck reset without changes
            if (Changing) {
                string OriDialogue = StrList.Items[value].ToString();
                if (CanReload(OriDialogue)) {
                    string NewDialogue = ReloadString(OriDialogue);
                    if (NewDialogue != OriDialogue) {
                        TextBox.Text = NewDialogue;
                        TextBox.ResetCache();
                    }
                } else if (TextBox.Text != OriDialogue)
                    TextBox.Text = OriDialogue;
            }

            int Pos = TestIndex ? GetPos(StrList, value) : value;
            int Total = TestIndex ? StrList.CheckedItems.Count : StrList.Items.Count;
            int Progress = (Pos * 100) / Total;
            string Freq = BackupFreq < 0 || string.IsNullOrEmpty(UserAccount.Name) || UserAccount.Name == "Anon" ? "Off" : (BackupFreq - Changes).ToString();
            if (BackupFreq > 0 && Changes == BackupFreq) {
                Changes = 0;
                string[] Strs = new string[StrList.Items.Count];
                for (int i = 0; i < Strs.Length; i++)
                    Strs[i] = StrList.Items[i].ToString();
                //Multi-Thread Backup
                (new System.Threading.Thread((arg) => { Backup((string[])arg); })).Start(Strs);
            }
            InfoLbl.Text = string.Format(LoadTranslation(TLID.InfoMask), Pos, Total, Progress, Freq);
        }
        #endregion

        #region Others    

        internal static void Append<T>(ref T[] Arr, T Var) {
            T[] NewArr = new T[Arr.Length + 1];
            Arr.CopyTo(NewArr, 0);
            NewArr[Arr.Length] = Var;
            Arr = NewArr;
        }
        internal static string UsingTheme() {
            if (!Program.IsRealWindows)
                return "Basic";
            return GetConfig("VNXTLP", "Theme", false).ToLower();
        }
        internal static void InitializeStrings() {
            using (TextReader TR = (new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Translation.ini", Encoding.UTF8)))
                while (TR.Peek() != -1) {
                    string Line = TR.ReadLine();
                    if (Line.StartsWith("//") || Line.StartsWith("[") || Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                        continue;
                    int ID = int.Parse(Line.Split('=')[0]);
                    while (ID > Translation.Count)
                        Translation.Add(string.Format("Missing Translation: {0} ({1})", Translation.Count, ((TLID)Translation.Count).ToString()));
                    Translation.Insert(ID, Line.Split('=')[1].Replace("\\n", "\n"));
                }
        }

        internal static string LoadTranslation(TLID ID, params object[] Format) {
            return LoadTranslation((int)ID, Format);
        }

        internal static string LoadTranslation(int ID, params object[] Format) {
            if (Format.Length == 0)
                return Translation[ID];
            else
                return string.Format(Translation[ID], Format);
        }
        internal static int GetPos(CheckedListBox StrList, int index) {
            int p = 0;
            for (int i = 0; i <= index; i++)
                if (StrList.GetItemChecked(i))
                    p++;
            return p;
        }

        internal static void ChangeTheme(string Theme) {
            if (UsingTheme().ToLower() == Theme.ToLower())
                return;
            Program.SearchOpen = false;

            SetConfig("VNXTLP", "Theme", Theme);
            if (DialogResult.Yes == MessageBox.Show(LoadTranslation(TLID.RestarNow), "VNXTLP", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Environment.Exit(0);
            }
        }

        private static BallonToolTip BF;
        private static Popup PU;
        private static Timer TTI = new Timer();
        private static bool LostFocus = false;
        private static Control control = null;
        private static Point TTLocation;
        internal static void UpdateToolTip(BallonToolTip NewBallon, bool CloseWithClick) {
            LostFocus = true;
            PU.Hide();
            BF.Dispose();
            BF = NewBallon;
            CreatePopup();
            PU.Show(control, TTLocation, ToolStripDropDownDirection.BelowRight);
            TTI.Enabled = !CloseWithClick;
        }
        internal static void ShowToolTip(Point Location, string Message, string Title, BallonToolTip bf = null, bool CloseWithClick = false, Control CustomControl = null) {
            if (bf == null)
                BF = new BallonToolTip();
            else
                BF = bf;
            control = CustomControl ?? MainForm;
            BF.Title = Title;
            BF.Message = Message;
            CreatePopup();
            TTLocation = LocationCalc(Location, 20, (BF.Size.Height / 2));
            PU.Show(control, TTLocation, ToolStripDropDownDirection.BelowRight);
            TTI.Enabled = !CloseWithClick;
        }
        internal static Point LocationCalc(Point Original, int X, int Y) {
            return new Point(Original.X + X, Original.Y + Y);
        }

        private static void CreatePopup() {
            PU = new Popup(BF);
            LostFocus = false;
            PU.LostFocus += (sender, e) => {
                LostFocus = true;
            };
            PU.Closing += (sender, e) => {
                if (!LostFocus) {
                    e.Cancel = true;
                }
            };
            PU.AnimationDuration = 300;
            TTI = new Timer();
            TTI.Tick += (sender, e) => {
                LostFocus = true;
                PU.Hide();
                TTI.Enabled = false;
            };
            TTI.Interval = 5000 + (BF.Message.Length * 50);
        }

        internal static Color LoadColor(string Name, Color Default) {
            if (GetConfigStatus("VNXTLP", Name) != ConfigStatus.Ok) {
                SetConfig("VNXTLP", Name, Default.IsNamedColor ? Default.Name : $"#{Default.A:X2}{Default.R:X2}{Default.G:X2}{Default.B:X2}");
                return Default;
            }
            string ColorName = GetConfig("VNXTLP", Name, true);

            try {
                if (ColorName.StartsWith("#")) {
                    string Hex = ColorName.Trim('#', ' ');
                    int A, R, G, B;
                    switch (Hex.Length) {
                        case 3:
                            R = Convert.ToInt32(Hex[0].ToString() + Hex[0].ToString(), 16);
                            G = Convert.ToInt32(Hex[1].ToString() + Hex[1].ToString(), 16);
                            B = Convert.ToInt32(Hex[2].ToString() + Hex[2].ToString(), 16);
                            return Color.FromArgb(0xFF, R, G, B);
                        case 4:
                            A = Convert.ToInt32(Hex[0].ToString() + Hex[0].ToString(), 16);
                            R = Convert.ToInt32(Hex[1].ToString() + Hex[1].ToString(), 16);
                            G = Convert.ToInt32(Hex[2].ToString() + Hex[2].ToString(), 16);
                            B = Convert.ToInt32(Hex[3].ToString() + Hex[3].ToString(), 16);
                            return Color.FromArgb(A, R, G, B);
                        case 6:
                            R = Convert.ToInt32(Hex[0].ToString() + Hex[1].ToString(), 16);
                            G = Convert.ToInt32(Hex[2].ToString() + Hex[3].ToString(), 16);
                            B = Convert.ToInt32(Hex[4].ToString() + Hex[5].ToString(), 16);
                            return Color.FromArgb(0xFF, R, G, B);
                        case 8:
                            A = Convert.ToInt32(Hex[0].ToString() + Hex[1].ToString(), 16);
                            R = Convert.ToInt32(Hex[2].ToString() + Hex[3].ToString(), 16);
                            G = Convert.ToInt32(Hex[4].ToString() + Hex[5].ToString(), 16);
                            B = Convert.ToInt32(Hex[6].ToString() + Hex[7].ToString(), 16);
                            return Color.FromArgb(A, R, G, B);
                    }

                } else if (ColorName.Contains(";")) {
                    string[] Array = ColorName.Replace(@" ", "").Split(';');
                    int A, R, G, B;
                    switch (Array.Length) {
                        case 3:
                            R = int.Parse(Array[0].Trim());
                            G = int.Parse(Array[1].Trim());
                            B = int.Parse(Array[2].Trim());
                            return Color.FromArgb(0xFF, R, G, B);
                        case 4:
                            A = int.Parse(Array[0].Trim());
                            R = int.Parse(Array[1].Trim());
                            G = int.Parse(Array[2].Trim());
                            B = int.Parse(Array[3].Trim());
                            return Color.FromArgb(A, R, G, B);
                    }
                } else
                    return Color.FromName(ColorName);
            } catch { }

            return Default;
        }

        internal static string TableName {
            get {
                return Path.GetDirectoryName(ScriptPath) + " /" + Path.GetFileNameWithoutExtension(ScriptPath) + "-Checks.bol";
            }
        }

        internal static int TextWidth(string Text, Font Font) {
            Graphics e = Graphics.FromImage(new Bitmap(1, 1));
            return (int)e.MeasureString(Text, Font).Width;
        }
        internal static int TextHeight(string Text, Font Font) {
            Graphics e = Graphics.FromImage(new Bitmap(1, 1));
            return (int)e.MeasureString(Text, Font).Height;
        }

        #endregion

        internal static void SaveWindowState(Form Form) {
            SetConfig("Window", "XY", string.Format("{0}; {1}", Form.Location.X, Form.Location.Y));
            SetConfig("Window", "WH", string.Format("{0}; {1}", Form.Size.Width, Form.Size.Height));
            SetConfig("Window", "Maxmized", Form.WindowState == FormWindowState.Maximized ? "True" : "False");
        }
        internal static void LoadWindowState(Form Form) {
            string XY = GetConfig("Window", "XY", false);
            string WH = GetConfig("Window", "WH", false);
            bool Maximed = GetConfig("Window", "Maxmized", false).ToLower() == "true";

            if (XY.Contains(";")) {
                Form.Location = new Point(int.Parse(XY.Split(';')[0].Trim()), int.Parse(XY.Split(';')[1].Trim()));
            }
            if (WH.Contains(";")) {
                Form.Size = new Size(int.Parse(WH.Split(';')[0].Trim()), int.Parse(WH.Split(';')[1].Trim()));
            }
            if (Maximed)
                Form.WindowState = FormWindowState.Maximized;
        }
    }
    internal class WordMeuItem : ToolStripMenuItem {
        internal string Word;
        internal int Index;
        internal int Length;
        internal Point Location;
    }

}
