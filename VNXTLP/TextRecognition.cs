using System;
using System.IO;

namespace VNXTLP {
    internal static partial class Engine {
        internal static void StartSelction() {
            if (File.Exists(TableName)) {
                bool[] bools = BinToBool(File.ReadAllBytes(TableName));
                for (int i = 0; i < StrList.Items.Count; i++) {
                    StrList.SetItemChecked(i, bools[i]);
                }
            }
            else {
                AutoSelect();
            }
        }
        internal static void UpdateSelection() {
            int Count = (StrList.Items.Count / 8) + (StrList.Items.Count % 8);
            
            byte[] Booleans = new byte[Count];
            for (int i = 0, b = 0; i < Booleans.Length; i++, b += 8) {
                byte Byt = 0;
                /*1, 2, 4, 8, 16, 32, 64, 128*/
                for (int loop = 0, multiple = 1; loop < 8; loop++) {
                    if (b + loop < StrList.Items.Count && StrList.GetItemChecked(b + loop))
                        Byt |= (byte)multiple;
                    multiple *= 2;
                }

                Booleans[i] = Byt;
            }
            File.WriteAllBytes(TableName, Booleans);
        }

        internal static bool[] BinToBool(byte[] Bin) {
            bool[] Booleans = new bool[Bin.Length * 8];
            for (int i = 0, b = 0; i < Bin.Length; i++, b += 8) {
                /*1, 2, 4, 8, 16, 32, 64, 128*/
                for (int loop = 0, multiple = 1; loop < 8; loop++) {
                    Booleans[b + loop] = (Bin[i] & multiple) != 0;
                    multiple *= 2;
                }
            }
            return Booleans;
        }
        internal static void AutoSelect() {
            if (StrList == null)
                return;
            string Conf = GetConfig("VNXTLP", "SelMode", false);
            if (string.IsNullOrWhiteSpace(Conf) || Conf == "0")
                AutoDetectMode();
            else {
                bool Asian = Conf == "1";
                for (int i = 0; i < StrList.Items.Count; i++) {
                    bool Status = IsDialogue(StrList.Items[i].ToString().ToLower(), Asian);
                    StrList.SetItemChecked(i, Status);
                }
            }
        }

        private static bool IsDialogue(string text, bool Asian) {
            bool Status = true;
            int Process = 0;
            while (Status) {
                switch (Process) {
                    default:
                        goto ExitWhile;
                    case 0:
                        Status = !ContainsOR(text, GetConfig("AutoSelectionEngine", "Deny-Chars"));
                        break;
                    case 1:
                        Status = NumberLimiter(text, text.Length / 2);
                        break;
                    case 2:
                        Status = text.Length >= 3;
                        break;
                    case 3:
                        if (Asian)
                            Status = MinimiumFound(text, Properties.Resources.JapCommon, text.Length / 4);
                        else
                            Status = text.Contains(((char)32).ToString()) || ContainsOR(text.Substring(text.Length - 2, 2), GetConfig("AutoSelectionEngine", "Marks"));
                        break;
                    case 4:
                        if (!Asian)
                            break;
                        Status = ContainsOR(text, GetConfig("AutoSelectionEngine", "Allowed-Chars"));
                        break;
                    case 5:
                        if (Asian)
                            break;
                        Status = !ContainsOR(text, Properties.Resources.JapCommon);
                        break;
                }
                Process++;
            }
            ExitWhile:
            ;
            return Status;
        }

        private static void AutoDetectMode() {
            int trie = 0;
            uint count = 0;
            uint NonAsian = 0;
            uint Asian = 0;
            again:
            ;
            for (int i = 0; i < StrList.Items.Count; i++) {
                bool Status = IsDialogue(StrList.Items[i].ToString().ToLower(), trie == 1 || trie == 3);
                if (Status)
                    count++;
                StrList.SetItemChecked(i, Status);
            }
            if (trie < 2) {
                switch (trie) {
                    case 0:
                        NonAsian = count;
                        break;
                    case 1:
                        Asian = count;
                        break;
                }
                trie++;
                count = 0;
                if (trie == 2 && Asian > NonAsian)
                    trie++;
                goto again;
            }
        }

        private static bool MinimiumFound(string text, string MASK, int min) {
            string[] Entries = MASK.Split(',');
            int found = 0;
            for (int i = 0; i < text.Length; i++) {
                for (int ind = 0; ind < Entries.Length; ind++)
                    if (text[i] == Entries[ind][0]) {
                        found++;
                        break;
                    }
            }
            return found >= min;
        }

        private static bool NumberLimiter(string text, int val) {
            int min = '0', max = '9', total = 0;
            int asmin = '０', asmax = '９';
            foreach (char chr in text)
                if (chr >= min && chr <= max)
                    total++;
                else if (chr >= asmin && chr <= asmax)
                    total++;
            return total < val;
        }
        private static bool ContainsOR(string text, string MASK) {
            string[] entries = MASK.Split(',');
            foreach (string entry in entries)
                if (text.Contains(entry))
                    return true;
            return false;
        }
        private static bool ContainsAND(string text, string MASK) {
            string[] entries = MASK.Split(',');
            foreach (string entry in entries)
                if (!text.Contains(entry))
                    return false;
            return true;
        }
    }
}
