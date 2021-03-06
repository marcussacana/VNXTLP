﻿//#define MONO
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VNXTLP
{
    internal partial class CheckUpdate : Form {

        internal new bool? Closed = false;
        internal bool FormReady = false;
        internal CheckUpdate()
        {
            InitializeComponent();
            UpdateStatus(Engine.LoadTranslation(Engine.TLID.ProcessingLogin));

            new System.Threading.Thread(() => {
                DateTime Begin = DateTime.Now;

                while (Closed == false) {
                    if ((DateTime.Now - Begin).TotalSeconds > 20) {
                        Invoke(new MethodInvoker(() => {
                            Process.Start(Application.ExecutablePath, "-retry " + (Program.Retry + 1));
                            Process.GetCurrentProcess().Kill();
                        }));
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }

                Closed = null;
            }).Start();


            //FadeIn
            new System.Threading.Thread((frm) => {
                CheckUpdate form = (CheckUpdate)frm;
                while (!form.FormReady)
                    System.Threading.Thread.Sleep(10);
                while (form.Opacity < 1.0) {
                    SetOpacity Updater = form.UpdateOpacity;
                    if (Updater != null)
                        form.Invoke(Updater, form.Opacity + 0.02);
                    System.Threading.Thread.Sleep(3);
                }
                
                while (Program.Connecting) {
                    System.Threading.Thread.Sleep(500);
                }

                Invoke(new SetText(UpdateStatus), Engine.LoadTranslation(Engine.TLID.SearchingUpdates));
                FindUpdates();
            }).Start(this);
        }

        private Version GetVersion() {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart);
        }
        private void FindUpdates() {
#if !MONO
            if (!Program.IsWindows) {
                FadeClose();
                return;
            }

            if (!CheckInternet()) {
                Program.OfflineMode = true;
                FadeClose();
                return;
            }
#endif
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
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PMan.exe")) {
                        Invoke(new SetText(UpdateStatus), Engine.LoadTranslation(Engine.TLID.UpdatngPluigins));

                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PMan.exe", "update").WaitForExit();
                    }

                    FadeClose();
                    return;
                }
                if (Closed != null)
                    Closed = true;
                Program.InitializeForm = false;
                Invoke(new SetText(UpdateStatus), Engine.LoadTranslation(Engine.TLID.InstallingUpdates));
#if DEBUG
                bool FoundAError = VM.GetUpdate(Application.ExecutablePath);
#else
                bool FoundAError = (bool)VM.Call("Updater", "GetUpdate", Application.ExecutablePath);
#endif
                if (FoundAError)
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.FailedOnUpdate), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    System.Threading.Thread.Sleep(4);
                }

                if (Closed != null) {
                    Closed = true;

                    while (Closed != null)
                        System.Threading.Thread.Sleep(100);
                }

                form.Invoke(new SendClose(() => { form.Close(); }));
            }).Start(this);
        }

        private void Ready(object sender, EventArgs e) {
            FormReady = true;
        }

#if !MONO
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        internal static bool CheckInternet() {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
#endif
    }
}
