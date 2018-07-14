using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        internal static string GetConfig(string Key, string Name, bool Required = true) {
#if DEBUG            
            return "null string";
#else
            string[] Lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Settings.ini", Encoding.UTF8);
            string AtualKey = string.Empty;
            foreach (string Line in Lines) {
                if (Line.StartsWith("[") && Line.EndsWith("]"))
                    AtualKey = Line.Substring(1, Line.Length - 2);
                if (Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                    continue;
                string AtualName = Line.Split('=')[0];
                string Value = Line.Split('=')[1];
                if (AtualName == Name && AtualKey == Key)
                    return Value;
            }
            if (!Required)
                return string.Empty;
            MessageBox.Show(string.Format(LoadTranslation(TLID.FailedToLoadSetting), Name));
            Application.Exit();
            Application.ExitThread();
            throw new Exception("Config Error");
#endif
        }

        internal static void SetConfig(string Key, string Name, string Value) {
#if !DEBUG
            string cfgfile = AppDomain.CurrentDomain.BaseDirectory + "Settings.ini";
            ConfigStatus cfg = GetConfigStatus(Key, Name);
            string[] Lines = File.ReadAllLines(cfgfile, Encoding.UTF8);
            string AtualKey = string.Empty;
            if (cfg == ConfigStatus.Ok) {
                for (int i = 0; i < Lines.Length; i++) {
                    string Line = Lines[i];
                    if (Line.StartsWith("[") && Line.EndsWith("]"))
                        AtualKey = Line.Substring(1, Line.Length - 2);
                    if (Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                        continue;
                    string AtualName = Line.Split('=')[0];
                    if (AtualKey == Key && Name == AtualName) {
                        Lines[i] = string.Format("{0}={1}", Name, Value);
                        break;
                    }
                }
            }
            if (cfg == ConfigStatus.NoName) {
                List<string> Cfgs = new List<string>();
                int KeyPos = 0;
                for (int i = 0; i < Lines.Length; i++) {
                    if (string.Format("[{0}]", Key) == Lines[i])
                        KeyPos = i;
                    Cfgs.Add(Lines[i]);
                }
                Cfgs.Insert(KeyPos + 1, string.Format("{0}={1}", Name, Value));
                Lines = Cfgs.ToArray();
            }
            if (cfg == ConfigStatus.NoKey) {
                string[] NewLines = new string[Lines.Length + 3];
                Lines.CopyTo(NewLines, 0);
                NewLines[Lines.Length + 1] = string.Format("[{0}]", Key);
                NewLines[Lines.Length + 2] = string.Format("{0}={1}", Name, Value);
                Lines = NewLines;
            }
            File.WriteAllLines(cfgfile, Lines, Encoding.UTF8);
#endif
        }

        internal enum ConfigStatus {
            NoKey, NoName, Ok
        }
        internal static ConfigStatus GetConfigStatus(string Key, string Name) {
#if DEBUG
            return ConfigStatus.NoKey;
#else
            string[] Lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Settings.ini", Encoding.UTF8);
            bool KeyFound = false;
            string AtualKey = string.Empty;
            foreach (string Line in Lines) {
                if (Line.StartsWith("[") && Line.EndsWith("]"))
                    AtualKey = Line.Substring(1, Line.Length - 2);
                if (AtualKey == Key)
                    KeyFound = true;
                if (Line.StartsWith("!") || string.IsNullOrWhiteSpace(Line) || !Line.Contains("="))
                    continue;

                string AtualName = Line.Split('=')[0];
                if (AtualName == Name && AtualKey == Key)
                    return ConfigStatus.Ok;
            }
            return KeyFound ? ConfigStatus.NoName : ConfigStatus.NoKey;
#endif
        }
    }
}