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
    /// Interaction logic for DanhSachKhachHang.xaml
    /// </summary>
    public partial class DanhSachKhachHang : Page
    {
        List<KhachHang> khachHangList = new List<KhachHang>();

        public DanhSachKhachHang()
        {
            InitializeComponent();
            try
            {
                SqlConnection thisConnection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                thisConnection.Open();

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from KHACHHANG";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    khachHangList.Add(new KhachHang(stt: count, maKhachHang: reader["MaKhachHang"].ToString(),
                        tenKhachHang: reader["TenKhachHang"].ToString(), diaChi: reader["DiaChi"].ToString(),
                        gioiTinh: reader["GioiTinh"].ToString(), maLoaiKhachHang: reader["MaLoaiKhachHang"].ToString(),
                        sdt: reader["SDT"].ToString(), email: reader["Email"].ToString(), trangThai: reader["TrangThai"].ToString()));
                    khachHangTable.ItemsSource = khachHangList;
                }
            }
            catch
            {
                MessageBox.Show("db error");
            }
        }

        private void khachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
