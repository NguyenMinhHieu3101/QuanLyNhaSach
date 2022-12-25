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

        private void soLuong_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (soLuong_txt.Text.Length > 0)
            {
                int soLuong = int.Parse(soLuong_txt.Text);
                decimal donGia = decimal.Parse(donGia_txt.Text);
                thanhTien_txt.Text = (soLuong * donGia).ToString();
            }
        }
    }
}
