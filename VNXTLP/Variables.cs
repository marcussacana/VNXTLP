using System.Collections.Generic;
using System.Windows.Forms;
using SacanaWrapper;

namespace VNXTLP {
    internal static partial class Engine {

        internal static string Filter = "Todos os Arquivos|*.*";
        internal static Wrapper Editor;

        internal static string UpdateScript = "VNXTLP.cs";

        internal static Dictionary<uint, uint> StrMap = new Dictionary<uint, uint>();
        internal static uint StringCount;

        internal static Dictionary<uint, string> Prefix = new Dictionary<uint, string>();
        internal static Dictionary<uint, string> Sufix = new Dictionary<uint, string>();

        internal static Dictionary<uint, string> BreakLineEscape = new Dictionary<uint, string>();

        internal static Dictionary<string, string> Database = new Dictionary<string, string>();
        internal static string LastString;

        internal static dynamic BackupEditor;
        internal static dynamic RemapBackup;
        internal static dynamic StringCountBackup;
        internal static dynamic PrefixBackup;
        internal static dynamic SufixBackup;
        internal static dynamic BreakLineEscapeBackup;

        internal static string UserDir {
            get
            {
                const string Mask = "Backup\\{0}\\";
                if (DebugMode && !string.IsNullOrWhiteSpace(AdminBackup))
                    return string.Format(Mask, AdminBackup);
                return string.Format(Mask, UserAccount.Name);
            }
        }
        internal static bool Authenticated {
            get
            {
                return UserAccount.Name != "Anon";
            }
        }

        internal static bool DebugMode {
            get {
                return GetConfig("VNXTLP", "IsMod", false).ToLower().Trim() == "vnxtlp";
            }
        }

        //SpeedUp Backup System
        private static bool BackupDirExist = false;
        private static bool UserDirExist = false;
        internal static bool UploadingBackup = false;

        private static Account UserAccount = new Account() { Name = "Anon" };
        internal static string ScriptPath;
        internal static string AdminBackup;
        private static List<string> Translation = new List<string>();

        //Variable Redirections
        private static int Index { get { return Program.UsingTheme ? Program.StyleForm.Index : Program.NoStyleForm.Index; } set { if (Program.UsingTheme) Program.StyleForm.Index = value; else Program.NoStyleForm.Index = value; } }
        internal static SpellTextBox TextBox { get { return Program.UsingTheme ? Program.StyleForm.TLBox : Program.NoStyleForm.TLBox; } }
        internal static CheckedListBox StrList { get { try { return Program.UsingTheme ? Program.StyleForm.StrList : Program.NoStyleForm.StrList; } catch { return null; } } }

        internal static Form MainForm { get { return Program.UsingTheme ? Program.StyleForm as Form: Program.NoStyleForm as Form; } }
        internal static bool FileOpen { get { return Program.UsingTheme ? Program.StyleForm.FileOpen : Program.NoStyleForm.FileOpen; } }

        internal static Commands ServerStatus = Commands.Closed;
        
        private static int SelIndex {
            get {
                int Val = -1;
                StrList.Invoke(new MethodInvoker(() => {
                    Val = StrList.SelectedIndex;
                }));
                return Val;
            }
            set {
                StrList.Invoke(new MethodInvoker(() => {
                    StrList.SelectedIndex = value;
                }));
            }
        }

        private static int StrCnt {
            get {
                int Val = -1;
                StrList.Invoke(new MethodInvoker(() => {
                    Val = StrList.Items.Count;
                }));
                return Val;
            }
        }

        private static int TopItem {
            get {
                int Val = -1;
                StrList.Invoke(new MethodInvoker(() => {
                    Val = StrList.TopIndex;
                }));
                return Val;
            }
            set {
                StrList.Invoke(new MethodInvoker(() => {
                    StrList.TopIndex = value;
                }));
            }
        }
    }
}
