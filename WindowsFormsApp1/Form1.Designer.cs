namespace WindowsFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button addPasswordButton;
        private System.Windows.Forms.Button showPasswordsButton;
        private System.Windows.Forms.Button deletePasswordButton;
        private System.Windows.Forms.ListBox passwordsListBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label passwordLabel;

        private void InitializeComponent()
        {
            // Form1 settings
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(34, 36, 49); // Dark background
            this.Text = "Password Manager";

            // Username Label
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameLabel.Location = new System.Drawing.Point(40, 50);
            this.usernameLabel.Size = new System.Drawing.Size(80, 20);
            this.usernameLabel.Text = "Логин:";
            this.usernameLabel.ForeColor = System.Drawing.Color.Gainsboro;

            // Password Label
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordLabel.Location = new System.Drawing.Point(40, 100);
            this.passwordLabel.Size = new System.Drawing.Size(80, 20);
            this.passwordLabel.Text = "Пароль:";
            this.passwordLabel.ForeColor = System.Drawing.Color.Gainsboro;

            // Username TextBox
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox.Location = new System.Drawing.Point(140, 50);
            this.usernameTextBox.Size = new System.Drawing.Size(200, 30);
            this.usernameTextBox.ForeColor = System.Drawing.Color.Gainsboro;
            this.usernameTextBox.BackColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Password TextBox
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox.Location = new System.Drawing.Point(140, 100);
            this.passwordTextBox.Size = new System.Drawing.Size(200, 30);
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.ForeColor = System.Drawing.Color.Gainsboro;
            this.passwordTextBox.BackColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Add Password Button
            this.addPasswordButton = new System.Windows.Forms.Button();
            this.addPasswordButton.Location = new System.Drawing.Point(40, 150);
            this.addPasswordButton.Size = new System.Drawing.Size(300, 40);
            this.addPasswordButton.Text = "Добавить пароль";
            this.addPasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPasswordButton.FlatAppearance.BorderSize = 0;
            this.addPasswordButton.BackColor = System.Drawing.Color.FromArgb(80, 120, 200);
            this.addPasswordButton.ForeColor = System.Drawing.Color.White;
            this.addPasswordButton.Click += new System.EventHandler(this.AddPasswordButton_Click);

            // Show Passwords Button
            this.showPasswordsButton = new System.Windows.Forms.Button();
            this.showPasswordsButton.Location = new System.Drawing.Point(40, 200);
            this.showPasswordsButton.Size = new System.Drawing.Size(300, 40);
            this.showPasswordsButton.Text = "Показать пароли";
            this.showPasswordsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showPasswordsButton.FlatAppearance.BorderSize = 0;
            this.showPasswordsButton.BackColor = System.Drawing.Color.FromArgb(80, 120, 200);
            this.showPasswordsButton.ForeColor = System.Drawing.Color.White;
            this.showPasswordsButton.Click += new System.EventHandler(this.ShowPasswordsButton_Click);

            // Delete Password Button
            this.deletePasswordButton = new System.Windows.Forms.Button();
            this.deletePasswordButton.Location = new System.Drawing.Point(40, 250);
            this.deletePasswordButton.Size = new System.Drawing.Size(300, 40);
            this.deletePasswordButton.Text = "Удалить пароль";
            this.deletePasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deletePasswordButton.FlatAppearance.BorderSize = 0;
            this.deletePasswordButton.BackColor = System.Drawing.Color.FromArgb(255, 90, 90);
            this.deletePasswordButton.ForeColor = System.Drawing.Color.White;
            this.deletePasswordButton.Click += new System.EventHandler(this.DeletePasswordButton_Click);

            // Passwords ListBox
            this.passwordsListBox = new System.Windows.Forms.ListBox();
            this.passwordsListBox.Location = new System.Drawing.Point(40, 310);
            this.passwordsListBox.Size = new System.Drawing.Size(300, 100);
            this.passwordsListBox.BackColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.passwordsListBox.ForeColor = System.Drawing.Color.Gainsboro;
            this.passwordsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Add controls to Form
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.addPasswordButton);
            this.Controls.Add(this.showPasswordsButton);
            this.Controls.Add(this.deletePasswordButton);
            this.Controls.Add(this.passwordsListBox);
        }
    }
}
