using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAAVDataApp
{
    public partial class ExpertUserForm : Form
    {
        private string dbPath;
        private TextBox txtQuery;
        private DataGridView dgvResults;
        private Button btnExecute;

        public ExpertUserForm(string databasePath)
        {
            dbPath = databasePath;
            InitializeComponent();
            LoadUIComponents();
        }

        private void LoadUIComponents()
        {
            this.Text = "Expert User Query";
            this.Width = 1120;
            this.Height = 600;

            Label lblQuery = new Label { Text = "Enter SQL Query:", Top = 20, Left = 20, Width = 200 };
            txtQuery = new TextBox { Top = 50, Left = 20, Width = 1060, Height = 100, Multiline = true, ScrollBars = ScrollBars.Vertical };
            btnExecute = new Button { Text = "Execute Query", Top = 160, Left = 20, Width = 150 };
            btnExecute.Click += BtnExecute_Click;

            dgvResults = new DataGridView
            {
                Top = 200,
                Left = 20,
                Width = 1060,
                Height = 350,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true
            };

            this.Controls.Add(lblQuery);
            this.Controls.Add(txtQuery);
            this.Controls.Add(btnExecute);
            this.Controls.Add(dgvResults);
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            string query = txtQuery.Text.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Query cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsSelectQuery(query))
            {
                MessageBox.Show("Only SELECT queries are allowed.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvResults.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}", "Query Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsSelectQuery(string query)
        {
            return Regex.IsMatch(query, @"^\s*SELECT", RegexOptions.IgnoreCase);
        }
    }
}

