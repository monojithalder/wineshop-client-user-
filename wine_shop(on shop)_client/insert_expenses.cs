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
    public partial class insert_expenses : Form
    {
        public insert_expenses()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Class1.Cn.Open();
                decimal amount = Convert.ToDecimal(this.txtamount.Text);
                if (this.comboBox1.Text == "dr")
                {
                    OleDbCommand cmd = new OleDbCommand("insert into incexp (head_name,dr,cr,exp_date) values ('" + this.txthead.Text + "'" +
                        "," + amount + ",0,'" + DateTime.Now + "')", Class1.Cn);
                    cmd.ExecuteNonQuery();
                    Class1.Cn.Close();
                    MessageBox.Show("Expenses Inserted");
                }
                else
                {
                    OleDbCommand cmd = new OleDbCommand("insert into incexp (head_name,cr,dr,exp_date) values ('" + this.txthead.Text + "'" +
                        "," + amount + ",0,'" + DateTime.Now + "')", Class1.Cn);
                    cmd.ExecuteNonQuery();
                    Class1.Cn.Close();
                    MessageBox.Show("Expenses Inserted");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void insert_expenses_Load(object sender, EventArgs e)
        {

        }
    }
}
