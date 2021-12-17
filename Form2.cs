using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net;
using System.IO;

namespace domainValidator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        
        public static  string conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+Application.StartupPath+ "/domainChecker.mdb;";
        OleDbConnection con = new OleDbConnection(conString);
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        readonly DataTable dt = new DataTable();
        private void add(string name)
        {
            //SQL STMT
            const string sql = "INSERT INTO domains(domainName) VALUES(@NAME)";
            cmd = new OleDbCommand(sql, con);

            //ADD PARAMS
            cmd.Parameters.AddWithValue("@NAME", name);
            

            //OPEN CON AND EXEC INSERT
            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    //clearTxts();
                    MessageBox.Show(@"Successfully Inserted");
                }
                con.Close();
                //retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        private void update(int id, string name, string propellant, string destination)
        {
            //SQL STATEMENT
            string sql = "UPDATE spacecraftsTB SET S_Name='" + name + "',S_Propellant='" + propellant + "',S_Destination='" + destination + "' WHERE ID=" + id + "";
            cmd = new OleDbCommand(sql, con);

            //OPEN CONNECTION,UPDATE,RETRIEVE DATAGRIDVIEW
            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd)
                {
                    UpdateCommand = con.CreateCommand()
                };
                adapter.UpdateCommand.CommandText = sql;
                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    //clearTxts();
                    MessageBox.Show(@"Successfully Updated");
                }
                con.Close();

                //REFRESH DATA
                //retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void delete(int id)
        {
            //SQL STATEMENTT
            String sql = "DELETE FROM spacecraftsTB WHERE ID=" + id + "";
            cmd = new OleDbCommand(sql, con);

            //'OPEN CONNECTION,EXECUTE DELETE,CLOSE CONNECTION
            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                //PROMPT FOR CONFIRMATION BEFORE DELETING
                if (MessageBox.Show(@"Are you sure to permanently delete this?", @"DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show(@"Successfully deleted");
                    }
                }
                con.Close();
                //retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add(txtDomain.Text);
        }
        string status;
        public void checkavailablity(string id,string domain)
        {
            string url = "http://www." + domain.Trim();

            WebResponse response = null;

            string data = string.Empty;

            try

            {

                WebRequest request = WebRequest.Create(url);

                response = request.GetResponse();

                //using (StreamReader reader = new StreamReader(response.GetResponseStream()))

                //{

                //    data = reader.ReadToEnd();

                //}

                // domain exists, this is valid domain
                
                status=  "Not Available";
                
               // lblText.ForeColor = Color.Green;

            }

            catch (WebException ee)

            {

                // return false, most likely this domain doesn't exists

                status=  "Available";
                
                //lblText.ForeColor = Color.Red;

            }

            catch (Exception ee)

            {

                // Some error occured, the domain might exists 

                MessageBox.Show(  "Error occured. " + ee.ToString());

            }

            finally

            {

                if (response != null) response.Close();
                this.dataGridView1.Rows.Add(id, domain, status);

            }
            

        }
        private void retrieve()
        {
            dataGridView1.Rows.Clear();
            //SQL STATEMENT
            String sql = "SELECT * FROM domains ";
            cmd = new OleDbCommand(sql, con);
            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
                //LOOP THROUGH DATATABLE
                foreach (DataRow row in dt.Rows)
                {

                    checkavailablity(row[0].ToString(), row[1].ToString());
                        
                       
                }

                con.Close();
                //CLEAR DATATABLE
                dt.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            retrieve();
        }
    }
}
