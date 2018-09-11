using System.IO;
using SacanaWrapper;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Linq;

namespace VNXTLP {
    internal static partial class Engine {
        internal static string[] Open(string Path, bool TempMode = false) {
            string[] ReturnContent;
            if (TempMode) {
                BackupEditor = Editor;
                RemapBackup = StrMap;
                StringCountBackup = StringCount;
                PrefixBackup = Prefix;
                SufixBackup = Sufix;
                BreakLineEscapeBackup = BreakLineEscape;
            } else {
                ScriptPath = Path;
                SetConfig("VNXTLP", "LastScript", ScriptPath);
            }

            try {
                byte[] Script = Decrypt(File.ReadAllBytes(Path));
                Editor = new Wrapper();
                ReturnContent = Editor.Import(Script, System.IO.Path.GetExtension(Path), true);

                StringCount = 0;
                if (File.Exists(Path + ".map"))
                    Remap(ref ReturnContent, Path + ".map");

                ClearStrings(ref ReturnContent);
                EscapeBreakLine(ref ReturnContent);
            } catch { ReturnContent = new string[0]; }

            if (TempMode) {
                Editor = BackupEditor;
                StrMap = RemapBackup;
                StringCountBackup = StringCount;
                Prefix = PrefixBackup;
                Sufix = SufixBackup;
                BreakLineEscape = BreakLineEscapeBackup;
            }
            return ReturnContent;
        }


        internal static void Save(string Path, string[] Content, bool Temp = false) {
            string bkp = ScriptPath;
            if (Path != ScriptPath) {
                if (File.Exists(ScriptPath + ".map")) {
                    File.Copy(ScriptPath + ".map", Path + ".map", true);
                }
                if (File.Exists(ScriptPath + "-checks.bol")) {
                    File.Copy(ScriptPath + "-Checks.bol", Path + "-Checks.bol", true);
                }
            }

            ScriptPath = Path;
            SetConfig("VNXTLP", "LastScript", ScriptPath);
            
            RestoreStrings(ref Content);
            RestoreBreakLine(ref Content);
            UndoRemap(ref Content);

            if (File.Exists(ScriptPath))
                File.Delete(ScriptPath);

            byte[] Output = Encrypt(Editor.Export(Content));
            File.WriteAllBytes(ScriptPath, Output);

            if (Temp)
                ScriptPath = bkp;
        }

        private static void ClearStrings(ref string[] Strings) {
            Engine.Prefix = new Dictionary<uint, string>();
            Engine.Sufix = new Dictionary<uint, string>();
            if (GetConfigStatus("Fitler", "Trim") != ConfigStatus.Ok)
                return;
            
            string[] Trims = GetConfig("Filter", "Trim", false).Split(',');
            for (uint i = 0; i < Strings.LongLength; i++) {
                string Prefix = string.Empty, Sufix = string.Empty, Bak = Strings[i];
                do {
                    Bak = Strings[i];
                    foreach (string Trim in Trims) {
                        if (Trim.Length == 2) {
                            char Open = Trim[0], Close = Trim[1];
                            if (Strings[i].StartsWith(Open.ToString()) && Strings[i].EndsWith(Close.ToString())) {
                                Strings[i] = Strings[i].Substring(1, Strings[i].Length - 2);
                                Prefix += Open;
                                Sufix = Close + Sufix;
                            }
                        } else {
                            if (Strings[i].StartsWith(Trim)) {
                                Strings[i] = Strings[i].Substring(Trim.Length);
                                Prefix += Trim;
                            }
                            if (Strings[i].EndsWith(Trim)) {
                                Strings[i] = Strings[i].Substring(0, Strings[i].Length - Trim.Length);
                                Sufix = Trim + Sufix;
                            }
                        }
                    }
                } while (Bak != Strings[i]);

                Engine.Prefix.Add(i, Prefix);
                Engine.Sufix.Add(i, Sufix);
            }
            
        }

        private static void EscapeBreakLine(ref string[] Content) {
            string[] Escapes = new string[] { "\\n", "\\N", "[\\B]", "<\\br>", "[\\BREAKLINE]" };
            BreakLineEscape = new Dictionary<uint, string>();
            for (uint i = 0; i < Content.LongLength; i++) {
                string Line = Content[i];
                if (!Line.Contains("\n")) {
                    BreakLineEscape.Add(i, null);
                    continue;
                }
                string Escape = string.Empty;
                for (uint x = 0; x < Escapes.LongLength; x++) {
                    if (!Line.Contains(Escapes[x])) {
                        Escape = Escapes[x];
                        break;
                    }
                }
                Content[i] = Line.Replace("\n", Escape);
                BreakLineEscape.Add(i, Escape);
            }
        }

        private static void RestoreBreakLine(ref string[] Content) {
#if DEBUG
            try {
#endif
                for (uint i = 0; i < Content.LongLength; i++) {
                    string Escape = BreakLineEscape[i];
                    if (Escape == null)
                        continue;
                    Content[i] = Content[i].Replace(Escape, "\n");
                }
#if DEBUG
            } catch {
                MessageBox.Show("RestoreBreakLine - FAILED", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }
        private static void RestoreStrings(ref string[] Strings) {
            for (uint i = 0; i < Strings.LongLength; i++) {
                if (Prefix.ContainsKey(i))
                    Strings[i] = Prefix[i] + Strings[i];

                if (Sufix.ContainsKey(i))
                    Strings[i] += Sufix[i];
            }
        }


        private static void Remap(ref string[] Strings, string Remap) {
            StrMap = new Dictionary<uint, uint>();
            BinaryReader Reader = new BinaryReader(File.Open(Remap, FileMode.Open, FileAccess.Read));
            uint OriLen = StringCount = Reader.ReadUInt32();
            uint RstLen = Reader.ReadUInt32();
            if (Strings.Length != OriLen) {
                MessageBox.Show(LoadTranslation(TLID.BadScriptRemap), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] Result = new string[RstLen];

            for (uint oi = 0; oi < OriLen; oi++) {
                uint Repeats = Reader.ReadUInt32();
                for (uint ri = 0; ri < Repeats; ri++) {
                    uint Pos = Reader.ReadUInt32();
                    Result[Pos] = Strings[oi];
                    if (!StrMap.ContainsKey(Pos))
                        StrMap.Add(Pos, oi);
                }
            }
            Strings = Result;
        }

        internal static bool Genmap(string OriPath, string RstPath) {
            string[] Ori = Open(OriPath, true);
            string[] Rst = Open(RstPath, true);


            BinaryWriter Writer = new BinaryWriter(File.Create(OriPath + ".map"));
            Writer.Write((uint)Ori.LongLength);
            Writer.Write((uint)Rst.LongLength);

            for (uint oi = 0; oi < Ori.LongLength; oi++) {
                List<uint> Map = new List<uint>();
                for (uint ri = 0; ri < Rst.LongLength; ri++) {
                    if (Rst[ri] == Ori[oi])
                        Map.Add(ri);
                }
                uint[] StrMap = Map.ToArray();

                if (StrMap.LongLength == 0) {
                    Writer.Close();
                    File.Delete(OriPath + ".map");
                    return false;
                }

                Writer.Write((uint)StrMap.LongLength);
                foreach (uint Pos in StrMap)
                    Writer.Write(Pos);
            }

            Writer.Close();

            return true;
        }
        private static void UndoRemap(ref string[] Strings) {
            if (StringCount == 0)
                return;

            string[] Result = new string[StringCount];
            for (uint i = 0; i < Strings.Length; i++) {
                Result[StrMap[i]] = Strings[i];
            }

            Strings = Result;
        }

        internal delegate void ScriptDraged(string File);
        internal static void EnableDragDrop(this Form Form, ScriptDraged Invoker) {
            Form.AllowDrop = true;
            Form.DragEnter += (a, e) => {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                    e.Effect = DragDropEffects.Copy;
                }
            };
            Form.DragDrop += (a, e) => {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                    string File = ((string[])e.Data.GetData(DataFormats.FileDrop)).First();
                    Invoker(File);
                }
            };
        }
    }
}
