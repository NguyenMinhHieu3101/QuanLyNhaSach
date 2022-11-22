using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for BaoCaoDoanhThu.xaml
    /// </summary>
    public partial class BaoCaoDoanhThu : Page
    {

        public BaoCaoDoanhThu()
        {
            InitializeComponent();
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal tongThu = 0;
                decimal tongChi = 0;
                DateTime? datepicker1 = dPickerTuNgay.SelectedDate;
                DateTime? datepicker2 = dPickerDenNgay.SelectedDate;
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                if (datepicker1 == null || datepicker2 == null)
                {
                    MessageBox.Show("Null òi");
                }
                else
                {
                    MessageBox.Show(datepicker1.Value.ToString());
                    string readString1 = "SELECT SUM(TongTienHoaDon) AS TongThu FROM HOADON  WHERE NgayLapHoaDon BETWEEN '" + datepicker1.Value.ToString() + "' AND '" + datepicker2.Value.ToString() + "'";

                    SqlCommand command = new SqlCommand(readString1, connection);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    tongThu = (decimal)dt.Rows[0]["TongThu"];
                    txtTongThu.Text = "TỔNG THU: " + tongThu;
                }

                //string readString2 = "SELECT SUM(ChiPhi) AS TongChi FROM CHITIETBAOCAODOANHTHU";
                //command = new SqlCommand(readString2, connection);
                //da = new SqlDataAdapter(command);
                //dt = new DataTable();
                //da.Fill(dt);
                //tongChi = (decimal)dt.Rows[0]["TongChi"];
                //txtTongChi.Text = "TỔNG CHI: " + tongChi;

                //decimal loiNhuan = tongThu - tongChi;
                //txtLoiNhuan.Text = "LỢI NHUẬN: " + loiNhuan;

                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void chiTraTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void thuNhapTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
