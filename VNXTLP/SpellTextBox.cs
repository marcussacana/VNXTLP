using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TLIB;
using VNXTLP;

internal class SpellTextBox : RichTextBox {
#if !DEBUG
    private ContextMenuStrip CMS;
    private Hunspell SpellChecker = null;

    internal delegate void HintMessage(string Text);
    internal event HintMessage OnHintChanged;
    internal bool DictionaryLoaded { get { return SpellChecker != null; } }

    internal char[] WordSeparators = new char[] { ' ', ',', '.', '?', '!', ';', ':', '\'', '"', '―', '*', '<', '>' };

    internal bool PreprocessSuggestions = false;

    internal bool SpellCheckEnable = true;
    internal int ErrorCount { get; private set; } = 0;
    internal int HintCount { get; private set; } = 0;

    private Dictionary<string, VNXTLP.Result> QueryCache = new Dictionary<string, VNXTLP.Result>();
    private Dictionary<string, string[]> WordTL = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> PhraseCache = new Dictionary<string, string[]>();
    private Dictionary<string, bool> WordCache = new Dictionary<string, bool>();


    private string LEC_Port;
    private string LastSpellCheck = string.Empty;
    public  string InputLang;
    public  string TargetLang;

    private int PortStatus;
    private bool SuggestionLoaded = false;
    private bool PaintTerms = false;
    private bool GetOnlineSynonyms = false;
    private bool CustomDic = false;

    private  Suggestion[] TextSuggestions = new Suggestion[0];
    internal new Action[] TextChanged = new Action[0];

    private Timer WaitEnds = new Timer() { Interval = 600 };
    private DateTime LastEdit = DateTime.Now;
    private List<string> ManualDictionary = new List<string>();

    private string TLDicPath {
        get {
            string Cfg = Engine.GetConfig("VNXTLP", "CustomDicPath", false);
            return Cfg != string.Empty ? Cfg : AppDomain.CurrentDomain.BaseDirectory + "TLDic.ini";
        }
    }  
    
    private struct Suggestion {
        internal string Word;
        internal string Info;
        internal int Pos;
        internal int Len;
        internal List<string> Suggestions;
    }

    internal void BootUP() {
        try {
            InputLang = Engine.GetConfig("VNXTLP", "SourceLang", false).ToUpper();
            TargetLang = Engine.GetConfig("VNXTLP", "TargetLang", false).ToUpper();
            GetOnlineSynonyms = !Program.OfflineMode && (InputLang == "EN" || TargetLang == "EN");
            PaintTerms = Engine.GetConfig("VNXTLP", "AllwaysPaintTerms", false) == "true";
           
            WaitEnds.Tick += StopWaiter;
            MouseMove += MouseMoveEvent;
            MouseUp += MouseUpEvent;
            base.TextChanged += TextChangedEvent;

            CustomDic = Engine.GetConfig("VNXTLP", "CustomDic", false) == "true";
            if (CustomDic) {
                if (File.Exists(TLDicPath))
                    using (StreamReader SR = new StreamReader(TLDicPath, Encoding.UTF8)) {
                        while (SR.Peek() != -1) {
                            string TL = SR.ReadLine();
                            if (!TL.Contains("="))
                                continue;
                            bool IsCustomTerm = TL.Contains("==");
                            if (TL.Contains("===")) {
                                ManualDictionary.Add(TL.Split('=').First().ToLower());
                                continue;
                            }

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

                for (int i = 0; i < ManualDictionary.Count; i++) {
                    string WTL = string.Format("{0}==={1}", ManualDictionary[i], true);
                    SW.WriteLine(WTL);
                }

                SW.Close();
            }
        }
    }

    private const string IGNORE = "<ignore>";

    internal void ResetCache() {
        WordCache = new Dictionary<string, bool>();
        foreach (string Word in ManualDictionary)
            WordCache.Add(Word.ToLower(), true);
    }

    private void StopWaiter(object sender, EventArgs e) {
        if ((DateTime.Now - LastEdit).Milliseconds > 500) {
            WaitEnds.Enabled = false;
            BeginInvoke(new MethodInvoker(() => {
                SpellCheck();
            }));
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
        if (!IsLoaded())
            return;
        if (!SpellCheckEnable)
            return;
        if (Text == string.Empty) {
            WordCache = new Dictionary<string, bool>();
            LastSpellCheck = null;
        }

        if (LastSpellCheck == Text)
            return;
        else
            LastSpellCheck = Text;

        //Backup and Reset values
        ErrorCount = 0;
        SuggestionLoaded = false;
        TextSuggestions = new Suggestion[0];

        string[] Words = Text.Split(WordSeparators);

        this.ClearAllFormatting(Font);

        //SpellCheck
        for (int i = 0, WIndex = 0; i < Words.Length; WIndex += Words[i].Length, i++) {
            if (!WordCache.ContainsKey(Words[i].ToLower()))
                WordCache.Add(Words[i].ToLower(), SpellChecker.Spell(Words[i]));
            if (WordTL.ContainsKey(Words[i])) {
                if (WordTL[Words[i]].Length > 0 && WordTL[Words[i]][0] == IGNORE)
                    continue;
                ErrorCount++;
                WaveWord(WIndex + i, Words[i].Length);
            } else if (!WordCache[Words[i].ToLower()]) {
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
                            SelectionColor = Color.Green;
                        }
                    }
                }
            }

        if (!Program.OfflineMode) {
            new System.Threading.Thread((Str) => {
                try {
                    string Text = (string)Str;
                    VNXTLP.Result Result;
                    if (QueryCache.ContainsKey(Text))
                        Result = QueryCache[Text];
                    else {
                        Result = LanguageTool.Check(Text, TargetLang, ProxyHelper.Proxy);
                        QueryCache[Text] = Result;
                    }
                    HintCount = Result.matches.Length;
                    List<Suggestion> Suggetions = new List<Suggestion>();
                    foreach (Match match in Result.matches) {
                        int Pos = match.offset;
                        while (Text[Pos] == ' ')
                            Pos++;
                        int Len = match.length - (Pos - match.offset);
                        if (Len != match.length)
                            continue;
                        string Word = Text.Substring(Pos, Len);
                        if (WordCache.ContainsKey(Word) && WordCache[Word])
                            continue;
                        WaveWord(Pos, Len);
                        Suggetions.Add(new Suggestion() {
                            Word = Word,
                            Suggestions = (from x in match.replacements select x.value).ToList(),
                            Info = match.message,
                            Pos = Pos,
                            Len = Len
                        });
                    }
                    TextSuggestions = Suggetions.ToArray();
                } catch { }
            }).Start(Text);
        }
    }
    private void WaveWord(int Start, int Len) {
        if (InvokeRequired) {
            Invoke(new MethodInvoker(() => {
                WaveWord(Start, Len);
            }));
            return;
        }
        int OriStart = SelectionStart;
        int OriLen = SelectionLength;
        Select(Start, Len);
        this.SetUnderline(true);
        Select(OriStart, OriLen);
        Application.DoEvents();
        return;
    }
    void TextChangedEvent(object sender, EventArgs e) {
        int SS = SelectionStart;
        int SL = SelectionLength;
        foreach (Action act in TextChanged)
            act?.Invoke();
        Select(SS, SL);
        LastEdit = DateTime.Now;
        WaitEnds.Enabled = false;
        WaitEnds.Enabled = true;
    }

    private string DownloadTranslation(string word, string InputLang = null, string TargetLang = null) {
        if (InputLang == null || TargetLang == null) {
            InputLang = this.InputLang;
            TargetLang = this.TargetLang;
        }
        //0 = LEC
        //1 = Google
        //2 = Bing
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
                    } catch {
                        LEC_Port = string.Empty;
                    }
                PortStatus = LEC.ServerIsOpen(LEC_Port) ? 1 : -1;
            }

            if (PortStatus == 1) {
                try {
                    return LEC.Translate(word, InputLang, TargetLang, LEC.Gender.Male, LEC.Formality.Formal, LEC_Port);
                } catch { }
            }
        } else if (TLClient == "1") {
            try {
                return Google.Translate(word, InputLang, TargetLang);
            } catch { }
        } else if (TLClient == "2") {
            try {
                return Bing.Translate(word, InputLang, TargetLang);
            } catch { }
        }
        return word;
    }

    void MouseMoveEvent(object sender, MouseEventArgs e) {
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
            if (HintCount > 0 && SpellCheckEnable) {
                int index = GetCharIndexFromPosition((e).Location);
                Suggestion Suggestion = (from x in TextSuggestions where index >= x.Pos && index <= x.Pos + x.Len select x).FirstOrDefault();

                while (index < Text.Length && WordSeparators.Contains(Text[index]))
                    index++;//Fix Rigth
                while (index - 1 >= 0 && !WordSeparators.Contains(Text[index - 1]))
                    index--;//Fix Left

                if (index < Text.Length) {
                    string Word = string.Empty;
                    int len = 0;
                    while (index + len < Text.Length && !WordSeparators.Contains(Text[index + len])) {
                        Word += Text[index + len];
                        len++;
                    }
                    if (ManualDictionary.Contains(Word.ToLower()))
                        Suggestion = new Suggestion();
                }

                if (Suggestion.Len != 0) {
                    OnHintChanged?.Invoke(Suggestion.Info);
                } else {
                    OnHintChanged?.Invoke(null);
                }
            } else {
                OnHintChanged?.Invoke(null);
            }
        }
        catch { }
    }

    internal void LoadDictionary(string Dic) {
        if (!File.Exists(Dic + ".aff") || !File.Exists(Dic + ".dic"))
            return;
        SpellChecker = Hunspell.GetHunspell(Dic);
    }
    void MouseUpEvent(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Right && SpellCheckEnable) {
            try {
                int index = GetCharIndexFromPosition((e).Location);
                Suggestion Hint = (from x in TextSuggestions where index >= x.Pos && index <= x.Pos + x.Len select x).FirstOrDefault();

                while (index < Text.Length && WordSeparators.Contains(Text[index]))
                    index++;//Fix Rigth
                while (index - 1 >= 0 && !WordSeparators.Contains(Text[index - 1]))
                    index--;//Fix Left

                if (index < Text.Length) {
                    string Word = string.Empty;
                    int len = 0;
                    while (index + len < Text.Length && !WordSeparators.Contains(Text[index + len])) {
                        Word += Text[index + len];
                        len++;
                    }
                    bool isWrong = false;
                    //Get Manual Suggestion, Tl Suggestion
                    bool CantSugest = false;
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
                                WordTL.Add(Word, new string[] { TL });
                                Suggestions = new List<string>();
                                Suggestions.Add(TL);
                            } else
                                CantSugest = (TL != null && TL == Word && TargetLang == "EN");
                        }
                    }

                    //Get Dic suggestions
                    if (Suggestions == null)
                        if (SuggestionLoaded) {
                            foreach (Suggestion Suggestion in TextSuggestions)
                                if (Suggestion.Word == Word)
                                    Suggestions = Suggestion.Suggestions;
                            if (Suggestions == null && !CantSugest)
                                return;
                            isWrong = true;
                        } else {
                            if (SpellChecker.Spell(Word) && !CantSugest) {
                                CantSugest = true;
                            } else {
                                Suggestions = SpellChecker.Suggest(Word);
                                isWrong = true;
                            }
                        }

                    CMS = new ContextMenuStrip();
                    bool Blank = true;
                    if (!CantSugest) {
                        for (int i = 0; i < Suggestions.Count; i++) {
                            WordMeuItem MI = new WordMeuItem();
                            MI.Word = Word;
                            MI.Text = Suggestions[i];
                            MI.Index = index;
                            MI.Length = len;
                            MI.Click += MenuItem_Click;
                            CMS.Items.Add(MI);
                            Blank = false;
                        }
                    }

                    if (Hint.Len != 0) {
                        for (int i = 0; i < Hint.Suggestions.Count; i++) {
                            WordMeuItem MI = new WordMeuItem();
                            MI.Word = Hint.Word;
                            MI.Text = Hint.Suggestions[i];
                            MI.Index = Hint.Pos;
                            MI.Length = Hint.Len;
                            MI.Click += MenuItem_Click;
                            CMS.Items.Add(MI);
                            Blank = false;
                        }
                    }

                    if (!CantSugest) {
                        if (CMS.Items.Count == 0)
                            CMS.Items.Add(new ToolStripMenuItem(Engine.LoadTranslation(Engine.TLID.NoSuggestions)) { Enabled = false });

                        if (isWrong) {
                            CMS.Items.Add(new ToolStripSeparator());
                            ToolStripMenuItem NWItem = new ToolStripMenuItem();
                            NWItem.Text = Engine.LoadTranslation(Engine.TLID.AddOnDictionary);
                            NWItem.Name = Word;
                            NWItem.Click += AddToDic;
                            CMS.Items.Add(NWItem);
                        }
                    }

                    if (WordCache.ContainsKey(Word.ToLower()) && WordCache[Word.ToLower()]) {
                        CantSugest = true;
                        Blank = true;
                        CMS = new ContextMenuStrip();
                    }

                    WordMeuItem SSM = new WordMeuItem();
                    SSM.Text = Engine.LoadTranslation(Engine.TLID.ShowSynonyms);
                    SSM.Word = Word;
                    SSM.Location = e.Location;
                    SSM.Click += ShowSynounyms;
                    if (GetOnlineSynonyms) {
                        if (!Blank && !(!CantSugest && isWrong))
                            CMS.Items.Add(new ToolStripSeparator());
                        CMS.Items.Add(SSM);
                    } else {
                        if (CantSugest && Blank)
                            return;
                    }

                    CMS.Show(this, e.Location);
                }
            } catch {
            }
        }
    }

    private void ShowSynounyms(object sender, EventArgs e) {
        WordMeuItem MI = (WordMeuItem)sender;
        MI.Word = MI.Word.ToLower();
        string TL = DownloadTranslation(MI.Word).ToLower();
        bool CanGiveSynounyms = !string.IsNullOrEmpty(TL);

        if (CanGiveSynounyms) {
            string cnt = Engine.LoadTranslation(Engine.TLID.LoadingSuggetionsPlzWait);
            //Asyc Download Suggestions
            BallonToolTip Ballon = new BallonToolTip();

            bool IsFromInputLang = TL != MI.Word;
            string Word = MI.Word;
            bool BRWord = false;
            if (IsFromInputLang && InputLang != "EN") {
                if (InputLang.Split('-')[0].ToUpper() == "PT")
                    BRWord = true;
                else
                    Word = DownloadTranslation(Word, InputLang, "EN");
            }
            if (!IsFromInputLang && TargetLang != "EN") {
                if (TargetLang.Split('-')[0].ToUpper() == "PT")
                    BRWord = true;
                else
                    Word = DownloadTranslation(Word, TargetLang, "EN");
            }

            if (Word.Contains(" ") && !BRWord) {
                foreach (string w in Word.Split(' ')) {
                    string TTL = string.Empty;
                    if (IsFromInputLang && InputLang != "EN") {
                        TTL = DownloadTranslation(w, "EN", InputLang);
                    }
                    if (!IsFromInputLang && TargetLang != "EN") {
                        TTL = DownloadTranslation(w, "EN", TargetLang);
                    }
                    if (MI.Word.ToLower().StartsWith(Word.ToLower()))
                        Word = TTL;
                }
            }

            string BRResult = string.Empty;
            string[] Suggestions = new string[0];
            System.Threading.Thread Thread = new System.Threading.Thread(() => {
                try {
                    if (BRWord) {
                        BRResult = BRSynonymous.SearchWord(Word);
                        return;
                    } else
                        Suggestions = WordAPI.DownloadSynonyms(Word);
                } catch { }
                if (Suggestions == null)
                    Suggestions = new string[0];
                if (!IsFromInputLang) {
                    for (int i = 0; i < Suggestions.Length; i++)
                        Suggestions[i] = DownloadTranslation(Suggestions[i], "EN", TargetLang);
                }
            });
            
            Timer ti = new Timer();
            ti.Interval = 500;
            ti.Tick += (sdr, ev) => {
                if (Thread.ThreadState == System.Threading.ThreadState.Stopped) {
                    ti.Enabled = false;
                    if (Suggestions == null && string.IsNullOrEmpty(BRResult)) {
                        Ballon = new BallonToolTip();
                        Ballon.Title = Engine.LoadTranslation(Engine.TLID.NoSynonymsFound);
                        Ballon.Message = Engine.LoadTranslation(Engine.TLID.TryManuallySearch);
                        Engine.UpdateToolTip(Ballon, true);
                        return;
                    }
                    string WordList = string.Empty;
                    if (BRWord) {
                        WordList = BRResult;
                    } else {
                        foreach (string Suggestion in Suggestions)
                            if (!string.IsNullOrWhiteSpace(Suggestion))
                                WordList += Suggestion + ", ";
                        if (string.IsNullOrWhiteSpace(WordList))
                            WordList = @"  ";
                        WordList = WordList.Substring(0, WordList.Length - 2);
                    }
                    Ballon = new BallonToolTip();
                    Ballon.Title = string.IsNullOrWhiteSpace(WordList) ? Engine.LoadTranslation(Engine.TLID.NoSynonymsFound) : string.Format(Engine.LoadTranslation(Engine.TLID.SynonymsFor), MI.Word);
                    Ballon.Message = string.IsNullOrWhiteSpace(WordList) ? Engine.LoadTranslation(Engine.TLID.TryManuallySearch) : WordList;
                    Engine.UpdateToolTip(Ballon, true);
                }
            };

            Engine.ShowToolTip(MI.Location, cnt, string.Format(Engine.LoadTranslation(Engine.TLID.SynonymsFor), MI.Word), Ballon, true, this);
            Thread.Start();
            ti.Enabled = true;
        } else {
            if (MessageBox.Show(Engine.LoadTranslation(Engine.TLID.FailedToDetectWordLang), "VNXTLP", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Retry)
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
        WordCache[bnt.Name.ToLower()] = true;
        if (!ManualDictionary.Contains(bnt.Name.ToLower())) {
            ManualDictionary.Add(bnt.Name.ToLower());
            WaitEnds.Enabled = false;
            WaitEnds.Enabled = true;
        }
    }
    
#endif
#if DEBUG
    internal SpellTextBox(){
    }
#endif
}

//From: https://stackoverflow.com/questions/1268009/reset-rtf-in-richtextbox
static class RichTextExtensions {
    public static void ClearAllFormatting(this RichTextBox te, Font font) {
        CHARFORMAT2 fmt = new CHARFORMAT2();

        fmt.cbSize = Marshal.SizeOf(fmt);
        fmt.dwMask = CFM_ALL2;
        fmt.dwEffects = CFE_AUTOCOLOR | CFE_AUTOBACKCOLOR;
        fmt.szFaceName = font.FontFamily.Name;

        double size = font.Size;
        size /= 72;//logical dpi (pixels per inch)
        size *= 1440.0;//twips per inch

        fmt.yHeight = (int)size;//165
        fmt.yOffset = 0;
        fmt.crTextColor = 0;
        fmt.bCharSet = 1;// DEFAULT_CHARSET;
        fmt.bPitchAndFamily = 0;// DEFAULT_PITCH;
        fmt.wWeight = 400;// FW_NORMAL;
        fmt.sSpacing = 0;
        fmt.crBackColor = 0;
        //fmt.lcid = ???
        fmt.dwMask &= ~CFM_LCID;//don't know how to get this...
        fmt.dwReserved = 0;
        fmt.sStyle = 0;
        fmt.wKerning = 0;
        fmt.bUnderlineType = 0;
        fmt.bAnimation = 0;
        fmt.bRevAuthor = 0;
        fmt.bReserved1 = 0;

        SendMessage(te.Handle, EM_SETCHARFORMAT, SCF_ALL, ref fmt);
    }

    public static void SetUnderline(this RichTextBox te, bool Underline) {
        var CharFormat = new CHARFORMAT2();
        CharFormat.cbSize = Marshal.SizeOf(CharFormat);
        SendMessage(te.Handle, EM_GETCHARFORMAT, SCF_SELECTION, ref CharFormat);


        CHARFORMAT2 fmt = new CHARFORMAT2();
        fmt.cbSize = Marshal.SizeOf(fmt);
        fmt.dwMask = CFM_UNDERLINETYPE;
        fmt.bUnderlineType = Underline ? CFU_UNDERLINEWAVE : CFU_UNDERLINENONE;
        SendMessage(te.Handle, EM_SETCHARFORMAT, SCF_SELECTION, ref fmt);
    }

    public static bool IsUnderlined(this RichTextBox te) {
        var CharFormat = new CHARFORMAT2();
        CharFormat.cbSize = Marshal.SizeOf(CharFormat);
        SendMessage(te.Handle, EM_GETCHARFORMAT, SCF_SELECTION, ref CharFormat);
        return ((CharFormat.dwMask & CFM_UNDERLINETYPE) != 0 && CharFormat.bUnderlineType == CFU_UNDERLINEWAVE);
    }

    private const UInt32 WM_USER = 0x0400;
    private const UInt32 EM_GETCHARFORMAT = (WM_USER + 58);
    private const UInt32 EM_SETCHARFORMAT = (WM_USER + 68);
    private const UInt32 SCF_ALL = 0x0004;
    private const UInt32 SCF_SELECTION = 0x0001;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, ref CHARFORMAT2 lParam);

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    struct CHARFORMAT2 {
        public int cbSize;
        public uint dwMask;
        public uint dwEffects;
        public int yHeight;
        public int yOffset;
        public int crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szFaceName;
        public short wWeight;
        public short sSpacing;
        public int crBackColor;
        public int lcid;
        public int dwReserved;
        public short sStyle;
        public short wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
        public byte bReserved1;
    }

    #region CFE_
    // CHARFORMAT effects 
    const UInt32 CFE_BOLD = 0x0001;
    const UInt32 CFE_ITALIC = 0x0002;
    const UInt32 CFE_UNDERLINE = 0x0004;
    const UInt32 CFE_STRIKEOUT = 0x0008;
    const UInt32 CFE_PROTECTED = 0x0010;
    const UInt32 CFE_LINK = 0x0020;
    const UInt32 CFE_AUTOCOLOR = 0x40000000;            // NOTE: this corresponds to 
    // CFM_COLOR, which controls it 
    // Masks and effects defined for CHARFORMAT2 -- an (*) indicates
    // that the data is stored by RichEdit 2.0/3.0, but not displayed
    const UInt32 CFE_SMALLCAPS = CFM_SMALLCAPS;
    const UInt32 CFE_ALLCAPS = CFM_ALLCAPS;
    const UInt32 CFE_HIDDEN = CFM_HIDDEN;
    const UInt32 CFE_OUTLINE = CFM_OUTLINE;
    const UInt32 CFE_SHADOW = CFM_SHADOW;
    const UInt32 CFE_EMBOSS = CFM_EMBOSS;
    const UInt32 CFE_IMPRINT = CFM_IMPRINT;
    const UInt32 CFE_DISABLED = CFM_DISABLED;
    const UInt32 CFE_REVISED = CFM_REVISED;



    // CFE_AUTOCOLOR and CFE_AUTOBACKCOLOR correspond to CFM_COLOR and
    // CFM_BACKCOLOR, respectively, which control them
    const UInt32 CFE_AUTOBACKCOLOR = CFM_BACKCOLOR;
    #endregion
    #region CFU_
    const byte CFU_UNDERLINEHEAVYWAVE = 0x0C;
    const byte CFU_UNDERLINEDOUBLEWAVE = 0x0B;
    const byte CFU_UNDERLINEWAVE = 0x08;
    const byte CFU_UNDERLINENONE = 0x00;
    #endregion
    #region CFM_
    // CHARFORMAT masks 
    const UInt32 CFM_BOLD = 0x00000001;
    const UInt32 CFM_ITALIC = 0x00000002;
    const UInt32 CFM_UNDERLINE = 0x00000004;
    const UInt32 CFM_STRIKEOUT = 0x00000008;
    const UInt32 CFM_PROTECTED = 0x00000010;
    const UInt32 CFM_LINK = 0x00000020;         // Exchange hyperlink extension 
    const UInt32 CFM_SIZE = 0x80000000;
    const UInt32 CFM_COLOR = 0x40000000;
    const UInt32 CFM_FACE = 0x20000000;
    const UInt32 CFM_OFFSET = 0x10000000;
    const UInt32 CFM_CHARSET = 0x08000000;

    const UInt32 CFM_SMALLCAPS = 0x0040;            // (*)  
    const UInt32 CFM_ALLCAPS = 0x0080;          // Displayed by 3.0 
    const UInt32 CFM_HIDDEN = 0x0100;           // Hidden by 3.0 
    const UInt32 CFM_OUTLINE = 0x0200;          // (*)  
    const UInt32 CFM_SHADOW = 0x0400;           // (*)  
    const UInt32 CFM_EMBOSS = 0x0800;           // (*)  
    const UInt32 CFM_IMPRINT = 0x1000;          // (*)  
    const UInt32 CFM_DISABLED = 0x2000;
    const UInt32 CFM_REVISED = 0x4000;

    const UInt32 CFM_BACKCOLOR = 0x04000000;
    const UInt32 CFM_LCID = 0x02000000;
    const UInt32 CFM_UNDERLINETYPE = 0x00800000;        // Many displayed by 3.0 
    const UInt32 CFM_WEIGHT = 0x00400000;
    const UInt32 CFM_SPACING = 0x00200000;      // Displayed by 3.0 
    const UInt32 CFM_KERNING = 0x00100000;      // (*)  
    const UInt32 CFM_STYLE = 0x00080000;        // (*)  
    const UInt32 CFM_ANIMATION = 0x00040000;        // (*)  
    const UInt32 CFM_REVAUTHOR = 0x00008000;

    const UInt32 CFE_SUBSCRIPT = 0x00010000;        // Superscript and subscript are 
    const UInt32 CFE_SUPERSCRIPT = 0x00020000;      //  mutually exclusive           

    const UInt32 CFM_SUBSCRIPT = (CFE_SUBSCRIPT | CFE_SUPERSCRIPT);
    const UInt32 CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

    // CHARFORMAT "ALL" masks
    const UInt32 CFM_EFFECTS = (CFM_BOLD | CFM_ITALIC | CFM_UNDERLINE | CFM_COLOR |
                         CFM_STRIKEOUT | CFE_PROTECTED | CFM_LINK);
    const UInt32 CFM_ALL = (CFM_EFFECTS | CFM_SIZE | CFM_FACE | CFM_OFFSET | CFM_CHARSET);

    const UInt32 CFM_EFFECTS2 = (CFM_EFFECTS | CFM_DISABLED | CFM_SMALLCAPS | CFM_ALLCAPS
                        | CFM_HIDDEN | CFM_OUTLINE | CFM_SHADOW | CFM_EMBOSS
                        | CFM_IMPRINT | CFM_DISABLED | CFM_REVISED
                        | CFM_SUBSCRIPT | CFM_SUPERSCRIPT | CFM_BACKCOLOR);

    const UInt32 CFM_ALL2 = (CFM_ALL | CFM_EFFECTS2 | CFM_BACKCOLOR | CFM_LCID
                        | CFM_UNDERLINETYPE | CFM_WEIGHT | CFM_REVAUTHOR
                        | CFM_SPACING | CFM_KERNING | CFM_STYLE | CFM_ANIMATION);
    #endregion
    
}