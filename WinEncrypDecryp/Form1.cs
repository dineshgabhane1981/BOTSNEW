using BOTS_BL.Common;
using System.Text;

namespace WinEncrypDecryp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == string.Empty || txtPassword.Text.Trim() == null)
            {
                MessageBox.Show("Please Enter Password to Encrypt");
                lblPasswordtoShow.Text = string.Empty;
            }
            else
            {
                var PasswordToEncrypt = txtPassword.Text.Trim();
                lblPasswordtoShow.Visible = true;
                lblPasswordtoShow.Text = EncryptionDecryption.EncryptString(PasswordToEncrypt);
            }

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == string.Empty || txtPassword.Text.Trim() == null)
            {
                MessageBox.Show("Please Enter Password to Decrypt");
                lblPasswordtoShow.Text = string.Empty;
            }
            else
            {
                var PasswordToDecrypt = txtPassword.Text.Trim();
                lblPasswordtoShow.Visible = true;
                lblPasswordtoShow.Text = EncryptionDecryption.DecryptString(PasswordToDecrypt);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Text = string.Empty;
            lblPasswordtoShow.Visible = false;
            lblPasswordtoShow.Text = string.Empty;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {            
            if (lblPasswordtoShow.Text != string.Empty)
            {
                Clipboard.SetText(lblPasswordtoShow.Text.ToString());
            }
        }
    }
}
