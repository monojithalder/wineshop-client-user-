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

namespace wine_shop_on_shop__client
{
    public partial class bill_print : Form
    {
        public bill_print()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        public PrintDocument pd;
        double total = 0;
        string bar,size;
        DateTime bill_date;
        private void bill_print_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Product_name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Size");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Barcode");
            dt.Columns.Add("Product_type");
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs;
            //this.dataGridView1.Columns[4].Visible = false;
            //this.dataGridView1.Columns[8].Visible = false;
            //this.dataGridView1.Columns[9].Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dt.Clear();
            this.dataGridView1.DataSource = bs;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from table_product where bill_no=" + Convert.ToInt32(this.txttableno.Text) + "", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {

                if (rd.GetInt32(6) > 1)
                {
                    for (int i = 0; i < rd.GetInt32(6); i++)
                    {
                        decimal per_price = Convert.ToDecimal(rd.GetValue(3)) / rd.GetInt32(6);
                        string si = select_pack_size(rd.GetString(2), per_price);
                        string bc = select_barcode(rd.GetString(2));
                        dt.Rows.Add(rd.GetString(2), per_price, si, "1", bc, rd.GetString(5));
                        bill_date = rd.GetDateTime(4);
                    }
                }
                else
                {
                    string si = select_pack_size(rd.GetString(2), Convert.ToDecimal(rd.GetValue(3)));
                    string bc = select_barcode(rd.GetString(2));
                    dt.Rows.Add(rd.GetString(2), rd.GetValue(3), si, rd.GetInt32(6), bc, rd.GetString(5));
                    bill_date = rd.GetDateTime(4);
                }
            }
            rd.Read();
            Class1.Cn.Close();
        }
        private string select_barcode(string name)
        {
            // Class1.Cn.Open();

            OleDbCommand cmd = new OleDbCommand("select * from pack_detail where link_product='" + name + "'", Class1.Cn);
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
            OleDbCommand cmd = new OleDbCommand("select * from pack_detail where link_product='" + name + "' and price=" + price + "", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                size = rd.GetString(4);
            }
            return size;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PaperSize pz = new PaperSize();
            pz.Height = 200;
            pz.Width = 500;
            pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = pz;
            pd.PrintPage += new PrintPageEventHandler(this.pd_print);
            pd.Print();
        }
        public void pd_print(object obj, PrintPageEventArgs pe)
        {
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
            graphics.DrawString("Date" + bill_date.ToShortDateString() + "   " + DateTime.Now.ToShortTimeString(), new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 450, startY);
            graphics.DrawString(address, new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 94, startY + 10);
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString("name", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
            graphics.DrawString("Quantity", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
            graphics.DrawString("Price", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            total = 0;
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {


                total += Convert.ToDouble(this.dataGridView1.Rows[i].Cells[1].Value) * Convert.ToDouble(this.dataGridView1.Rows[i].Cells[3].Value);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[0].Value.ToString() + "(" + this.dataGridView1.Rows[i].Cells[2].Value.ToString() + ")", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
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
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString("Total_Anount", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 50, startY + Offset);
            graphics.DrawString(total.ToString("00.00"), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
            total = 0;
            dt.Clear();
            this.dataGridView1.DataSource = bs;
            //insert_table_no();


        }

       

       
    }
}
