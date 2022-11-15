using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Xml.Linq;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemKhuyenMai.xaml
    /// </summary>
    public partial class ThemKhuyenMai : Page
    {
        public ThemKhuyenMai()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                string readString = "select Count(*) from KHUYENMAI";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;


                string insertString = "INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, TrangThai) " +
                    "VALUES (@MaKhuyenMai, @ThoiGianBatDau, @ThoiGianKetThuc, @MaLoaiKhachHang, @SoLuongKhuyenMai, @TrangThai)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                command.Parameters["@MaKhuyenMai"].Value = "KM" + count.ToString();

                command.Parameters.Add("@ThoiGianBatDau", SqlDbType.SmallDateTime);
                command.Parameters["@ThoiGianBatDau"].Value = DateTime.Now;

                command.Parameters.Add("@ThoiGianKetThuc", SqlDbType.SmallDateTime);
                command.Parameters["@ThoiGianKetThuc"].Value = DateTime.Now;

                command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                    (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                command.Parameters.Add("@SoLuongKhuyenMai", SqlDbType.Int);
                command.Parameters["@SoLuongKhuyenMai"].Value = int.Parse(soLuong.Text);

                command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                command.Parameters["@TrangThai"].Value = "1";

                command.ExecuteNonQuery();

                MessageBox.Show("Thêm thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
