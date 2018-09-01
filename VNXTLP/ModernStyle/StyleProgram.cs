using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VNXTLP.NewStyle {
    internal partial class StyleProgram : Form {

        Engine.RadioToolStrip RadioEngine;
        Engine.RadioToolStrip ThemeEngine;
        Engine.RadioToolStrip TLEngine;
        Engine.RadioToolStrip SelEngine;
        Engine.OverTimerEvent OVE;

        public SpellTextBox TLBox;

        internal bool FileOpen = false;
        internal bool FileSaved = true;

        internal int SelectedIndex = -1;

        public override string Text { get { return base.Text; } set { base.Text = value; ZSKN.Text = value; Invalidate(); } }
        internal int Index {
            get
            {
                if (ZLimiteAvanco.Checked)
                    SkipDelay.Enabled = true;
                return StrList.SelectedIndex < 0 ? 0 : StrList.SelectedIndex;
            }
            set
            {
                Engine.UpdateInfo(value, ref InfoLbl, GetBackupFrequence(), ref Changes, ZValidar.Checked);
                SelectedIndex = StrList.SelectedIndex;
            }
        }

        private int Changes = 0;
        internal string[] RefScript;
        internal StyleProgram() {
            InitializeComponent();

            #region SpeelTextBox
            //Create TLBox
            TLBox = new SpellTextBox();
            ZSKN.Controls.Add(TLBox);
            TLBox.Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right);
            TLBox.BorderStyle = BorderStyle.None;
            TLBox.Location = new System.Drawing.Point(13, 296);
            TLBox.Name = "TLBox";
            TLBox.Size = new System.Drawing.Size(620, 20);
            TLBox.TabIndex = 13;
            TLBox.Visible = true;
            TLBox.Enabled = false;
            TLBox.Multiline = false;
            TLBox.BringToFront();

            //Initialize Events
            Engine.Append(ref TLBox.TextChanged, TLBox_TextChanged);
            TLBox.KeyDown += new KeyEventHandler(TLBox_KeyDown);
            TLBox.Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right);
            ZTextBox.GotFocus += (sender, e) => { TLBox.Focus(); };

            #endregion

            #region RadioToolStrip

            //Initialize RadioToolStrip Engine 
            ToolStripMenuItem[] BackupItems = new ToolStripMenuItem[] { ZAoSalvar, Z200Dialogos, Z100Dialogos, Z50Dialogos, Z25Dialogos, Z10Dialogos, ZNunca };
            RadioEngine = new Engine.RadioToolStrip(ref BackupItems, 2);

            ToolStripMenuItem[] ThemeItems = new ToolStripMenuItem[] { ZBasico, ZModerno, ZBernMenu };
            ThemeEngine = new Engine.RadioToolStrip(ref ThemeItems, 1);
            ThemeEngine.CheckedChange += ChangeTheme;

            ToolStripMenuItem[] TLCLients = new ToolStripMenuItem[] { ZLEC, ZGoogle, ZBing };
            TLEngine = new Engine.RadioToolStrip(ref TLCLients, 1);
            TLEngine.CheckedChange += TLEngine_CheckedChange;

            ToolStripMenuItem[] SelItems = new ToolStripMenuItem[] { ZAutoSelMode, ZAsianSel, ZLatimSel };
            SelEngine = new Engine.RadioToolStrip(ref SelItems, 0);
            SelEngine.CheckedChange += SelEngine_CheckedChange;

            //Initialize DeleyedMouseOver Event
            OVE = new Engine.OverTimerEvent() {
                sender = StrList
            };
            OVE.MouseStopOver += StrList_MouseStopOver;
            OVE.Initialize();

            #endregion

            //Set File Filter
            OpenScript.Filter = Engine.Filter;
            SaveScript.Filter = Engine.Filter;

            //Initalize TLBox
            TLBox.Font = ZTextBox.Font;
            TLBox.LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "Dictionary");
            TLBox.BootUP();

            //Initialize Config
            ZVerificacao.Checked = Engine.GetConfig("VNXTLP", "SpellCheck", false).ToLower() == "true";
            TLBox.SpellCheckEnable = ZVerificacao.Checked;
            ZValidar.Checked = Engine.GetConfig("VNXTLP", "AutoJump", false).ToLower() == "true";
            ZAltaRel.Checked = Engine.GetConfig("VNXTLP", "HighFont", false).ToLower() == "true";
            ZAltoContraste.Checked = Engine.GetConfig("VNXTLP", "BlackTheme", false).ToLower() == "true";
            ZLimiteAvanco.Checked = Engine.GetConfig("VNXTLP", "SkipDelay", false).ToLower() == "true";
            ZModoDianmico.Checked = Engine.GetConfig("VNXTLP", "DynamicMode", false).ToLower() == "true";

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
                ZTLClient.Visible = false;

            //Load Translation
            ZContinue.Text = Engine.LoadTranslation(Engine.TLID.Next);
            ZReturn.Text = Engine.LoadTranslation(Engine.TLID.Back);
            ZArquivo.Text = Engine.LoadTranslation(Engine.TLID.File);
            ZAbrir.Text = Engine.LoadTranslation(Engine.TLID.Open);
            ZSaveAsItem.Text = Engine.LoadTranslation(Engine.TLID.SaveAs);
            ZMinhaConta.Text = Engine.LoadTranslation(Engine.TLID.MyAccount);
            ZSelecao.Text = Engine.LoadTranslation(Engine.TLID.Selection);
            ZSelecionarTodos.Text = Engine.LoadTranslation(Engine.TLID.SelectAll);
            ZDesselecionarTodos.Text = Engine.LoadTranslation(Engine.TLID.UnselectAll);
            ZSelecaoAutomatica.Text = Engine.LoadTranslation(Engine.TLID.AutoSelect);
            ZOpcoes.Text = Engine.LoadTranslation(Engine.TLID.Options);
            ZTema.Text = Engine.LoadTranslation(Engine.TLID.Theme);
            ZBasico.Text = Engine.LoadTranslation(Engine.TLID.Basic);
            ZModerno.Text = Engine.LoadTranslation(Engine.TLID.Modern);

            ZPeriodo.Text = Engine.LoadTranslation(Engine.TLID.BackupFrequence);
            ZAoSalvar.Text = Engine.LoadTranslation(Engine.TLID.OnSave);
            Z50Dialogos.Text = Engine.LoadTranslation(Engine.TLID.BackOn50);
            Z25Dialogos.Text = Engine.LoadTranslation(Engine.TLID.BackOn25);
            Z10Dialogos.Text = Engine.LoadTranslation(Engine.TLID.BackOn10);
            ZNunca.Text = Engine.LoadTranslation(Engine.TLID.Never);
            ZVerificacao.Text = Engine.LoadTranslation(Engine.TLID.SpellChecking);
            ZValidar.Text = Engine.LoadTranslation(Engine.TLID.ValidateIndex);
            OpenScript.Title = Engine.LoadTranslation(Engine.TLID.SelectAScript);
            SaveScript.Title = Engine.LoadTranslation(Engine.TLID.SelectAScript);
            ZPesquisa.Text = Engine.LoadTranslation(Engine.TLID.SearchOrReplace);
            ZTLClient.Text = Engine.LoadTranslation(Engine.TLID.TranslationSystem);
            ZLEC.Text = Engine.LoadTranslation(Engine.TLID.LEC);
            ZGoogle.Text = Engine.LoadTranslation(Engine.TLID.Google);
            ZScriptRef.Text = Engine.LoadTranslation(Engine.TLID.ReferenceScript);
            ZAltoContraste.Text = Engine.LoadTranslation(Engine.TLID.HighContrast);
            ZAltaRel.Text = Engine.LoadTranslation(Engine.TLID.HighResolution);
            ZSelMode.Text = Engine.LoadTranslation(Engine.TLID.SelectMode);
            ZAutoSelMode.Text = Engine.LoadTranslation(Engine.TLID.AutoDetect);
            ZAsianSel.Text = Engine.LoadTranslation(Engine.TLID.Asian);
            ZLatimSel.Text = Engine.LoadTranslation(Engine.TLID.Latim);
            ZSaveAsItem.Text = Engine.LoadTranslation(Engine.TLID.SaveAs);
            ZLimiteAvanco.Text = Engine.LoadTranslation(Engine.TLID.LimitSkip);
            ZModoDianmico.Text = Engine.LoadTranslation(Engine.TLID.DynamicMode);
            ZOtherOptions.Text = Engine.LoadTranslation(Engine.TLID.MoreOptions);
            ZSaveWindowState.Text = Engine.LoadTranslation(Engine.TLID.SaveWindowState);
            ZSaveItem.Text = Engine.LoadTranslation(Engine.TLID.Save);

            //Load Custom Resources from a VNXTL Build
            foreach (ToolStripMenuItem item in Engine.CustomResources(ref TLBox))
                ZMenu.Items.Add(item);

            Engine.LoadWindowState(this);
        }

        private void SelEngine_CheckedChange(object sender, EventArgs e) {
            Engine.SetConfig("VNXTLP", "SelMode", SelEngine.SelectedIndex.ToString());
            Engine.AutoSelect();
        }

        private void TLEngine_CheckedChange(object sender, EventArgs e) {
            Engine.SetConfig("VNXTLP", "TLClient", TLEngine.SelectedIndex.ToString());
        }

        private void ChangeTheme(object sender, EventArgs e) {
            if (ZBasico.Checked)
                Engine.ChangeTheme("Basic");
            else if (ZBernMenu.Checked)
                Engine.ChangeTheme("Bern");
        }

        private void ZAbrir_Click(object sender, EventArgs e) {
            OpenScript.ShowDialog();
        }

        private void ZSalvar_Click(object sender, EventArgs e) {
            if (!FileOpen) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BeforeSaveOpenAScript), "VNX+ Translation Platform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveScript.ShowDialog();
        }

        private void OpenScript_FileOk(object sender, CancelEventArgs e) {
            Text = "VNX+ Translation Platform - " + System.IO.Path.GetFileName(OpenScript.FileName);
            string[] StringList = Engine.Open(OpenScript.FileName);
            FileOpen = true;
            FileSaved = true;

            //Update Strings
            StrList.Items.Clear();
            StrList.Items.AddRange(StringList);


            Engine.StartSelction();
            TLBox.Enabled = true;
            TLBox.ReadOnly = false;
        }

        private void SaveScript_FileOk(object sender, CancelEventArgs e) {
            //Generate String Array 
            string[] StringList = new string[StrList.Items.Count];
            for (int i = 0; i < StringList.Length; i++)
                StringList[i] = StrList.Items[i].ToString();

            //Backup if needed
            if (!Program.OfflineMode && (!(ZNunca.Checked && Engine.DebugMode)))
                (new System.Threading.Thread((str) => { Engine.Backup(StringList, true); })).Start(StringList);

            //Save Script
            Engine.Save(SaveScript.FileName, StringList);
            FileSaved = true;
            Engine.UpdateSelection();
            MessageBox.Show(Engine.LoadTranslation(Engine.TLID.SaveAsSucess, System.IO.Path.GetFileName(Engine.ScriptPath)), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ZReturn_Click(object sender, EventArgs e) {
            if (FileOpen)
                Index--;
        }

        private void ZContinue_Click(object sender, EventArgs e) {
            if (!FileOpen)
                return;
            Index++;
        }

        private void StrList_SelectedIndexChanged(object sender, EventArgs e) {
            if (ZModoDianmico.Checked && StrList.SelectedIndex >= 0) {
                Index = StrList.SelectedIndex;
                TLBox.Focus();
            }

            ZContinue.Enabled = Index != StrList.Items.Count - 1;
            ZReturn.Enabled = Index > 0;
        }

        private void ZSelecaoAutomatica_Click(object sender, EventArgs e) {
            Engine.AutoSelect();
        }

        private void ZSelecionarTodos_Click(object sender, EventArgs e) {
            for (int i = 0; i < StrList.Items.Count; i++)
                StrList.SetItemChecked(i, true);
        }

        private void ZDesselecionarTodos_Click(object sender, EventArgs e) {
            for (int i = 0; i < StrList.Items.Count; i++)
                StrList.SetItemChecked(i, false);
        }

        private void StrKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r')
                Index = StrList.SelectedIndex;
        }

        private void ValChanged(object sender, EventArgs e) {
            TLBox.SpellCheckEnable = ZValidar.Checked;
        }
        private void VerChanged(object sender, EventArgs e) {
            TLBox.SpellCheckEnable = ZVerificacao.Checked;
        }

        private void ZMinhaConta_Click(object sender, EventArgs e) {
            if (!Engine.Authenticated)
                (new StyleLogin()).ShowDialog();
            else {
                StyleBackup BackupForm = new StyleBackup();
                BackupForm.BackupSelected += BackupForm_BackupSelected;
                BackupForm.ShowDialog();
            }
        }

        internal void SetStr(int i, string Content) {
            if (Index == i)
                TLBox.Text = Content;
            else
                StrList.Items[i] = Content;
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

        private void StyleFrmClosing(object sender, FormClosingEventArgs e) {
            if (Engine.UploadingBackup) {
                e.Cancel = true;
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.WaitBackupUpload), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!FileSaved) {
                DialogResult dr = MessageBox.Show(Engine.LoadTranslation(Engine.TLID.NoScriptOpen), "VNXTLP", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes) {
                    ZSalvar_Click(null, null);
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

            Engine.SetConfig("VNXTLP", "DynamicMode", ZModoDianmico.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "SkipDelay", ZLimiteAvanco.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "SpellCheck", TLBox.SpellCheckEnable ? "true" : "false");
            Engine.SetConfig("VNXTLP", "AutoJump", ZValidar.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "BlackTheme", ZAltoContraste.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "HighFont", ZAltaRel.Checked ? "true" : "false");
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

                    ZReturn_Click(null, null);
                }
                if (e.KeyCode == Keys.Down) {
                    //Stop "Ding" Sound
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    ZContinue_Click(null, null);
                }
            }
        }

        private void TLBox_EnabledChanged(object sender, EventArgs e) {
            ZTextBox.Enabled = TLBox.Enabled;
        }

        int Widht;

        private void TLBox_TextChanged() {
            if (TLBox == null)
                return;
            //Update the ScrollBox Values;
            Widht = Engine.TextWidth(TLBox.Text, TLBox.Font);
            Scroll.Visible = Widht > TLBox.Width;
            Scroll.Maximum = Widht;
            Scroll.SmallChange = Widht / 15;
            Scroll.LargeChange = Widht / 8;
        }

        private void ScrollChange(object sender) {
            if (TLBox == null)
                return;
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

        internal string GetStr(int i) {
            return Index == i ? TLBox.Text : StrList.Items[i].ToString();
        }        
        private void StrList_MouseStopOver(object sender, MouseEventArgs e) {
            if (!ZScriptRef.Checked)
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
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BeforeSaveOpenAScript), "VNX+ Translation Platform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ZScriptRef.Checked = !ZScriptRef.Checked;
            if (ZScriptRef.Checked) {
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
                ZScriptRef.Checked = false;
            }
        }

        private void ZAltaRel_CheckedChanged(object sender, EventArgs e) {
            if (ZAltaRel.Checked)
                StrList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f);
            else
                StrList.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f);
        }

        private void ZAltoContraste_CheckedChanged(object sender, EventArgs e) {
            if (ZAltoContraste.Checked) {
                StrList.BackColor = Engine.LoadColor("ContrastBackColor", System.Drawing.Color.Black);
                StrList.ForeColor = Engine.LoadColor("ContrastForeColor", System.Drawing.Color.Green);

                StrList.Color1 = StrList.BackColor;
                StrList.Color2 = Engine.LoadColor("ContrastAlternateColor", System.Drawing.Color.FromArgb(30, 30, 30));
            }
            else {
                StrList.BackColor = System.Drawing.Color.FromArgb(235, 235, 235);
                StrList.ForeColor = System.Drawing.Color.Black;

                StrList.Color1 = StrList.BackColor;
                StrList.Color2 = System.Drawing.Color.White;
            }
        }

        private void ZPesquisa_Click(object sender, EventArgs e) {
            if (Program.SearchOpen)
                return;
            Program.SearchForm = new Search();
            Program.SearchForm.Show();
        }

        private void StyleProgram_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.Control | Keys.O:
                    e.SuppressKeyPress = true;
                    ZAbrir_Click(null, null);
                    break;
                case Keys.Control | Keys.S:
                    e.SuppressKeyPress = true;
                    FastSave(null, null);
                    break;
                case Keys.Control | Keys.Shift | Keys.S:
                    e.SuppressKeyPress = true;
                    ZSalvar_Click(null, null);
                    break;
                case Keys.Control | Keys.F:
                    ZPesquisa_Click(null, null);
                    break;
            }            
        }

        private void Enabled_Changed(object sender, EventArgs e) {
            ZTextBox.Enabled = TLBox.Enabled;
        }

        private void StyleFrmOpen(object sender, EventArgs e) {
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

            //Prevent Hidden incon in taskbar
            Form frm = new Form() {
                Size = new System.Drawing.Size(1, 1)
            };
            frm.Shown += (a, b) => { frm.Close(); };
            frm.ShowDialog();

            TopMost = true;
            Focus();
            TopMost = false;
        }

        private void Resized(object sender, EventArgs e) {
            TLBox_TextChanged();
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

        private void ZSaveWindowState_Click(object sender, EventArgs e) {
            Engine.SaveWindowState(this);
        }
    }
}
