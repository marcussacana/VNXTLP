using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace NSFilter
{
    public class Umineko {
        private TextReader Script;
        private Dictionary<long, string> Prefix = new Dictionary<long, string>();
        private Dictionary<long, string> Sufix = new Dictionary<long, string>();
        private bool ENG = false;
        public Umineko(TextReader Script, bool EN) {
            this.Script = Script;
            ENG = EN;
        }

        public string[] Import() {
            string Prefix = ENG ? "langen" : "langjp";
            List<string> Lines = new List<string>();
            while (Script.Peek() != -1) {
                string Line = Script.ReadLine();
                if (Line.ToLower().StartsWith(Prefix))
                    Lines.Add(Line.Substring(Prefix.Length, Line.Length - Prefix.Length));
            }

            //Reset Stream
            (Script as StreamReader).BaseStream.Position = 0;
            (Script as StreamReader).DiscardBufferedData();

            string[] Result = Lines.ToArray();
            Lines = new List<string>();
            this.Prefix = new Dictionary<long, string>();
            Sufix = new Dictionary<long, string>();

            long Count = Result.LongLength;
            for (long i = 0; i < Count; i++) {
                string Line = Result[i];
                string[] Values = new string[] { "^\\", "^@", "^/", "^^", /*<== Double Chars | Single Char==>*/ "/", "\\", "@", "^" };
                Result[i] = InLineFilter(i, Line, Values);
            }
            return Result;
        }

        public void Export(TextWriter Out, string[] Lines) {
            string Prefix = ENG ? "langen" : "langjp";
            long ID = 0;
            while (Script.Peek() != -1) {
                string Line = Script.ReadLine();
                if (Line.ToLower().StartsWith(Prefix))
                    Out.WriteLine("{0}{1}{2}{3}", Prefix, this.Prefix[ID], Lines[ID].Replace("\\n", "^@^"), this.Sufix[ID++]);
                else
                    Out.WriteLine(Line);
            }

            //Reset Stream
            (Script as StreamReader).BaseStream.Position = 0;
            (Script as StreamReader).DiscardBufferedData();

            Out.Close();
        }
        private string InLineFilter(long ID, string line, string[] values = null) {
            string Prefix = string.Empty;
            string Sufix = string.Empty;
            for (int i = 0; i < values.Length; i++) {
                string Act = values[i];
                while (line.StartsWith(Act)) {
                    Prefix += Act;
                    line = line.Substring(Act.Length, line.Length - Act.Length);
                }
                while (line.EndsWith(Act)) {
                    Sufix = Act + Sufix;

                    line = line.Substring(0, line.Length - Act.Length);
                }
            }
            this.Prefix[ID] = Prefix;
            this.Sufix[ID] = Sufix;
            return line.Replace("^@^", "\\n");
        }
    }
}
