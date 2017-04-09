using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EndUpdate {
    class Program {
        static void Main(string[] args) {
            Console.Write("VNXTLP Auto Update...");
            foreach (Process proc in Process.GetProcessesByName("VNXTLP"))
                proc.Kill();
            System.Threading.Thread.Sleep(1500);

            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] Files = System.IO.Directory.GetFiles(BaseDir, "*-Updated.*");
            string NewExe = string.Empty;
            //All Files Replaced
            for (int i = 0; i < Files.Length; i++) {
                string File = Files[i];
                //-Updated = 8 Len
                int PrefixLen = 8 + System.IO.Path.GetExtension(File).Length;
                string OriginalFile = File.Substring(0, File.Length - PrefixLen);
                if (!OriginalFile.Contains("Launcher") && OriginalFile.EndsWith(".exe"))
                    NewExe = OriginalFile;
                //Try-Force Delete
                int noproctries = 0;
                if (!OriginalFile.EndsWith(".ini"))
                    while (System.IO.File.Exists(OriginalFile) || noproctries < 0)
                        try {
                            System.IO.File.Delete(OriginalFile);
                        }
                        catch {
                            try {
                                List<Process> Processes = FileUtil.WhoIsLocking(OriginalFile);
                                if (Processes.Count == 0) {
                                    noproctries++;
                                    if (noproctries < 2)
                                        continue;
                                    else
                                        break;
                                }
                                foreach (Process proc in Processes)
                                    proc.Kill();
                            }
                            catch {
                                break;
                            }
                        }
                if (System.IO.File.Exists(OriginalFile) && !OriginalFile.EndsWith(".ini"))
                    return;
                if (File.EndsWith(".ini")) {
                    UpdateIni(OriginalFile, File);
                    System.IO.File.Delete(File);
                }
                else
                    System.IO.File.Move(File, OriginalFile);
            }
            Process.Start(NewExe, "EndUpdate");
        }

        private static void UpdateIni(string OriginalSett, string NewSett) {
            string[] OldIni = System.IO.File.ReadAllLines(OriginalSett, Encoding.UTF8);
            string[] NewIni = System.IO.File.ReadAllLines(NewSett, Encoding.UTF8);
            Entry[] Old = OpenIni(OldIni);
            Entry[] New = OpenIni(NewIni);

            List<Entry> Result = new List<Entry>();
            foreach (Entry entry in New) {
                Entry OutEntry = new Entry();
                OutEntry.Name = entry.Name;
                List<Var> Vars = new List<Var>();
                foreach (Var var in entry.Vars) {
                    Var NewVar = new Var();
                    NewVar.Name = var.Name;
                    string OldVal = GetCfg(Old, entry.Name, var.Name);
                    if (OldVal != null)
                        NewVar.Value = OldVal;
                    else
                        NewVar.Value = var.Value;
                    Vars.Add(NewVar);
                }
                OutEntry.Vars = Vars.ToArray();
                Result.Add(OutEntry);
            }

            string[] OutIni = CompileIni(Result.ToArray());

            System.IO.File.WriteAllLines(OriginalSett, OutIni, Encoding.UTF8);
        }

        private static string[] CompileIni(Entry[] Ini) {
            List<string> Lines = new List<string>();
            foreach (Entry entry in Ini) {
                Lines.Add(string.Format("[{0}]", entry.Name));
                foreach (Var var in entry.Vars)
                    Lines.Add(string.Format("{0}={1}", var.Name, var.Value));
                Lines.Add(string.Empty);
            }
            return Lines.ToArray();
        }

        private static string GetCfg(Entry[] Ini, string Name, string VarName) {
            foreach (Entry entry in Ini) {
                if (entry.Name != Name)
                    continue;
                foreach (Var var in entry.Vars) {
                    if (var.Name == VarName)
                        return var.Value;
                }
            }
            return null;
        }

        private static Entry[] OpenIni(string[] File) {
            List<Entry> Entries = new List<Entry>();
            List<Var> Vars = new List<Var>();
            string KeyName = string.Empty;
            for (int i = 0; i < File.Length; i++) {
                string Line = File[i];
                if (Line.StartsWith("//") || Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line))
                    continue;
                if (Line.StartsWith("[") && Line.EndsWith("]")) {
                    if (KeyName != string.Empty) {
                        Entries.Add(new Entry() {
                            Name = KeyName,
                            Vars = Vars.ToArray()
                        });
                    }
                    KeyName = GetTextArroundOf(Line, '[', ']');
                    Vars = new List<Var>();
                    continue;
                }
                if (Line.Contains("=")) {
                    Vars.Add(new Var {
                        Name = Line.Split('=')[0],
                        Value = Line.Split('=')[1]
                    });
                }
            }
            Entries.Add(new Entry() { Name = KeyName, Vars = Vars.ToArray() });
            return Entries.ToArray();
        }

        private static string GetTextArroundOf(string FullStr, char Start, char End) {
            return FullStr.Split(Start)[1].Split(End)[0];
        }
        
        private struct Entry {
            internal string Name;
            internal Var[] Vars;
        }

        private struct Var {
            internal string Name;
            internal string Value;
        }
    }
}
