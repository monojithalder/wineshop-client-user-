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
    public partial class table_option : Form
    {
        public table_option()
        {
            InitializeComponent();
        }
        public delegate void AddressUpdateHandler(object sender, AddressUpdateEventArgs e);
        public event AddressUpdateHandler AddressUpdated;
        public int bill_no;
        public int old_table_no;
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        int row_index = -1;
        public class AddressUpdateEventArgs : System.EventArgs
        {
            // add local member variables to hold text
            private int cre;
            //private string mCity;
            //private string mState;
            //private string mZipCode;

            // class constructor
            public AddressUpdateEventArgs(int cre)
            {
                this.cre = cre;

            }

            // Properties - Viewable by each listener

            public int _creditor
            {
                get
                {
                    return cre;
                }
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("update billing set table_no=" + Convert.ToInt32(this.txtnew.Text) + " where id=" + bill_no + " and status='no'", Class1.Cn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("update table_gross_product set table_no=" + Convert.ToInt32(this.txtnew.Text) + " where bill_no=" + bill_no + " and status='no'", Class1.Cn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("update table_product set table_no=" + Convert.ToInt32(this.txtnew.Text) + " where bill_no=" + bill_no + " and status='no'", Class1.Cn);
            cmd.ExecuteNonQuery();
            Class1.Cn.Close();
            int new_table_no = Convert.ToInt32(this.txtnew.Text);
            AddressUpdateEventArgs args = new AddressUpdateEventArgs(new_table_no);
            AddressUpdated(this, args);
            this.Dispose();
        }

        private void table_option_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("id");
            dt.Columns.Add("Table_no");
            dt.Columns.Add("Product_name");
            dt.Columns.Add("Product_Price");
            dt.Columns.Add("Pro_date");
            dt.Columns.Add("Pro_type");
            dt.Columns.Add("quantity");
            dt.Columns.Add("Total_ml");
            dt.Columns.Add("Bill_no");
            dt.Columns.Add("satus");
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[8].Visible = false;
            this.dataGridView1.Columns[9].Visible = false;
            this.txtold.Text = old_table_no.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            show_data();
        }
        public void show_data()
        {
            dt.Clear();
            this.dataGridView1.DataSource = bs;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from table_product where table_no=" + Convert.ToInt32(this.txttableno.Text) + " and status='no'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dt.Rows.Add(rd.GetInt32(0), rd.GetInt32(1), rd.GetString(2), rd.GetValue(3), rd.GetDateTime(4), rd.GetString(5), rd.GetInt32(6), rd.GetInt32(7), rd.GetInt32(8), rd.GetString(9));
            }
            rd.Read();
            Class1.Cn.Close();
        }
        public void delete_data()
        {
            if (row_index >= 0)
            {
                int id = Convert.ToInt32(this.dataGridView1.Rows[row_index].Cells[0].Value);
                int quan = Convert.ToInt32(this.dataGridView1.Rows[row_index].Cells[6].Value);
                // int price = Convert.ToInt32(this.dataGridView1.Rows[row_index].Cells[6].Value);
                int temp_quan = quan;
                int total_ml = Convert.ToInt32(this.dataGridView1.Rows[row_index].Cells[7].Value);
                if (quan > 1)
                {
                    int per_ml = total_ml / quan;
                    int new_quan = temp_quan - 1;
                    decimal per_price = Convert.ToDecimal(this.dataGridView1.Rows[row_index].Cells[3].Value) / Convert.ToInt32(this.dataGridView1.Rows[row_index].Cells[6].Value);
                    decimal total_price = Convert.ToDecimal(this.dataGridView1.Rows[row_index].Cells[3].Value);
                    decimal new_price = total_price - per_price;
                    Class1.Cn.Open();
                    OleDbCommand cmd = new OleDbCommand("update table_product set quantity=" + new_quan + ",total_ml=" + per_ml * new_quan + ",product_price=" + new_price + " where id=" + id + "", Class1.Cn);
                    cmd.ExecuteNonQuery();
                    this.dataGridView1.Rows[row_index].Cells[6].Value = new_quan;
                    Class1.Cn.Close();
                    MessageBox.Show("Deleted");


                }
                else
                {
                    Class1.Cn.Open();
                    OleDbCommand cmd = new OleDbCommand("delete from table_product where id=" + id + "", Class1.Cn);
                    cmd.ExecuteNonQuery();
                    this.dataGridView1.Rows.RemoveAt(row_index);
                    Class1.Cn.Close();
                    MessageBox.Show("deleted");
                }
                show_data();
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            row_index = Convert.ToInt32(this.dataGridView1.SelectedCells[0].RowIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            delete_data();
        }
    }
}
