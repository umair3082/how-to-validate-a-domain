using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace domainValidator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://www." + txtDomain.Text.Trim();

            WebResponse response = null;

            string data = string.Empty;

            try

            {

                WebRequest request = WebRequest.Create(url);

                response = request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))

                {

                    data = reader.ReadToEnd();

                }

                // domain exists, this is valid domain

                lblText.Text = "Valid domain name !";
                lblText.ForeColor = Color.Green;

            }

            catch (WebException ee)

            {

                // return false, most likely this domain doesn't exists

                lblText.Text = "Most probably this domain name doesn't exists." + ee.ToString();
                lblText.ForeColor = Color.Red;

            }

            catch (Exception ee)

            {

                // Some error occured, the domain might exists 

                lblText.Text = "Error occured. " + ee.ToString();

            }

            finally

            {

                if (response != null) response.Close();

            }

            //txtBox1.Text = data;
        }
    }
}
