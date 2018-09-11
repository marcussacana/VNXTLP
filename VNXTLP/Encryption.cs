//#define ShareKey
using System.Text;

namespace VNXTLP {
    static partial class Engine {
        private static byte[] EncKey = Encoding.ASCII.GetBytes("VNX+ENC");
        private static byte[] EncSig = Encoding.ASCII.GetBytes("VNX+");
        private static bool IsEncrypted(byte[] Script) {
            return EqualsAt(Script, EncKey, 0) || EqualsAt(Script, EncSig, 0);
        }

        private static byte[] Signature(byte[] Script) {
            if (EncSig.Length >= EncKey.Length) {
                if (EqualsAt(Script, EncSig, 0))
                    return EncSig;
                return EncKey;
            } else {
                if (EqualsAt(Script, EncKey, 0))
                    return EncKey;
                return EncSig;
            }
        }
        private static byte[] Decrypt(byte[] Script) {
            if (!IsEncrypted(Script))
                return Script;

            byte[] Sig = Signature(Script);

            byte[] Result = new byte[Script.Length - Sig.Length];
            for (int i = Sig.Length; i < Script.Length; i++) {
                byte b = (byte)(Script[i] ^ (EncKey[(i-Sig.Length) % EncKey.Length] ^ 0xFF));
                Result[i - Sig.Length] = (byte)((b << 4) | (b >> 4));
            }

            return Result;
        }

        private static byte[] Encrypt(byte[] Script) {
            if (IsEncrypted(Script))
                return Script;

            //If the user Isn't in the offline mode, we have a online backup of the script
            if (!Program.OfflineMode || DebugMode)
                return Script;
#if ShareKey
            byte[] Sig = EncKey;
#else
            byte[] Sig = EncSig;
#endif

            byte[] Result = new byte[Script.Length + Sig.Length];
            Sig.CopyTo(Result, 0);

            for (int i = 0; i < Script.Length; i++) {
                byte b = (byte)((Script[i] << 4) | (Script[i] >> 4));
                Result[i + Sig.Length] = (byte)(b ^ (EncKey[i % EncKey.Length] ^ 0xFF));
            }

            return Result;
        }
    }
}
