using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
namespace RemoteMontring
{
    public partial class Patient : Form
    {
        private string temprture;
        public Patient(string p, string d)
        {
            InitializeComponent();
            id_doc.Text = d;
           name_txt.Text = p;
           DoctorName();
            FillLabels();
           show_temp();
            connectwithclient();
           // Fillbox();
           // FillData();
        }
        void DoctorName()
        {
            string DoctorName = "select * from doctor where Id='" + id_doc.Text + "';";
            string constring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\tahani\Documents\Data.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conDB = new SqlConnection(constring);
            conDB.Open();
            SqlCommand quiry = new SqlCommand(DoctorName, conDB);
            SqlDataReader reader = quiry.ExecuteReader();
            if (reader.Read())
            {
                label8.Text = reader["Name"].ToString();
                reader.Close();

            }
        }
        void FillLabels()
        {
            string str;
            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\tahani\Documents\Data.mdf;Integrated Security=True;Connect Timeout=30;");
            str = "select * from Patient where PName='" + name_txt.Text + "' ";
            
            con1.Open();
            SqlCommand sda = new SqlCommand(str, con1);
            SqlDataReader reader = sda.ExecuteReader();
            if (reader.Read())
            {
                
           id_txt.Text = reader["PatientID"].ToString();
                diseas_txt.Text = reader.GetString(reader.GetOrdinal("Disease"));
                address_txt.Text = reader["Address"].ToString();
                age_txt.Text = reader["Age"].ToString();
                phone_txt.Text = reader["PPhone"].ToString();
                reader.Close();
                con1.Close();
            }
        }

        void show_temp() {
            try
            {
            string DoctorName = "select * from temp_p where IDPatient='" + id_txt.Text + "';";
            string constring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\tahani\Documents\Data.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conDB = new SqlConnection(constring);
            SqlCommand quiry = new SqlCommand(DoctorName, conDB);
            
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = quiry;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbdataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbdataset);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

            
        private void Patient_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataDataSet2.Table' table. You can move, or remove it, as needed.
           // this.tableTableAdapter.Fill(this.dataDataSet2.Table);
            // TODO: This line of code loads data into the 'dataDataSet1.temp_p' table. You can move, or remove it, as needed.
            this.temp_pTableAdapter.Fill(this.dataDataSet1.temp_p);

           
            // TODO: This line of code loads data into the 'dataDataSet.Temprture' table. You can move, or remove it, as needed.

        }

        void connectwithclient()
        {
            SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            Socket slistener = null;
            slistener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//declaer socket
            IPAddress ipadd = IPAddress.Parse("127.168.1.1");
            while (true)
            {
                slistener.Bind(new IPEndPoint(0, 8001));
                slistener.Listen(10);//lisen into 10 clinet at same time
                slistener.Accept();

                byte[] buf = new byte[8192];
                int rec = slistener.Receive(buf, buf.Length, 0);
                string temprture = Encoding.Unicode.GetString(buf, 0, rec);
                byte[] buff = new byte[8192];
                int rec1 = slistener.Receive(buff, buff.Length, 0);
                String datetime = Encoding.Unicode.GetString(buff, 0, rec1);
                DateTime date = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm tt", null);
                byte[] buf2 = new byte[8192];
                int rec2 = slistener.Receive(buf2, buf2.Length, 0);
                String id = Encoding.Unicode.GetString(buf2, 0, rec2);
                while (slistener.Available > 0)
                {
                    rec = slistener.Receive(buf);
                    temprture += Encoding.Unicode.GetString(buf, 0, rec);
                    rec1 = slistener.Receive(buff);
                    datetime += Encoding.Unicode.GetString(buff, 0, rec1);//rename as columm on table this is temprture 

                    date = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm tt", null);// rename as columm on table this is time that type is datatime
                    rec2 = slistener.Receive(buf2);
                    id += Encoding.Unicode.GetString(buf2, 0, rec2);
                    int numVal = Convert.ToInt32(id);// rename as columm on table this is id 
                    string constring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\tahani\Documents\Data.mdf;Integrated Security=True;Connect Timeout=30;";
                    string query = "INSERT INTO temp_p (IDPatient, temprture, date) VALUES (@id, @temprture, @date;";//aplly quiry to database
                    SqlConnection conDB = new SqlConnection(constring);
                    conDB.Open();
                    SqlCommand cmdDB = new SqlCommand(query, conDB);

                }
            }
        }
        

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void temprtureBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
            
        
        
    }
}
    

