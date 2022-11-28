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

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                if (dPickerTuNgay.SelectedDate == null || dPickerDenNgay.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn khoảng thời gian cần xuất báo cáo!");
                }
                else
                {
                    txtThoiGian.Text = "TỪ " + dPickerTuNgay.SelectedDate.Value.ToShortDateString() + " ĐẾN " + dPickerDenNgay.SelectedDate.Value.ToShortDateString();

                    string readString1 = "SELECT SUM(TongTienHoaDon) AS TongThu FROM HOADON WHERE NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay";
                    SqlCommand command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongThu = (decimal)command.ExecuteScalar();
                    txtTongThu.Text = "TỔNG THU: " + tongThu;


                    string readString2 = "SELECT SUM(TongTien) AS TongChi FROM PHIEUNHAP WHERE NgayNhap BETWEEN @TuNgay AND @DenNgay";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongChi = (decimal)command.ExecuteScalar();
                    txtTongChi.Text = "TỔNG CHI: " + tongChi;

                    decimal loiNhuan = tongThu - tongChi;
                    txtLoiNhuan.Text = "LỢI NHUẬN: " + loiNhuan;


                    string readString3 = "SELECT COUNT(*) FROM CHITIETBAOCAODOANHTHU";
                    command = new SqlCommand(readString3, connection);
                    Int32 count = (Int32)command.ExecuteScalar() + 1;

                    string insertString = "INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES (@MaChiTietBaoCao, @TuNgay, @DenNgay, @MaNVBC, @DoanhThu, @ChiPhi)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    command.Parameters["@MaChiTietBaoCao"].Value = "DT" + count.ToString();

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien;

                    command.Parameters.Add("@DoanhThu", SqlDbType.Money);
                    command.Parameters["@DoanhThu"].Value = tongThu.ToString();

                    command.Parameters.Add("@ChiPhi", SqlDbType.Money);
                    command.Parameters["@ChiPhi"].Value = tongChi.ToString();

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //MessageBox.Show("Vui lòng chọn khoảng thời gian hợp lí!");
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
