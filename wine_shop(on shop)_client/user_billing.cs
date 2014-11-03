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
    public partial class user_billing : Form
    {
        public user_billing()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        string[] user_id;
        int arr = 0;
        int total_amount = 0;
        private void user_billing_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("USER ID");
            dt.Columns.Add("BILLING TIME");
            dt.Columns.Add("TOTAL AMOUNT");
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs;
            insitilise_arrey_bond();
            insert_into_array();
        }
        private void show_data()
        {
            int month = Convert.ToDateTime(this.dateTimePicker1.Value).Month;
            // int date=Convert.ToDateTime(this.dateTimePicker1.Value).Date;
            int year = Convert.ToDateTime(this.dateTimePicker1.Value).Year;
            Class1.Cn.Open();
            for (int i = 0; i < arr; i++)
            {
                OleDbCommand cmd = new OleDbCommand("select * from user_biling where user_name='" + user_id[i] + "'", Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    if (rd.GetDateTime(3).Year == year)
                    {
                        if (rd.GetDateTime(3).Month == month && rd.GetDateTime(3).Date == Convert.ToDateTime(this.dateTimePicker1.Value).Date)
                        {
                            total_amount += rd.GetInt32(2);
                        }
                    }
                }
                dt.Rows.Add(user_id[i], this.dateTimePicker1.Value, total_amount);
                total_amount = 0;
            }
            Class1.Cn.Close();



        }
        private void insitilise_arrey_bond()
        {
            int flag = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from user_ac", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Read();
            Class1.Cn.Close();
            user_id = new string[flag];
        }
        private void insert_into_array()
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from user_ac", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                user_id[arr] = rd.GetString(0);
                arr++;
            }
            rd.Close();
            Class1.Cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dt.Clear();
            this.dataGridView1.DataSource = bs;
            show_data();
        }

      
       
      

      

     
    }
}
