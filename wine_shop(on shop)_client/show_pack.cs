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
    public partial class show_pack : Form
    {
        public show_pack()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        private void show_stock_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Name");
            dt.Columns.Add("Short Name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Size");
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from pack_detail", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dt.Rows.Add(rd.GetString(1), rd.GetString(2), rd.GetValue(3), rd.GetString(4));
            }
            rd.Close();
            Class1.Cn.Close();

        }

       
    }
}
