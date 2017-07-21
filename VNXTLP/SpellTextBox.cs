using NHunspell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TLIB;
using VNXTLP;

internal class SpellTextBox : RichTextBox {
#if !DEBUG
    private ContextMenuStrip CMS;
    private Hunspell SpellChecker;    
    internal bool DictionaryLoaded { get { return !Equals(SpellChecker, default(Hunspell)); } }

    internal char[] WordSeparators = new char[] { ' ', ',', '.', '?', '!', ';', ':', '\'', '"', '―', '*', '<', '>' };

    internal bool PreprocessSuggestions = false;

    internal bool SpellCheckEnable = true;
    internal int ErrorCount { get; private set; } = 0;
    private bool SuggestionLoaded = false;
    private Suggestion[] TextSuggestions = new Suggestion[0];
    private Dictionary<string, string[]> WordTL = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> PhraseCache = new Dictionary<string, string[]>();
    private string LEC_Port;
    private int PortStatus;
    private DateTime LastEdit = DateTime.Now;
    private Timer WaitEnds = new Timer() { Interval = 600 };
    private Dictionary<string, bool> WordCache = new Dictionary<string, bool>();
    private bool SpellChecking = false;
    private bool PaintTerms = false;

    internal string InputLang;
    internal string TargetLang;
    private bool GetOnlineSynonyms = false;
    private string TLDicPath {
        get {
            string Cfg = Engine.GetConfig("VNXTLP", "CustomDicPath", false);
            return Cfg != string.Empty ? Cfg : AppDomain.CurrentDomain.BaseDirectory + "TLDic.ini";
        }
    }

    internal new Action[] TextChanged = new Action[0];
    
    private bool CustomDic = false;
    
    
    private struct Suggestion {
        internal string Word;
        internal List<string> Suggestions;
    }
    internal void BootUP() {
        try {
            InputLang = Engine.GetConfig("VNXTLP", "SourceLang", false);
            TargetLang = Engine.GetConfig("VNXTLP", "TargetLang", false);
            GetOnlineSynonyms = !Program.OfflineMode && (InputLang == "EN" || TargetLang == "EN");
            PaintTerms = Engine.GetConfig("VNXTLP", "AllwaysPaintTerms", false) == "true";

            WaitEnds.Tick += StopWaiter;
            CustomDic = Engine.GetConfig("VNXTLP", "CustomDic", false) == "true";
            if (CustomDic) {
                if (File.Exists(TLDicPath))
                    using (StreamReader SR = new StreamReader(TLDicPath, Encoding.UTF8)) {
                        while (SR.Peek() != -1) {
                            string TL = SR.ReadLine();
                            if (!TL.Contains("="))
                                continue;
                            bool IsCustomTerm = TL.Contains("==");
                            string[] WTL = TL.Split('=');
                            string[] TLS = WTL[IsCustomTerm ? 2 : 1].Split(';');
                            if (IsCustomTerm)
                                PhraseCache.Add(WTL[0].ToLower(), TLS);
                             else
                                WordTL.Add(WTL[0].ToLower(), TLS);
                        }
                        SR.Close();
                    }
            }
        }
        catch { }
    }

    internal void SaveWords() {
        if (CustomDic) {
            using (StreamWriter SW = new StreamWriter(TLDicPath, false, Encoding.UTF8)) {
                for (int i = 0; i < WordTL.Count; i++) {
                    string List = string.Empty;
                    foreach (string TL in WordTL.Values.ElementAt(i)) {
                        List += TL + ";";
                    }
                    List = List.Substring(0, List.Length - 1);
                    string WTL = string.Format("{0}={1}", WordTL.Keys.ElementAt(i), List);
                    SW.WriteLine(WTL);
                }
                for (int i = 0; i < PhraseCache.Count; i++) {
                    string List = string.Empty;
                    foreach (string Term in PhraseCache.Values.ElementAt(i)) {
                        List += Term + ";";
                    }
                    List = List.Substring(0, List.Length - 1);
                    string WTL = string.Format("{0}=={1}", PhraseCache.Keys.ElementAt(i), List);
                    SW.WriteLine(WTL);
                }
                SW.Close();
            }
        }
    }

    private const string IGNORE = "<ignore>";

    internal void ResetCache() {
        WordCache = new Dictionary<string, bool>();
    }

    private void StopWaiter(object sender, EventArgs e) {
        if ((DateTime.Now - LastEdit).Milliseconds > 500) {
            WaitEnds.Enabled = false;
            SpellCheck();
        }
    }
    private bool IsLoaded() {
        try {
            int i = SelectionStart;
            return true;
        }
        catch {
            return false;
        }
    }
    private void SpellCheck() {
        if (Text == string.Empty)
            WordCache = new Dictionary<string, bool>();
        if (!IsLoaded())
            return;
        if (SpellChecking || !SpellCheckEnable)
            return;
        SpellChecking = true;

        //Backup and Reset values
        int SS = SelectionStart;
        int SL = SelectionLength;
        ErrorCount = 0;
        SuggestionLoaded = false;
        TextSuggestions = new Suggestion[0];

        string[] Words = Text.Split(WordSeparators);

        //Reformat All Text
        string tmp = Text;
        Clear();
        Text = tmp;
        //SetColorTable();

        //SpellCheck
        for (int i = 0, WIndex = 0; i < Words.Length; WIndex += Words[i].Length, i++) {
            if (!WordCache.ContainsKey(Words[i]))
                WordCache.Add(Words[i], SpellChecker.Spell(Words[i]));
            if (WordTL.ContainsKey(Words[i])) {
                if (WordTL[Words[i]].Length > 0 && WordTL[Words[i]][0] == IGNORE)
                    continue;
                ErrorCount++;
                WaveWord(WIndex + i, Words[i].Length);
            } else if (!WordCache[Words[i]]) {
                ErrorCount++;
                WaveWord(WIndex + i, Words[i].Length);
            } 
        }

        foreach (string Phrase in PhraseCache.Keys)
            if (Text.ToLower().Contains(Phrase)) {
                string lt = Text.ToLower();
                for (int i = 0; i < Text.Length; i++) {
                    if (i + Phrase.Length > Text.Length)
                        break;
                    string sub = lt.Substring(i, Phrase.Length);
                    if (sub == Phrase) {
                        ErrorCount++;
                        WaveWord(i, Phrase.Length);
                        if (PaintTerms) {
                            Select(i, Phrase.Length);
                            SelectionColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
        Select(SS, SL);
        SpellChecking = false;
    }
    /*
    private void SetColorTable() {
        //Generate ColorTable
        const string ColorMask = "\\red{0}\\green{1}\\blue{2};";
        string DefColor = string.Format(ColorMask, BaseTB.ForeColor.R, BaseTB.ForeColor.G, BaseTB.ForeColor.B);
        string ErrColor = string.Format(ColorMask, 255, 0, 0);
        string HinColor = string.Format(ColorMask, 0, 255, 0);
        string ColorTable = DefColor + ErrColor + HinColor;

        //Update or Insert ColorTable
        string RTF = BaseTB.Rtf;
        int TableStart = RTF.IndexOf("colortbl;");
        if (TableStart != -1) {
            int TableEnd = RTF.IndexOf('}', TableStart);
            RTF = RTF.Remove(TableStart, TableEnd - TableStart);
            RTF = RTF.Insert(TableStart, "colortbl;" + ColorTable + "}");
        } else {
            int RTFLoc = RTF.IndexOf("\\rtf");
            int InsertLoc = RTF.IndexOf('{', RTFLoc);
            if (InsertLoc == -1)
                InsertLoc = RTF.IndexOf('}', RTFLoc) - 1;
            RTF = RTF.Insert(InsertLoc, "{\\colortbl;" + ColorTable + "}");
        }
        BaseTB.Rtf = RTF;
    }*/
    private void WaveWord(int Start, int Len) {
        Select(Start, Len);

        string RTF = SelectedRtf.Replace("\r\n", "");
        int LastTag = RTF.IndexOf("\\fs") + 3;
        while (char.IsNumber(RTF[LastTag]))
            LastTag++;

        int StringPos = LastTag;
        if (RTF[StringPos] == ' ')
            StringPos++;

        string Word = RTF.Substring(StringPos, RTF.Length - StringPos - 1);
        string SubRTF = RTF.Substring(0, LastTag);

        SubRTF += "\\ulwave " + Word + "\\ulnone}";
        SelectedRtf = SubRTF;
    }
    protected override void OnTextChanged(EventArgs e) {
        if (SpellChecking)
            return;
        int SS = SelectionStart;
        int SL = SelectionLength;
        foreach (Action act in TextChanged)
            act?.Invoke();
        Select(SS, SL);
        WaitEnds.Enabled = true;
        LastEdit = DateTime.Now;
    }

    private string DownloadTranslation(string word) {
        //0 = LEC
        //1 = Google
        //3 = Both
        string TLClient = Engine.GetConfig("VNXTLP", "TLClient", false).ToLower();
        if (Program.OfflineMode && TLClient != "0" || TLClient == "off")
            return word;
        if (TLClient == "0") {
            if (PortStatus != 1 && PortStatus != -1) {
                LEC_Port = Engine.GetConfig("VNXTLP", "LEC-Port", false);
                if (LEC_Port == "auto")
                    try {
                        LEC_Port = LEC.TryDiscoveryPort();
                    }
                    catch {
                        LEC_Port = string.Empty;
                    }
                PortStatus = LEC.ServerIsOpen(LEC_Port) ? 1 : -1;
            }

            if (PortStatus == 1) {
                try {
                    return LEC.Translate(word, InputLang, TargetLang, LEC.Gender.Male, LEC.Formality.Formal, LEC_Port);
                }
                catch { }
            }
        } else if (TLClient == "1") {

            try {
                return Google.Translate(word, InputLang, TargetLang);
            }
            catch { }
        }
        return word;
    }

    protected override void OnMouseMove(MouseEventArgs e) {
        try {
            if (ErrorCount > 0 && PreprocessSuggestions && !SuggestionLoaded && DictionaryLoaded && SpellCheckEnable) {
                SuggestionLoaded = true;
                TextSuggestions = new Suggestion[ErrorCount];
                string[] Words = Text.Split(WordSeparators);
                for (int i = 0, s = 0, len = 0; i < Words.Length; i++) {
                    if (i + len + Words[i].Length != SelectionStart && !SpellChecker.Spell(Words[i])) {
                        TextSuggestions[s++] = new Suggestion() {
                            Word = Words[i],
                            Suggestions = SpellChecker.Suggest(Words[i])
                        };
                    }
                    len += Words[i].Length;
                }
            }
        }
        catch { }
        base.OnMouseMove(e);
    }

    internal void LoadDictionary(string Affix, string Dic) {
        if (!File.Exists(Affix) || !File.Exists(Dic))
            return;
        SpellChecker = new Hunspell(Affix, Dic);
    }
    protected override void OnMouseUp(MouseEventArgs e) {
        if (e.Button == MouseButtons.Right && SpellCheckEnable) {
            try {
                int index = GetCharIndexFromPosition((e).Location);
                while (index < Text.Length && !char.IsLetter(Text[index]))
                    index++;//Fix Rigth
                while (index - 1 >= 0 && char.IsLetter(Text[index - 1]))
                    index--;//Fix Left

                if (index < Text.Length) {
                    string Word = string.Empty;
                    int len = 0;
                    while (index + len < Text.Length && char.IsLetter(Text[index + len])) {
                        Word += Text[index + len];
                        len++;
                    }
                    bool isWrong = false;
                    //Get Manual Suggestion, Tl Suggestion
                    bool Syounym = false;
                    List<string> Suggestions = null;
                    if (WordTL.ContainsKey(Word.ToLower())) {
                        Suggestions = new List<string>();
                        string[] TLS = WordTL[Word.ToLower()];
                        if (TLS.Length > 0 && TLS[0] == IGNORE)
                            return;
                        foreach (string TL in TLS)
                            Suggestions.Add(TL);
                    } else {
                        bool Contains = false;
                        foreach (string Phrase in PhraseCache.Keys)
                            if (Text.ToLower().Contains(Phrase)) {
                                int PhraseIndex = Text.ToLower().IndexOf(Phrase);
                                if (PhraseIndex <= index && index <= PhraseIndex + Phrase.Length) {
                                    Contains = true;
                                    index = PhraseIndex;
                                    len = Phrase.Length;
                                    Word = Phrase;
                                    Suggestions = new List<string>();
                                    foreach (string sugg in PhraseCache[Word])
                                        Suggestions.Add(sugg);
                                    break;
                                }
                            }
                        if (!Contains) {
                            string TL = DownloadTranslation(Word);
                            if (TL != null && TL != Word) {
                                string[] Synounyms = new string[0];
                                if (GetOnlineSynonyms)
                                    Synounyms = WordAPI.DownloadSynonyms(Word);
                                WordTL.Add(Word, new string[] { TL });
                                Suggestions = new List<string>();
                                Suggestions.Add(TL);
                            } else
                                Syounym = (TL != null && TL == Word && TargetLang == "EN");
                        }
                    }

                    //Get Dic suggestions
                    if (Suggestions == null)
                        if (SuggestionLoaded) {
                            foreach (Suggestion Suggestion in TextSuggestions)
                                if (Suggestion.Word == Word)
                                    Suggestions = Suggestion.Suggestions;
                            if (Suggestions == null && !Syounym)
                                return;
                            isWrong = true;
                        } else {
                            if (SpellChecker.Spell(Word) && !Syounym)
                                return;
                            Suggestions = SpellChecker.Suggest(Word);
                            isWrong = true;
                        }

                    CMS = new ContextMenuStrip();
                    if (!Syounym) {
                        for (int i = 0; i < Suggestions.Count; i++) {
                            WordMeuItem MI = new WordMeuItem();
                            MI.Word = Word;
                            MI.Text = Suggestions[i];
                            MI.Index = index;
                            MI.Length = len;
                            MI.Click += MenuItem_Click;
                            CMS.Items.Add(MI);
                        }

                        if (CMS.Items.Count == 0)
                            //CMS.Items.Add(new ToolStripMenuItem("Sem Sugestões") { Enabled = false });
                            CMS.Items.Add(new ToolStripMenuItem(Engine.LoadTranslation(56)) { Enabled = false });

                        if (isWrong) {
                            CMS.Items.Add(new ToolStripSeparator());
                            ToolStripMenuItem NWItem = new ToolStripMenuItem();
                            //NWItem.Text = "Adicionar ao Dicionário";
                            NWItem.Text = Engine.LoadTranslation(57);
                            NWItem.Name = Word;
                            NWItem.Click += AddToDic;
                            CMS.Items.Add(NWItem);
                        }
                    }
                    WordMeuItem SSM = new WordMeuItem();
                    //SSM.Text = "Mostrar Sinônimos";
                    SSM.Text = Engine.LoadTranslation(87);
                    SSM.Word = Word;
                    SSM.Location = e.Location;
                    SSM.Click += ShowSynounyms;
                    if (GetOnlineSynonyms) {
                        if (!isWrong)
                            CMS.Items.Add(new ToolStripSeparator());
                        CMS.Items.Add(SSM);
                    }

                    CMS.Show(this, e.Location);
                }
            } catch {
            }
        }
        base.OnMouseUp(e);
    }

    private void ShowSynounyms(object sender, EventArgs e) {
        WordMeuItem MI = (WordMeuItem)sender;
        string TL = DownloadTranslation(MI.Word);
        if (!string.IsNullOrEmpty(TL) && ((TL != MI.Word && InputLang == "EN") || TL == MI.Word && TargetLang == "EN")) {
            string cnt = Engine.LoadTranslation(90);
            //Asyc Download Suggestions
            BallonToolTip Ballon = new BallonToolTip();

            string[] Suggestions = new string[0];
            System.Threading.Thread Thread = new System.Threading.Thread(() => {
                Suggestions = WordAPI.DownloadSynonyms(MI.Word);
            });

            Timer ti = new Timer();
            ti.Interval = 500;
            ti.Tick += (sdr, ev) => {
                if (Thread.ThreadState == System.Threading.ThreadState.Stopped) {
                    ti.Enabled = false;
                    string WordList = string.Empty;
                    if (Suggestions == null) {
                        Ballon = new BallonToolTip();
                        Ballon.Title = Engine.LoadTranslation(91);
                        Ballon.Message = Engine.LoadTranslation(92);
                        Engine.UpdateToolTip(Ballon, true);
                        return;
                    }
                    foreach (string Suggestion in Suggestions)
                        if (!string.IsNullOrWhiteSpace(Suggestion))
                            WordList += Suggestion + ", ";
                    if (string.IsNullOrWhiteSpace(WordList))
                        WordList = @"  ";
                    WordList = WordList.Substring(0, WordList.Length - 2);
                    Ballon = new BallonToolTip();
                    Ballon.Title = string.IsNullOrWhiteSpace(WordList) ? Engine.LoadTranslation(91) : string.Format(Engine.LoadTranslation(89), MI.Word);
                    Ballon.Message = string.IsNullOrWhiteSpace(WordList) ? Engine.LoadTranslation(92) : WordList;
                    Engine.UpdateToolTip(Ballon, true);
                }
            };

            Engine.ShowToolTip(MI.Location, cnt, string.Format(Engine.LoadTranslation(89), MI.Word), Ballon, true, this);
            Thread.Start();
            ti.Enabled = true;
        } else {
            if (MessageBox.Show(Engine.LoadTranslation(88), "VNXTLP", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Retry)
                ShowSynounyms(sender, e);
            return;
        }
    }


    private void MenuItem_Click(object sender, EventArgs e) {
        WordMeuItem bnt = (WordMeuItem)sender;
        SelectionStart = bnt.Index;
        SelectionLength = bnt.Length;
        SelectedText = bnt.Text;
    }

    private void AddToDic(object sender, EventArgs e) {
        ToolStripMenuItem bnt = (ToolStripMenuItem)sender;
        SpellChecker.Add(bnt.Name);
    }
    
#endif
#if DEBUG
    internal SpellTextBox(){
    }
#endif
}