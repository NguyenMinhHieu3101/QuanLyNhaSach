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
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for SanPhamPdf.xaml
    /// </summary>
    public partial class SanPhamPdf : Window
    {
        List<ThongKeSanPham> thongKeList = new List<ThongKeSanPham>();

        public SanPhamPdf()
        {
            InitializeComponent();
            loadInfo();
            loadDataGrid();
        }

        private void print_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Báo cáo doanh thu");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        void loadInfo()
        {
            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand();
                string readString = "SELECT TenChucVu FROM NHANVIEN, CHUCVU WHERE MaNhanVien = @MaNhanVien AND NHANVIEN.MaChucVu = CHUCVU.MaChucVu";
                command = new SqlCommand(readString, connection);

                command.Parameters.Add("@MaNhanVien", SqlDbType.VarChar);
                command.Parameters["@MaNhanVien"].Value = NhanVienDangDangNhap.MaNhanVien;

                string chucVu = command.ExecuteScalar().ToString();

                date.Text = "Ngày lập báo cáo: " + DateTime.Now.ToString();
                author.Text = "Người lập báo cáo: " + chucVu + " - " + NhanVienDangDangNhap.HoTen;

                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        void loadDataGrid()
        {
            int tongBanRa = 0;

            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand();

                string readString = "SELECT COUNT(*) FROM CHITIETBAOCAOSANPHAM";
                command = new SqlCommand(readString, connection);
                Int32 count = (Int32)command.ExecuteScalar();

                string readString2 = "SELECT * FROM CHITIETBAOCAOSANPHAM WHERE MaChiTietBaoCao = @MaChiTietBaoCao";
                command = new SqlCommand(readString2, connection);

                command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                command.Parameters["@MaChiTietBaoCao"].Value = "BCSP" + count.ToString();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                string tuNgay = reader["TuNgay"].ToString();
                string denNgay = reader["DenNgay"].ToString();
                from_to.Text = "TỪ NGÀY " + tuNgay + " ĐẾN NGÀY " + denNgay;
                reader.Close();

                string readString3 = "SELECT SUM(SoLuong) AS TongBanRa FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay)";
                command = new SqlCommand(readString3, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;

                if (command.ExecuteScalar() == DBNull.Value)
                    tongBanRa = 0;
                else
                    tongBanRa = (int)command.ExecuteScalar();

                txtTongBanRa.Text = "TỔNG BÁN RA: " + tongBanRa;

                //Xử lí bảng thống kê sản phẩm

                string readString4 = "SELECT TenSanPham, CHITIETPHIEUNHAP.SoLuong AS SoLuongNhap,CHITIETHOADON.SoLuong AS SoLuongBan FROM SANPHAM, HOADON, CHITIETHOADON, PHIEUNHAP, CHITIETPHIEUNHAP WHERE (SANPHAM.MaSanPham = CHITIETHOADON.MaSanPham) AND (SANPHAM.MaSanPham = CHITIETPHIEUNHAP.MaSanPham) AND (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay) AND (PHIEUNHAP.NgayNhap BETWEEN @TuNgay AND @DenNgay)";
                command = new SqlCommand(readString4, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;

                reader = command.ExecuteReader();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
