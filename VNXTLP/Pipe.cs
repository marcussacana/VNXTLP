using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace VNXTLP {
    static partial class Engine {

        public static bool PipeIsOpen(string Name) {
            return Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath)).Length > 1;
        }

        public static void ConnectPipe() {
            try {
                using (NamedPipeClientStream Pipe = new NamedPipeClientStream("VNXTLP")) {
                    Pipe.Connect();
                    ServerStatus = Commands.Running;
                    bool Initialized = false;

                    int LstRmt = -1, LstLocal = -1;

                    while (Pipe.IsConnected) {
                        byte Reply = 0;
                        do {
                            Thread.Sleep(50);

                            Pipe.WriteByte((byte)ServerStatus);
                            Pipe.Flush();
                            Reply = (byte)Pipe.ReadByte();
                            
                        } while (!FileOpen && Reply != (byte)Commands.Closing && ServerStatus != Commands.Closing);

                        if (Reply == (byte)Commands.Closing || ServerStatus == Commands.Closing) {
                            ServerStatus = Commands.Closing;
                            break;
                        }
                        
                        Pipe.WriteByte((byte)Commands.GetCount);
                        Pipe.Flush();
                        byte[] Buff = new byte[4];
                        Pipe.Read(Buff, 0, Buff.Length);
                        int Cnt = BitConverter.ToInt32(Buff, 0);

                        if (Cnt != StrCnt)
                            continue;

                        if (SelIndex < 0)
                            continue;

                        if (!Initialized) {
                            Initialized = true;
                            bool OK = false;
                            MainForm.Invoke(new MethodInvoker(() => {
                                OK = MessageBox.Show(LoadTranslation(TLID.SyncConfirm), "VNXTLP", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                                if (OK) {
                                    ToolStripSeparator Separator = new ToolStripSeparator();
                                    ToolStripMenuItem Item = new ToolStripMenuItem(LoadTranslation(TLID.DisableSync));
                                    Item.Click += (a, b) => {
                                        ServerStatus = Commands.Closing;
                                        ItemHost.DropDownItems.Remove(Item);
                                        ItemHost.DropDownItems.Remove(Separator);
                                    };

                                    ItemHost.DropDownItems.Add(Separator);
                                    ItemHost.DropDownItems.Add(Item);
                                }

                            }));
                            if (!OK) {
                                Pipe.Close();
                                break;
                            }
                            Pipe.WriteByte((byte)Commands.Connected);
                            Pipe.Flush();
                            HalfMaxmize(true);
                        }
                        if (SelIndex != LstLocal) {
                            Pipe.WriteByte((byte)Commands.SetTop);
                            Pipe.Write(BitConverter.GetBytes(TopItem), 0, 4);

                            Pipe.WriteByte((byte)Commands.SetSel);
                            Pipe.Write(BitConverter.GetBytes(SelIndex), 0, 4);
                            Pipe.Flush();

                            LstLocal = SelIndex;
                            LstRmt = SelIndex;
                        } else {
                            Pipe.WriteByte((byte)Commands.GetSel);
                            Pipe.Flush();
                            Pipe.Read(Buff, 0, Buff.Length);
                            int RemoteSel = BitConverter.ToInt32(Buff, 0);

                            Pipe.WriteByte((byte)Commands.GetTop);
                            Pipe.Flush();
                            Pipe.Read(Buff, 0, Buff.Length);
                            int RemoteTop = BitConverter.ToInt32(Buff, 0);

                            if (LstRmt != RemoteSel) {
                                LstRmt = RemoteSel;
                                TopItem = RemoteTop;
                                SelIndex = LstRmt;
                                LstLocal = LstRmt;
                            }
                        }
                    }
                    Pipe.WaitForPipeDrain();
                    Pipe.Close();
                }
            } catch { }

            ServerStatus = Commands.Closed;
        }

       
        public static void InitializePipe() {
            while (true) {
                try {
                    if (StrList != null)
                        break;
                } catch { }
            }

            if (PipeIsOpen("VNXTLP")) {
                ConnectPipe();
                return;
            }

            try {
                using (NamedPipeServerStream Pipe = new NamedPipeServerStream("VNXTLP", PipeDirection.InOut)) {
                    Pipe.WaitForConnection();

                    while (!FileOpen)
                        Thread.Sleep(50);

                    bool Initialized = false;
                    
                    while (Pipe.IsConnected) {
                        Thread.Sleep(50);
                        Commands Command = (Commands)Pipe.ReadByte();

                        if (!Initialized) {
                            ServerStatus = Commands.Running;
                            Initialized = true;
                        }

                        byte[] Buff = new byte[4];
                        switch (Command) {
                            case Commands.GetCount:
                                Pipe.Write(BitConverter.GetBytes(StrCnt), 0, 4);
                                Pipe.Flush();
                                break;
                            case Commands.GetSel:
                                Pipe.Write(BitConverter.GetBytes(SelIndex), 0, 4);
                                Pipe.Flush();
                                break;
                            case Commands.SetSel:
                                Pipe.Read(Buff, 0, Buff.Length);
                                SelIndex = BitConverter.ToInt32(Buff, 0);
                                break;
                            case Commands.GetTop:
                                Pipe.Write(BitConverter.GetBytes(TopItem), 0, 4);
                                Pipe.Flush();
                                break;
                            case Commands.SetTop:
                                Pipe.Read(Buff, 0, Buff.Length);
                                TopItem = BitConverter.ToInt32(Buff, 0);
                                break;
                            case Commands.Connected:
                                HalfMaxmize(false);
                                break;
                        }
                        if (Command == Commands.Closing || ServerStatus == Commands.Closing) {
                            Pipe.WriteByte((byte)Commands.Closing);
                            Pipe.Flush();
                            Pipe.WaitForPipeDrain();
                            Pipe.Close();
                            break;
                        } else if (Command == Commands.Running) {
                            Pipe.WriteByte((byte)ServerStatus);
                            Pipe.Flush();
                        }
                        Pipe.WaitForPipeDrain();
                    }
                }
            } catch { }

            ServerStatus = Commands.Closed;
        }
    }
}
