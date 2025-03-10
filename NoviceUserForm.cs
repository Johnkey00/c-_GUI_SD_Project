using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class NoviceUserForm : Form
    {
        private string dbPath = "Readings.db";
        private ComboBox cmbDays;
        private DataGridView dgvResults;

        public NoviceUserForm()
        {
            InitializeComponent();
            LoadUIComponents();
            LoadAvailableDays();
        }

        private void LoadUIComponents()
        {
            this.Text = "Novice User Panel";
            Label lblDay = new Label { Text = "Select Day:", Top = 20, Left = 20, Width = 100 };
            cmbDays = new ComboBox { Name = "cmbDays", Top = 20, Left = 130, Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            Button btnFetchData = new Button { Text = "Fetch Data", Top = 20, Left = 250, Width = 100 };
            btnFetchData.Click += BtnFetchData_Click;
            Button btnViewAllData = new Button { Text = "View All Data", Top = 20, Left = 370, Width = 120 };
            btnViewAllData.Click += BtnViewAllData_Click;
            Button btnViewStatistics = new Button { Text = "View Statistics", Top = 20, Left = 510, Width = 130 };
            btnViewStatistics.Click += BtnViewStatistics_Click;
            dgvResults = new DataGridView
            {
                Name = "dgvResults",
                Top = 60,
                Left = 20,
                Width = 1040,
                Height = 360,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true
            };

            this.Controls.Add(lblDay);
            this.Controls.Add(cmbDays);
            this.Controls.Add(btnFetchData);
            this.Controls.Add(btnViewAllData);
            this.Controls.Add(btnViewStatistics);
            this.Controls.Add(dgvResults);
        }

        private void LoadAvailableDays()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "SELECT DISTINCT Day FROM UAAVData ORDER BY Day";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbDays.Items.Add(reader["Day"].ToString());
                    }
                }
            }
        }

        private void BtnFetchData_Click(object sender, EventArgs e)
        {
            if (cmbDays.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid day.", "Error");
                return;
            }
            FetchNoviceUserData(cmbDays.SelectedItem.ToString());
        }

        private void FetchNoviceUserData(string day)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT 
                                    MAX(Speed) AS MaxSpeed, AVG(Speed) AS AvgSpeed, 
                                    MAX(CabTemp) AS MaxCabTemp, AVG(CabTemp) AS AvgCabTemp,
                                    MAX(EngTemp) AS MaxEngTemp, AVG(EngTemp) AS AvgEngTemp,
                                    MAX(ShockWear) AS MaxShockWear, AVG(ShockWear) AS AvgShockWear,
                                    MIN(Fuel) AS MinFuel, MIN(Battery) AS MinBattery 
                                FROM UAAVData WHERE Day = @Day";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Day", day);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvResults.DataSource = dataTable;
                    }
                }
            }
        }

        private void BtnViewAllData_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "SELECT * FROM UAAVData ORDER BY Day, RunningTime";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvResults.DataSource = dataTable;
                }
            }
        }

        private void BtnViewStatistics_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT Day, 
                                    MAX(Speed) AS MaxSpeed, AVG(Speed) AS AvgSpeed, 
                                    MAX(CabTemp) AS MaxCabTemp, AVG(CabTemp) AS AvgCabTemp,
                                    MAX(EngTemp) AS MaxEngTemp, AVG(EngTemp) AS AvgEngTemp,
                                    MAX(ShockWear) AS MaxShockWear, AVG(ShockWear) AS AvgShockWear,
                                    MIN(Fuel) AS MinFuel, MIN(Battery) AS MinBattery 
                                FROM UAAVData GROUP BY Day ORDER BY Day";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvResults.DataSource = dataTable;
                }
            }
        }
    }
}
