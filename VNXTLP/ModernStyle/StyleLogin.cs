using System;
using System.Windows.Forms;

namespace VNXTLP.NewStyle
{
    internal partial class StyleLogin : Form
    {
        internal StyleLogin()
        {
            InitializeComponent();

            //Load Translations
            LB1.Text = Engine.LoadTranslation(Engine.TLID.Username);
            LB2.Text = Engine.LoadTranslation(Engine.TLID.Password);
            ZEnt.Text = Engine.LoadTranslation(Engine.TLID.Login);
            ZRegistrar.Text = Engine.LoadTranslation(Engine.TLID.Register);
            Text = Engine.LoadTranslation(Engine.TLID.MyAccount) + " - VNXTLP";
        }

        private void ZRegistrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            StyleRegister Form = new StyleRegister();
            Form.StartPosition = FormStartPosition.Manual;
            Form.Location = Location;
            Visible = false;
            Form.ShowDialog();
            Location = Form.Location;
            Visible = true;
        }

        private void ZEnt_Click(object sender, EventArgs e) {
            if (Engine.Login(LoginTB.Text, PassTB.Text, true))
                Close();
            else
                MessageBox.Show(Engine.LoadTranslation(Engine.TLID.FailedToAuth), "VNXTLP - Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void KeyLoginPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r' || e.KeyChar == '\n') {
                ZEnt_Click(null, null);
                e.Handled = true;
            }
        }
    }
}
