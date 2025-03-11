using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace UAAVDataApp
{
    public partial class NoviceReport : Form
    {
        private string dbPath;
        private DataGridView dgvReport;

        public NoviceReport(string databasePath)
        {
            dbPath = databasePath;
            //InitializeComponent();
            LoadUIComponents();
            LoadReportData();
        }

        private void LoadUIComponents()
        {
            this.Text = "UAAV Report";
            this.Width = 1120;
            this.Height = 600;

            dgvReport = new DataGridView
            {
                Top = 20,
                Left = 20,
                Width = 1060,
                Height = 500,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            };

            this.Controls.Add(dgvReport);
        }

        private void LoadReportData()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT 
                                        CAST(Day AS TEXT) AS Day, 
                                        RunningTime, 
                                        ROUND(Speed, 2) AS Speed, 
                                        ROUND(CabTemp, 2) AS CabTemp, 
                                        ROUND(EngTemp, 2) AS EngTemp, 
                                        ROUND(Fuel, 2) AS Fuel, 
                                        ROUND(Battery, 2) AS Battery, 
                                        ROUND(ShockWear, 2) AS ShockWear, 
                                        CASE 
                                            WHEN Fuel = 0 OR Battery = 0 THEN 'Breakdown'
                                            WHEN Speed = 0 OR CabTemp > 30 THEN 'Failure'
                                            ELSE 'Successful Run'
                                        END AS Status
                                FROM UAAVData ORDER BY Day, RunningTime";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvReport.DataSource = dataTable;
                }
            }
        }
    }
}
