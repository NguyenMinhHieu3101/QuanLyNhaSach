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
                    readString = "select * from KHACHHANG WHERE KHACHHANG.TrangThai = 1";
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
                    /*
                    reader.Close();
                    readString = "select * from KHUYENMAI";
                    command = new SqlCommand(readString, connection);
                    reader = command.ExecuteReader();

                    List<String> itemsKhuyenMai = new List<String>();
                    while (reader.Read())
                    {
                        itemsKhuyenMai.Add((String)reader["MaKhuyenMai"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        khuyenMai_cbo.ItemsSource = itemsKhuyenMai;
                    }));
                    */
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

        void loadKhuyenMai()
        {
            if (maKhachHang_cbo.SelectedIndex != -1)
            {
                String khachHang = (String)maKhachHang_cbo.Items[maKhachHang_cbo.SelectedIndex];
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    List<String> itemsKhuyenMai = new List<String>();

                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                        connection.Open();
                        string readString = "select * from KHUYENMAI, KHACHHANG WHERE KHACHHANG.MaKhachHang = '" + khachHang + "' AND KHUYENMAI.TrangThai = 1 AND KHACHHANG.MaLoaiKhachHang = KHUYENMAI.MaLoaiKhachHang";
                        SqlCommand command = new SqlCommand(readString, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            itemsKhuyenMai.Add((String)reader["MaKhuyenMai"]);
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

        void resetAddProduct()
        {
            maSanPham_cbo.SelectedIndex = -1;
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
            if (soLuong_txt.Text.Length > 0)
            {
                int soLuong = int.Parse(soLuong_txt.Text);
                string value = Regex.Replace(donGia_txt.Text, "[^0-9]", "");
                decimal donGia;
                if (decimal.TryParse(value, out donGia))
                {
                    chiTietHDList.Add(new ChiTietHoaDon(chiTietHDList.Count + 1, maSanPham_cbo.Text, soLuong, donGia, donGia * soLuong));
                    hoaDonTable.ItemsSource = new List<ChiTietHoaDon>();
                    hoaDonTable.ItemsSource = chiTietHDList;
                    resetAddProduct();
                    decimal sum = 0;
                    foreach (ChiTietHoaDon chiTiet in chiTietHDList)
                    {
                        sum += chiTiet.getThanhTien();
                    }
                    tongTien_txt.Text = sum.ToString();
                    if (khuyenMai_txt.Text.Length > 0)
                    {
                        giamGia_txt.Text = (sum * int.Parse(khuyenMai_txt.Text) / 100).ToString();
                        phaiThanhToan_txt.Text = (sum * (100 - int.Parse(khuyenMai_txt.Text)) / 100).ToString();
                    }
                    else
                    {
                        giamGia_txt.Text = "0";
                        phaiThanhToan_txt.Text = sum.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào số lượng");
            }    
        }

        private void maNhanVien_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maNhanVien_cbo.SelectedIndex != -1)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from NHANVIEN WHERE MaNhanVien = '" + maNhanVien_cbo.Items[maNhanVien_cbo.SelectedIndex] + "'";
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
        }

        private void maSanPham_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maSanPham_cbo.SelectedIndex != -1)
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
        }

        private void maKhachHang_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maKhachHang_cbo.SelectedIndex != -1)
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
                    loadKhuyenMai();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }   

        private void soLuong_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            int soLuong;
            if (int.TryParse(soLuong_txt.Text, out soLuong)) {
                string value = Regex.Replace(donGia_txt.Text, "[^0-9]", "");
                decimal donGia;
                if (decimal.TryParse(value, out donGia))
                {
                    soLuong = int.Parse(soLuong_txt.Text);
                    thanhTien_txt.Text = (soLuong * donGia).ToString();
                }
            }
            else
            {
                thanhTien_txt.Text = "0";
            }    
        }

        private void luuHD_btn_Click(object sender, RoutedEventArgs e)
        {

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

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void tongTien_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(tongTien_txt.Text, "[^0-9]", "");
            decimal ul;
            if (decimal.TryParse(value, out ul))
            {
                tongTien_txt.TextChanged -= tongTien_txt_TextChanged;
                tongTien_txt.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                tongTien_txt.TextChanged += tongTien_txt_TextChanged;
                tongTien_txt.Select(tongTien_txt.Text.Length, 0);
            }
        }

        private void giamGia_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(giamGia_txt.Text, "[^0-9]", "");
            decimal ul;
            if (decimal.TryParse(value, out ul))
            {
                giamGia_txt.TextChanged -= giamGia_txt_TextChanged;
                giamGia_txt.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                giamGia_txt.TextChanged += giamGia_txt_TextChanged;
                giamGia_txt.Select(giamGia_txt.Text.Length, 0);
            }
        }

        private void phaiThanhToan_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(phaiThanhToan_txt.Text, "[^0-9]", "");
            decimal ul;
            if (decimal.TryParse(value, out ul))
            {
                phaiThanhToan_txt.TextChanged -= phaiThanhToan_txt_TextChanged;
                phaiThanhToan_txt.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                phaiThanhToan_txt.TextChanged += phaiThanhToan_txt_TextChanged;
                phaiThanhToan_txt.Select(phaiThanhToan_txt.Text.Length, 0);
            }
        }

        private void donGia_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(donGia_txt.Text, "[^0-9]", "");
            decimal ul;
            if (decimal.TryParse(value, out ul))
            {
                donGia_txt.TextChanged -= donGia_txt_TextChanged;
                donGia_txt.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                donGia_txt.TextChanged += donGia_txt_TextChanged;
                donGia_txt.Select(donGia_txt.Text.Length, 0);
            }
        }

        private void thanhTien_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(thanhTien_txt.Text, "[^0-9]", "");
            decimal ul;
            if (decimal.TryParse(value, out ul))
            {
                thanhTien_txt.TextChanged -= donGia_txt_TextChanged;
                thanhTien_txt.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                thanhTien_txt.TextChanged += donGia_txt_TextChanged;
                thanhTien_txt.Select(thanhTien_txt.Text.Length, 0);
            }
        }

        private void khuyenMai_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)            
        {
            if (khuyenMai_cbo.SelectedIndex != -1)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from KHUYENMAI WHERE KHUYENMAI.MaKhuyenMai = '" + khuyenMai_cbo.Items[khuyenMai_cbo.SelectedIndex] + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    khuyenMai_txt.Text = reader["PhanTram"].ToString();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            resetData();
        }
    }
}
