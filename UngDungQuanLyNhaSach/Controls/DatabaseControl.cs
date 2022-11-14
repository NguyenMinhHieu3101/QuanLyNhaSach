using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UngDungQuanLyNhaSach.Controls
{
    public class DatabaseControl
    {
        void connect()
        {
            try
            {
                SqlConnection thisConnection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                thisConnection.Open();

                //string Get_Data = "SELECT * FROM emp";

                //SqlCommand cmd = thisConnection.CreateCommand();
                //cmd.CommandText = Get_Data;

                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable("emp");
                //sda.Fill(dt);

                //dataGrid1.ItemsSource = dt.DefaultView;
            }
            catch
            {
                MessageBox.Show("db error");
            }
        }
    }
}
