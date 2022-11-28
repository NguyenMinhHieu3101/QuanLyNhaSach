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
    /// Interaction logic for BieuDoDoanhThu.xaml
    /// </summary>
    public partial class BaoCaoSanPham : Page
    {
        public BaoCaoSanPham()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int tongBanRa = 0;

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                if (dPickerTuNgay.SelectedDate == null || dPickerDenNgay.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn khoảng thời gian cần xuất báo cáo!");
                }
                else
                {
                    txtThoiGian.Text = "TỪ " + dPickerTuNgay.SelectedDate.Value.ToShortDateString() + " ĐẾN " + dPickerDenNgay.SelectedDate.Value.ToShortDateString();

                    string readString = "SELECT SUM(SoLuong) AS TongBanRa FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay)";
                    SqlCommand command = new SqlCommand(readString, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongBanRa = (int)command.ExecuteScalar();
                    txtTongBanRa.Text = "TỔNG BÁN RA: " + tongBanRa;


                    //string readString2 = "SELECT COUNT(*) FROM CHITIETBAOCAOSANPHAM";
                    //command = new SqlCommand(readString2, connection);
                    //Int32 count = (Int32)command.ExecuteScalar() + 1;

                    //string insertString = "INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES (@MaChiTietBaoCao, @MaSanPham, @MaNVBC, @TuNgay, @DenNgay, @SoLuongDaBan)";
                    //command = new SqlCommand(insertString, connection);

                    //command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    //command.Parameters["@MaChiTietBaoCao"].Value = "BCSP" + count.ToString();

                    //command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                    //command.Parameters["@MaSanPham"].Value = "???";

                    //command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    //command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien; 

                    //command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    //command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    //command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    //command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    //command.Parameters.Add("@SoLuongDaBan", SqlDbType.Int);
                    //command.Parameters["@SoLuongDaBan"].Value = "???";

                    //command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //MessageBox.Show("Vui lòng chọn khoảng thời gian hợp lí!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void sanPhamTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
