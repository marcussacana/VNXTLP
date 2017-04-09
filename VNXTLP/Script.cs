using System.Text;
using System.IO;

#region BuildImport
#if Eushully
using VNX.EushullyEditor;
#endif
#if KiriKiri
using KrKrSceneManager;
#endif
#if SteinsGate
using SGFilter;
#endif
#if ExHIBIT
using RLDManager;
#endif
#if KrKrFate
using KrKrFateFilter;
#endif
#endregion

namespace VNXTLP {
    internal static partial class Engine {
        internal static string[] Open(string Path, bool TempMode = false) {
            string[] ReturnContent;
            if (TempMode)
                Editor = BackupEditor;
            else {
                ScriptPath = Path;
                SetConfig("VNXTLP", "LastScript", ScriptPath);
            }
#if Eushully
            EushullyEditor Backup = Editor;
            Editor = new EushullyEditor(File.ReadAllBytes(Path), EditorConfig);
#if VNX && !Sankai
            Editor.SJISBase = new SJExt();
            Editor.LoadScript();
            Resources.MergeStrings(ref Editor, true);
#else
            Editor.LoadScript();
#endif
            string[] Out = new string[Editor.Strings.Length];
            for (int i = 0; i < Out.Length; i++)
                Out[i] = Editor.Strings[i].getString();
            if (TempMode)
                Editor = Backup;
            ReturnContent = Out;
#endif
#if KiriKiri
            PSBStringManager Backup = Editor;
            Editor = new PSBStringManager();
            byte[] Script = File.ReadAllBytes(Path);
            Status = Editor.GetPackgetStatus(Script);
            if (Status == PSBStringManager.PackgetStatus.Invalid) {
                if (TempMode)
                    BackupSectors = Sectors;
                Sector[] BackupSectors = Sectors;
                Sectors = TJS2SManager.Split(Script);
                for (int i = 0; i < Sectors.Length; i++)
                    if (Sectors[i].SectorType == SectorType.DATA)
                        MainSector = i;
                if (TempMode)
                    Sectors = BackupSectors;
                ReturnContent = TJS2SManager.GetContent(Sectors[MainSector]);
                    
            }
            Editor.Import(Script);
            ReturnContent = Editor.Strings;
#endif
#if SteinsGate
            string[] Script = File.ReadAllLines(Path, Encoding.UTF8);
            string cfg = GetConfig("SGFilter", "Level", false);
            if (string.IsNullOrEmpty(cfg)) {
                SetConfig("SGFilter", "Level", "2");
                Level = FullFilter.FilterLevel.Max;
            }
            if (cfg == "Max" || cfg == "2")
                Level = FullFilter.FilterLevel.Max;
            if (cfg == "Normal" || cfg == "1")
                Level = FullFilter.FilterLevel.Normal;
            if (cfg == "Low" || cfg == "0" || cfg == "off")
                Level = FullFilter.FilterLevel.Low;
            Editor = new FullFilter(Script, Level);
            ReturnContent = Editor.Import();
#endif
#if ExHIBIT
            if (new FileInfo(Path).Length < 0xFFCF) {
                Editor = new RLD(File.ReadAllBytes(Path));
                ReturnContent = Editor.Import();
            } else
                ReturnContent = new string[0];
#endif
#if Umineko
            System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("Pressione SIM para abrir o script em inglês\nPressione NÃO para abrir o script em Japonês", "Qual lingua deseja?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            ENG = dr == System.Windows.Forms.DialogResult.Yes;
            TextBox.InputLang = ENG ? "EN" : "JA";
            Editor = new NSFilter.Umineko(File.OpenText(Path), ENG);
            ReturnContent =  Editor.Import();
#endif
#if KrKrFate

            Editor = new KSFilter(Path);
            ReturnContent = Editor.Import();
#endif
            if (TempMode)
                Editor = BackupEditor;
            return ReturnContent;
        }

        internal static void Save(string Path, string[] Content) {
            ScriptPath = Path;
#if Eushully
            for (int i = 0; i < Content.Length; i++)
                Editor.Strings[i].setString(Content[i]);
            File.WriteAllBytes(Path, Editor.Export());
#endif
#if KiriKiri
            byte[] Script;
            if (Status == PSBStringManager.PackgetStatus.Invalid) {
                TJS2SManager.SetContent(ref Sectors[MainSector], Content);
                Script = TJS2SManager.Merge(Sectors);
            }
            else {
                Editor.Strings = Content;
                Script = Editor.Export();
            }
            File.WriteAllBytes(Path, Script);
#endif
#if SteinsGate
            string[] rst = Editor.Export(Content);
            string Result = string.Empty;
            for (uint i = 0; i < rst.Length; i++)
                Result += rst[i] + '\n';
            Result = Result.Substring(0, Result.Length - 1);
            File.WriteAllText(Path, Result, Encoding.UTF8);
#endif
#if ExHIBIT
            File.WriteAllBytes(ScriptPath, Editor.Export(Content));
#endif
#if Umineko
            if (File.Exists(ScriptPath))
                File.Delete(ScriptPath);
            Editor.Export(File.CreateText(ScriptPath), Content);
#endif
#if KrKrFate
            if (File.Exists(ScriptPath))
                File.Delete(ScriptPath);
            Editor.Export(ScriptPath, Content);
#endif
        }
    }
}
