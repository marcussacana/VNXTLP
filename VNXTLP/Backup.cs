using System;
using System.IO;
using System.Linq;
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

            string[] List = FTP.TreeDir(UserDir);
            return (from x in List where x != "VNX+ - Backup Trash" select x.Replace("§", "/")).ToArray();
        }       

        private static string BackupFileName {
            get {
                string DIR = Path.GetDirectoryName(ScriptPath) + "\\";
                string FN = Path.GetFileNameWithoutExtension(ScriptPath);
                string EXT =  Path.GetExtension(ScriptPath);
                if (!FN.ToLower().EndsWith("_backup"))
                    FN += "_Backup";
                return DIR + FN + EXT; 
            }
        }

        internal static bool HideBackup(string Backup) {
            if (string.IsNullOrEmpty(UserAccount.Name))
                return false;
            
            string Trash = "VNX+ - Backup Trash\\";
            Backup = Backup.Replace("/", "§");

            string[] List = FTP.TreeDir(UserDir);
            if (!List.Contains(Backup))
                return false;

            if (!List.Contains("VNX+ - Backup Trash"))
                FTP.CreateDirectory(UserDir + Trash);            

            return FTP.MoveFile(UserDir + Backup, "\\" + UserDir + Trash + Backup);
        }
        internal static bool Backup(string[] Strings, bool IsSave = false) {
            try {
                if (GetConfig("VNXTLP", "OfflineBackup", false) == "true" && !IsSave) {
                    Save(BackupFileName, Strings, true);                    
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
                string BackupName = string.Format("\\{0} - {1}§{2}§{3} At {4}:{5}{6} ({7})",
                    Path.GetFileNameWithoutExtension(ScriptPath), Now.Day, Now.Month, Now.Year, Now.Hour, Now.Minute, Path.GetExtension(ScriptPath), (IsSave ? "Saved" : "Auto"));

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
