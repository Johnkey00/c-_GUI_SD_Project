using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAAVDataApp
{
    public partial class AdminForm : Form
    {
        private string dbPath;
        private DataGridView dgvUAAVData;
        private Button btnLoadData, btnAdd, btnUpdate, btnDelete;

        public AdminForm(string databasePath)
        {
            dbPath = databasePath;
            //InitializeComponent();
            LoadUIComponents();
            LoadUAAVData();
        }

        private void LoadUIComponents()
        {
            this.Text = "Admin - Manage UAAV Data";
            this.Width = 1200;
            this.Height = 600;

            dgvUAAVData = new DataGridView
            {
                Top = 20,
                Left = 20,
                Width = 1150,
                Height = 400,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false
            };

            btnLoadData = new Button { Text = "Load Data", Top = 440, Left = 20, Width = 120 };
            btnLoadData.Click += BtnLoadData_Click;

            btnAdd = new Button { Text = "Add Entry", Top = 440, Left = 160, Width = 120 };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button { Text = "Update Entry", Top = 440, Left = 300, Width = 120 };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button { Text = "Delete Entry", Top = 440, Left = 440, Width = 120 };
            btnDelete.Click += BtnDelete_Click;

            this.Controls.Add(dgvUAAVData);
            this.Controls.Add(btnLoadData);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
        }

        private void LoadUAAVData()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "SELECT * FROM UAAVData ORDER BY Day, RunningTime";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvUAAVData.DataSource = dataTable;
                }
            }
        }

        private void BtnLoadData_Click(object sender, EventArgs e)
        {
            LoadUAAVData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature to add a new entry will be implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature to update an entry will be implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature to delete an entry will be implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

