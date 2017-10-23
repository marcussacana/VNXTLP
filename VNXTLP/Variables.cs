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

        internal static dynamic BackupEditor;
        internal static dynamic RemapBackup;
        internal static dynamic StringCountBackup;
        internal static dynamic PrefixBackup;
        internal static dynamic SufixBackup;
        internal static string UserDir {
            get
            {
                return "Backup\\" + UserAccount.Name + "\\";
            }
        }
        internal static bool Authenticated {
            get
            {
                return UserAccount.Name != "Anon";
            }
        }

        //SpeedUp Backup System
        private static bool BackupDirExist = false;
        private static bool UserDirExist = false;
        internal static bool UploadingBackup = false;

        private static Account UserAccount = new Account() { Name = "Anon" };
        internal static string ScriptPath;
        private static List<string> Translation = new List<string>();

        //Variable Redirections
        private static int Index { get { return Program.UsingTheme ? Program.StyleForm.Index : Program.NoStyleForm.Index; } set { if (Program.UsingTheme) Program.StyleForm.Index = value; else Program.NoStyleForm.Index = value; } }
        internal static SpellTextBox TextBox { get { return Program.UsingTheme ? Program.StyleForm.TLBox : Program.NoStyleForm.TLBox; } }
        internal static CheckedListBox StrList { get { try { return Program.UsingTheme ? Program.StyleForm.StrList : Program.NoStyleForm.StrList; } catch { return null; } } }

        internal static Form MainForm { get { return Program.UsingTheme ? Program.StyleForm as Form: Program.NoStyleForm as Form; } }
        internal static bool FileOpen { get { return Program.UsingTheme ? Program.StyleForm.FileOpen : Program.NoStyleForm.FileOpen; } }

    }
}
