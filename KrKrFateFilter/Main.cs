using System.Collections.Generic;
using System.IO;

namespace KrKrFateFilter {
    public class KSFilter {

        private string Path;
        private Dictionary<uint, string> Prefix = new Dictionary<uint, string>();
        private Dictionary<uint, string> Sufix = new Dictionary<uint, string>();
        public KSFilter(string Script) {
            Path = Script;
        }

        public string[] Import() {
            List<string> Lines = new List<string>();
            Prefix = new Dictionary<uint, string>();
            Sufix = new Dictionary<uint, string>();
            uint ID = 0;
            TextReader Reader = File.OpenText(Path);
            while (Reader.Peek() != -1) {
                string Line = Reader.ReadLine();
                if (IsString(Line))
                    Lines.Add(LineWork(true, ID++, Line));
            }
            Reader.Close();
            return Lines.ToArray();
        }

        public void Export(string ScriptPath, string[] Content) {
            uint ID = 0;
            TextWriter OutFile = new StreamWriter(new FileStream(ScriptPath, FileMode.CreateNew), System.Text.Encoding.Unicode);
            TextReader Reader = File.OpenText(Path);
            while (Reader.Peek() != -1) {
                string Line = Reader.ReadLine();
                if (!IsString(Line)) {
                    OutFile.WriteLine(Line);
                    continue;
                }
                OutFile.WriteLine(LineWork(false, ID, Content[ID]));
                ID++;
            }
            Reader.Close();
            OutFile.Close();
        }

        private string LineWork(bool Mode, uint ID, string Line, bool While = false) {
            string[] Tags = new string[] { ";", " ", "[line1]", "[line2]", "[line3]", "[line4]", "[line5]", "[line6]", "[line7]", "[line8]", "[line9]", "[r]", "[l]"};
            string ResultLine = Line;
            if (Mode) {
                for (uint i = 0; i < Tags.Length; i++) {
                    if (ResultLine.StartsWith(Tags[i])) {
                        if (!Prefix.ContainsKey(ID))
                            Prefix[ID] = string.Empty;
                        Prefix[ID] += Tags[i];
                        ResultLine = ResultLine.Substring(Tags[i].Length, ResultLine.Length - Tags[i].Length);
                    }
                    if (ResultLine.EndsWith(Tags[i])) {
                        if (!Sufix.ContainsKey(ID))
                            Sufix[ID] = string.Empty;
                        Sufix[ID] = Tags[i] + Sufix[ID];
                        ResultLine = ResultLine.Substring(0, ResultLine.Length - Tags[i].Length); 
                    }
                }
            } else {
                if (Prefix.ContainsKey(ID))
                    ResultLine = Prefix[ID] + ResultLine;
                if (Sufix.ContainsKey(ID))
                    ResultLine = ResultLine + Sufix[ID];
            }
            while (!While && Mode) {
                string L = LineWork(true, ID, ResultLine, true);
                if (ResultLine == L) {
                    ResultLine = L;
                    break;
                } else {
                    ResultLine = L;
                }
            }
            return ResultLine;
        }

        private bool IsString(string Line) {
            Line = Line.Trim();
            if (string.IsNullOrEmpty(Line))
                return false;
            char Start = Line[0];
            bool TagTest = false;
            if (Line.StartsWith("["))
                for (int i = 0, c = 0; i < Line.Length; i++) {
                    char l = Line[i];
                    if (l == ']')
                        c--;
                    if (l == '[')
                        c++;
                    if (c == 0 && i + 1 == Line.Length) {
                        TagTest = true;
                        break;
                    } else if (c == 0)
                        break;
                }
            return !(Start == '@' || Start == '*' || TagTest); //It's Right?
        }

    }
}
