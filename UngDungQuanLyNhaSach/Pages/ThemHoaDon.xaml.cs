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
using UngDungQuanLyNhaSach.Model;
using System.ComponentModel;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Reflection;
using System.Globalization;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace UngDungQuanLyNhaSach.Pages    
{
    /// <summary>
    /// Interaction logic for ThemHoaDon.xaml
    /// </summary

    public partial class ThemHoaDon : Microsoft.Office.Interop.Excel.Page
    {
        List<HoaDon> hoaDonList = new List<HoaDon>();
        List<ChiTietHoaDon> chiTietHDList = new List<ChiTietHoaDon>();

        public ThemHoaDon()
        {
            InitializeComponent();
            updateMaHoaDon();
            ngayHoaDon.SelectedDate = DateTime.Now;
            loadData();
            hoaDonTable.ItemsSource = chiTietHDList;
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from NHANVIEN";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<String> itemsMaNV = new List<String>();

                    while (reader.Read())
                    {
                        itemsMaNV.Add((String)reader["MaNhanVien"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maNhanVien_cbo.ItemsSource = itemsMaNV;
                    }));

                    reader.Close();
                    readString = "select * from KHACHHANG";
                    command = new SqlCommand(readString, connection);
                    reader = command.ExecuteReader();

                    List<String> itemsMaKH = new List<String>();

                    while (reader.Read())
                    {
                        itemsMaKH.Add((String)reader["MaKhachHang"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maKhachHang_cbo.ItemsSource = itemsMaKH;
                    }));
                    
                    reader.Close();
                    readString = "select * from SANPHAM";
                    command = new SqlCommand(readString, connection);
                    reader = command.ExecuteReader();

                    List<String> itemsMaSP = new List<String>();

                    while (reader.Read())
                    {
                        itemsMaSP.Add((String)reader["MaSanPham"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maSanPham_cbo.ItemsSource = itemsMaSP;
                    }));


                    reader.Close();
                    readString = "select * from KHUYENMAI";
                    command = new SqlCommand(readString, connection);
                    reader = command.ExecuteReader();

                    List<Int32> itemsKhuyenMai = new List<Int32>();

                    while (reader.Read())
                    {
                        itemsKhuyenMai.Add((Int32)reader["PhanTram"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        khuyenMai_cbo.ItemsSource = itemsKhuyenMai;
                    }));


                    connection.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }
        void updateMaHoaDon()
        {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from HOADON";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;
                        maHoaDon_txt.Text = "HD" + count.ToString("000");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
        }
        void resetData()
        {
            updateMaHoaDon();
            maSanPham_cbo.SelectedIndex = -1;
            khuyenMai_cbo.SelectedIndex = -1;
            tenSanPham_txt.Text = "";
            soLuong_txt.Text = "";
            thanhTien_txt.Text = "";
            donGia_txt.Text = "";

        }    

        private void hoaDonTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void themSP_btn_Click(object sender, RoutedEventArgs e)
        {
            int soLuong = int.Parse(soLuong_txt.Text);
            decimal donGia = decimal.Parse(donGia_txt.Text);
            chiTietHDList.Add(new ChiTietHoaDon(chiTietHDList.Count + 1, maSanPham_cbo.Text, soLuong, donGia, donGia * soLuong));
            hoaDonTable.ItemsSource = new List<ChiTietHoaDon>();
            hoaDonTable.ItemsSource = chiTietHDList;
            for (int i = 0; i < chiTietHDList.Count; i++)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from HOADON, CHITIETHOADON WHERE HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon";
                    SqlCommand commandReader = new SqlCommand(readString, connection);

                    string insertString = "INSERT INTO CHITIETHOADON (MaSanPham, SoLuong, DonGia, GiamGia, ThanhTien) " +
                        "VALUES (@MaHoaDon, @MaSanPham, @SoLuong, @DonGia, @GiamGia, @ThanhTien)";
                    SqlCommand command = new SqlCommand(insertString, connection);


                    command.Parameters.Add("@MaHoaDon", SqlDbType.VarChar);
                    command.Parameters["@MaHoaDon"].Value = maHoaDon_txt.Text;
                    //command.Parameters["@MaHoaDon"].Value = "HD011";

                    command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                    command.Parameters["@MaSanPham"].Value = maSanPham_cbo.Text;
                    //command.Parameters["@MaNhanVien"].Value = "NV006";

                    command.Parameters.Add("@SoLuong", SqlDbType.Int);
                    command.Parameters["@SoLuong"].Value = int.Parse(soLuong_txt.Text);
                    //command.Parameters["@MaKhachHang"].Value = "KH001";

                    command.Parameters.Add("@DonGia", SqlDbType.Decimal);
                    command.Parameters["@DonGia"].Value = decimal.Parse(donGia_txt.Text);

                    command.Parameters.Add("@GiamGia", SqlDbType.Float);
                    command.Parameters["@GiamGia"].Value = "30";

                    command.Parameters.Add("@ThanhTien", SqlDbType.Decimal);
                    command.Parameters["@ThanhTien"].Value = "30000";
                    //command.Parameters["@TongTienHoaDon"].Value = Convert.ToDecimal(thanhTien_txt.Text);
                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    MessageBox.Show("Thêm thành công");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }    
            
            //resetData();
        }

        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();

        private void maNhanVien_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select * from NHANVIEN WHERE MaNhanVien = '" + maNhanVien_cbo.Items[maNhanVien_cbo.SelectedIndex] +"'";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                tenNhanVien_txt.Text = (String)reader["HoTen"];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void maSanPham_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select * from SANPHAM WHERE MaSanPham = '" + maSanPham_cbo.Items[maSanPham_cbo.SelectedIndex] + "'";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                tenSanPham_txt.Text = (String)reader["TenSanPham"];
                donGia_txt.Text = reader["GiaNhap"].ToString();
                if (soLuong_txt.Text.Length > 0)
                {
                    int soLuong = int.Parse(soLuong_txt.Text);
                    decimal donGia = decimal.Parse(donGia_txt.Text);
                    thanhTien_txt.Text = (soLuong * donGia).ToString();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void maKhachHang_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select * from KHACHHANG WHERE MaKhachHang = '" + maKhachHang_cbo.Items[maKhachHang_cbo.SelectedIndex] + "'";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                tenKhachHang_txt.Text = (String)reader["TenKhachHang"];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void cost ()
        {
            //tongTien_txt.Text = 
        }    

        private void soLuong_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (soLuong_txt.Text.Length > 0)
            {
                int soLuong = int.Parse(soLuong_txt.Text);
                decimal donGia = decimal.Parse(donGia_txt.Text);
                thanhTien_txt.Text = (soLuong * donGia).ToString();
            }
        }

        private void luuHD_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                
                string readString = "select Count(*) from HOADON, CHITIETHOADON WHERE HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon"; ;
                SqlCommand commandReader = new SqlCommand(readString, connection);
                //Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                for (int i = 0; i<chiTietHDList.Count; i++)
                {
                    string insertString = "INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) " +
                    "VALUES (@MaHoaDon, @MaNhanVien, @MaKhachHang, @MaKhuyenMai, @NgayLapHoaDon, @TongTienHoaDon)";
                    SqlCommand command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaHoaDon", SqlDbType.VarChar);
                    command.Parameters["@MaHoaDon"].Value = maHoaDon_txt.Text;
                    //command.Parameters["@MaHoaDon"].Value = "HD011";

                    command.Parameters.Add("@MaNhanVien", SqlDbType.VarChar);
                    command.Parameters["@MaNhanVien"].Value = maNhanVien_cbo.Text;
                    //command.Parameters["@MaNhanVien"].Value = "NV006";

                    command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                    //command.Parameters["@MaKhachHang"].Value = maKhachHang_cbo.Text;
                    command.Parameters["@MaKhachHang"].Value = "KH001";

                    command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                    command.Parameters["@MaKhuyenMai"].Value = "KM006";

                    command.Parameters.Add("@NgayLapHoaDon", SqlDbType.SmallDateTime);
                    command.Parameters["@NgayLapHoaDon"].Value = ngayHoaDon.SelectedDate;

                    command.Parameters.Add("@TongTienHoaDon", SqlDbType.Decimal);
                    command.Parameters["@TongTienHoaDon"].Value = "30000";
                    command.Parameters["@TongTienHoaDon"].Value = Convert.ToDecimal(thanhTien_txt.Text);


                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    MessageBox.Show("Thêm thành công");
                }    
                



            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void xuatHD_btn_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            excel.ActiveWindow.DisplayGridlines = false;
            //sheet1.Cells[1, 1].Text = "HÓA ĐƠN";
            sheet1.Columns["A1"].CellStyle = workbook.Styles.Add("PageHeaderStyle");
            sheet1.Columns["A1:F1"].Merge();


            for (int j = 0; j < hoaDonTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[2, j + 2];
                sheet1.Cells[2, j + 2].Font.Bold = true;
                sheet1.Columns[j + 2].ColumnWidth = 15;
                myRange.Value2 = hoaDonTable.Columns[j].Header;
            }
            for (int i = 0; i < hoaDonTable.Columns.Count; i++)
            {
                for (int j = 0; j < hoaDonTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)hoaDonTable.Columns[i].GetCellContent(hoaDonTable.Items[j]);
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 3, i + 2];
                    myRange.Value2 = b.Text;
                }
            }
        }
    }
}
