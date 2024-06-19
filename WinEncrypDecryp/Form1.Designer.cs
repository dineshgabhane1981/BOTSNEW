namespace WinEncrypDecryp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitleEncrypt = new Label();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnEncrypt = new Button();
            btnDecrypt = new Button();
            btnClear = new Button();
            lblPasswordtoShow = new Label();
            btnCopy = new Button();
            SuspendLayout();
            // 
            // lblTitleEncrypt
            // 
            lblTitleEncrypt.AutoSize = true;
            lblTitleEncrypt.Location = new Point(297, 32);
            lblTitleEncrypt.Name = "lblTitleEncrypt";
            lblTitleEncrypt.Size = new Size(151, 25);
            lblTitleEncrypt.TabIndex = 0;
            lblTitleEncrypt.Text = "Encrypt Password";
            lblTitleEncrypt.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(177, 104);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(87, 25);
            lblPassword.TabIndex = 1;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(364, 98);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(150, 31);
            txtPassword.TabIndex = 2;
            // 
            // btnEncrypt
            // 
            btnEncrypt.Location = new Point(177, 185);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(112, 34);
            btnEncrypt.TabIndex = 3;
            btnEncrypt.Text = "Encrypt";
            btnEncrypt.UseVisualStyleBackColor = true;
            btnEncrypt.Click += btnEncrypt_Click;
            // 
            // btnDecrypt
            // 
            btnDecrypt.Location = new Point(364, 185);
            btnDecrypt.Name = "btnDecrypt";
            btnDecrypt.Size = new Size(112, 34);
            btnDecrypt.TabIndex = 4;
            btnDecrypt.Text = "Decrypt";
            btnDecrypt.UseVisualStyleBackColor = true;
            btnDecrypt.Click += btnDecrypt_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(516, 185);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(112, 34);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // lblPasswordtoShow
            // 
            lblPasswordtoShow.AutoSize = true;
            lblPasswordtoShow.Location = new Point(177, 264);
            lblPasswordtoShow.Name = "lblPasswordtoShow";
            lblPasswordtoShow.Size = new Size(59, 25);
            lblPasswordtoShow.TabIndex = 6;
            lblPasswordtoShow.Text = "label1";
            lblPasswordtoShow.Visible = false;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(516, 259);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(112, 34);
            btnCopy.TabIndex = 7;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCopy);
            Controls.Add(lblPasswordtoShow);
            Controls.Add(btnClear);
            Controls.Add(btnDecrypt);
            Controls.Add(btnEncrypt);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(lblTitleEncrypt);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitleEncrypt;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnEncrypt;
        private Button btnDecrypt;
        private Button btnClear;
        private Label lblPasswordtoShow;
        private Button btnCopy;
    }
}
