/*
VNXTLP - Visual Novel X Translation Plataform
Apesar de não baseado, substitui o RTT usado no
projeto de tradução para Katawa Shoujo, que
depois de 2 anos de trabalho teremos essa nova
plataforma com os recursos que sentimos falta
na versão antiga do RTT no decorrer do projeto.
*/

using System;
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args) {
            bool EndUpdate = false;
            foreach (string str in Args) {
                string Command = str.ToLower();
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

            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Launcher.exe"))
                try {
                    System.Threading.Thread.Sleep(500);
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Launcher.exe");
                }
                catch { }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Engine.InitializeStrings();
            if (EndUpdate)
                MessageBox.Show(Engine.LoadTranslation(52), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (!OfflineMode)
                try {
                    (new CheckUpdate()).ShowDialog();
                }
                catch { }
            UsingTheme = Engine.UseTheme();
            if (UsingTheme) {
                StyleForm = new NewStyle.StyleProgram();
                Application.Run(StyleForm);
            } else {
                NoStyleForm = new NoStyle();
                Application.Run(NoStyleForm);
            }
        }

    }
}
