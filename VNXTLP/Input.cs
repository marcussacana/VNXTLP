using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class Input : Form {

        internal delegate dynamic TestInput(string Value);
        internal TestInput Type = null;
        internal dynamic Value = null;
        internal Input() {
            InitializeComponent();
        }

        private void Enter_Click(object sender, EventArgs e) {
            try {
                Value = Type?.Invoke(TbValue.Text);
                Close();
            } catch {
                MessageBox.Show(Engine.LoadTranslation(93), "VNXTLP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
