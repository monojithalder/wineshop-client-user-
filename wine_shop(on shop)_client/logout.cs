using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace wine_shop_on_shop__client
{
    public partial class logout : Form
    {
        public logout()
        {
            InitializeComponent();
        }

        private void logout_Load(object sender, EventArgs e)
        {
            string user = Class1.user;
            if (user == "admin")
            {
                //admin_panel ap = new admin_panel();
                //ap.Hide();
                //ap.Dispose();
                Class1.Cn.Open();
                //string time = DateTime.Now.ToString("HH:mm:ss tt");
                string time = DateTime.Now.ToShortTimeString();
                OleDbCommand cmd = new OleDbCommand("update user_login set logout_time='" + time + "' where id=" + Class1.login_id + "", Class1.Cn);
                cmd.ExecuteNonQuery();
                Class1.Cn.Close();
                //login lg = new login();
                //lg.Show();
                // this.Dispose();
                Application.Restart();
            }
            else
            {
                Class1.Cn.Open();
                string time;
                time = DateTime.Now.ToShortTimeString();
                OleDbCommand cmd = new OleDbCommand("update user_login set logout_time='" + time + "' where id=" + Class1.login_id + "", Class1.Cn);
                cmd.ExecuteNonQuery();
                Class1.Cn.Close();
                //user_control_panel ucp = new user_control_panel();
                //ucp.Dispose();
                //login lg = new login();
                //lg.Show();
                //this.Dispose();
                Application.Restart();
            }
        }
    }
}
