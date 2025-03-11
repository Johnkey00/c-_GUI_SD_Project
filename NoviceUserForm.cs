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
using UAAVDataApp;

namespace WindowsFormsApp1
{
    public partial class NoviceUserForm : Form
    {
        private string dbPath = "Readings.db";
        private ComboBox cmbDays;
        private DataGridView dgvStatistics;
        private DataGridView dgvBreakdowns;

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
            cmbDays = new ComboBox { Name = "cmbDays", Top = 20, Left = 130, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            Button btnFetchData = new Button { Text = "Fetch Data", Top = 20, Left = 280, Width = 80 };
            btnFetchData.Click += BtnFetchData_Click;
            Button btnViewAllData = new Button { Text = "View All Data", Top = 20, Left = 500, Width = 130 };
            btnViewAllData.Click += BtnViewAllData_Click;
            Button btnViewStatistics = new Button { Text = "View Statistics", Top = 20, Left = 700, Width = 130 };
            btnViewStatistics.Click += BtnViewStatistics_Click;
            Button btnViewReport = new Button { Text = "View Report", Top = 20, Left = 900, Width = 130 };
            btnViewReport.Click += BtnViewReport_Click;
            

            Label lblStatistics = new Label { Text = "Statistics:", Top = 60, Left = 20, Width = 200 };
            dgvStatistics = new DataGridView
            {
                Name = "dgvStatistics",
                Top = 85,
                Left = 20,
                Width = 1050,
                Height = 150,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true
            };

            Label lblBreakdowns = new Label { Text = "Breakdowns:", Top = 250, Left = 20, Width = 200 };
            dgvBreakdowns = new DataGridView
            {
                Name = "dgvBreakdowns",
                Top = 275,
                Left = 20,
                Width = 1050,
                Height = 150,
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
            this.Controls.Add(btnViewReport);
            this.Controls.Add(lblStatistics);
            this.Controls.Add(dgvStatistics);
            this.Controls.Add(lblBreakdowns);
            this.Controls.Add(dgvBreakdowns);
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
            FetchStatisticsData(cmbDays.SelectedItem.ToString());
            FetchBreakdownData(cmbDays.SelectedItem.ToString());
        }

        private void FetchStatisticsData(string day)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT 
                                    ROUND(MAX(Speed), 2) AS MaxSpeed, ROUND(AVG(Speed), 2) AS AvgSpeed, 
                                    ROUND(MAX(CabTemp), 2) AS MaxCabTemp, ROUND(AVG(CabTemp), 2) AS AvgCabTemp,
                                    ROUND(MAX(EngTemp), 2) AS MaxEngTemp, ROUND(AVG(EngTemp), 2) AS AvgEngTemp,
                                    ROUND(MAX(ShockWear), 2) AS MaxShockWear, ROUND(AVG(ShockWear), 2) AS AvgShockWear,
                                    ROUND(MIN(Fuel), 2) AS MinFuel, ROUND(MIN(Battery), 2) AS MinBattery 
                                FROM UAAVData WHERE Day = @Day";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Day", day);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvStatistics.DataSource = dataTable;
                    }
                }
            }
        }

        private void FetchBreakdownData(string day)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT RunningTime, ROUND(Speed, 2) AS Speed, ROUND(CabTemp, 2) AS CabTemp, ROUND(EngTemp, 2) AS EngTemp, 
                                        ROUND(Fuel, 2) AS Fuel, ROUND(Battery, 2) AS Battery, ROUND(ShockWear, 2) AS ShockWear 
                                FROM UAAVData  
                                WHERE Day = @Day AND 
                                    (Fuel = 0 OR Battery = 0)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Day", day);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvBreakdowns.DataSource = dataTable;
                    }
                }
            }
        }

        private void BtnViewAllData_Click(object sender, EventArgs e)
        {
            dgvBreakdowns.DataSource = null;
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT Day, RunningTime, 
                                        ROUND(Speed, 2) AS Speed, ROUND(CabTemp, 2) AS CabTemp, 
                                        ROUND(EngTemp, 2) AS EngTemp, ROUND(Fuel, 2) AS Fuel, 
                                        ROUND(Battery, 2) AS Battery, ROUND(ShockWear, 2) AS ShockWear 
                                FROM UAAVData ORDER BY Day, RunningTime";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvStatistics.DataSource = dataTable;
                }
            }
        }

        private void BtnViewStatistics_Click(object sender, EventArgs e)
        {
            dgvBreakdowns.DataSource = null;
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT Day, 
                                    ROUND(MAX(Speed), 2) AS MaxSpeed, ROUND(AVG(Speed), 2) AS AvgSpeed, 
                                    ROUND(MAX(CabTemp), 2) AS MaxCabTemp, ROUND(AVG(CabTemp), 2) AS AvgCabTemp,
                                    ROUND(MAX(EngTemp), 2) AS MaxEngTemp, ROUND(AVG(EngTemp), 2) AS AvgEngTemp,
                                    ROUND(MAX(ShockWear), 2) AS MaxShockWear, ROUND(AVG(ShockWear), 2) AS AvgShockWear,
                                    ROUND(MIN(Fuel), 2) AS MinFuel, ROUND(MIN(Battery), 2) AS MinBattery 
                                FROM UAAVData GROUP BY Day ORDER BY Day";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvStatistics.DataSource = dataTable;
                }
            }
        }

        private void BtnViewReport_Click(object sender, EventArgs e)
        {
            dgvBreakdowns.DataSource = null;
            NoviceReport noviceReportForm = new NoviceReport(dbPath);
            noviceReportForm.Show();
        }
    }
}
