using System;
using System.Windows.Forms;

namespace VNXTLP {

    internal partial class NoStyle : Form {
        Engine.RadioToolStrip RadioEngine;
        Engine.RadioToolStrip ThemeEngine;
        Engine.RadioToolStrip TLEngine;
        Engine.RadioToolStrip SelEngine;

        Engine.OverTimerEvent OVE;
        internal bool FileOpen = false;
        internal bool FileSaved = true;

        internal int SelectedIndex = -1;
        internal int Index {
            get
            {
                if (delimitarAvançoToolStripMenuItem.Checked)
                    SkipDelay.Enabled = true;
                return StrList.SelectedIndex < 0 ? 0 : StrList.SelectedIndex;
            }
            set
            {
                Engine.UpdateInfo(value, ref InfoLbl, GetBackupFrequence(), ref Changes, IndexTestEnableMenuItem.Checked);
                SelectedIndex = StrList.SelectedIndex;
            }
        }

        internal SpellTextBox TLBox;

        private int Changes = 0;
        internal string[] RefScript;
        internal NoStyle() {
            InitializeComponent();
#if !DEBUG
            //Initialize TLBOX
            TLBox = new SpellTextBox();
            MainPanel.Controls.Add(TLBox);
            TLBox.Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right);
            TLBox.Enabled = false;
            TLBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            TLBox.Location = new System.Drawing.Point(Scroll.Location.X, Scroll.Location.Y - (Scroll.Size.Height + 5));
            TLBox.Margin = new Padding(3, 2, 3, 2);
            TLBox.Multiline = false;
            TLBox.Size = new System.Drawing.Size(Scroll.Size.Width, 26);

            TLBox.SizeChanged += new EventHandler(TLBox_SizeChanged);
            Engine.Append(ref TLBox.TextChanged, TLBox_TextChanged);
            TLBox.KeyDown += new KeyEventHandler(TLBox_KeyDown);

            //Initialize RadioToolStrip Engine 
            ToolStripMenuItem[] BackupItems = new ToolStripMenuItem[] { BackupOnSaveItem, BackupOn200MenuItem, BackupOn100MenuItem, BackupOn50Item, BackupOn25Item, BackupOn10Item, NeverBackupItem };
            RadioEngine = new Engine.RadioToolStrip(ref BackupItems, 3);

            ToolStripMenuItem[] ThemeItems = new ToolStripMenuItem[] { BasicThemeMenuItem, ModernThemeMenuItem, bernToolStripMenuItem };
            ThemeEngine = new Engine.RadioToolStrip(ref ThemeItems, 1);
            ThemeEngine.CheckedChange += ChangeTheme;

            ToolStripMenuItem[] TLCLients = new ToolStripMenuItem[] { lECToolStripMenuItem, googleToolStripMenuItem, zBingToolStripMenuItem };
            TLEngine = new Engine.RadioToolStrip(ref TLCLients, 1);
            TLEngine.CheckedChange += TLEngine_CheckedChange;

            ToolStripMenuItem[] SelItems = new ToolStripMenuItem[] { AutomaticoToolStripMenuItem, asiaticaToolStripMenuItem, latimToolStripMenuItem };
            SelEngine = new Engine.RadioToolStrip(ref SelItems, 0);
            SelEngine.CheckedChange += SelEngine_CheckedChange;

            //Initialize DeleyedMouseOver Event
            OVE = new Engine.OverTimerEvent() {
                sender = StrList
            };
            OVE.MouseStopOver += StrList_MouseStopOver;
            OVE.Initialize();

            //Set File Filter
            OpenScript.Filter = Engine.Filter;
            SaveScript.Filter = Engine.Filter;

            //Initalize SpellCheck Engine
            TLBox.LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "Dictionary");
            TLBox.BootUP();

            //Initialize Config
            SpellCheckEnableMenuItem.Checked = Engine.GetConfig("VNXTLP", "SpellCheck", false).ToLower() == "true";
            TLBox.SpellCheckEnable = SpellCheckEnableMenuItem.Checked;
            IndexTestEnableMenuItem.Checked = Engine.GetConfig("VNXTLP", "AutoJump", false).ToLower() == "true";
            altaResoluçãoToolStripMenuItem.Checked = Engine.GetConfig("VNXTLP", "HighFont", false).ToLower() == "true";
            altoContrasteToolStripMenuItem.Checked = Engine.GetConfig("VNXTLP", "BlackTheme", false).ToLower() == "true";
            delimitarAvançoToolStripMenuItem.Checked = Engine.GetConfig("VNXTLP", "SkipDelay", false).ToLower() == "true";
            modoDinâmicoToolStripMenuItem.Checked = Engine.GetConfig("VNXTLP", "DynamicMode", false).ToLower() == "true";


            //get int
            string cfg = Engine.GetConfig("VNXTLP", "BackupSpeed", false);
            int Val = 0;
            if (int.TryParse(cfg, out Val))
                RadioEngine.SelectedIndex = Val;

            //get int
            cfg = Engine.GetConfig("VNXTLP", "SelMode", false);
            if (int.TryParse(cfg, out Val))
                SelEngine.SelectedIndex = Val;

            //get int
            cfg = Engine.GetConfig("VNXTLP", "TLClient", false);
            if (int.TryParse(cfg, out Val))
                TLEngine.SelectedIndex = Val;
            else if (cfg == "off")
                sistemaDeTraduçãoToolStripMenuItem.Visible = false;

            //Load Translation
            SkipBnt.Text = Engine.LoadTranslation(Engine.TLID.Next);
            RetBnt.Text = Engine.LoadTranslation(Engine.TLID.Back);
            arquivoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.File);
            OpenItem.Text = Engine.LoadTranslation(Engine.TLID.Open);
            SaveAsItem.Text = Engine.LoadTranslation(Engine.TLID.SaveAs);
            TLAccMenuItem.Text = Engine.LoadTranslation(Engine.TLID.MyAccount);
            seleçãoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Selection);
            SelectAll.Text = Engine.LoadTranslation(Engine.TLID.SelectAll);
            UnselectAll.Text = Engine.LoadTranslation(Engine.TLID.UnselectAll);
            AutoSelect.Text = Engine.LoadTranslation(Engine.TLID.AutoSelect);
            opçõesToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Options);
            temaToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Theme);
            BasicThemeMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Basic);
            ModernThemeMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Modern);
            períodoDeBackupToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.BackupFrequence);
            BackupOnSaveItem.Text = Engine.LoadTranslation(Engine.TLID.OnSave);
            BackupOn50Item.Text = Engine.LoadTranslation(Engine.TLID.BackOn50);
            BackupOn25Item.Text = Engine.LoadTranslation(Engine.TLID.BackOn25);
            BackupOn10Item.Text = Engine.LoadTranslation(Engine.TLID.BackOn10);
            NeverBackupItem.Text = Engine.LoadTranslation(Engine.TLID.Never);
            SpellCheckEnableMenuItem.Text = Engine.LoadTranslation(Engine.TLID.SpellChecking);
            IndexTestEnableMenuItem.Text = Engine.LoadTranslation(Engine.TLID.ValidateIndex);
            OpenScript.Title = Engine.LoadTranslation(Engine.TLID.SelectAScript);
            SaveScript.Title = Engine.LoadTranslation(Engine.TLID.SelectAScript);
            pesquisaToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.SearchOrReplace);
            sistemaDeTraduçãoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.TranslationSystem);
            lECToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.LEC);
            googleToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Google);
            RefScriptMenuItem.Text = Engine.LoadTranslation(Engine.TLID.ReferenceScript);
            altoContrasteToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.HighContrast);
            altaResoluçãoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.HighResolution);
            SelecaoAutomaticaMenuItem1.Text = Engine.LoadTranslation(Engine.TLID.SelectMode);
            AutomaticoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.AutoDetect);
            asiaticaToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Asian);
            latimToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Latim);
            SaveItem.Text = Engine.LoadTranslation(Engine.TLID.Save);
            delimitarAvançoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.LimitSkip);
            modoDinâmicoToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.DynamicMode);
            outrasopçõesmenuitem.Text = Engine.LoadTranslation(Engine.TLID.MoreOptions);
            salvarEstadoDaJanelaToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.SaveWindowState);

            //Special Items
            foreach (ToolStripMenuItem item in Engine.CustomResources(ref TLBox))
                MainMenu.Items.Add(item);

            Engine.LoadWindowState(this);
#endif
        }

        private void SelEngine_CheckedChange(object sender, EventArgs e) {
            Engine.SetConfig("VNXTLP", "SelMode", SelEngine.SelectedIndex.ToString());
            Engine.AutoSelect();
        }

        private void TLEngine_CheckedChange(object sender, EventArgs e) {
            Engine.SetConfig("VNXTLP", "TLClient", TLEngine.SelectedIndex.ToString());
        }

        private void ChangeTheme(object sender, EventArgs e) {
            if (ModernThemeMenuItem.Checked)
                Engine.ChangeTheme("Modern");
            else if (bernToolStripMenuItem.Checked)
                Engine.ChangeTheme("Bern");
        }

        private void OpenItem_Click(object sender, EventArgs e) {
            OpenScript.ShowDialog();
        }

        private void SaveItem_Click(object sender, EventArgs e) {
            if (!FileOpen) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BeforeSaveOpenAScript), "VNX+ Translation Plataform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveScript.ShowDialog();
        }

        private void Sroll_ValueChanged(object sender, EventArgs e) {
            //Update TextCursor  ~ Force the cursor jump to end of the text if the scroll value is the max
            int i = 0;
            while (i < TLBox.Text.Length && Engine.TextWidth(TLBox.Text.Substring(0, i++), TLBox.Font) < Scroll.Value)
                continue;
            int ni = 0;
            while (ni < TLBox.Text.Length && Engine.TextWidth(TLBox.Text.Substring(0, ni++), TLBox.Font) < Scroll.Value + Scroll.LargeChange)
                continue;
            if (ni == TLBox.Text.Length)
                i = ni;
            TLBox.Select(i, 0);
            TLBox.Focus();
        }

        int Widht;

        private void TLBox_TextChanged() {
            //Update the ScrollBox Values;
            Widht = Engine.TextWidth(TLBox.Text, TLBox.Font);
            Scroll.Visible = Widht > TLBox.Width;
            Scroll.Maximum = Widht;
            Scroll.SmallChange = Widht / 4;
            Scroll.LargeChange = Widht / 3;
        }
        private void OpenScript_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            Text = "VNX+ Translation Plataform - " + System.IO.Path.GetFileName(OpenScript.FileName);
            FileOpen = true;
            string[] StringList = Engine.Open(OpenScript.FileName);

            //Clear Update Strings
            StrList.Items.Clear();
            foreach (string String in StringList)
                StrList.Items.Add(String);
            Engine.StartSelction();
            TLBox.Enabled = true;
            FileSaved = true;
        }

        private void SaveScript_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            //Generate String Array 
            string[] StringList = new string[StrList.Items.Count];
            for (int i = 0; i < StringList.Length; i++)
                StringList[i] = StrList.Items[i].ToString();

            //Backup if needed
            if (!Program.OfflineMode && (!(NeverBackupItem.Checked && Engine.DebugMode)))
                (new System.Threading.Thread((str) => { Engine.Backup(StringList, true); })).Start(StringList);
            
            //Save Script
            Engine.Save(SaveScript.FileName, StringList);
            FileSaved = true;
            Engine.UpdateSelection();
            MessageBox.Show(Engine.LoadTranslation(Engine.TLID.SaveAsSucess), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void TLBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (SkipDelay.Enabled)
                    return;

                //Stop "Ding" Sound
                e.Handled = true;
                e.SuppressKeyPress = true;

                //Update translation and get next string
                StrList.Items[SelectedIndex] = TLBox.Text;
                Engine.FinishString(TLBox.Text);

                FileSaved = false;
                Changes++;
                Index = SelectedIndex + 1;
            }

            if (!SkipDelay.Enabled) {
                if (e.KeyCode == Keys.Up) {
                    //Stop "Ding" Sound
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    RetBnt_Click(null, null);
                }
                if (e.KeyCode == Keys.Down) {
                    //Stop "Ding" Sound
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    SkipBnt_Click(null, null);
                }
            }
        }

        private int GetBackupFrequence() {
            int SI = RadioEngine.SelectedIndex;
            switch (SI) {
                case 1:
                    return 200;
                case 2:
                    return 100;
                case 3:
                    return 50;
                case 4:
                    return 25;
                case 5:
                    return 10;

                default:
                    if (!Engine.DebugMode)
                        goto case 1;
                    return -1;
            }
        }

        private void SkipBnt_Click(object sender, EventArgs e) {
            if (FileOpen)
                Index++;
        }

        private void RetBnt_Click(object sender, EventArgs e) {
            if (FileOpen)
                Index--;
        }

        private void StrList_SelectedIndexChanged(object sender, EventArgs e) {
            if (modoDinâmicoToolStripMenuItem.Checked && StrList.SelectedIndex >= 0) {
                Index = StrList.SelectedIndex;
                TLBox.Focus();
            }
            
            SkipBnt.Enabled = Index != StrList.Items.Count - 1;
            RetBnt.Enabled = Index > 0;
        }

        private void TLBox_SizeChanged(object sender, EventArgs e) {
            TLBox_TextChanged();
        }

        private void AutoSelect_Click(object sender, EventArgs e) {
            Engine.AutoSelect();
        }

        private void SelectAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < StrList.Items.Count; i++)
                StrList.SetItemChecked(i, true);
        }

        private void UnselectAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < StrList.Items.Count; i++)
                StrList.SetItemChecked(i, false);
        }

        private void StrList_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r')
                Index = StrList.SelectedIndex;
        }

        private void SpellCheck_Changed(object sender, EventArgs e) {
            TLBox.SpellCheckEnable = SpellCheckEnableMenuItem.Checked;
        }

        private void TLAccMenuItem_Click(object sender, EventArgs e) {
            if (!Engine.Authenticated)
                (new NoStyleLogin()).ShowDialog();
            else {
                NoStyleBackup BackupForm = new NoStyleBackup();
                BackupForm.BackupSelected += BackupForm_BackupSelected;
                BackupForm.ShowDialog();
            }
        }

        private void BackupForm_BackupSelected(object sender, EventArgs e) {
            string[] BackupStrings = (string[])sender;
            if (BackupStrings.Length != StrList.Items.Count) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.WrongBackup), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < BackupStrings.Length; i++)
                StrList.Items[i] = BackupStrings[i];
            Index = 0; //Update Textbox
            MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BackupLoaded), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NoStyle_FormClosing(object sender, FormClosingEventArgs e) {
            if (Engine.UploadingBackup) {
                e.Cancel = true;
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.WaitBackupUpload), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!FileSaved) {
                DialogResult dr = MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoScriptOpen), "VNXTLP", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes) {
                    SaveItem_Click(null, null);
                    if (!FileSaved) {
                        e.Cancel = true;
                        return;
                    }
                }
                if (dr == DialogResult.Cancel) {
                    e.Cancel = true;
                    return;
                }
            }
            if (!e.Cancel)
                TLBox.SaveWords();

            Engine.SetConfig("VNXTLP", "DynamicMode", modoDinâmicoToolStripMenuItem.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "SkipDelay", delimitarAvançoToolStripMenuItem.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "SpellCheck", TLBox.SpellCheckEnable ? "true" : "false");
            Engine.SetConfig("VNXTLP", "AutoJump", IndexTestEnableMenuItem.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "BlackTheme", altoContrasteToolStripMenuItem.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "HighFont", altaResoluçãoToolStripMenuItem.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "LastScript", Engine.ScriptPath + "|" + Index);

            if (GetBackupFrequence() > 0)
                Engine.SetConfig("VNXTLP", "BackupSpeed", RadioEngine.SelectedIndex.ToString());

            if (Engine.ServerStatus == Engine.Commands.Running) {
                DateTime WaitBegin = DateTime.Now;
                Engine.ServerStatus = Engine.Commands.Closing;
                while (Engine.ServerStatus != Engine.Commands.Closed && (DateTime.Now - WaitBegin).Seconds > 3) {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
            }

            Environment.Exit(0);
        }
        internal string GetStr(int i) {
            return Index == i ? TLBox.Text : StrList.Items[i].ToString();
        }

        internal void SetStr(int i, string Content) {
            if (Index == i)
                TLBox.Text = Content;
            else
                StrList.Items[i] = Content;
        }        
        private void StrList_MouseStopOver(object sender, MouseEventArgs e) {
            if (!RefScriptMenuItem.Checked)
                return;
            int i = StrList.IndexFromPoint(e.Location);
            string Reference = RefScript[i];
            Engine.ShowToolTip(Engine.LocationCalc(e.Location, 0, (Cursor.Current.Size.Height / 2)), Reference, Engine.LoadTranslation(Engine.TLID.Reference));
        }

        private void StrList_MouseEnter(object sender, EventArgs e) {
            OVE.Over = true;
        }

        private void StrList_MouseLeave(object sender, EventArgs e) {
            OVE.Over = false;
        }

        private void StrList_MouseMove(object sender, MouseEventArgs e) {
            OVE.Point = e.Location;
        }

        private void ZScriptRef_Click(object sender, EventArgs e) {
            if (!FileOpen) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoScriptOpen), "VNX+ Translation Plataform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RefScriptMenuItem.Checked = !RefScriptMenuItem.Checked;
            if (RefScriptMenuItem.Checked) {
                OpenFileDialog FD = new OpenFileDialog {
                    Filter = Engine.Filter,
                    Title = Engine.LoadTranslation(Engine.TLID.SelectAReferenceScript)
                };
                if (FD.ShowDialog() == DialogResult.OK) {
                    string[] Script = Engine.Open(FD.FileName, true);
                    if (Script.Length != StrList.Items.Count)
                        MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BadReferenceScript), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    else {
                        RefScript = Script;
                        return;
                    }
                }
                RefScriptMenuItem.Checked = false;
            }
        }

        private void AltoContrasteToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            if (altoContrasteToolStripMenuItem.Checked) {
                StrList.BackColor = System.Drawing.Color.Black;
                StrList.ForeColor = System.Drawing.Color.Green;
            }
            else {
                StrList.BackColor = System.Drawing.SystemColors.Window;
                StrList.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void AltaResoluçãoToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            if (altaResoluçãoToolStripMenuItem.Checked) {
                StrList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f);
            } else {
                StrList.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
            }
        }

        private void PesquisaToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Program.SearchOpen)
                return;
            Program.SearchForm = new Search();
            Program.SearchForm.Show();
        }

        private void NoStyle_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.Control | Keys.O:
                    e.SuppressKeyPress = true;
                    OpenItem_Click(null, null);
                    break;
                case Keys.Control | Keys.S:
                    e.SuppressKeyPress = true;
                    FastSave(null, null);
                    break;
                case Keys.Control | Keys.Shift| Keys.S:
                    e.SuppressKeyPress = true;
                    SaveItem_Click(null, null);
                    break;
                case Keys.Control | Keys.F:
                    PesquisaToolStripMenuItem_Click(null, null);
                    break;
            }
        }

        private void NoStyle_Shown(object sender, EventArgs e) {
            try {
                string cfg = Engine.GetConfig("VNXTLP", "LastScript", false);
                string File = cfg.Split('|')[0];
                string Index = cfg.Split('|')[1];

                if (System.IO.File.Exists(File)) {
                    OpenScript.FileName = File;
                    OpenScript_FileOk(null, null);

                    int ID = int.Parse(Index);
                    if (ID > 0 && ID < StrList.Items.Count)
                        this.Index = ID;
                }
            }
            catch { }
        }

        private void DelayEnd(object sender, EventArgs e) {
            SkipDelay.Enabled = false;
        }

        private void FastSave(object sender, EventArgs e) {
            if (!FileOpen)
                return;
            SaveScript.FileName = Engine.ScriptPath;
            SaveScript_FileOk(null, null);
        }

        private void salvarEstadoDaJanelaToolStripMenuItem_Click(object sender, EventArgs e) {
            Engine.SaveWindowState(this);
        }
    }

}
