using System;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class NoStyleBackup : Form {
        private string[] Files;
        internal event EventHandler BackupSelected;
        internal NoStyleBackup() {
            InitializeComponent();
            new System.Threading.Thread(() => {
                Files = Engine.ListBackups();
                ShowBackups handle = Initialize;
                if (handle != null)
                    Invoke(handle, null);
            }).Start();
        }

        private delegate void ShowBackups();
        private void Initialize() {
            Text = Engine.LoadTranslation(2);
            if (Files.Length == 0)
                MessageBox.Show(Engine.LoadTranslation(3), "VNXTLP - " + Engine.LoadTranslation(4), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            BackupList.Items.Clear();
            foreach (string file in Files)
                BackupList.Items.Add(file);
        }
        private void BackupList_DoubleClick(object sender, EventArgs e) {
            string[] Lines = Engine.LoadBackup(BackupList.SelectedIndex);
            BackupSelected?.Invoke(Lines, new EventArgs());
            Close();
        }
        
    }
}
