using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace wine_shop_on_shop__client
{
    class Class1
    {
        public static string user = "admin";
        public static int login_id;
        public static string path = System.Environment.CurrentDirectory + "\\wineshopmanagement.mdb";
        public static int ml2000 = 6;
        public static int ml1000 = 9;
        public static int ml750 = 12;
        public static int ml500 = 24;
        public static int ml375 = 24;
        public static int ml180 = 48;
        public static int ml275 = 24;
        public static int ml650 = 12;
        public static string temp;
        public static int temp_no = 0;
        public static DateTime start_date;
        public static OleDbConnection Cn = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + Class1.path + "; Jet OLEDB:Database password = medicine");
        public static void insert_opeingstock()
        {

            DateTime opening_date, insert_date;
            int flag = 0, flag1 = 0, month = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from start_date", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                start_date = rd.GetDateTime(1);
            }
            rd.Close();
            cmd = new OleDbCommand("select * from opening_date", Class1.Cn);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Close();
            if (flag > 0)
            {
                cmd = new OleDbCommand("select * from opening_date order by id DESC", Class1.Cn);
                rd = cmd.ExecuteReader();
                rd.Read();
                opening_date = rd.GetDateTime(1);
                month = DateTime.DaysInMonth(opening_date.Year, opening_date.Month);
                insert_date = opening_date.AddDays(month);
                rd.Close();

            }
            else
            {
                month = DateTime.DaysInMonth(DateTime.Now.Year, start_date.Month);
                insert_date = start_date.AddDays(month);
                //if (DateTime.Now == insert_date)
                //{
                //    cmd = new OleDbCommand("insert into opening_date (opening_date) values ('" + insert_date + "')", Class1.Cn);
                //    cmd.ExecuteNonQuery();
                //}
            }
            if (DateTime.Now >= insert_date)
            {
                int month1 = insert_date.Month;
                int year = insert_date.Year;
                // int date = insert_date.Day;
                cmd = new OleDbCommand("select * from opening_stock", Class1.Cn);
                rd = cmd.ExecuteReader();
                rd.Read();
                if (rd.GetDateTime(1).Year == year)
                {
                    if (rd.GetDateTime(1).Month == month1)
                    {
                        if (rd.GetDateTime(1).Day == insert_date.Day)
                        {
                            flag1++;
                        }
                    }
                }
                rd.Close();

            }
            if (flag1 < 1)
            {
                cmd = new OleDbCommand("select * from stock where total_ml > 0", Class1.Cn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    int total_ml = rd.GetInt32(7);
                    string product_name = rd.GetString(14);
                    string product_type = rd.GetString(2);
                    Decimal product_price = Convert.ToDecimal(rd.GetValue(8));
                    int ml_per_bottle = rd.GetInt32(3);
                    OleDbCommand cmd1 = new OleDbCommand("insert into opening_stock (product_ml,opening_date," +
                        "product_name,product_price,ml_per_bottle,product_type) values (" + total_ml + ",'" + insert_date + "" +
                        ",'" + product_name + "'," + product_price + "," + ml_per_bottle + ",'" + product_type + "')", Class1.Cn);

                }
                rd.Close();
                cmd = new OleDbCommand("insert into opening_date (opening_date) values ('" + insert_date + "')", Class1.Cn);
                cmd.ExecuteNonQuery();
            }
            Class1.Cn.Close();


        }
        public static void insert_closeingstock()
        {
            DateTime closeing_date, insert_date;
            int flag = 0, flag1 = 0, month = 0;
            Class1.Cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from start_date", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                start_date = rd.GetDateTime(1);
            }
            rd.Close();
            cmd = new OleDbCommand("select * from closeing_date", Class1.Cn);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Close();
            if (flag > 0)
            {
                cmd = new OleDbCommand("select * from closeing_date order by id DESC", Class1.Cn);
                rd = cmd.ExecuteReader();
                rd.Read();
                closeing_date = rd.GetDateTime(1);
                month = DateTime.DaysInMonth(closeing_date.Year, closeing_date.Month - 1);
                insert_date = closeing_date.AddDays(month);
                rd.Close();

            }
            else
            {
                month = DateTime.DaysInMonth(DateTime.Now.Year, start_date.Month - 1);
                insert_date = start_date.AddDays(month);
                //if (DateTime.Now == insert_date)
                //{
                //    cmd = new OleDbCommand("insert into opening_date (opening_date) values ('" + insert_date + "')", Class1.Cn);
                //    cmd.ExecuteNonQuery();
                //}
            }
            cmd = new OleDbCommand("select * from setting where type='closeing_time", Class1.Cn);
            rd = cmd.ExecuteReader();
            rd.Read();
            DateTime time = Convert.ToDateTime(rd.GetValue(1));
            insert_date.AddHours(time.Hour);
            insert_date.AddMinutes(time.Minute);
            if (DateTime.Now >= insert_date)
            {
                int month1 = insert_date.Month;
                int year = insert_date.Year;
                int date = insert_date.Day;
                cmd = new OleDbCommand("select * from closeing_date", Class1.Cn);
                rd = cmd.ExecuteReader();
                if (rd.GetDateTime(1).Year == year)
                {
                    if (rd.GetDateTime(1).Month == month1)
                    {
                        if (rd.GetDateTime(1).Day == insert_date.Day)
                        {
                            flag1++;
                        }
                    }
                }

            }
            rd.Close();
            if (flag1 < 1)
            {
                cmd = new OleDbCommand("select * from stock where total_ml > 0", Class1.Cn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    int total_ml = rd.GetInt32(7);
                    string product_name = rd.GetString(14);
                    string product_type = rd.GetString(2);
                    Decimal product_price = Convert.ToDecimal(rd.GetValue(8));
                    int ml_per_bottle = rd.GetInt32(3);
                    OleDbCommand cmd1 = new OleDbCommand("insert into closeing_date (product_ml,opening_date,'" +
                        "'product_name,product_price,ml_per_bottle,product_type) values (" + total_ml + ",'" + insert_date + "'" +
                        ",'" + product_name + "'," + product_price + "," + ml_per_bottle + ",'" + product_type + "')", Class1.Cn);

                }
                rd.Close();
                cmd = new OleDbCommand("insert into closeing_date (closeing_date) values ('" + insert_date + "')", Class1.Cn);
                cmd.ExecuteNonQuery();
            }
            Class1.Cn.Close();


        }
        public void claculate_opening_date()
        {
            Class1.Cn.Open();
            int sd = chaeck_starting_date();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int flag = 0, flag1 = 0;
            OleDbCommand cmd = new OleDbCommand("select * from opening_stock", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag1++;
                if (year == rd.GetDateTime(1).Year)
                {
                    flag++;
                }

            }
            rd.Close();
            if (flag1 > 0)
            {
                if (flag > 0)
                {

                }
                else
                {
                    int remain_month = 12 - month + 1;
                    for (int i = 0; i <= remain_month; i++)
                    {

                    }
                }
            }
            else
            {
                if (sd > 0)
                {

                }
            }

        }
        public int chaeck_starting_date()
        {
            Class1.Cn.Open();
            int flag = 0;
            OleDbCommand cmd = new OleDbCommand("select * from start_date", Class1.Cn);
            OleDbDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                flag++;
            }
            rd.Close();
            Class1.Cn.Close();
            if (flag > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

    }
}
