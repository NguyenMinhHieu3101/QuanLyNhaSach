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
    /// Interaction logic for BaoCaoKho.xaml
    /// </summary>
    public partial class BaoCaoKho : Page
    {
        List<NhapKho> nhapList = new List<NhapKho>();
        List<XuatKho> xuatList = new List<XuatKho>();

        public BaoCaoKho()
        {
            InitializeComponent();
            DataContext = this;
            loadChart();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

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
                nhapList = new List<NhapKho>();
                xuatList = new List<XuatKho>();

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

                    //Xử lí số liệu sách đã bán

                    string readString1 = "SELECT SUM(SoLuong) AS TongXuat FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay)";
                    SqlCommand command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongXuat = (int)command.ExecuteScalar();
                    txtTongXuat.Text = "TỔNG XUẤT: " + tongXuat;

                    xuatList.Add(new XuatKho(1, "Bán sách", tongXuat));
                    sachDaBanTable.ItemsSource = xuatList;

                    //Xử lí số liệu sách nhập

                    string readString2 = "SELECT SUM(SoLuong) AS TongNhap FROM CHITIETPHIEUNHAP, PHIEUNHAP  WHERE (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (NgayNhap BETWEEN @TuNgay AND @DenNgay)";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    tongNhap = (int)command.ExecuteScalar();
                    txtTongNhap.Text = "TỔNG NHẬP: " + tongNhap;

                    int chenhLech = tongNhap - tongXuat;
                    txtChenhLech.Text = "CHÊNH LỆCH: " + chenhLech;


                    string readString3 = "SELECT CHITIETPHIEUNHAP.MaPhieuNhap, SUM(SoLuong) AS TongSoNhap FROM PHIEUNHAP, CHITIETPHIEUNHAP WHERE (PHIEUNHAP.MaPhieuNhap = CHITIETPHIEUNHAP.MaPhieuNhap) AND (NgayNhap BETWEEN @TuNgay AND @DenNgay) GROUP BY CHITIETPHIEUNHAP.MaPhieuNhap";
                    command = new SqlCommand(readString3, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    SqlDataReader reader = command.ExecuteReader();

                    int countPN = 0;

                    while (reader.Read())
                    {
                        countPN++;
                        nhapList.Add(new NhapKho(stt: countPN, maPhieuNhap: (String)reader["MaPhieuNhap"],
                            soLuongSP: (int)reader["TongSoNhap"]));
                    }
                    sachNhapTable.ItemsSource = nhapList;
                    reader.Close();

                    //Insert chi tiết báo cáo vào database

                    string readString4 = "SELECT COUNT(*) FROM CHITIETBAOCAOKHO";
                    command = new SqlCommand(readString4, connection);
                    Int32 count = (Int32)command.ExecuteScalar() + 1;

                    string insertString = "INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES (@MaChiTietBaoCao, @TuNgay, @DenNgay, @MaNVBC, @MaKho)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    command.Parameters["@MaChiTietBaoCao"].Value = "BCK" + count.ToString();

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien;

                    command.Parameters.Add("@MaKho", SqlDbType.VarChar);
                    command.Parameters["@MaKho"].Value = "K001";

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

        void loadChart()
        {
            int thangHienTai = 5;
            //int thangHienTai = DateTime.Now.Month;
            int[] tongNhap = new int[5];
            int[] tongXuat = new int[5];
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
                    string readString1 = "SELECT SUM(SoLuong) AS TongXuat FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (MONTH(NgayLapHoaDon) = @Thang)";
                    command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    command.Parameters["@Thang"].Value = thangHienTai - i;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongXuat[i] = 0;
                    else
                        tongXuat[i] = (int)command.ExecuteScalar();
                }

                for (int i = 0; i < 5; i++)
                {

                    string readString2 = "SELECT SUM(SoLuong) AS TongNhap FROM CHITIETPHIEUNHAP, PHIEUNHAP  WHERE (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (MONTH(NgayNhap) = @Thang)";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    command.Parameters["@Thang"].Value = thangHienTai - i;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongNhap[i] = 0;
                    else
                        tongNhap[i] = (int)command.ExecuteScalar();
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
                        Title = "Nhập",
                        Values = new ChartValues<int> {(int)tongNhap[4], (int)tongNhap[3], (int)tongNhap[2], (int)tongNhap[1], (int)tongNhap[0]}
                    },
                    new ColumnSeries
                    {
                        Title = "Xuất",
                        Values = new ChartValues<int> { (int)tongXuat[4], (int)tongXuat[3], (int)tongXuat[2], (int)tongXuat[1], (int)tongXuat[0] }
                    }
                };

            Labels = new[] { label[4], label[3], label[2], label[1], label[0] };
            Formatter = value => value.ToString("N");
        }
    }
}
