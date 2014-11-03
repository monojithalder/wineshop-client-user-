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
    public partial class daily_report : Form
    {
        public daily_report()
        {
            InitializeComponent();
        }
        public DataTable dt;
        public BindingSource bs;
        public decimal total_price;
        public int total_ml;
        PrintDocument pd;
        double total;
        private void daily_report_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            bs = new BindingSource();
            dt.Columns.Add("Name");
            dt.Columns.Add("Ml");
            dt.Columns.Add("price");
            dt.Columns.Add("Type");
            dt.Columns.Add("Today's_Income");
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from billing", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.GetDateTime(1).Year == DateTime.Now.Year)
                {
                    if (rd.GetDateTime(1).Date == DateTime.Now.Date && rd.GetDateTime(1).Month == DateTime.Now.Month)
                    {
                        OleDbCommand cmd2 = new OleDbCommand("select * from bill_product where bill_no=" + rd.GetInt32(0) + "", Class1.Cn);
                        OleDbDataReader rd2 = cmd2.ExecuteReader();
                        while (rd2.Read())
                        {
                            total_price += Convert.ToDecimal(rd2.GetValue(3));
                            total_ml += rd2.GetInt32(4);
                            dt.Rows.Add(rd2.GetString(2), rd2.GetInt32(4), rd2.GetValue(3), rd2.GetValue(5));

                        }
                        rd2.Close();
                    }

                }
            }
            dt.Rows.Add("Total", total_ml, "", "", total_price);
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs;
            Class1.Cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PaperSize pz = new PaperSize();
            //pz.Height = 200;
            pz.Width = 700;
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
            graphics.DrawString("Date" + DateTime.Now.ToShortDateString() + "   " + DateTime.Now.ToShortTimeString(), new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 450, startY);
            graphics.DrawString(address, new Font("Courier New", 5), new SolidBrush(Color.Black), startX + 100, startY + 10);
            graphics.DrawString("DAILY SALE REPORT", new Font("Courier New", 10), new SolidBrush(Color.Black), startX + 250, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            graphics.DrawString("NAME", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
            graphics.DrawString("ML", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
            graphics.DrawString("PRICE", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 320, startY + Offset);
            graphics.DrawString("TYPE", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
            Offset += 15;
            graphics.DrawString(line, new Font("Courier New", 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset += 15;
            total = 0;
            for (int i = 0; i < this.dataGridView1.Rows.Count - 2; i++)
            {


                //total += Convert.ToDouble(this.dataGridView1.Rows[i].Cells[2].Value);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() + "(" + this.dataGridView1.Rows[i].Cells[2].Value.ToString() + ")", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 10, startY + Offset);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 320, startY + Offset);
                graphics.DrawString(this.dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 420, startY + Offset);
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
            graphics.DrawString("Total", new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 50, startY + Offset);
            graphics.DrawString(total_ml.ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 230, startY + Offset);
            graphics.DrawString(total_price.ToString("00.00"), new Font("Courier New", 8), new SolidBrush(Color.Black), startX + 320, startY + Offset);
            total = 0;
            //dt1.Clear();
            //this.gridbill.DataSource = bs1;
            //bill_no = 0;



        }
    }
}
