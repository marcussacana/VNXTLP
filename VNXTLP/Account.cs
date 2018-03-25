using System;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        internal static bool Register(string Username, string Password) {
            try {
                Account[] Accs = GetAccounts();
                foreach (Account acc in Accs)
                    if (acc.Name == Username)
                        return false;

                if (!ConfirmPermission(Username)) {
                    MessageBox.Show(LoadTranslation(TLID.RegisterNotAllowed), "VNXTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                byte[] Accounts = FTP.Download("Accs.bin");
                byte[] DW = new byte[4];
                Array.Copy(Accounts, 0, DW, 0, DW.Length);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(DW, 0, DW.Length);
                int Count = BitConverter.ToInt32(DW, 0);
                Count++;
                BitConverter.GetBytes(Count).CopyTo(DW, 0);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(DW, 0, DW.Length);
                Array.Copy(DW, 0, Accounts, 0, DW.Length);
                byte[] Name = Encoding.UTF8.GetBytes(Username);
                Array.Resize(ref Name, Name.Length + 1);

                byte[] Pass = GetHash(Password);

                for (int i = 0; i < Name.Length; i++)
                    Name[i] = XOR(Name[i]);

                for (int i = 0; i < Pass.Length; i++)
                    Pass[i] = XOR(Pass[i]);

                int Pos = Accounts.Length;

                Array.Resize(ref Accounts, Accounts.Length + (Name.Length + Pass.Length));
                Name.CopyTo(Accounts, Pos);
                Pass.CopyTo(Accounts, Pos + Name.Length);

                FTP.Upload("Accs.bin", Accounts);
                return true;
            }
            catch {
                return false;
            }
        }

        private static bool ConfirmPermission(string Nick) {
            byte[] Hash = GetHash(Nick);
            uint Seed = (uint)(DateTime.Now.ToBinary() & 0xFFFFFFFFu);
            for (int i = 0; i < Hash.Length; i++) {
                Seed ^= (uint)Hash[i] << (8 * (i % 7));
            }

            uint Key = Seed & 0xFFFFF;

            MessageBox.Show(LoadTranslation(TLID.YourAuthCodeIs, Key), "VNXTL - Register", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RegKey KeyWindow = new RegKey();
            if (KeyWindow.ShowDialog() != DialogResult.OK)
                return false;

            if (GetRefSeed(Key) != KeyWindow.Key)
                return false;

            return true;
        }

        internal static uint GetRefSeed(uint Key) {
            uint Base = (uint)DateTime.Now.Year * (uint)DateTime.Now.DayOfYear;
            Key *= (uint)DateTime.Now.Year;

            for (int i = 0; i < 4; i++) {
                Key ^= (Base << (i));
            }

            return Key;
        }

        internal static bool Login(string Username, string Password, bool Remember) {
            try {
                UserDirExist = false;
                Account[] Users = GetAccounts();
                byte[] Pass = GetHash(Password);
                foreach (Account Acc in Users)
                    if (Acc.Name == Username && EqualsAt(Acc.Hash, Pass)) {
                        UserAccount = Acc;
                        if (Remember) {
                            SetConfig("FTP", "AutoLogin", "true");
                            SetConfig("FTP", "AutoUser", Username);
                            SetConfig("FTP", "AutoPass", Password);
                        }
                        return true;
                    }
                return false;
            }
            catch {
                return false;
            }
        }

        private static bool EqualsAt(byte[] Arr, byte[] Arr2, int At = 0) {
            if (Arr2.Length + At > Arr.Length)
                return false;
            for (int i = 0; i < Arr2.Length; i++)
                if (Arr[i + At] != Arr2[i])
                    return false;
            return true;
        }
        internal struct Account {
            internal string Name;
            internal byte[] Hash;
        }
        internal static Account[] GetAccounts() {
            if (!Exist("Accs.bin")) {
                FTP.Upload("Accs.bin", new byte[4]);
                return new Account[0];
            }
            byte[] Accounts = FTP.Download("Accs.bin");
            byte[] DW = new byte[4];
            Array.Copy(Accounts, 0, DW, 0, DW.Length);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(DW, 0, DW.Length);
            int Count = BitConverter.ToInt32(DW, 0);
            Account[] Accs = new Account[Count];
            for (int i = 4, a = 0; i < Accounts.Length && a < Accs.Length; a++) {
                int len = 0;
                while (Accounts[i + len] != XOR(0x00))
                    len++;

                byte[] UserName = new byte[len];
                Array.Copy(Accounts, i, UserName, 0, len);

                for (int ind = 0; ind < len; ind++)
                    UserName[ind] = XOR(UserName[ind]);

                string User = Encoding.UTF8.GetString(UserName);

                i += len;
                i++;
                byte[] HASH = new byte[20];
                for (int ind = 0; ind < HASH.Length; ind++)
                    HASH[ind] = XOR(Accounts[i + ind]);
                i += HASH.Length;

                Accs[a] = new Account() {
                    Name = User,
                    Hash = HASH
                };
            }
            return Accs;
        }

        private static byte[] GetHash(string Pass) {
            byte[] Password = Encoding.UTF8.GetBytes(Pass);
            return (new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Password);
        }
        internal static byte XOR(byte b) {
            return (byte)(b ^ 0xFF);
        }
    }
}
