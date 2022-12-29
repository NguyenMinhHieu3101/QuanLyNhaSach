using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for KhoPdf.xaml
    /// </summary>
    public partial class KhoPdf : Window
    {
        List<NhapKho> nhapList = new List<NhapKho>();
        List<XuatKho> xuatList = new List<XuatKho>();

        public KhoPdf()
        {
            InitializeComponent();
            loadInfo();
            loadDataGrid();
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void print_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Báo cáo kho");
                }
            }
            finally
            {
                this.IsEnabled = true;
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
            int tongNhap = 0;
            int tongXuat = 0;

            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            connection.Open();           

            try
            {
                SqlCommand command = new SqlCommand();

                string readString = "SELECT COUNT(*) FROM CHITIETBAOCAOKHO";
                command = new SqlCommand(readString, connection);
                Int32 count = (Int32)command.ExecuteScalar();

                string readString2 = "SELECT * FROM CHITIETBAOCAOKHO WHERE MaChiTietBaoCao = @MaChiTietBaoCao";
                command = new SqlCommand(readString2, connection);

                command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                command.Parameters["@MaChiTietBaoCao"].Value = "BCK" + count.ToString();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                string tuNgay = reader["TuNgay"].ToString();
                string denNgay = reader["DenNgay"].ToString();
                from_to.Text = "TỪ NGÀY " + tuNgay + " ĐẾN NGÀY " + denNgay;
                reader.Close();

                string readString3 = "SELECT SUM(SoLuong) AS TongXuat FROM CHITIETHOADON, HOADON WHERE (CHITIETHOADON.MaHoaDon = HOADON.MaHoaDon) AND (NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay)";
                command = new SqlCommand(readString3, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;

                if (command.ExecuteScalar() == DBNull.Value)
                    tongXuat = 0;
                else
                    tongXuat = (int)command.ExecuteScalar();

                txtTongXuat.Text = "TỔNG XUẤT: " + tongXuat;

                xuatList.Add(new XuatKho(1, "Bán sách", tongXuat));
                sachDaBanTable.ItemsSource = xuatList;

                //Xử lí số liệu sách nhập

                string readString4 = "SELECT SUM(SoLuong) AS TongNhap FROM CHITIETPHIEUNHAP, PHIEUNHAP  WHERE (CHITIETPHIEUNHAP.MaPhieuNhap = PHIEUNHAP.MaPhieuNhap) AND (PHIEUNHAP.NgayNhap BETWEEN @TuNgay AND @DenNgay)";
                command = new SqlCommand(readString4, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;
                if (command.ExecuteScalar() == DBNull.Value)
                    tongNhap = 0;
                else
                    tongNhap = (int)command.ExecuteScalar();

                txtTongNhap.Text = "TỔNG NHẬP: " + tongNhap;

                int chenhLech = tongNhap - tongXuat;
                txtChenhLech.Text = "CHÊNH LỆCH: " + chenhLech;


                string readString5 = "SELECT CHITIETPHIEUNHAP.MaPhieuNhap, SUM(SoLuong) AS TongSoNhap FROM PHIEUNHAP, CHITIETPHIEUNHAP WHERE (PHIEUNHAP.MaPhieuNhap = CHITIETPHIEUNHAP.MaPhieuNhap) AND (PHIEUNHAP.NgayNhap BETWEEN @TuNgay AND @DenNgay) GROUP BY CHITIETPHIEUNHAP.MaPhieuNhap";
                command = new SqlCommand(readString5, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;

                reader = command.ExecuteReader();

                int countPN = 0;

                while (reader.Read())
                {
                    countPN++;
                    nhapList.Add(new NhapKho(stt: countPN, maPhieuNhap: (String)reader["MaPhieuNhap"],
                        soLuongSP: (int)reader["TongSoNhap"]));
                }
                sachNhapTable.ItemsSource = nhapList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
}
