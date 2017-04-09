using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VNXTLP
{
    internal partial class CheckUpdate : Form {
        internal bool FormReady = false;
        internal CheckUpdate()
        {
            InitializeComponent();
            UpdateStatus(Engine.LoadTranslation(51));
            //FadeIn
            new System.Threading.Thread((frm) => {
                CheckUpdate form = (CheckUpdate)frm;
                while (!form.FormReady)
                    System.Threading.Thread.Sleep(10);
                while (form.Opacity < 1.0) {
                    SetOpacity Updater = form.UpdateOpacity;
                    if (Updater != null)
                        form.Invoke(Updater, form.Opacity + 0.02);
                    System.Threading.Thread.Sleep(1);
                }
                FindUpdates();
            }).Start(this);
        }

        private Version GetVersion() {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart);
        }
        private void FindUpdates() {
            if (!CheckInternet()) {
                Program.OfflineMode = true;
                FadeClose();
            }
            try {
                Version ThisVersion = GetVersion();
#if DEBUG
                Updater VM = new Updater();
                bool HaveUpdate = VM.HaveUpdate(ThisVersion.Major, ThisVersion.Minor);

#else
                string Script = (new System.Net.WebClient() { Encoding = System.Text.Encoding.UTF8 }).DownloadString("http://vnx.uvnworks.com/Client/" + Engine.UpdateScript);
                HighLevelCodeProcessator VM = new HighLevelCodeProcessator(Script);
                bool HaveUpdate = (bool)VM.Call("Updater", "HaveUpdate", ThisVersion.Major, ThisVersion.Minor);
#endif

                if (!HaveUpdate) {
                    FadeClose();
                    return;
                }
                Program.InitializeForm = false;
                Invoke(new SetText(UpdateStatus), Engine.LoadTranslation(53));
#if DEBUG
                bool FoundAError = VM.GetUpdate(Application.ExecutablePath);
#else
                bool FoundAError = (bool)VM.Call("Updater", "GetUpdate", Application.ExecutablePath);
#endif
                if (FoundAError)
                    MessageBox.Show(Engine.LoadTranslation(54), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else {
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Launcher.exe");
                    Application.Exit();
                }
            } catch {}
            FadeClose();
        }

        delegate void SetText(string Text);
        delegate void SendClose();
        delegate void SetOpacity(double opacity);
        internal void UpdateOpacity(double opacity) {
            Opacity = opacity;
        }

        internal void UpdateStatus(string Status) {
            WaitLBL.Text = Status;
        }

        private void FadeClose() {
            new System.Threading.Thread((frm) => {
                CheckUpdate form = (CheckUpdate)frm;
                while (!form.FormReady)
                    System.Threading.Thread.Sleep(10);
                while (form.Opacity > 0.0) {
                    SetOpacity Updater = form.UpdateOpacity;
                    if (Updater != null)
                        form.Invoke(Updater, form.Opacity - 0.02);
                    System.Threading.Thread.Sleep(1);
                }
                form.Invoke(new SendClose(() => { form.Close(); }));
            }).Start(this);
        }

        private void Ready(object sender, EventArgs e) {
            FormReady = true;
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        internal static bool CheckInternet() {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}
