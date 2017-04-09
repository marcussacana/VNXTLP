using System.Collections.Generic;
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
#if ExHIBIT
using RLDManager;
#endif
#if KrKrFate
using KrKrFateFilter;
#endif
#endregion

namespace VNXTLP {
    internal static partial class Engine {
#region Variables
#if Eushully
        internal static string Filter = "Todos os Scripts da Eushully|*.bin";
        private static EushullyEditor Editor;
#if Sankai
        private static FormatOptions EditorConfig = new FormatOptions() {
            BruteValidator = true,
            ClearOldStrings = true
        };
#else
        private static FormatOptions EditorConfig = new FormatOptions();
#endif
        internal static string UpdateScript = "Eushully.cs";
#endif
#if KiriKiri
        internal static string Filter = "Todos os arquivos suportados da KiriKiri|*.scn;*.psb;*.tjs";
        private static PSBStringManager Editor;
        private static PSBStringManager.PackgetStatus Status;

        private static Sector[] Sectors;
        private static int MainSector;
        internal static string UpdateScript = "KRKR.cs";
#endif
#if SteinsGate
        internal static string Filter = "Todos os Arquivos de Texto|*.txt";
        internal static FullFilter Editor;
        internal static FullFilter.FilterLevel Level;
        internal static string UpdateScript = "SG.cs";
#endif
#if ExHIBIT
        internal static string Filter = "All ExHIBIT Files|*.rld";
        internal static RLD Editor;
        internal static string UpdateScript = "EHKS.cs";
#endif
#if Umineko
        internal static string Filter = "All Umineko Scripts|*.utf";
        internal static Umineko Editor;
        internal static string UpdateScript = "Umnk.cs";
        private static bool ENG;
#endif
#if KrKrFate
        internal static string Filter = "All KiriKiri Scripts|*.ks";
        internal static KSFilter Editor;
        internal static string UpdateScript = "KrKrFate.cs";
#endif
        internal static dynamic BackupEditor;
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
        internal static CheckedListBox StrList { get { return Program.UsingTheme ? Program.StyleForm.StrList : Program.NoStyleForm.StrList; } }

        internal static Form MainForm { get { return Program.UsingTheme ? Program.StyleForm as Form: Program.NoStyleForm as Form; } }
        internal static bool FileOpen { get { return Program.UsingTheme ? Program.StyleForm.FileOpen : Program.NoStyleForm.FileOpen; } }
#endregion
    }
}
