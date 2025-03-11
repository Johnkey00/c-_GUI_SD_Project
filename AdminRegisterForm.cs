using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAAVDataApp
{
    public partial class AdminRegisterForm : Form
    {
        private string dbPath;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;

        public AdminRegisterForm(string databasePath)
        {
            dbPath = databasePath;
            //InitializeComponent();
            LoadUIComponents();
        }

        private void LoadUIComponents()
        {
            this.Text = "User Registration";
            this.Width = 400;
            this.Height = 250;

            Label lblUsername = new Label { Text = "Username:", Top = 30, Left = 50, Width = 100 };
            txtUsername = new TextBox { Top = 30, Left = 150, Width = 180 };

            Label lblPassword = new Label { Text = "Password:", Top = 70, Left = 50, Width = 100 };
            txtPassword = new TextBox { Top = 70, Left = 150, Width = 180, PasswordChar = '*' };

            Label lblConfirmPassword = new Label { Text = "Confirm Password:", Top = 110, Left = 50, Width = 100 };
            txtConfirmPassword = new TextBox { Top = 110, Left = 150, Width = 180, PasswordChar = '*' };

            btnRegister = new Button { Text = "Register", Top = 160, Left = 150, Width = 120 };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (var checkCmd = new SQLiteCommand(checkUserQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string insertUserQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                using (var insertCmd = new SQLiteCommand(insertUserQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", ComputeHash(password));
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

