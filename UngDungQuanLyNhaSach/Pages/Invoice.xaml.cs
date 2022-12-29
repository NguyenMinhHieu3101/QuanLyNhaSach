using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        String maHoaDon;

        public Invoice(string maHoaDon)
        {
            this.maHoaDon = maHoaDon;
            InitializeComponent();
            loadDataHD();
        }

        void loadDataHD()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select * from HOADON where HOADON.MaHoaDon = '" + maHoaDon + "'";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    HoaDon hoaDon = new HoaDon(stt: 0,
                            maHoaDon: (String)reader["MaHoaDon"],
                            maKhachHang: (String)reader["MaKhachHang"],
                            ngayLapHD: (DateTime)reader["NgayLapHoaDon"],
                            maNhanVien: (String)reader["MaNhanVien"],
                            maKhuyenMai: (String)reader["MaKhuyenMai"],
                            tongTienHD: (double)reader["TongTienHoaDon"]);
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        date.Text = "Ngày: " + hoaDon.ngayLapHD.ToString("dd/MM/yyyy");
                        maHD.Text = "Mã Hóa Đơn: " + hoaDon.maHoaDon;
                        khachHang.Text = hoaDon.maKhachHang;
                        sdt.Text = hoaDon.getSDT();
                        nhanVien.Text = hoaDon.maNhanVien;
                        tongTien.Text = hoaDon.tongTienHD;
                        string val = Regex.Replace(hoaDon.tongTienHD, "[^0-9]", "");
                        giamGia.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", (double.Parse(val) * hoaDon.getKhuyenMai() / 100));
                        thanhToan.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", (double.Parse(val) * (100 - hoaDon.getKhuyenMai()) / 100));
                    }));
                    connection.Close();
                    loadDataCTHD();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }));

            thread.IsBackground = true;
            thread.Start();
        }

        void loadDataCTHD()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<ChiTietHoaDon> chiTietHDList = new List<ChiTietHoaDon>();
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select * from CHITIETHOADON, SANPHAM where CHITIETHOADON.MaSanPham = SANPHAM.MaSanPham And MaHoaDon = '" + maHoaDon + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        double donGia = (double)reader["GiaNhap"];
                        int soLuong = (int)reader["SoLuong"];
                        chiTietHDList.Add(new ChiTietHoaDon(count, (String)reader["MaSanPham"],
                            soLuong, donGia, donGia * soLuong));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        hoaDonTable.ItemsSource = chiTietHDList;
                    }));
                    connection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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

        private void print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog pDialog = new PrintDialog();
                if (pDialog.ShowDialog() == true)
                {
                    pDialog.PrintVisual(doc, "Hóa Đơn");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }        

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
