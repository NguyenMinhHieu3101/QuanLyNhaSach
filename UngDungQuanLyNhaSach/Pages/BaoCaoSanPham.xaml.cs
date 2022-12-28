using LiveCharts;
using LiveCharts.Wpf;
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
        List<ThongKeSanPham> thongKeList = new List<ThongKeSanPham>();
        public BaoCaoSanPham()
        {
            InitializeComponent();
            DataContext = this;
            loadChart();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                thongKeList = new List<ThongKeSanPham>();

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

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongBanRa = 0;
                    else
                        tongBanRa = (int)command.ExecuteScalar();

                    txtTongBanRa.Text = "TỔNG BÁN RA: " + tongBanRa;

                    //Xử lí bảng thống kê sản phẩm

                    string readString2 = "SELECT TenSanPham, CHITIETPHIEUNHAP.SoLuong AS SoLuongNhap,CHITIETHOADON.SoLuong AS SoLuongBan FROM SANPHAM, HOADON, CHITIETHOADON, PHIEUNHAP, CHITIETPHIEUNHAP WHERE (SANPHAM.MaSanPham = CHITIETHOADON.MaSanPham) AND (SANPHAM.MaSanPham = CHITIETPHIEUNHAP.MaSanPham) AND (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay) AND (PHIEUNHAP.NgayNhap BETWEEN @TuNgay AND @DenNgay)";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    SqlDataReader reader = command.ExecuteReader();

                    int countSP = 0;
                    int soLuongNhap = 0;
                    int soLuongTon = 0;

                    while (reader.Read())
                    {
                        countSP++;
                        soLuongNhap = (int)reader["SoLuongNhap"];
                        soLuongTon = soLuongNhap - (int)reader["SoLuongBan"];
                        thongKeList.Add(new ThongKeSanPham(stt: countSP, tenSP: (string)reader["TenSanPham"], 
                            soLuongBan: (int)reader["SoLuongBan"], soLuongTon: soLuongTon));
                    }
                    sanPhamTable.ItemsSource = thongKeList;
                    reader.Close();

                    string readString3 = "SELECT COUNT(*) FROM CHITIETBAOCAOSANPHAM";
                    command = new SqlCommand(readString3, connection);
                    Int32 count = (Int32)command.ExecuteScalar() + 1;

                    string insertString = "INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES (@MaChiTietBaoCao, @MaSanPham, @MaNVBC, @TuNgay, @DenNgay, @SoLuongDaBan)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    command.Parameters["@MaChiTietBaoCao"].Value = "BCSP" + count.ToString();

                    command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                    command.Parameters["@MaSanPham"].Value = "SP001";

                    command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien;

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    command.Parameters.Add("@SoLuongDaBan", SqlDbType.Int);
                    command.Parameters["@SoLuongDaBan"].Value = "0";

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //MessageBox.Show("Khoảng thời gian không hợp lệ!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SanPhamPdf sanPhamPdf = new SanPhamPdf();
            sanPhamPdf.Show();
        }

        void loadChart()
        {
            int thangHienTai = 5;
            //int thangHienTai = DateTime.Now.Month;
            int[] tongBanRa = new int[5];
            String[] label = new String[5];

            for (int i = 0; i < 5; i++)
            {
                label[i] = "Tháng " + (thangHienTai - i).ToString();
            }

            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                SqlCommand command;

                for (int i = 0; i < 5; i++)
                {
                    string readString1 = "SELECT SUM(SoLuong) AS TongBanRa FROM CHITIETHOADON, HOADON WHERE(CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND(MONTH(NgayLapHoaDon) = @Thang)";
                    command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    command.Parameters["@Thang"].Value = thangHienTai - i;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongBanRa[i] = 0;
                    else
                        tongBanRa[i] = (int)command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SeriesCollection = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                        Title = "Tổng bán ra",
                        Values = new ChartValues<int> {(int)tongBanRa[4], (int)tongBanRa[3], (int)tongBanRa[2], (int)tongBanRa[1], (int)tongBanRa[0]}
                    },
                };

            Labels = new[] { label[4], label[3], label[2], label[1], label[0] };
            Formatter = value => value.ToString("N");
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
