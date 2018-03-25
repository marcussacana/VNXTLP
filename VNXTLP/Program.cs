/*
VNXTLP - Visual Novel X Translation Plataform
Apesar de não baseado, substitui o RTT usado no
projeto de tradução para Katawa Shoujo, que
depois de 2 anos de trabalho teremos essa nova
plataforma com os recursos que sentimos falta
na versão antiga do RTT no decorrer do projeto.
*/

using System;
using System.Threading;
using System.Windows.Forms;

namespace VNXTLP {
    static class Program {
        internal static NewStyle.StyleProgram StyleForm;
        internal static NoStyle NoStyleForm;
        internal static Search SearchForm;
        internal static bool SearchOpen = false;
        internal static bool OfflineMode = false;
        internal static bool InitializeForm = true;
        internal static bool UsingTheme = false;
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
                    System.Threading.Thread.Sleep(500);
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

            UsingTheme = Engine.UseTheme();

            if (!OfflineMode) {
                try {
                    (new CheckUpdate()).ShowDialog();
                } catch { }
                if (!Engine.FTP.Avaliable) {
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoFTPServer), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Engine.Authenticated) {
                    Form Login = UsingTheme ? new NewStyle.StyleLogin() : (Form)new NoStyleLogin();

                    Login.ShowDialog();
                    if (!Engine.Authenticated)
                        return;
                }
            }

            if (UsingTheme) {
                StyleForm = new NewStyle.StyleProgram();

                new Thread(() => {
                    Engine.InitializePipe();
                }).Start();

                Application.Run(StyleForm);
            } else {
                NoStyleForm = new NoStyle();

                new Thread(() => {
                    Engine.InitializePipe();
                }).Start();

                Application.Run(NoStyleForm);
            }
        }

    }
}
