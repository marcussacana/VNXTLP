using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VNXTLP {
    public partial class BackupViewer : Form {
        public BackupViewer() {
            InitializeComponent();

            Initialize();
        }

        private void Initialize() {
            DialogResult = DialogResult.Cancel;

            Text = Engine.LoadTranslation(Engine.TLID.LoadingUsers);
            new System.Threading.Thread(() => {
                string[] Accounts = Engine.FTP.TreeDir("Backup\\");
                Invoke(new ShowUsers(ListUsers), new object[] { Accounts } );
            }).Start();
        }

        private delegate void ShowUsers(string[] Users);
        private void ListUsers(string[] Users) {
            UserListBox.Items.Clear();
            foreach (string User in Users)
                UserListBox.Items.Add(User);

            Text = Engine.LoadTranslation(Engine.TLID.UsersLoaded);
        }

        private void UserListBox_DoubleClick(object sender, EventArgs e) {
            if (UserListBox.SelectedIndex < 0 || UserListBox.SelectedIndex > UserListBox.Items.Count)
                return;

            Engine.AdminBackup = UserListBox.Items[UserListBox.SelectedIndex].ToString();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
