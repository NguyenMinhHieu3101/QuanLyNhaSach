using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DanhSachKhuyenMai.xaml
    /// </summary>
    public partial class DanhSachKhuyenMai : Page
    {
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public DanhSachKhuyenMai()
        {
            InitializeComponent();
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from KHUYENMAI";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    khuyenMaiList.Add(new KhuyenMai(stt: count, maKhuyenMai: reader["MaKhuyenMai"].ToString(),
                        batDau: DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), 
                        ketThuc: DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), 
                        maLoaiKhachHang: reader["MaLoaiKhachHang"].ToString(), 
                        soLuong: int.Parse(reader["SoLuongKhuyenMai"].ToString()),trangThai: reader["TrangThai"].ToString()));
                    khuyenMaiTable.ItemsSource = khuyenMaiList;
                }
            }
            catch
            {
                MessageBox.Show("db error");
            }
        }

        private void khuyenMaiTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
