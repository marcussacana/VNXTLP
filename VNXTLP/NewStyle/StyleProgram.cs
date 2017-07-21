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

        public override string Text { get { return base.Text; } set { base.Text = value; ZSKN.Text = value; Invalidate(); } }
        internal int Index {
            get
            {
                return StrList.SelectedIndex < 0 ? 0 : StrList.SelectedIndex;
            }
            set
            {
                Engine.UpdateInfo(value, ref InfoLbl, GetBackupFrequence(), ref Changes, ZValidar.Checked);
            }
        }

        private int Changes = 0;
        internal string[] RefScript;
        internal StyleProgram() {
            InitializeComponent();

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

            //Initialize RadioToolStrip Engine 
            ToolStripMenuItem[] BackupItems = new ToolStripMenuItem[] { ZAoSalvar, Z200Dialogos, Z100Dialogos, Z50Dialogos, Z25Dialogos, Z10Dialogos, ZNunca };
            RadioEngine = new Engine.RadioToolStrip(ref BackupItems, 2);

            ToolStripMenuItem[] ThemeItems = new ToolStripMenuItem[] { ZBasico, ZModerno };
            ThemeEngine = new Engine.RadioToolStrip(ref ThemeItems, 1);
            ThemeEngine.CheckedChange += ChangeTheme;

            ToolStripMenuItem[] TLCLients = new ToolStripMenuItem[] { ZLEC, ZGoogle };
            TLEngine = new Engine.RadioToolStrip(ref TLCLients, 1);
            TLEngine.CheckedChange += TLEngine_CheckedChange;

            ToolStripMenuItem[] SelItems = new ToolStripMenuItem[] { ZAutoSelMode, ZAsianSel, ZLatimSel };
            SelEngine = new Engine.RadioToolStrip(ref SelItems, 0);
            SelEngine.CheckedChange += SelEngine_CheckedChange;

            //Initialize DeleyedMouseOver Event
            OVE = new Engine.OverTimerEvent();
            OVE.sender = StrList;
            OVE.MouseStopOver += StrList_MouseStopOver;
            OVE.Initialize();

            //Set File Filter
            OpenScript.Filter = Engine.Filter;
            SaveScript.Filter = Engine.Filter;

            //Initalize TLBox
            TLBox.Font = ZTextBox.Font;
            TLBox.LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "Dictionary.aff", AppDomain.CurrentDomain.BaseDirectory + "Dictionary.dic");
            TLBox.BootUP();

            //Initialize Config
            ZVerificacao.Checked = Engine.GetConfig("VNXTLP", "SpellCheck", false) == "true";
            TLBox.SpellCheckEnable = ZVerificacao.Checked;
            ZValidar.Checked = Engine.GetConfig("VNXTLP", "AutoJump", false) == "true";
            ZAltaRel.Checked = Engine.GetConfig("VNXTLP", "HighFont", false) == "true";
            ZAltoContraste.Checked = Engine.GetConfig("VNXTLP", "BlackTheme", false) == "true";

            //get int
            string cfg = Engine.GetConfig("VNXTLP", "BackupSpeed", false);
            int Val;
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
            ZContinue.Text = Engine.LoadTranslation(11);
            ZReturn.Text = Engine.LoadTranslation(12);
            ZArquivo.Text = Engine.LoadTranslation(13);
            ZAbrir.Text = Engine.LoadTranslation(14);
            ZSalvar.Text = Engine.LoadTranslation(15);
            ZMinhaConta.Text = Engine.LoadTranslation(5);
            ZSelecao.Text = Engine.LoadTranslation(16);
            ZSelecionarTodos.Text = Engine.LoadTranslation(17);
            ZDesselecionarTodos.Text = Engine.LoadTranslation(18);
            ZSelecaoAutomatica.Text = Engine.LoadTranslation(19);
            ZOpcoes.Text = Engine.LoadTranslation(20);
            ZTema.Text = Engine.LoadTranslation(21);
            ZBasico.Text = Engine.LoadTranslation(22);
            ZModerno.Text = Engine.LoadTranslation(23);
            ZPeriodo.Text = Engine.LoadTranslation(24);
            ZAoSalvar.Text = Engine.LoadTranslation(25);
            Z50Dialogos.Text = Engine.LoadTranslation(26);
            Z25Dialogos.Text = Engine.LoadTranslation(27);
            Z10Dialogos.Text = Engine.LoadTranslation(28);
            ZNunca.Text = Engine.LoadTranslation(29);
            ZVerificacao.Text = Engine.LoadTranslation(30);
            ZValidar.Text = Engine.LoadTranslation(31);
            OpenScript.Title = Engine.LoadTranslation(33);
            SaveScript.Title = Engine.LoadTranslation(33);
            ZPesquisa.Text = Engine.LoadTranslation(58);
            ZTLClient.Text = Engine.LoadTranslation(62);
            ZLEC.Text = Engine.LoadTranslation(63);
            ZGoogle.Text = Engine.LoadTranslation(64);
            ZScriptRef.Text = Engine.LoadTranslation(68);
            ZAltoContraste.Text = Engine.LoadTranslation(70);
            ZAltaRel.Text = Engine.LoadTranslation(71);
            ZSelMode.Text = Engine.LoadTranslation(101);
            ZAutoSelMode.Text = Engine.LoadTranslation(102);
            ZAsianSel.Text = Engine.LoadTranslation(103);
            ZLatimSel.Text = Engine.LoadTranslation(104);

            //Load Custom Resources from a VNXTL Build
            foreach (ToolStripMenuItem item in Engine.CustomResources(ref TLBox))
                ZMenu.Items.Add(item);
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
                Engine.ChangeTheme(new NoStyle(), this, false);
        }

        private void ZAbrir_Click(object sender, EventArgs e) {
            OpenScript.ShowDialog();
        }

        private void ZSalvar_Click(object sender, EventArgs e) {
            if (!FileOpen) {
                MessageBox.Show(Engine.LoadTranslation(34), "VNX+ Translation Plataform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveScript.ShowDialog();
        }

        private void OpenScript_FileOk(object sender, CancelEventArgs e) {
            string[] StringList = Engine.Open(OpenScript.FileName);
            FileOpen = true;
            FileSaved = true;

            //Clear Update Strings
            StrList.Items.Clear();
            foreach (string String in StringList)
                StrList.Items.Add(String);
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
            if (!Program.OfflineMode && !ZNunca.Checked)
                (new System.Threading.Thread((str) => { Engine.Backup(StringList, true); })).Start(StringList);

            //Save Script
            Engine.Save(SaveScript.FileName, StringList);
            FileSaved = true;
            Engine.UpdateSelection();
            MessageBox.Show(Engine.LoadTranslation(46), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    return -1;
            }
        }

        private void ZReturn_Click(object sender, EventArgs e) {
            if (FileOpen)
                Index--;
        }

        private void ZContinue_Click(object sender, EventArgs e) {
            if (FileOpen)
                Index++;
        }

        private void StrList_SelectedIndexChanged(object sender, EventArgs e) {
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
                NewStyle.StyleBackup BackupForm = new NewStyle.StyleBackup();
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

                MessageBox.Show(Engine.LoadTranslation(35), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < BackupStrings.Length; i++)
                StrList.Items[i] = BackupStrings[i];
            Index = 0; //Update Textbox
            MessageBox.Show(Engine.LoadTranslation(36), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StyleFrmClosing(object sender, FormClosingEventArgs e) {
            if (Engine.UploadingBackup) {
                e.Cancel = true;
                MessageBox.Show(Engine.LoadTranslation(37), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!FileSaved) {
                DialogResult dr = MessageBox.Show(Engine.LoadTranslation(96), "VNXTLP", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
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
            Engine.SetConfig("VNXTLP", "SpellCheck", TLBox.SpellCheckEnable ? "true" : "false");
            Engine.SetConfig("VNXTLP", "AutoJump", ZValidar.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "BlackTheme", ZAltoContraste.Checked ? "true" : "false");
            Engine.SetConfig("VNXTLP", "HighFont", ZAltaRel.Checked ? "true" : "false");
            if (GetBackupFrequence() > 0)
                Engine.SetConfig("VNXTLP", "BackupSpeed", RadioEngine.SelectedIndex.ToString());
            Engine.SetConfig("VNXTLP", "LastScript", Engine.ScriptPath + "|" + Index);
        }
        internal void TLBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                //Stop "Ding" Sound
                e.Handled = true;
                e.SuppressKeyPress = true;

                //Update translation and get next string
                StrList.Items[Index] = TLBox.Text;
                FileSaved = false;
                Changes++;
                Index++;
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
            Engine.ShowToolTip(Engine.LocationCalc(e.Location, 0, (Cursor.Current.Size.Height / 2)), Reference, Engine.LoadTranslation(67));
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
                MessageBox.Show(Engine.LoadTranslation(34), "VNX+ Translation Plataform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ZScriptRef.Checked = !ZScriptRef.Checked;
            if (ZScriptRef.Checked) {
                OpenFileDialog FD = new OpenFileDialog {
                    Filter = Engine.Filter,
                    Title = Engine.LoadTranslation(65)
                };
                if (FD.ShowDialog() == DialogResult.OK) {
                    string[] Script = Engine.Open(FD.FileName, true);
                    if (Script.Length != StrList.Items.Count)
                        MessageBox.Show(Engine.LoadTranslation(66), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                StrList.BackColor = System.Drawing.Color.Black;
                StrList.ForeColor = System.Drawing.Color.Green;
            }
            else {
                StrList.BackColor = System.Drawing.SystemColors.Window;
                StrList.ForeColor = System.Drawing.Color.Black;
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

                if (System.IO.File.Exists(cfg)) {
                    OpenScript.FileName = cfg;
                    OpenScript_FileOk(null, null);
                    this.Index = int.Parse(Index);
                }
            }
            catch { }

            //Prevent Hidden incon in taskbar
            Form frm = new Form();
            frm.Size = new System.Drawing.Size(1, 1);
            frm.Shown += (a, b) => { frm.Close(); };
            frm.ShowDialog();
        }

        private void Resized(object sender, EventArgs e) {
            TLBox_TextChanged();
        }
    }
}
