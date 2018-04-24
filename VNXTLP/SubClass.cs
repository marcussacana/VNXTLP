using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {

        #region FTP
        private static string ParsePass(string Hex) {
            if (Hex.Length % 2 != 0) {
                MessageBox.Show(LoadTranslation(TLID.BadPassowordFormat), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            byte[] Str = new byte[Hex.Length / 2];
            for (int i = 0; i < Str.Length; i++) {
                string h = Hex.Substring(i * 2, 2);
                byte B = byte.Parse(h, System.Globalization.NumberStyles.HexNumber);
                Str[i] = XOR(B);
            }
            string Result = Encoding.UTF8.GetString(Str);
            return Result;
        }

        internal static class FTP {
            private static NetworkCredential ServerAcess { get {
                    string User = GetConfig("FTP", "Login", false);
                    if (string.IsNullOrWhiteSpace(User))
                        throw new UnauthorizedAccessException();
                    string Pass = ParsePass(GetConfig("FTP", "Pass"));
                    return new NetworkCredential(User, Pass);
                } }
            private static string Server  {
                get {
                    string Ip = GetConfig("FTP", "Host", false);
                    if (string.IsNullOrWhiteSpace(Ip))
                        throw new Exception();
                    return string.Format("ftp://{0}/", Ip);
                }
            }

            internal static bool Avaliable {
                get {
                    if (GetConfigStatus("FTP", "Host") != ConfigStatus.Ok)
                        return false;
                    if (GetConfigStatus("FTP", "Login") != ConfigStatus.Ok)
                        return false;
                    if (GetConfigStatus("FTP", "Pass") != ConfigStatus.Ok)
                        return false;

                    return true;
                }
            }
            internal static byte[] Download(string file) {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Server + file.Replace("\\", "/"));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = ServerAcess;
                request.UseBinary = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) {
                    using (Stream rs = response.GetResponseStream()) {
                        using (MemoryStream ws = new MemoryStream()) {
                            byte[] buffer = new byte[2048];
                            int bytesRead = rs.Read(buffer, 0, buffer.Length);
                            while (bytesRead > 0) {
                                ws.Write(buffer, 0, bytesRead);
                                bytesRead = rs.Read(buffer, 0, buffer.Length);
                            }
                            return ws.ToArray();
                        }
                    }
                }
            }
            internal static void Upload(string SavePath, byte[] DataToSend) {
                FtpWebRequest request;
                request = WebRequest.Create(new Uri(Server + SavePath.Replace("\\", "/"))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;
                request.Credentials = ServerAcess;
                request.ConnectionGroupName = "VNX-BackupEngine";
                using (MemoryStream ms = new MemoryStream(DataToSend)) {
                    byte[] buffer = new byte[ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    ms.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    requestStream.Flush();
                }
                request.Abort();
            }
            internal static string[] TreeDir(string Path) {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Server + Path.Replace("\\", "/"));
                ftpRequest.Credentials = ServerAcess;
                ftpRequest.KeepAlive = false;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                string[] files = new string[0];

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line)) {
                    if (line == "." || line == "..")
                        line = streamReader.ReadLine();
                    else {
                        string[] temp = new string[files.Length + 1];
                        files.CopyTo(temp, 0);
                        temp[files.Length] = line;
                        files = temp;
                        line = streamReader.ReadLine();
                    }
                }
                streamReader.Close();
                ftpRequest.Abort();
                return files;
            }
            internal static void CreateDirectory(string Path) {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Server + Path.Replace("\\", "/"));
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = ServerAcess;
                request.KeepAlive = false;
                using (var resp = (FtpWebResponse)request.GetResponse()) {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            internal static bool MoveFile(string Path, string NewPath) {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Server + Path.Replace("\\", "/"));
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = NewPath.Replace("\\", "/");
                request.UseBinary = true;
                request.Credentials = ServerAcess;
                request.KeepAlive = false;
                using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse()) {
                    return resp.StatusCode == FtpStatusCode.CommandOK || resp.StatusCode == FtpStatusCode.FileActionOK;
                }
            }
        }

        #endregion
        internal class RadioToolStrip {
            private ToolStripMenuItem[] Items;
            private int Default = 0;
            internal event EventHandler CheckedChange;
            internal int SelectedIndex {
                get
                {
                    for (int i = 0; i < Items.Length; i++)
                        if (Items[i].Checked)
                            return i;
                    return -1;
                }
                set
                {
                    for (int i = 0; i < Items.Length; i++)
                        if (i == value)
                            Items[i].Checked = true;
                }
            }
            internal RadioToolStrip(ref ToolStripMenuItem[] Radios, int DefaultIndex) {
                Items = Radios;
                Default = DefaultIndex;
                foreach (ToolStripMenuItem Item in Items) {
                    Item.CheckOnClick = true;
                    Item.CheckedChanged += CheckedChanged;
                }
            }

            private void CheckedChanged(object sender, EventArgs e) {
                ToolStripMenuItem Item = (ToolStripMenuItem)sender;
                if (Item.Checked) {
                    for (int i = 0; i < Items.Length; i++)
                        if (Item.GetHashCode() != Items[i].GetHashCode())
                            Items[i].Checked = false;
                        else
                            CheckedChange?.Invoke(this, new EventArgs());
                }
                else {
                    bool HaveSelection = false;
                    for (int i = 0; i < Items.Length; i++)
                        if (Items[i].Checked)
                            HaveSelection = true;
                    if (!HaveSelection)
                        Item.Checked = true;
                }
            }
        }

        internal class OverTimerEvent {
            internal bool Over = false;
            private int Time = 0;
            internal Point Point;
            private Point OPoint = new Point(0, 0);
            internal object sender;

            internal event MouseEventHandler MouseStopOver;
            internal void Initialize() {
                if (sender == null)
                    throw new Exception("Need define the sender");
                Timer t = new Timer() {
                    Interval = 500
                };

                //MouseOver more than 500ms
                t.Tick += (s, e) => {
                    if (Over) {
                        if (OPoint != Point) {
                            OPoint = Point;
                            Time = 0;
                        }
                        else if (Time == 2) {
                            MouseEventArgs MEA = new MouseEventArgs(MouseButtons.None, 0, OPoint.X, OPoint.Y, 0);
                            MouseStopOver?.Invoke(sender, MEA);
                        }
                        Time++;
                    }
                    else {
                        Time = 0;
                    }
                };
                t.Enabled = true;
            }
        }


        internal enum Commands : sbyte {
            GetCount, GetSel, SetSel, GetTop, SetTop, Closing, Closed, Running, Connected
        }

        internal enum TLID : int {
            LoadingBackups = 0,
            Close = 1,
            BackupsLoaded = 2,
            WelcomeBackup = 3,
            Welcome = 4,
            MyAccount = 5,
            Register = 6,
            Login = 7,
            Password = 8,
            Username = 9,
            FailedToAuth = 10,
            Next = 11,
            Back = 12,
            File = 13,
            Open = 14,
            SaveAs = 15,
            Selection = 16,
            SelectAll = 17,
            UnselectAll = 18,
            AutoSelect = 19,
            Options = 20,
            Theme = 21,
            Basic = 22,
            Modern = 23,
            BackupFrequence = 24,
            OnSave = 25,
            BackOn50 = 26,
            BackOn25 = 27,
            BackOn10 = 28,
            Never = 29,
            SpellChecking = 30,
            ValidateIndex = 31,
            About = 32,
            SelectAScript = 33,
            BeforeSaveOpenAScript = 34,
            WrongBackup = 35,
            BackupLoaded = 36,
            WaitBackupUpload = 37,
            CreateNewAccount = 38,
            ConfirmPassword = 40,
            PasswordMissmatch = 41,
            BadUsername = 42,
            RegisterSucess = 43,
            RegisterFailed = 44,
            Confim = 45,
            SaveAsSucess = 46,
            InfoMask = 47,
            ScriptComplete = 48,
            Congratulation = 49,
            FailedToLoadSetting = 50,
            SearchingUpdates = 51,
            UpdatesInstaled = 52,
            InstallingUpdates = 53,
            FailedOnUpdate = 54,
            BadPassowordFormat = 55,
            NoSuggestions = 56,
            AddOnDictionary = 57,
            SearchOrReplace = 58,
            SearchFor = 59,
            ReplaceWith = 60,
            NoMatchFound = 61,
            TranslationSystem = 62,
            LEC = 63,
            Google = 64,
            SelectAReferenceScript = 65,
            BadReferenceScript = 66,
            Reference = 67,
            ReferenceScript = 68,
            DoesNotLooksADialogue = 69,
            HighContrast = 70,
            HighResolution = 71,
            XResultsReplaced = 72,
            AdvancedSettings = 73,
            CaseSensetive = 74,
            SearchFullDialogue = 75,
            SearchNonDialogue = 76,
            CircleSearch = 77,
            ReplaceFullDialogue = 78,
            SearchDirection = 79,
            Forward = 80,
            Behind = 81,
            ReplaceAll = 84,
            XResultsFoundAt = 85,
            SearchInFolder = 86,
            ShowSynonyms = 87,
            FailedToDetectWordLang = 88,
            SynonymsFor = 89,
            LoadingSuggetionsPlzWait = 90,
            NoSynonymsFound = 91,
            TryManuallySearch = 92,
            InvalidInput = 93,
            EnableAutoLogin = 94,
            RestarNow = 95,
            NoScriptOpen = 96,
            BadScriptRemap = 97,
            FailedScriptsNotEqual = 98,
            RemapNow = 99,
            RemapGenerated = 100,
            SelectMode = 101,
            AutoDetect = 102,
            Asian = 103,
            Latim = 104,
            NoChangeDialogueNow = 105,
            Save = 106,
            UpdatngPluigins = 107,
            Tools = 108,
            DialoguesCount = 109,
            LinesCount = 110,
            SelectFilesToCount = 111,
            PressCtrlCToCopy = 112,
            WithTotalOf = 113,
            YourAuthCodeIs = 114,
            AuthToken = 115,
            RegisterNotAllowed = 116,
            GenerateToken = 117,
            NoFTPServer = 118,
            DecryptFiles = 119,
            OperationClear = 120,
            ProcessingLogin = 121,
            BackupDeleted = 122,
            DeleteBackupFailed = 123,
            DeleteIt = 124,
            SyncConfirm = 125,
            DisableSync = 126,
            LimitSkip = 127,
            DynamicMode = 128,
            LoadingUsers = 129,
            UsersLoaded = 130,
            BrowseUser = 131
        }
    }
}
