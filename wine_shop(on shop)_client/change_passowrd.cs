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
    public partial class change_passowrd : Form
    {
        public change_passowrd()
        {
            InitializeComponent();
        }
        int flag = 0, flag2 = 0;
        private void change_passowrd_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flag1 = 0;
            int flag3 = 0;
            if (this.txtuser.Text != "")
            {
                flag1 = check_user_id(this.txtuser.Text);
                flag3 = check_passowrd(this.txtold.Text, this.txtuser.Text);
            }
            else
            {
                MessageBox.Show("Please Enter User Id");
            }
            if (flag1 > 0)
            {
                if (this.txtnew.Text != "" && this.txtold.Text != "")
                {
                    if (flag3 > 0)
                    {
                        Class1.Cn.Open();
                        OleDbCommand cmd = new OleDbCommand("update user_ac set pass='" + this.txtnew.Text + "' where userid='" + this.txtuser.Text + "'", Class1.Cn);
                        cmd.ExecuteNonQuery();
                        Class1.Cn.Close();
                        MessageBox.Show("Password Changed");
                        this.txtuser.Text = "";
                        this.txtold.Text = "";
                        this.txtnew.Text = "";
                        this.txtuser.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Old Passowrd Not Correct");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Old And New Password");
                }
            }
        }
        private int check_user_id(string user_id)
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from user_ac where userid='" + user_id + "'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Close();
            Class1.Cn.Close();
            return flag;
        }
        private int check_passowrd(string passsword, string user_id)
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from user_ac where userid='" + user_id + "'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.GetString(1) == passsword)
                {
                    flag2++;
                }
            }
            rd.Close();
            Class1.Cn.Close();
            return flag2;
        }

       
    }
}
