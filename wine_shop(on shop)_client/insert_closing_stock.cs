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
    public partial class insert_closing_stock : Form
    {
        public insert_closing_stock()
        {
            InitializeComponent();
        }

        private void insert_closing_stock_Load(object sender, EventArgs e)
        {
            int flag1 = check_closing_stock();
            if (flag1 < 1)
            {
                Class1.Cn.Open();
                DateTime dt = DateTime.Now;
                int month = dt.Month;
                int year = dt.Year;
                int flag = 0;
                OleDbCommand cmd = new OleDbCommand("select * from stock where total_ml > 0", Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {

                    flag++;
                    OleDbCommand cmd1 = new OleDbCommand("insert into closing_stock(product_ml,closeing_date,product_name,product_price" +
                        ",ml_per_bottle,product_type) values(" + rd.GetInt32(7) + ",'" + dt + "','" + rd.GetString(14) + "'," +
                        "" + Convert.ToDecimal(rd.GetValue(8)) + "," + rd.GetInt32(3) + ",'" + rd.GetString(2) + "')", Class1.Cn);
                    cmd1.ExecuteNonQuery();

                }
                if (flag > 0)
                {
                    MessageBox.Show("Closing Stock Inserted");
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Nothing Insertd");
                    this.Dispose();
                }
                rd.Read();
                Class1.Cn.Close();
            }
            else
            {
                MessageBox.Show("Closing Stock Already Inserted");
                this.Dispose();
            }
        }
        private int check_closing_stock()
        {
            int flag = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from closing_stock", Class1.Cn);
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
