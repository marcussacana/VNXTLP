using System;
using System.IO;
using System.Text;

namespace VNXTLP {
    internal static partial class Engine {
        #region BackupSystem
        internal static string[] LoadBackup(int ID) {
            string BackupPath = UserDir + FTP.TreeDir(UserDir)[ID];
            byte[] Backup = FTP.Download(BackupPath);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(Backup, 0, 4);
            string[] StringList = new string[BitConverter.ToUInt32(Backup, 0)];
            for (int i = 0, b = 4; i < StringList.Length; i++) {
                MemoryStream ms = new MemoryStream();
                while (Backup[b] != XOR(0x00))
                    ms.WriteByte(XOR(Backup[b++]));
                b++;
                StringList[i] = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
            }
            return StringList;
        }
        internal static string[] ListBackups() {
            if (!UserDirExist || !BackupDirExist) {
                if (!Exist("Backup"))
                    return new string[0];
                if (!Exist(UserAccount.Name, "Backup"))
                    return new string[0];
            }
            return FTP.TreeDir("Backup\\" + UserAccount.Name);
        }

        internal static bool Backup(string[] Strings, bool IsSave = false) {
            try {
                if (GetConfig("VNXTLP", "OfflineBackup", false) == "true") {
                    string Path = ScriptPath;
                    Save(System.IO.Path.GetDirectoryName(Path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(Path) + "_backup" + System.IO.Path.GetExtension(Path), Strings, true);
                    ScriptPath = Path;
                }

                if (string.IsNullOrEmpty(UserAccount.Name))
                    return false;
                UploadingBackup = true;
                string UserDir = @"Backup\" + UserAccount.Name;

                if (!UserDirExist || !BackupDirExist) {
                    BackupDirExist = Exist("Backup");
                    if (!BackupDirExist)
                        FTP.CreateDirectory("Backup");
                    UserDirExist = Exist(UserAccount.Name, "Backup");
                    if (!UserDirExist) {
                        FTP.CreateDirectory(UserDir);
                    }
                }

                DateTime Now = DateTime.Now;
                string BackupName = string.Format("\\{0} - {1},{2},{3} At {4}:{5}.{6} " + (IsSave ? "(Saved)" : "(Auto)"),
                    Path.GetFileNameWithoutExtension(ScriptPath), Now.Day, Now.Month, Now.Year, Now.Hour, Now.Minute, Path.GetExtension(ScriptPath));

                byte[] Backup = new byte[4];
                BitConverter.GetBytes((uint)Strings.Length).CopyTo(Backup, 0);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(Backup, 0, Backup.Length);

                foreach (string Str in Strings) {
                    byte[] String = Encoding.UTF8.GetBytes(Str);
                    Array.Resize(ref String, String.Length + 1);
                    for (int i = 0; i < String.Length; i++)
                        String[i] = XOR(String[i]);
                    int Pos = Backup.Length;
                    Array.Resize(ref Backup, Backup.Length + String.Length);
                    String.CopyTo(Backup, Pos);
                }

                FTP.Upload(UserDir + BackupName, Backup);
                UploadingBackup = false;
                return true;
            }
            catch {
                UploadingBackup = false;
                return false;
            }
        }

        internal static bool Exist(string FileToFind, string Dir = "") {
            string[] Tree = FTP.TreeDir(Dir);

            foreach (string Path in Tree)
                if (Path == FileToFind)
                    return true;
            return false;
        }
        
        #endregion

    }
}
