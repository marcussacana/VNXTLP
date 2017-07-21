using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using VNXTLP.NewStyle;
using System.Drawing;
using PopupControl;
//Suported Engines: KiriKiri, Eushully, SteinsGate, ExHIBIT, KrKrFate, Umineko
//My Project Extesions: 
//VNX: Kamidori, Kami no Rhapsody (KNR), Sankai Ou no Yubiwa (Sankai)

namespace VNXTLP {
    internal static partial class Engine {

        #region LabelInfoEngine
        internal static void UpdateInfo(int value, ref Label InfoLbl, int BackupFreq, ref int Changes, bool TestIndex) {
            bool CanJump = true;
            if (value == StrList.Items.Count) {
                Changes = 0;
                InfoLbl.Text = LoadTranslation(49) + ": " + LoadTranslation(48);
                MessageBox.Show(LoadTranslation(48), LoadTranslation(49), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    InfoLbl.Text = LoadTranslation(49) + ": " + LoadTranslation(48);
                    MessageBox.Show(LoadTranslation(48), LoadTranslation(49), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                } else
                    return;
            }
            StrList.SelectedIndex = value;
            TextBox.Text = StrList.Items[value].ToString();
            TextBox.ResetCache();
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
#if VNX && !Sankai
#if Kamidori
            InfoLbl.Text = string.Format(LoadTranslation(47), Editor.Strings[Index].EndLine ? Pos + " && " + (Pos + 1) : ( Index > 0 && Editor.Strings[Index - 1].EndLine ? (Pos - 1) + " && " + Pos: Pos.ToString()), Total, Progress, Freq);
#endif
#if KNR
            int Start = Pos;
            for (; Start > 0 && Editor.Strings[Pos].IsString; Start--)
                if (Editor.Strings[Start].IsString && !Editor.Strings[Start].EndText)
                    break;
            int Ends = Start;
            for (; Ends < Editor.Strings.Length && Editor.Strings[Pos].IsString; Ends++)
                if (Editor.Strings[Ends].IsString && Editor.Strings[Ends].EndText)
                    break;
            string Links = string.Empty;
            for (int i = Start; i <= Ends; i++)
                Links += i + 1 <= Ends ? i + " && " : i.ToString();
            InfoLbl.Text = string.Format(LoadTranslation(47), Links, Total, Progress, Freq);
#endif
#else
            InfoLbl.Text = string.Format(LoadTranslation(47), Pos, Total, Progress, Freq);
#endif
        }
        #endregion

        #region Others    

        internal static void Append<T>(ref T[] Arr, T Var) {
            T[] NewArr = new T[Arr.Length + 1];
            Arr.CopyTo(NewArr, 0);
            NewArr[Arr.Length] = Var;
            Arr = NewArr;
        }
        internal static bool UseTheme() {
            return GetConfig("VNXTLP", "Theme", false) == "Modern";
        }
        internal static void InitializeStrings() {
            TextReader TR = (new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Translation.ini", Encoding.UTF8));
            while (TR.Peek() != -1) {
                string Line = TR.ReadLine();
                if (Line.StartsWith("//") || Line.StartsWith("[") || Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                    continue;
                Translation.Insert(int.Parse(Line.Split('=')[0]), Line.Split('=')[1].Replace("\\n", "\n"));
            }
            if (GetConfig("FTP", "AutoLogin", false) == "true") {
                Login(GetConfig("FTP", "AutoUser", true), GetConfig("FTP", "AutoPass", true), false);
            }
        }
        internal static string LoadTranslation(int ID) {
            return Translation[ID];
        }
        internal static int GetPos(CheckedListBox StrList, int index) {
            int p = 0;
            for (int i = 0; i <= index; i++)
                if (StrList.GetItemChecked(i))
                    p++;
            return p;
        }

        internal static void ChangeTheme(NoStyle NForm, StyleProgram SForm, bool EnableTheme) {
            if (UseTheme() == EnableTheme)
                return;
            Program.SearchOpen = false;

            SetConfig("VNXTLP", "Theme", EnableTheme ? "Modern" : "Basic");
            if (DialogResult.Yes == MessageBox.Show(LoadTranslation(95), "VNXTLP", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                Application.Restart();
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
            control = CustomControl == null ? MainForm : CustomControl;
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

    }
    internal class WordMeuItem : ToolStripMenuItem {
        internal string Word;
        internal int Index;
        internal int Length;
        internal Point Location;
    }
    
}
