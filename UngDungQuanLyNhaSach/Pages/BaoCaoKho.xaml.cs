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
    /// Interaction logic for BaoCaoKho.xaml
    /// </summary>
    public partial class BaoCaoKho : Page
    {
        public BaoCaoKho()
        {
            InitializeComponent();
        }     

        private void sachNhapTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void sachDaBanTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tongNhap = 0;
                int tongXuat = 0;

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                if (dPickerTuNgay.SelectedDate == null || dPickerDenNgay.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn khoảng thời gian cần xuất báo cáo!");
                }
                else
                {
                    txtThoiGian.Text = "TỪ " + dPickerTuNgay.SelectedDate.Value.ToShortDateString() + " ĐẾN " + dPickerDenNgay.SelectedDate.Value.ToShortDateString();

                    string readString1 = "SELECT SUM(SoLuong) AS TongNhap FROM CHITIETPHIEUNHAP, PHIEUNHAP  WHERE (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (NgayNhap BETWEEN @TuNgay AND @DenNgay)";
                    SqlCommand command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongNhap = (int)command.ExecuteScalar();
                    txtTongNhap.Text = "TỔNG NHẬP: " + tongNhap;


                    string readString2 = "SELECT SUM(SoLuong) AS TongXuat FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay)";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongXuat = (int)command.ExecuteScalar();
                    txtTongXuat.Text = "TỔNG XUẤT: " + tongXuat;

                    int chenhLech = tongNhap - tongXuat;
                    txtChenhLech.Text = "CHÊNH LỆCH: " + chenhLech;


                    //string readString3 = "SELECT COUNT(*) FROM CHITIETBAOCAOKHO";
                    //command = new SqlCommand(readString3, connection);
                    //Int32 count = (Int32)command.ExecuteScalar() + 1;

                    //string insertString = "INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES (@MaChiTietBaoCao, @TuNgay, @DenNgay, @MaNVBC, @MaKho)";
                    //command = new SqlCommand(insertString, connection);

                    //command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    //command.Parameters["@MaChiTietBaoCao"].Value = "BCK" + count.ToString();

                    //command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    //command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    //command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    //command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    //command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    //command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien;

                    //command.Parameters.Add("@MaKho", SqlDbType.VarChar);
                    //command.Parameters["@MaKho"].Value = "???";

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
    }
}
