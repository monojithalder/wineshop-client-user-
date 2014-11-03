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
    public partial class insert_opening_stock : Form
    {
        public insert_opening_stock()
        {
            InitializeComponent();
        }

        private void insert_opening_stock_Load(object sender, EventArgs e)
        {
            int flag1 = check_opening_stock();
            if (flag1 < 1)
            {
                Class1.Cn.Open();
                DateTime dt = DateTime.Now.AddMonths(-1);
                int month = dt.Month;
                int year = dt.Year;
                int flag = 0;
                DateTime today = DateTime.Now;
                OleDbCommand cmd = new OleDbCommand("select * from closing_stock", Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    if (rd.GetDateTime(2).Year == year)
                    {
                        if (rd.GetDateTime(2).Month == month)
                        {
                            flag++;
                            OleDbCommand cmd1 = new OleDbCommand("insert into opening_stock(product_ml,opening_date,product_name,product_price" +
                                ",ml_per_bottle,product_type) values(" + rd.GetInt32(1) + ",'" + today + "','" + rd.GetString(3) + "'," +
                                "" + Convert.ToDecimal(rd.GetValue(4)) + "," + rd.GetInt32(5) + ",'" + rd.GetString(6) + "')", Class1.Cn);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }

                rd.Read();
                Class1.Cn.Close();
                if (flag > 0)
                {
                    MessageBox.Show("Opening Stock Inserted");
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Nothing Insertd");
                    this.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Opening Stock Already Inserted");
            }
        }
        private int check_opening_stock()
        {
            int flag = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from opening_stock", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (DateTime.Now.Month == rd.GetDateTime(2).Month && DateTime.Now.Year == rd.GetDateTime(2).Year)
                {
                    flag++;
                }
            }
            rd.Close();
            Class1.Cn.Close();
            return flag;
        }
    }
}
