/*
VNXTLP - Visual Novel X Translation Plataform
Apesar de não baseado, substitui o RTT usado no
projeto de tradução para Katawa Shoujo, que
depois de 2 anos de trabalho teremos essa nova
plataforma com os recursos que sentimos falta
na versão antiga do RTT no decorrer do projeto.
*/

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace VNXTLP {
    static class Program {
        internal static Form MainForm;
        internal static Search SearchForm;
        internal static bool SearchOpen = false;
        internal static bool OfflineMode = false;
        internal static bool InitializeForm = true;
        internal static bool Connecting = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args) {
            bool EndUpdate = false;
            foreach (string str in Args) {
                string Command = str.ToLower().Trim('-', '\\', '/');
                switch (Command) {
                    default:
                        continue;
                    case "offline":
                        OfflineMode = true;
                        continue;
                    case "endupdate":
                        EndUpdate = true;
                        continue;
                }
            }

            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Launcher.exe")) {
                try {
                    Thread.Sleep(500);
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Launcher.exe");
                } catch { }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Engine.InitializeStrings();

            if (EndUpdate) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.UpdatesInstaled), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            new Thread(() => {
                try {
                    if (Engine.GetConfig("FTP", "AutoLogin", false) == "true") {
                        Connecting = true;
                        Engine.Login(Engine.GetConfig("FTP", "AutoUser", true), Engine.GetConfig("FTP", "AutoPass", true), false);
                    }
                } catch { }
                Connecting = false;
            }).Start();

            if (!OfflineMode) {
                try {
                    (new CheckUpdate()).ShowDialog();
                } catch { }
                if (!Engine.FTP.Avaliable) {
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoFTPServer), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
                if (!Engine.Authenticated) {
                    Form Login;
                    switch (Engine.UsingTheme()) {
                        case "modern":
                            Login = new NewStyle.StyleLogin();
                            break;
                        case "bern":
                            Login = new BernStyle.StyleLogin();
                            break;
                        default:
                            Login = new NoStyleLogin();
                            break;
                    }

                    Login.ShowDialog();
                    if (!Engine.Authenticated)
                        return;
                }
            }

            switch (Engine.UsingTheme()) {
                case "modern":
                    MainForm = new NewStyle.StyleProgram();
                    break;
                case "bern":
                    MainForm = new BernStyle.StyleProgram();
                    break;
                default:
                    MainForm = new NoStyle();
                    break;
            }

            new Thread(() => {
                Engine.InitializePipe();
            }).Start();

            Application.Run(MainForm);

        }

        #region Non-Windows Support

        [DllImport(@"kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(@"kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        static bool? isWin;

        internal static bool IsWindows {
            get {
                if (isWin.HasValue) return isWin.Value;

                isWin = true;
                if (Info.IsRunningOnLinux())
                    isWin = false;
                if (Info.IsRunningOnMac())
                    isWin = false;

                if (isWin == false)
                    return false;

                IntPtr hModule = GetModuleHandle(@"ntdll.dll");
                if (hModule == IntPtr.Zero)
                    isWin = true;
                else {
                    IntPtr fptr = GetProcAddress(hModule, @"wine_get_version");
                    isWin = fptr == IntPtr.Zero;
                }

                return isWin.Value;
            }
        }

        #endregion

    }
}
