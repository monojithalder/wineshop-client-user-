using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Security.AccessControl;

namespace wine_shop_on_shop__client
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        int flag = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from user_ac", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.GetString(0) == this.txtuser.Text && rd.GetString(1) == this.txtpass.Text)
                {
                    flag++;
                    Class1.user = rd.GetString(0);
                }
            }
            rd.Close();
            Class1.Cn.Close();
            if (flag > 0)
            {
                //string time = DateTime.Now.ToString("HH:mm:ss tt");
                string time = DateTime.Now.ToShortTimeString();
                Class1.Cn.Open();
                cmd = new OleDbCommand("insert into user_login (username,log_time,lod_date) values ('" + Class1.user + "','" + time + "','" + DateTime.Now + "')", Class1.Cn);
                cmd.ExecuteNonQuery();
                cmd = new OleDbCommand("select * from user_login order by id DESC", Class1.Cn);
                rd = cmd.ExecuteReader();
                rd.Read();
                Class1.login_id = rd.GetInt32(0);
                rd.Close();
                Class1.Cn.Close();
                //first_time ft = new first_time();
                //ft.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong userid or password");
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            string a = Environment.CurrentDirectory;
            string acc = "Everyone";
            //string user = Environment.UserDomainName + "\\" + Environment.UserName;
            string user = Environment.UserName;
            DirectorySecurity ds = Directory.GetAccessControl(a);

            FileSystemAccessRule fsa = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            ds.AddAccessRule(fsa);
            Directory.SetAccessControl(a, ds);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
