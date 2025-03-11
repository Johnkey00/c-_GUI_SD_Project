using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp1;

namespace UAAVDataApp
{
    public partial class MainForm : Form
    {
        private string dbPath = "Readings.db";
        //private bool isXmlLoaded = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeDatabase();
            //UpdateButtonStates();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS UAAVData (
                    Day INTEGER,
                    RunningTime INTEGER,
                    Speed REAL,
                    CabTemp REAL,
                    EngTemp REAL,
                    Fuel REAL,
                    Battery REAL,
                    ShockWear REAL,
                    PRIMARY KEY (Day, RunningTime)
                );";
                SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnLoadXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files|*.xml",
                Title = "Select an XML File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XDocument xmlDoc = XDocument.Load(openFileDialog.FileName);
                    ParseAndStoreData(xmlDoc);
                    //isXmlLoaded = true;
                    //UpdateButtonStates();
                    MessageBox.Show("Data successfully loaded into the database.", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading XML file: " + ex.Message, "Error");
                }
            }
        }

        private void ParseAndStoreData(XDocument xmlDoc)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                foreach (var entry in xmlDoc.Root.Elements())
                {
                    try
                    {
                        int day = int.Parse(entry.Element("Day").Value);
                        int runningTime = int.Parse(entry.Element("RunningTime").Value);
                        double speed = double.Parse(entry.Element("Speed").Value);
                        double cabTemp = double.Parse(entry.Element("CabTemp").Value);
                        double engTemp = double.Parse(entry.Element("EngTemp").Value);
                        double fuel = double.Parse(entry.Element("Fuel").Value);
                        double battery = double.Parse(entry.Element("Battery").Value);
                        double shockWear = double.Parse(entry.Element("ShockWear").Value);

                        string insertQuery = @"INSERT OR IGNORE INTO UAAVData (Day, RunningTime, Speed, CabTemp, EngTemp, Fuel, Battery, ShockWear) 
                                              VALUES (@Day, @RunningTime, @Speed, @CabTemp, @EngTemp, @Fuel, @Battery, @ShockWear);";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Day", day);
                            cmd.Parameters.AddWithValue("@RunningTime", runningTime);
                            cmd.Parameters.AddWithValue("@Speed", speed);
                            cmd.Parameters.AddWithValue("@CabTemp", cabTemp);
                            cmd.Parameters.AddWithValue("@EngTemp", engTemp);
                            cmd.Parameters.AddWithValue("@Fuel", fuel);
                            cmd.Parameters.AddWithValue("@Battery", battery);
                            cmd.Parameters.AddWithValue("@ShockWear", shockWear);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Data parsing error: {ex.Message}", "Error");
                    }
                }
            }
        }

        private void btnNoviceUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            NoviceUserForm noviceForm = new NoviceUserForm();
            //noviceForm.Show();
            noviceForm.FormClosed += (s, args) => this.Show();
            noviceForm.ShowDialog();
        }

        private void btnExpertUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            ExpertUserForm expertForm = new ExpertUserForm(dbPath);
            //expertForm.Show();
            expertForm.FormClosed += (s, args) => this.Show();
            expertForm.ShowDialog();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminLoginForm adminForm = new AdminLoginForm(dbPath);
            //adminForm.Show();
            adminForm.FormClosed += (s, args) => this.Show();
            adminForm.ShowDialog();
        }

        //private void UpdateButtonStates()
        //{
        //    btnNovice.Enabled = isXmlLoaded;
        //    btnExpert.Enabled = isXmlLoaded;
        //    btnAdmin.Enabled = isXmlLoaded;
        //}
    }
}
