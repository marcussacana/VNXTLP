using System;
using System.Windows.Forms;

namespace VNXTLP.NewStyle
{
    internal partial class StyleBackup : Form
    {
        public override string Text { get { return base.Text; } set { base.Text = value; ZSKN.Text = value; Invalidate(); } }
        private string[] Files;
        internal event EventHandler BackupSelected;

        internal StyleBackup()
        {
            InitializeComponent();
            //Load Translation
            Text = Engine.LoadTranslation(0);
            ZOK.Text = Engine.LoadTranslation(1);

            new System.Threading.Thread(() => {
                try {
                    Files = Engine.ListBackups();
                    ShowBackups handle = Initialize;
                    if (handle != null)
                        Invoke(handle, null);
                } catch { }
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
        private void BackupList_DoubleClick(object sender, EventArgs e)
        {
            string[] Lines = Engine.LoadBackup(BackupList.SelectedIndex);
            BackupSelected?.Invoke(Lines, new EventArgs());
            Close();
        }
        private void ZOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
