using System;
using System.Windows.Forms;

namespace VNXTLP {
    public partial class RegKey : Form {
        public uint Key = 0;
        public RegKey() {
            InitializeComponent();
            Text = Engine.LoadTranslation(Engine.TLID.AuthToken);
            DialogResult = DialogResult.Cancel;
        }

        private void bntOK_Click(object sender, EventArgs e) {
            try {
                Key = uint.Parse(tbKey.Text);
            } catch {
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
