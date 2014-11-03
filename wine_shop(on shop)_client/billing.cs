using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing.Printing;

namespace  wine_shop_on_shop__client

{
    public partial class billing : Form
    {
        public billing()
        {
            InitializeComponent();
        }
        public DataTable dt = null;
        public DataTable dt1 = null;
        public DataTable dt2 = null;
        public BindingSource bs2 = null;
        public BindingSource bs = null;
        public BindingSource bs1 = null;
        public decimal total_amount = 0;
        public int quan;
        int address;
        int bill_no;
        int rowindex;
        int row_index1 = -1;
        string product_name;
        public Decimal billamount;
        public double total;
        public PrintDocument pd;
        string bar;
        int quan1;
        string size;
        string method, vat_stat;
        decimal vat_persentage = 0;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int flag = 0;
            if (this.textBox1.Text != "")
            {
                Class1.Cn.Open();
                OleDbCommand cmd = new OleDbCommand("select * from pack_detail where name='" + this.textBox1.Text.ToLower() + "' or short_name='" + this.textBox1.Text.ToLower() + "'", Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    
                    
                    
                        dt.Rows.Add(rd.GetString(1) + "(" + rd.GetValue(4) + ")", rd.GetValue(4), rd.GetString(1), rd.GetValue(6), rd.GetInt32(0), rd.GetString(7));

                   
                }
                rd.Close();
                cmd = new OleDbCommand("select * from stock where product_name='"+this.textBox1.Text+"' and product_type='bear'",Class1.Cn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dt.Rows.Add(rd.GetString(14),rd.GetInt32(3),rd.GetString(14),rd.GetString(1),rd.GetInt32(0),rd.GetString(2));
                }
                rd.Close();
                Class1.Cn.Close();
            }
            else
            {
                Class1.Cn.Close();
                dt.Clear();
                this.gridshow.Refresh();
            }

        }
        
        private void billing_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            bs = new BindingSource();
            dt1 = new DataTable();
            bs1 = new BindingSource();
            dt2 = new DataTable();
            bs2 = new BindingSource();
            dt.Columns.Add("Product_name");
            dt.Columns.Add("Size");
            dt.Columns.Add("Name");
            dt.Columns.Add("barcode");
            dt.Columns.Add("id");
            dt.Columns.Add("product_type");
            bs.DataSource = dt;
            this.gridshow.DataSource = bs;
            this.gridshow.Columns[2].Visible = false;
            this.gridshow.Columns[3].Visible = false;
            this.gridshow.Columns[4].Visible = false;
            dt1.Columns.Add("Product_name");
            dt1.Columns.Add("Price");
            dt1.Columns.Add("Size");
            dt1.Columns.Add("Quantity");
            dt1.Columns.Add("Barcode");
            dt1.Columns.Add("Product_type");
            bs1.DataSource = dt1;
            this.gridbill.DataSource = bs1;
            this.gridbill.Columns[4].Visible = false;
            this.gridbill.Columns[5].Visible = false;
            dt2.Columns.Add("Table no");
            dt2.Columns.Add("bill_no");
            bs2.DataSource = dt2;
            this.dataGridView1.DataSource = bs2;
            this.dataGridView1.Columns[1].Visible = false;
            //this.Focus();

        }

        private void gridshow_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("asd");
            //try
            //{ string pro_name = find_product_name(rd.GetString(6));
               
                Class1.Cn.Open();
                int flag = 0;
                //dt1.Clear();
                //this.gridbill.DataSource = bs1;
                string name = this.gridshow.Rows[this.gridshow.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
                string size = this.gridshow.Rows[this.gridshow.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                string barcode = this.gridshow.Rows[this.gridshow.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
                int id = Convert.ToInt32(this.gridshow.Rows[this.gridshow.SelectedCells[0].RowIndex].Cells[4].Value);
                string type = this.gridshow.Rows[this.gridshow.SelectedCells[0].RowIndex].Cells[5].Value.ToString();
                int flag1 = 0;
                //Class1.Cn.Close();
                int pack_size = find_product_size(size);
                string pro_name = find_product_name(barcode);
                //Class1.Cn.Open();
                OleDbCommand cmd1 = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pack_size + "", Class1.Cn);
                OleDbDataReader rd1 = cmd1.ExecuteReader();
                while (rd1.Read())
                {
                    flag1++;
                }
                if (flag1 > 0)
                {

                    
                    for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                    {
                        if (this.gridbill.Rows[i].Cells[0].Value.ToString() == name && this.gridbill.Rows[i].Cells[2].Value.ToString() == size)
                        {
                            quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                            flag++;
                            address = i;
                        }
                    }
                    if (flag > 0)
                    {

                        
                        //OleDbCommand cmd = new OleDbCommand("select * from pack_detail where name='"+name+"' and size1='"+size+"'",Class1.Cn);
                        OleDbCommand cmd = new OleDbCommand("select * from pack_detail where id=" + id + "", Class1.Cn);

                        OleDbDataReader rd = cmd.ExecuteReader();
                        rd.Read();
                        //dt1.Rows.Add(name, rd.GetValue(3), size,quan+1, barcode);
                        this.gridbill.Rows[address].Cells[3].Value = quan + 1;
                        total_billamount();
                        //billamount = 0;
                        rd.Close();
                    }
                    else
                    {
                        if (type != "bear")
                        {
                            //OleDbCommand cmd = new OleDbCommand("select * from pack_detail where name='" + name + "' or short_name='"+name+"' and size1='"+size+"'", Class1.Cn);
                            OleDbCommand cmd = new OleDbCommand("select * from pack_detail where id=" + id + "", Class1.Cn);

                            OleDbDataReader rd = cmd.ExecuteReader();
                            rd.Read();
                            dt1.Rows.Add(name, rd.GetValue(3), size, '1', barcode, type);
                            total_billamount();
                            // billamount = 0;
                            rd.Close();
                        }
                        else
                        {
                            OleDbCommand cmd = new OleDbCommand("select * from stock where id=" + id + "", Class1.Cn);

                            OleDbDataReader rd = cmd.ExecuteReader();
                            rd.Read();
                            dt1.Rows.Add(name, rd.GetValue(9), size, '1', barcode, type);
                            total_billamount();
                            // billamount = 0;
                            rd.Close();
                        }
                    }
                    this.textBox1.Focus();
                    Class1.Cn.Close();
                }
                else
                {
                    Class1.Cn.Close();
                    MessageBox.Show("This pack stock is not avilable");
                }
            //}
            //catch(Exception e1)
            //{
            //    Class1.Cn.Close();
            //    MessageBox.Show("Error"+e1.Message);
            //}

        }
        public void total_billamount()
        {
            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
            {
                billamount = Convert.ToDecimal(this.txttotalamount.Text)+ Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);
            }
            this.txttotalamount.Text = billamount.ToString();
           // billamount = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void billing_KeyUp(object sender, KeyEventArgs e)
        {

            MessageBox.Show("knkn");
        }

        private void button17_Click(object sender, EventArgs e)//insert button
        {
            try
            {
                this.txttotalamount.Text = "0";
                int fl = check_table_exsit(Convert.ToInt32(this.txttableno.Text));
                int ch = check_bill(Convert.ToInt32(this.txttableno.Text));
                int table_no = Convert.ToInt32(this.txttableno.Text);
                if (fl > 0)
                {
                    if (ch < 1)
                    {
                        Class1.Cn.Open();
                        
                        // int flag = 0;
                        DateTime pro_date = DateTime.Now;
                        OleDbCommand cmd = new OleDbCommand("insert into billing(pro_date,table_no,status,username" +
                            ") values('" + pro_date + "'," + table_no + ",'no','"+Class1.user+"')", Class1.Cn);
                        cmd.ExecuteNonQuery();
                        //int bill_no;
                        cmd = new OleDbCommand("select * from billing where table_no=" + table_no + " and status='no'", Class1.Cn);
                        OleDbDataReader rd = cmd.ExecuteReader();
                        rd.Read();
                        bill_no = rd.GetInt32(0);
                        DateTime today = DateTime.Now;
                        for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                        {
                            if (this.gridbill.Rows[i].Cells[5].Value.ToString() != "bear")
                            {
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_ml = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                int quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                                int total_ml = pro_ml * quan;
                                string type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * quan;
                                cmd = new OleDbCommand("insert into table_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                    ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_gross_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                   ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                int pro_ml = Convert.ToInt32(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                int quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                                int total_ml = pro_ml * quan;
                                string type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * quan;
                                cmd = new OleDbCommand("insert into table_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                    ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_gross_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                   ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }

                        }
                        //dt2.Rows.Add(table_no,bill_no);
                        dt1.Clear();
                        this.gridbill.DataSource = bs1;
                        Class1.Cn.Close();
                        insert_table_no();
                    }
                    else
                    {
                        Class1.Cn.Open();
                        OleDbCommand cmd = new OleDbCommand("select * from billing where table_no=" + table_no + " and status='no'", Class1.Cn);
                        OleDbDataReader rd = cmd.ExecuteReader();
                        rd.Read();
                        bill_no = rd.GetInt32(0);
                        DateTime today = DateTime.Now;
                        for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                        {
                            if (this.gridbill.Rows[i].Cells[5].Value.ToString() != "bear")
                            {
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_ml = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                int quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                                int total_ml = pro_ml * quan;
                                string type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * quan;
                                cmd = new OleDbCommand("insert into table_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                    ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_gross_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                   ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                int pro_ml = Convert.ToInt32(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                int quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                                int total_ml = pro_ml * quan;
                                string type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * quan;
                                cmd = new OleDbCommand("insert into table_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                    ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_gross_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                   ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }

                        }
                        //dt2.Rows.Add(table_no,bill_no);
                        dt1.Clear();
                        this.gridbill.DataSource = bs1;
                        Class1.Cn.Close();
                        this.txttableno.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Table no does not exsit");
                }
            }
            catch
            {
                  Class1.Cn.Close();
                  MessageBox.Show("Please Enter Table No");
            }
        }
        private int check_bill(int table_no)
        {
            Class1.Cn.Open();
            int flag = 0;
            OleDbCommand cmd = new OleDbCommand("select * from billing where table_no="+table_no+" and status='no'",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Close();
            Class1.Cn.Close();
            return flag;
        }
        private void insert_table_no()
        {
            
            //bs.Clear();
            //this.dataGridView1.Rows.Clear();
            dt2.Clear();
            this.dataGridView1.DataSource = bs2;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from billing where status='no'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dt2.Rows.Add(rd.GetInt32(2),bill_no);
            }
            rd.Close();
            Class1.Cn.Close();
            this.txttableno.Text = "";

        }
        private void clear_datagrid()
        {
            dt2.Rows.Clear();
            this.dataGridView1.DataSource = bs2;
            MessageBox.Show("asda");
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //try
            //{
            rowindex = Convert.ToInt32(this.dataGridView1.SelectedCells[0].RowIndex);
            //MessageBox.Show(key.ToString());
            //if (key == Keys.Alt)
            //{

            //}
            //int tbo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
            //int bilo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[1].Value);
            //table_option to = new table_option();
            //to.bill_no = bilo;
            //to.old_table_no = tbo;
            //to.AddressUpdated += new table_option.AddressUpdateHandler(AddressForm_ButtonClicked);
            //to.Show();
            //}
            //catch
            //{
            //}

        }
        private void AddressForm_ButtonClicked(object sender, wine_shop_on_shop__client.table_option.AddressUpdateEventArgs e)
        {
            // update the forms values from the event args
            //MessageBox.Show(e._creditor.ToString());
            //for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            //{
            //    int tbo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
            //    if (Convert.ToInt32(this.dataGridView1.Rows[i].Cells[0].Value) == tbo)
            //    {
            //        //MessageBox.Show("knknkn");
            //        this.dataGridView1.Rows[i].Cells[0].Value = e._creditor;
            //        MessageBox.Show("Table No change");
            //    }

            //}
            //MessageBox.Show("asdas");
            clear_datagrid();
            insert_table_no();
            //this.txtsup.Text = e._creditor;
        }
       
        private void button1_Click(object sender, EventArgs e) //billing Button Clicking
        {
            try
            {
                if (this.txttableno.Text == "")
                {
                    MessageBox.Show("Please Select Table No");
                }
                else
                {
                    stock_maintain();
                    string user = Class1.user;
                    Class1.Cn.Open();
                    DateTime pro_date = DateTime.Now;
                    if (this.txttableno.Text != "")
                    {
                        int table_no = Convert.ToInt32(this.txttableno.Text);

                        OleDbCommand cmd = new OleDbCommand("select * from billing where table_no=" + table_no + " and status='no' order by id DESC", Class1.Cn);
                        OleDbDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            bill_no = rd.GetInt32(0);
                        }
                        rd.Close();
                        if (bill_no != 0)
                        {
                            //cmd = new OleDbCommand("insert into billing (pro_date) values ('" + pro_date + "')", Class1.Cn);
                            //cmd.ExecuteNonQuery();
                            //cmd = new OleDbCommand("select * from billing order by id DESC", Class1.Cn);
                            //OleDbDataReader rd = cmd.ExecuteReader();
                            //rd.Read();
                            //int bill_no = rd.GetInt32(0);

                            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                            {
                                if (Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value) == 1)
                                {
                                    string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                    decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);
                                    int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                    //string product_type = this.gridbill.Rows[i].Cells[4].Value.ToString();
                                    string product_type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                    cmd = new OleDbCommand("insert into bill_product (bill_no,product_name,product_price,product_quan,product_type) values (" + bill_no + "" +
                                        ",'" + pro_name + "'," + pro_price + "," + pro_quan + ",'" + product_type + "')", Class1.Cn);
                                    cmd.ExecuteNonQuery();
                                    cmd = new OleDbCommand("insert into incexp(head_name,dr,cr,exp_date) values ('sales'," + pro_price + ",0,'" + DateTime.Now + "')", Class1.Cn);
                                    cmd.ExecuteNonQuery();
                                    cmd = new OleDbCommand("insert into user_biling (user_name,price,bill_date) values ('" + user + "'," + pro_price + ",'" + DateTime.Now + "')", Class1.Cn);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    for (int j = 0; j < Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value); j++)
                                    {
                                        string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                        decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);
                                        //string product_type = this.gridbill.Rows[i].Cells[4].Value.ToString();
                                        string product_type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                        int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                        cmd = new OleDbCommand("insert into bill_product (bill_no,product_name,product_price,product_quan,product_type) values (" + bill_no + "" +
                                            ",'" + pro_name + "'," + pro_price + "," + pro_quan + ",'" + product_type + "')", Class1.Cn);
                                        cmd.ExecuteNonQuery();
                                        cmd = new OleDbCommand("insert into incexp(head_name,dr,cr,exp_date) values ('sales'," + pro_price + ",0,'" + DateTime.Now + "')", Class1.Cn);
                                        cmd.ExecuteNonQuery();
                                        cmd = new OleDbCommand("insert into user_biling (user_name,price,bill_date) values ('" + user + "'," + pro_price + ",'" + DateTime.Now + "')", Class1.Cn);
                                        cmd.ExecuteNonQuery();
                                    }

                                }

                            }
                            Class1.Cn.Close();
                            print_bill();
                            Class1.Cn.Open();

                        }
                        else
                        {
                            // int table_no = Convert.ToInt32(this.txttableno.Text);
                            // int flag = 0;
                            // DateTime pro_date = DateTime.Now;
                            cmd = new OleDbCommand("insert into billing(pro_date,table_no,status,username" +
                               ") values('" + pro_date + "'," + table_no + ",'no','" + Class1.user + "')", Class1.Cn);
                            cmd.ExecuteNonQuery();
                            //int bill_no;
                            cmd = new OleDbCommand("select * from billing where table_no=" + table_no + " and status='no'", Class1.Cn);
                            rd = cmd.ExecuteReader();
                            rd.Read();

                            bill_no = rd.GetInt32(0);
                            rd.Close();
                            //MessageBox.Show(bill_no.ToString());
                            DateTime today = DateTime.Now;
                            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                            {
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_ml = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                int quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value);
                                int total_ml = pro_ml * quan;
                                string product_type = this.gridbill.Rows[i].Cells[5].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * quan;
                                int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("insert into bill_product (bill_no,product_name,product_price,product_quan,product_type) values (" + bill_no + "" +
                                        ",'" + pro_name + "'," + pro_price + "," + total_ml + ",'" + product_type + "')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into user_biling (user_name,price,bill_date) values ('" + user + "'," + pro_price + ",'" + DateTime.Now + "')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                    ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + product_type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into incexp(head_name,dr,cr,exp_date) values ('sales'," + pro_price + ",0,'" + DateTime.Now + "')", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                cmd = new OleDbCommand("insert into table_gross_product (table_no,product_name,product_price,pro_date,pro_type,quantity,total_ml,bill_no,status" +
                                   ") values (" + table_no + ",'" + pro_name + "'," + pro_price + ",'" + today + "','" + product_type + "'," + quan + "," + total_ml + "," + bill_no + ",'no')", Class1.Cn);
                                cmd.ExecuteNonQuery();

                            }
                            Class1.Cn.Close();
                            print_bill();
                            Class1.Cn.Open();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Enter Table No");
                    }

                    Class1.Cn.Close();

                }
            }
            catch(Exception e1)
            {
                Class1.Cn.Close();
                MessageBox.Show(e1.Message);
            }
        }
        private void print_bill()
        {

            Class1.Cn.Open();
            int table_no = Convert.ToInt32(this.txttableno.Text);
            OleDbCommand cmd = new OleDbCommand("select * from billing where table_no=" + table_no + " and status='no'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int bill_no = rd.GetInt32(0);
            cmd = new OleDbCommand("insert into bill_print (bill_no) values (" + bill_no + ")", Class1.Cn);
            cmd.ExecuteNonQuery();
            rd.Close();
            

            cmd = new OleDbCommand("update billing set status='yes' where id="+bill_no+" and table_no="+table_no+"",Class1.Cn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("update table_gross_product set status='yes' where bill_no=" + bill_no + " and table_no=" + table_no + "", Class1.Cn);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand("update table_product set status='yes' where bill_no=" + bill_no + " and table_no=" + table_no + "", Class1.Cn);
            cmd.ExecuteNonQuery();
            Class1.Cn.Close();
            PaperSize pz = new PaperSize();
            //pz.Height = 200;
            pz.Width = 500;
            pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = pz;
            pd.PrintPage += new PrintPageEventHandler(this.pd_print);
            pd.Print();
        }
        private void stock_maintain()
        {
            try
            {
                string met = fetch_method();
                Class1.Cn.Open();
                if (method == "FIFO")
                {
                    OleDbCommand cmd;
                    for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                    {

                        if (Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value) == 1)
                        {
                            if (this.gridbill.Rows[i].Cells[5].Value.ToString() != "bear")
                            {
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id ASC", Class1.Cn);
                                OleDbDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                int id = rd.GetInt32(0);
                                int total_ml = rd.GetInt32(7);
                                int ml_per_bottle = rd.GetInt32(3);
                                int bottle_per_box = showbottle(ml_per_bottle);
                                int total_bottle;
                                int lose_bottle;
                                int no_of_case;
                                int remail_ml = total_ml - pro_quan;
                                total_bottle = remail_ml / ml_per_bottle;
                                no_of_case = total_bottle / bottle_per_box;
                                lose_bottle = total_bottle % bottle_per_box;
                                //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                                    ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                //Class1.Cn.Close();

                            }
                            else
                            {
                                string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                int pro_quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id ASC", Class1.Cn);
                                OleDbDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                int id = rd.GetInt32(0);
                                int total_ml = rd.GetInt32(7);
                                int ml_per_bottle = rd.GetInt32(3);
                                int bottle_per_box = showbottle(ml_per_bottle);
                                int total_bottle;
                                int lose_bottle;
                                int no_of_case;
                                int remail_ml = total_ml - pro_quan;
                                total_bottle = remail_ml / ml_per_bottle;
                                no_of_case = total_bottle / bottle_per_box;
                                lose_bottle = total_bottle % bottle_per_box;
                                //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                                    ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value); j++)
                            {
                                // Class1.Cn.Open();
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id ASC", Class1.Cn);
                                OleDbDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                int id = rd.GetInt32(0);
                                int total_ml = rd.GetInt32(7);
                                int ml_per_bottle = rd.GetInt32(3);
                                int bottle_per_box = showbottle(ml_per_bottle);
                                int total_bottle;
                                int lose_bottle;
                                int no_of_case;
                                int remail_ml = total_ml - pro_quan;
                                total_bottle = remail_ml / ml_per_bottle;
                                no_of_case = total_bottle / bottle_per_box;
                                lose_bottle = total_bottle % bottle_per_box;
                                //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                               ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                //Class1.Cn.Close();
                            }

                        }

                    }
                }
                else
                {
                    OleDbCommand cmd;
                    for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
                    {
                        
                            if (Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value) == 1)
                            {
                                if (this.gridbill.Rows[i].Cells[5].Value.ToString() != "bear")
                                {
                                    string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                    int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                    cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id DESC", Class1.Cn);
                                    OleDbDataReader rd = cmd.ExecuteReader();
                                    rd.Read();
                                    int id = rd.GetInt32(0);
                                    int total_ml = rd.GetInt32(7);
                                    int ml_per_bottle = rd.GetInt32(3);
                                    int bottle_per_box = showbottle(ml_per_bottle);
                                    int total_bottle;
                                    int lose_bottle;
                                    int no_of_case;
                                    int remail_ml = total_ml - pro_quan;
                                    total_bottle = remail_ml / ml_per_bottle;
                                    no_of_case = total_bottle / bottle_per_box;
                                    lose_bottle = total_bottle % bottle_per_box;
                                    //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                    decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                    cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                                        ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                    cmd.ExecuteNonQuery();
                                    //Class1.Cn.Close();
                            }
                            else
                            {
                                string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                int pro_quan = Convert.ToInt32(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id ASC", Class1.Cn);
                                OleDbDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                int id = rd.GetInt32(0);
                                int total_ml = rd.GetInt32(7);
                                int ml_per_bottle = rd.GetInt32(3);
                                int bottle_per_box = showbottle(ml_per_bottle);
                                int total_bottle;
                                int lose_bottle;
                                int no_of_case;
                                int remail_ml = total_ml - pro_quan;
                                total_bottle = remail_ml / ml_per_bottle;
                                no_of_case = total_bottle / bottle_per_box;
                                lose_bottle = total_bottle % bottle_per_box;
                                //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                                    ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        else
                        {
                            for (int j = 0; j < Convert.ToInt32(this.gridbill.Rows[i].Cells[3].Value); j++)
                            {
                                // Class1.Cn.Open();
                                string pro_name = find_product_name(this.gridbill.Rows[i].Cells[4].Value.ToString());
                                int pro_quan = find_product_size(this.gridbill.Rows[i].Cells[2].Value.ToString());
                                cmd = new OleDbCommand("select * from stock where product_name='" + pro_name + "' and total_ml > " + pro_quan + " order by id DESC", Class1.Cn);
                                OleDbDataReader rd = cmd.ExecuteReader();
                                rd.Read();
                                int id = rd.GetInt32(0);
                                int total_ml = rd.GetInt32(7);
                                int ml_per_bottle = rd.GetInt32(3);
                                int bottle_per_box = showbottle(ml_per_bottle);
                                int total_bottle;
                                int lose_bottle;
                                int no_of_case;
                                int remail_ml = total_ml - pro_quan;
                                total_bottle = remail_ml / ml_per_bottle;
                                no_of_case = total_bottle / bottle_per_box;
                                lose_bottle = total_bottle % bottle_per_box;
                                //string pro_name = this.gridbill.Rows[i].Cells[0].Value.ToString();
                                decimal pro_price = Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value);

                                cmd = new OleDbCommand("update stock set no_of_case=" + no_of_case + ",no_of_lose_bottle=" + lose_bottle + "" +
                               ",total_bottle=" + total_bottle + ",total_ml=" + remail_ml + " where id=" + id + "", Class1.Cn);
                                cmd.ExecuteNonQuery();
                                //Class1.Cn.Close();
                            }

                        }

                    }
                }

                Class1.Cn.Close();
            }
            catch
            {
                Class1.Cn.Close();
                MessageBox.Show("This product Stock Is Not Available");
            }
        }
        private int showbottle(int ml_per_bottle)
        {
            //Class1.Cn.Open();
            string ml = "ml" + ml_per_bottle.ToString();
            OleDbCommand cmd = new OleDbCommand("select * from setting where type='"+ml+"'",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int bottle_per_box = Convert.ToInt32(rd.GetString(2));
            return bottle_per_box;
        }
        public void pd_print(object obj, PrintPageEventArgs pe)
        {
            //MessageBox.Show(bill_no.ToString());
            this.txttotalamount.Text = "0";
            Graphics graphics = pe.Graphics;
            int startX = 0;
            int startY = 0;
            int Offset = 15;
            int tempx = 0;
            int tempx2 = 0;

            string line = "--------------------------------------------------------------------------------------------";
            string name = "PUJA F.L ON SHOP";
            string ph = "ph:-03324430207";
            string address = "(SMT-BINTA PROSAD GAZOLE,MALDA)";
            graphics.DrawString(name, new Font("Courier New", 10), new SolidBrush(Color.Black), startX + 100, startY);
            graphics.DrawString(ph, new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 250, startY);
            graphics.DrawString("Date" + DateTime.Now.ToShortDateString() + "   " + DateTime.Now.ToShortTimeString(), new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 450, startY);
            graphics.DrawString(address, new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 94, startY + 10);
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString("Recipt No:-"+bill_no.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString("name", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
            graphics.DrawString("Quantity", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
            graphics.DrawString("Price(per pack)", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            total = 0;
            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
            {


                total += Convert.ToDouble(this.gridbill.Rows[i].Cells[1].Value) * Convert.ToDouble(this.gridbill.Rows[i].Cells[3].Value);
                graphics.DrawString(this.gridbill.Rows[i].Cells[0].Value.ToString()+"("+this.gridbill.Rows[i].Cells[2].Value.ToString()+")", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
                graphics.DrawString(this.gridbill.Rows[i].Cells[3].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
                graphics.DrawString(this.gridbill.Rows[i].Cells[1].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
                Offset += 15;

                //MessageBox.Show(this.dataGridView1.Rows[i].Cells[3].Value.ToString());

                //graphics.DrawString("Medicine", new Font("Courier New", 14), new SolidBrush(Color.Black), startX + tempx2, startY + tempx);
                //tempx2 += 340;
                //graphics.DrawString(this.dataGridView1.Rows[i].Cells[0].Value.ToString(), new Font("Courier New", 14), new SolidBrush(Color.Black), startX + tempx2, startY+tempx);
                //tempx2 += 340;
                //graphics.DrawString(this.dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font("Courier New", 14), new SolidBrush(Color.Black), startX + tempx2, startY+tempx);
                ////Offset = Offset + 20;
                //graphics.DrawString(line, new Font("Courier New", 14), new SolidBrush(Color.Black), startX, startY + tempx+20);
                //tempx += 40;
                //tempx2 = 0;

            }
            string vt_st = vat_status();
            decimal vt_per = get_vat_persentage();
            if (vt_st == "yes")
            {
                decimal calculate_vat = (Convert.ToDecimal(total) * vt_per) / 100;
                decimal total_with_vat = Convert.ToDecimal(total) + calculate_vat;
                graphics.DrawString("Vat" + "(" + vt_per.ToString("00.00") + ")", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
                graphics.DrawString(calculate_vat.ToString("00.00"), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
                Offset += 15;
                graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 15;
                graphics.DrawString("Total_Anount", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 50, startY + Offset);
                graphics.DrawString(total_with_vat.ToString("00.00"), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 520, startY + Offset);
                total = 0;
                total_with_vat = 0;
                update_vat(vt_per);
            }
            else
            {

                graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 15;
                graphics.DrawString("Total_Anount", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 50, startY + Offset);
                graphics.DrawString(total.ToString("00.00"), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
                total = 0;
            }
            dt1.Clear();
            this.gridbill.DataSource = bs1;
            bill_no = 0;
            insert_table_no();


        }
        private string find_product_name(string barcode)
        {
           // Class1.Cn.Open();
           // MessageBox.Show(barcode);
            string b = barcode;
            OleDbCommand cmd = new OleDbCommand("select * from barcode where barcode='"+barcode+"'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                product_name = rd.GetString(1);
            }
            rd.Close();
            //Class1.Cn.Close();
            return product_name;
        }
        private int find_product_size(string size)
        {
            //Class1.Cn.Open();
            if (size == "large")
            {
                OleDbCommand cmd = new OleDbCommand("select * from setting where type='large_pack'",Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    quan1 = Convert.ToInt32(rd.GetString(2));
                }
                rd.Close();
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand("select * from setting where type='small_pack'",Class1.Cn);
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    quan1 = Convert.ToInt32(rd.GetString(2));
                }
                rd.Close();
            }
            //Class1.Cn.Close();
            return quan1;
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }
        private int check_table_exsit(int table_no)
        {
            Class1.Cn.Open();
            int fl = 0;
            OleDbCommand cmd = new OleDbCommand("select * from table_no where table_no="+table_no+"", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                fl++;
            }
            rd.Close();
            Class1.Cn.Close();
            return fl;
        }

        

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                rowindex = Convert.ToInt32(this.dataGridView1.SelectedCells[0].RowIndex);
            }
            catch
            {
            }
            //int tbo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
            //int bilo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[1].Value);
            //table_option to = new table_option();
            //to.bill_no = bilo;
            //to.old_table_no = tbo;
            //to.AddressUpdated += new table_option.AddressUpdateHandler(AddressForm_ButtonClicked);
            //to.Show();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //rowindex = Convert.ToInt32(this.dataGridView1.SelectedCells[0].RowIndex);
            //int tbo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
            //int bilo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[1].Value);
            //MessageBox.Show(bilo.ToString());
            dt1.Clear();
            this.gridbill.DataSource = bs1;
            int ri = Convert.ToInt32(this.dataGridView1.SelectedCells[0].RowIndex);
            int tn = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
            this.txttableno.Text = tn.ToString();
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from billing where table_no="+tn+" and status='no'",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int bn = rd.GetInt32(0);
            rd.Close();
            cmd = new OleDbCommand("select * from table_product where bill_no="+bn+"",Class1.Cn);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.GetInt32(6) > 1)
                {
                    for (int i = 0; i < rd.GetInt32(6); i++)
                    {
                        if (rd.GetString(5) != "bear")
                        {
                            decimal per_price = Convert.ToDecimal(rd.GetValue(3)) / rd.GetInt32(6);
                            string si = select_pack_size(rd.GetString(2), per_price);
                            string bc = select_barcode(rd.GetString(2));
                            dt1.Rows.Add(rd.GetString(2), per_price, si, "1", bc, rd.GetString(5));
                            //total_amount += Convert.ToDecimal(rd.GetValue(3));
                            //this.txttotalamount.Text = rd.GetValue(3).ToString();
                        }
                        else
                        {
                            decimal per_price = Convert.ToDecimal(rd.GetValue(3)) / rd.GetInt32(6);
                            string si = select_bear_pack_size(rd.GetString(2), per_price);
                            string bc = select_bear_barcode(rd.GetString(2));
                            dt1.Rows.Add(rd.GetString(2), per_price, si, "1", bc, rd.GetString(5));
                        }
                    }
                }
                else
                {
                    if (rd.GetString(5) != "bear")
                    {
                        string si = select_pack_size(rd.GetString(2), Convert.ToDecimal(rd.GetValue(3)));
                        string bc = select_barcode(rd.GetString(2));
                        dt1.Rows.Add(rd.GetString(2), rd.GetValue(3), si, rd.GetInt32(6), bc, rd.GetString(5));
                        //this.txttotalamount.Text = rd.GetValue(3).ToString();
                        //total_amount += Convert.ToDecimal(rd.GetValue(3));
                    }
                    else
                    {
                        decimal per_price = Convert.ToDecimal(rd.GetValue(3)) / rd.GetInt32(6);
                        string si = select_bear_pack_size(rd.GetString(2), per_price);
                        string bc = select_bear_barcode(rd.GetString(2));
                        dt1.Rows.Add(rd.GetString(2), per_price, si, "1", bc, rd.GetString(5));
                    }

                }
            }
            rd.Close();
            Class1.Cn.Close();
            this.gridbill.DataSource = bs1;
            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
            {
                total_amount +=Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value)*Convert.ToDecimal(this.gridbill.Rows[i].Cells[3].Value);
            }
            this.txttotalamount.Text = total_amount.ToString();
            total_amount = 0;

            

        }


        private string select_barcode(string name)
        {
           // Class1.Cn.Open();
           
            OleDbCommand cmd = new OleDbCommand("select * from pack_detail where name='"+name+"'",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                bar=rd.GetValue(6).ToString();
            }
            rd.Close();
           // Class1.Cn.Close();
            return bar;
        }
        private string select_bear_barcode(string name)
        {
            // Class1.Cn.Open();

            OleDbCommand cmd = new OleDbCommand("select * from stock where product_name='" + name + "'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                bar = rd.GetValue(6).ToString();
            }
            rd.Close();
            // Class1.Cn.Close();
            return bar;
        }
        private string select_pack_size(string name, decimal price)
        {
            OleDbCommand cmd = new OleDbCommand("select * from pack_detail where link_product='"+name+"' and price="+price+"",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                size = rd.GetString(4);
            }
            return size;
        }
        private string select_bear_pack_size(string name, decimal price)
        {
            OleDbCommand cmd = new OleDbCommand("select * from stock where product_name='" + name + "' and salling_price=" + price + "", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                size = rd.GetInt32(3).ToString();
            }
            return size;
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (rowindex >= 0)
            {
                int tbo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[0].Value);
                int bilo = Convert.ToInt32(this.dataGridView1.Rows[rowindex].Cells[1].Value);
                table_option to = new table_option();
                to.bill_no = bilo;
                to.old_table_no = tbo;
                to.AddressUpdated += new table_option.AddressUpdateHandler(AddressForm_ButtonClicked);
                to.Show();
                rowindex = -1;
            }
            else
            {
                MessageBox.Show("Please Select Row");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            dt1.Clear();
            this.gridbill.DataSource = bs1;
            this.txttotalamount.Text = "0";
        }

        private void txtgivmoney_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal temp = Convert.ToDecimal(this.txttotalamount.Text);
                decimal temp1 = Convert.ToDecimal(this.txtgivmoney.Text);
                this.txtbalance.Text = (temp1 - temp).ToString("00.00");
            }
            catch
            {
            }
        }
        private decimal get_vat_persentage()
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from setting where type='vat_persentage'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                vat_persentage = Convert.ToDecimal(rd.GetString(2));
            }
            rd.Close();
            Class1.Cn.Close();
            return vat_persentage;
        }
        private string vat_status()
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from setting where type='vat_status'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                vat_stat = rd.GetString(2);
            }
            rd.Close();
            Class1.Cn.Close();
            return vat_stat;
        }
        private void update_vat(decimal vat_per)
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("update bill_product set vat=" + vat_per + " where bill_no=" + bill_no + "", Class1.Cn);
            cmd.ExecuteNonQuery();
            Class1.Cn.Close();
        }
        private string fetch_method()
        {
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from setting where type='method'", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                method = rd.GetString(2);
            }
            rd.Close();
            Class1.Cn.Close();
            return method;

        }

       

        private void button19_Click(object sender, EventArgs e)
        {
            int flag3 = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from billing where status='no'",Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag3++;
            }
            rd.Close();
            Class1.Cn.Close();
            if (flag3 > 0)
            {
                MessageBox.Show("Please Close All Table Bill");
            }
            else
            {
                //admin_panel ap = new admin_panel();
                //ap.bl = null;
                Close();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                if (row_index1 < 0)
                {
                    MessageBox.Show("Please Select Row");
                }
                else
                {
                    if (Convert.ToInt32(this.gridbill.Rows[row_index1].Cells[3].Value) > 1)
                    {
                        int quan = Convert.ToInt32(this.gridbill.Rows[row_index1].Cells[3].Value) - 1;
                        this.gridbill.Rows[row_index1].Cells[3].Value = quan;
                        row_index1 = -1;
                        calculate_total_amount();
                    }
                    else
                    {
                        this.gridbill.Rows.RemoveAt(row_index1);
                        row_index1 = -1;
                        calculate_total_amount();
                    }
                }
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        private void gridbill_MouseClick(object sender, MouseEventArgs e)
        {
            row_index1 = Convert.ToInt32(this.gridbill.SelectedCells[0].RowIndex);
        }
        private void calculate_total_amount()
        {
            for (int i = 0; i < this.gridbill.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDecimal(this.gridbill.Rows[i].Cells[1].Value) * Convert.ToDecimal(this.gridbill.Rows[i].Cells[3].Value);

            }
            this.txttotalamount.Text = total_amount.ToString("00.00");
            total_amount = 0;
        }

        
    }
}
