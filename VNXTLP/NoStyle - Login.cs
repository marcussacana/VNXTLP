using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class NoStyleLogin : Form {
        internal NoStyleLogin() {
            InitializeComponent();
        }

        private void LoginBnt_Click(object sender, EventArgs e) {
            if (Engine.Login(LoginUser.Text, LoginPass.Text, AutoLoginChkBx.Checked))
                Close();
            else
                MessageBox.Show(Engine.LoadTranslation(10), "VNXTLP - Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ChangeLBL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            LoginPanel.Visible = !LoginPanel.Visible;
            RegisterPanel.Visible = !RegisterPanel.Visible;
            ChangeLBL.Text = LoginPanel.Visible ? Engine.LoadTranslation(39) : Engine.LoadTranslation(7);
        }

        private void button1_Click(object sender, EventArgs e) {
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
                    ChangeLBL_LinkClicked(null, null);
                    //Efetuar Login Automaticamente
                    LoginUser.Text = RegisterLogin.Text;
                    LoginPass.Text = RegisterPass.Text;
                    LoginBnt_Click(null, null);
                }
                else {
                    DialogResult DR = MessageBox.Show(Engine.LoadTranslation(44), "VNXTLP - Engine", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (DR != DialogResult.Retry)
                        break;
                }
            }
        }

        private void LoginKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r' || e.KeyChar == '\n') {
                e.Handled = true;
                LoginBnt_Click(null, null);
            }
        }
    }
}
