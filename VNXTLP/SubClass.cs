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
                MessageBox.Show(LoadTranslation(55), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            private Point OPoint = new System.Drawing.Point(0, 0);
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
                        else if (Time == 3) {
                            MouseEventArgs MEA = new MouseEventArgs(MouseButtons.None, 0, OPoint.X, OPoint.Y, 0);
                            MouseStopOver?.Invoke(sender, MEA);
                        }
                        t.Interval = 1000;
                        Time++;
                    }
                    else {
                        Time = 0;
                        t.Interval = 500;
                    }
                };
                t.Enabled = true;
            }
        }
    }
}
