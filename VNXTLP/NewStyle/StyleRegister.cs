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
            LB1.Text = Engine.LoadTranslation(9);
            LB2.Text = Engine.LoadTranslation(8);
            LB3.Text = Engine.LoadTranslation(40);
            ZReg.Text = Engine.LoadTranslation(39);
            Text = Engine.LoadTranslation(38) + " - VNX+";
        }

        private void ZReg_Click(object sender, EventArgs e) {
            while (true) {
                if (RegisterConfirmPass.Text != RegisterPass.Text) {
                    MessageBox.Show(Engine.LoadTranslation(41), "VNXTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (RegisterLogin.Text.Length < 4) {
                    MessageBox.Show(Engine.LoadTranslation(42), "VNTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Engine.Register(RegisterLogin.Text, RegisterPass.Text)) {
                    MessageBox.Show(Engine.LoadTranslation(43), "VNXTLP - Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else {
                    DialogResult DR = MessageBox.Show(Engine.LoadTranslation(44), "VNXTLP - Engine", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (DR != DialogResult.Retry)
                        break;
                }
            }
        }
    }
}
