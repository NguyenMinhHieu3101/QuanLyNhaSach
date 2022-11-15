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
    /// Interaction logic for DanhSachNhanVien.xaml
    /// </summary>
    public partial class DanhSachNhanVien : Page
    {
        List<NhanVien> nhanVienList = new List<NhanVien>();

        public DanhSachNhanVien()
        {
            InitializeComponent();
            //try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from NHANVIEN";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;

                    nhanVienList.Add(new NhanVien(stt: count, maNhanVien: reader["MaNhanVien"].ToString(),
                        hoTen: reader["HoTen"].ToString(), maChucVu: reader["MaChucVu"].ToString(),
                        ngaySinh: DateTime.ParseExact(reader["NgaySinh"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        cccd: reader["CCCD"].ToString(),gioiTinh: reader["GioiTinh"].ToString(), sdt: reader["SDT"].ToString(),
                        diaChi: reader["DiaChi"].ToString(), luong: double.Parse(reader["Luong"].ToString()), trangThai: reader["TrangThai"].ToString()));
                    nhanVienTable.ItemsSource = nhanVienList;
                }
            }
           /* catch
            {
                MessageBox.Show("db error");
            }*/
        }

        private void nhanVienTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
