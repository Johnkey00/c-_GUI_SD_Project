using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAAVDataApp
{
    public partial class AdminLoginForm : Form
    {
        private string dbPath;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public AdminLoginForm(string databasePath)
        {
            dbPath = databasePath;
            InitializeComponent();
            LoadUIComponents();
            InitializeDatabase();
        }

        private void LoadUIComponents()
        {
            this.Text = "Admin Login";
            this.Width = 400;
            this.Height = 200;

            Label lblUsername = new Label { Text = "Username:", Top = 30, Left = 50, Width = 100 };
            txtUsername = new TextBox { Top = 30, Left = 150, Width = 180 };

            Label lblPassword = new Label { Text = "Password:", Top = 70, Left = 50, Width = 100 };
            txtPassword = new TextBox { Top = 70, Left = 150, Width = 180, PasswordChar = '*' };

            btnLogin = new Button { Text = "Login", Top = 120, Left = 50, Width = 120 };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button { Text = "Register", Top = 120, Left = 210, Width = 120 };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
        }

        private void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                            Username TEXT PRIMARY KEY, 
                                            PasswordHash TEXT NOT NULL
                                        );";
                using (var cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
                AdminForm adminForm = new AdminForm(dbPath);
                adminForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            //string username = txtUsername.Text.Trim();
            //string password = txtPassword.Text.Trim();

            //if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            //{
            //    MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            //{
            //    conn.Open();
            //    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            //    using (var checkCmd = new SQLiteCommand(checkUserQuery, conn))
            //    {
            //        checkCmd.Parameters.AddWithValue("@Username", username);
            //        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
            //        if (count > 0)
            //        {
            //            MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //    }

            //    string insertUserQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            //    using (var insertCmd = new SQLiteCommand(insertUserQuery, conn))
            //    {
            //        insertCmd.Parameters.AddWithValue("@Username", username);
            //        insertCmd.Parameters.AddWithValue("@PasswordHash", ComputeHash(password));
            //        insertCmd.ExecuteNonQuery();
            //        MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            this.Hide();
            AdminRegisterForm adminRegisterForm = new AdminRegisterForm(dbPath);
            //adminRegisterForm.Show();
            adminRegisterForm.FormClosed += (s, args) => this.Show();
            adminRegisterForm.ShowDialog();
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result.ToString() == ComputeHash(password))
                    {
                        return true;
                    }
                }
            }
            return false;
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

