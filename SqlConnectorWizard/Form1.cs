
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SqlConnectorWizard
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String Servername = txtServerName.Text;
               
                String UserName = txtUserName.Text;
                String Password = txtPassword.Text;
                String connetionString = "";
                if (checkBox1.Checked)
                {
                  
                    connetionString = "SERVER=" + Servername + ";"  + "UID=" + UserName + ";" + "PASSWORD=" + Password + ";";
                  
                    MySqlConnection connection;
                 
                    connection = new MySqlConnection(connetionString);
                    // cnn.Open();

                    List<string> Dbs = GetMysqlDatabaseList(connection);
                    dbCombobox.DataSource = Dbs;
                }
                else
                {
                    connetionString = "Data Source=" + Servername + "; User ID = " + UserName + "; Password = " + Password;
                    SqlConnection cnn;
                    cnn = new SqlConnection(connetionString);                  
                    List<string> Dbs = GetDatabaseList(cnn);
                    dbCombobox.DataSource = Dbs;
                }
              
               // cnn.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public List<string> GetMysqlDatabaseList(MySqlConnection con)
        {
            List<string> list = new List<string>();
            try
            {              


                con.Open();
                MessageBox.Show("Connection Established");              

                using (MySqlCommand cmd = new MySqlCommand("SHOW DATABASES;", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                        }
                    }
                }
                con.Close();

                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return list;
            }
        }
        public List<string> GetDatabaseList(SqlConnection con)
        {
            List<string> list = new List<string>();
            try
            {           

                // Open connection to the database


                con.Open();
                MessageBox.Show("Connection Established");
                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                        }
                    }
                }
                con.Close();

                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return list;
            }
        }
        public static void EditconnetionString(String URL,String name)
        {
            try
            {
                String iniPath = AppDomain.CurrentDomain.BaseDirectory+ name;            
                StringBuilder newFile = new StringBuilder();                
                newFile.Append(URL);
                File.WriteAllText(@iniPath, newFile.ToString());
            }
            catch (Exception)
            {
                //  log.Error(e);

            }

        }
        public static void EditTillFile(String param,String desc)
        {
            try
            {
                if (desc == "MpesaTill")
                {
                    String iniPath = AppDomain.CurrentDomain.BaseDirectory + "MpesaTill.ini";
                    StringBuilder newFile = new StringBuilder();
                    newFile.AppendLine(param);
                    File.WriteAllText(@iniPath, newFile.ToString());
                }else if (desc == "MpesaUrl")
                {
                    String iniPath = AppDomain.CurrentDomain.BaseDirectory + "MpesaUrl.ini";
                    StringBuilder newFile = new StringBuilder();
                    newFile.AppendLine(param);
                    File.WriteAllText(@iniPath, newFile.ToString());
                }
                else if (desc == "EazzyTill")
                {
                    String iniPath = AppDomain.CurrentDomain.BaseDirectory + "EazzyTill.ini";
                    StringBuilder newFile = new StringBuilder();
                    newFile.AppendLine(param);
                    File.WriteAllText(@iniPath, newFile.ToString());
                }
                else if (desc == "EazzyUrl")
                {
                    String iniPath = AppDomain.CurrentDomain.BaseDirectory + "EazzyUrl.ini";
                    StringBuilder newFile = new StringBuilder();
                    newFile.AppendLine(param);
                    File.WriteAllText(@iniPath, newFile.ToString());
                }
                else
                {

                }

            }
            catch (Exception)
            {
                //  log.Error(e);

            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                if ( String.IsNullOrEmpty(txtUserName.Text)
                 || String.IsNullOrEmpty(txtServerName.Text)
                 || String.IsNullOrWhiteSpace(dbCombobox.SelectedItem.ToString()))
                {
                    MessageBox.Show("Fill All Database  Fields are Required");
                    return;
                }
               
                String Servername = txtServerName.Text;
                String DatabaseName = dbCombobox.SelectedItem.ToString();
                String UserName = txtUserName.Text;
                String Password = txtPassword.Text;
                String en = "";
                if (checkBox1.Checked)
                {
                    string connectionString;
                    connectionString = "SERVER=" + Servername + ";" + "DATABASE=" +
                    DatabaseName + ";" + "UID=" + UserName + ";" + "PASSWORD=" + Password + ";";
                    en = Crypto.Encrypt(connectionString);
                    EditconnetionString(en, "mysqlconnetionString.ini");
                }
                else
                {
                    String connetionString = "Data Source=" + Servername + ";Initial Catalog = " + DatabaseName + "; User ID = " + UserName + "; Password = " + Password;
                    en = Crypto.Encrypt(connetionString);
                    EditconnetionString(en, "connetionString.ini");
                }
               

                String EazzyTillNo = txtEazzyTillNo.Text;
                String EazzyUrl = txtEazzyUrl.Text;
                String MpesaTillNo = txtMpesaTillNo.Text;
                String mpesaUrl = txtmpesaUrl.Text;               

                if (!String.IsNullOrEmpty(EazzyTillNo) && !String.IsNullOrEmpty(EazzyUrl))
                {
                    //save eazzy url and till
                    String enEazzyTillNo = Crypto.Encrypt(EazzyTillNo);
                    String enEazzyUrl = Crypto.Encrypt(EazzyUrl);
                    EditTillFile(enEazzyTillNo, "EazzyTill");
                    EditTillFile(enEazzyUrl, "EazzyUrl");
                }
                if (!String.IsNullOrEmpty(MpesaTillNo) && !String.IsNullOrEmpty(mpesaUrl))
                {
                    //save mpesa url adn till
                    String enMpesaTillNo = Crypto.Encrypt(MpesaTillNo);
                    String enmpesaUrl = Crypto.Encrypt(mpesaUrl);
                    EditTillFile(enMpesaTillNo, "MpesaTill");
                    EditTillFile(enmpesaUrl, "MpesaUrl");
                }
                
               
                // String conn = Crypto.Decrypt(en);
                MessageBox.Show("Configurations Saved");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            txtPassword.PasswordChar = '*';
            // log.Info("Hello logging world!");
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
