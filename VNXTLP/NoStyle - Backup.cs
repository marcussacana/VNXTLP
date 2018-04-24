using System;
using System.Drawing;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class NoStyleBackup : Form {
        private string[] Files = null;
        internal event EventHandler BackupSelected;
        internal NoStyleBackup() {
            InitializeComponent();

            FormClosing += (a, b) => { Engine.AdminBackup = null; };

            abrirToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.Open);
            deletarToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.DeleteIt);
            explorarUsuáriosToolStripMenuItem.Text = Engine.LoadTranslation(Engine.TLID.BrowseUser);

            if (!Engine.DebugMode)
                ItemMenu.Items.Remove(explorarUsuáriosToolStripMenuItem);

            Initialize();
        }

        private delegate void ShowBackups();
        private void Initialize() {
            Text = Engine.LoadTranslation(Engine.TLID.LoadingBackups);
            if (Files == null) {
                new System.Threading.Thread(() => {
                    Files = Engine.ListBackups();
                    ShowBackups handle = Initialize;
                    if (handle != null)
                        Invoke(handle, null);
                }).Start();
                return;
            }
            Text = Engine.LoadTranslation(Engine.TLID.BackupsLoaded);
            if (Files.Length == 0)
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.WelcomeBackup), "VNXTLP - " + Engine.LoadTranslation(Engine.TLID.Welcome), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            BackupList.Items.Clear();
            foreach (string file in Files)
                BackupList.Items.Add(file);
        }
        private void BackupList_DoubleClick(object sender, EventArgs e) {
            string[] Lines = Engine.LoadBackup(BackupList.SelectedIndex);
            BackupSelected?.Invoke(Lines, new EventArgs());
            Close();
        }

        int MenuStripID = -1;
        private void ItemMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            Point Pointer = Cursor.Position;
            Pointer = BackupList.PointToClient(Pointer);
            MenuStripID = BackupList.IndexFromPoint(Pointer);


            bool ItemSelected = MenuStripID >= 0;

            abrirToolStripMenuItem.Visible = ItemSelected;
            deletarToolStripMenuItem.Visible = ItemSelected;

            if (!ItemSelected && !Engine.DebugMode)
                e.Cancel = true;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e) {
            BackupList.SelectedIndex = MenuStripID;
            BackupList_DoubleClick(null, null);
        }

        private void deletarToolStripMenuItem_Click(object sender, EventArgs e) {
            string Backup = BackupList.Items[MenuStripID].ToString();
            if (Engine.HideBackup(Backup)) {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BackupDeleted), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Text = Engine.LoadTranslation(Engine.TLID.LoadingBackups);
                BackupList.Items.Clear();
                Files = null;
                Initialize();
            } else {
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.DeleteBackupFailed), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void explorarUsuáriosToolStripMenuItem_Click(object sender, EventArgs e) {
            var Form = new BackupViewer();
            if (Form.ShowDialog() == DialogResult.OK) {
                Files = null;
                BackupList.Items.Clear();
                Initialize();
            }
        }
    }
}
