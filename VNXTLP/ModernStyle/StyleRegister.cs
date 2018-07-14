using System;
using System.Windows.Forms;

namespace VNXTLP.NewStyle
{
    internal partial class StyleRegister : Form
    {
        internal StyleRegister()
        {
            InitializeComponent();

            //Load Translation
            LB1.Text = Engine.LoadTranslation(Engine.TLID.Username);
            LB2.Text = Engine.LoadTranslation(Engine.TLID.Password);
            LB3.Text = Engine.LoadTranslation(Engine.TLID.ConfirmPassword);
            ZReg.Text = Engine.LoadTranslation(Engine.TLID.Register);
            Text = Engine.LoadTranslation(Engine.TLID.CreateNewAccount) + " - VNX+";
        }

        private void ZReg_Click(object sender, EventArgs e) {
            while (true) {
                if (RegisterConfirmPass.Text != RegisterPass.Text) {
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.PasswordMissmatch), "VNXTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (RegisterLogin.Text.Length < 4) {
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.BadUsername), "VNTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (Engine.Register(RegisterLogin.Text, RegisterPass.Text)) {
                    MessageBox.Show(Engine.LoadTranslation(Engine.TLID.RegisterSucess), "VNXTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    break;
                }
                else {
                    DialogResult DR = MessageBox.Show(Engine.LoadTranslation(Engine.TLID.RegisterFailed), "VNXTLP - Engine", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (DR != DialogResult.Retry)
                        break;
                }
            }
        }
    }
}
