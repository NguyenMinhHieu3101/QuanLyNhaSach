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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemHoaDon.xaml
    /// </summary

    public partial class ThemHoaDon : Page
    {
        List<HoaDon> hoaDonList = new List<HoaDon>();
        List<ChiTietHoaDon> chiTietHDList = new List<ChiTietHoaDon>();
        //List<KhachHang> khachHangList = new List<KhachHang>();
        //List<NhanVien> nhanVienList = new List<NhanVien>();
        public ThemHoaDon()
        {
            InitializeComponent();
            ngayHoaDon.SelectedDate = DateTime.Now;
            //themSP_btn.IsEnabled = true;
            //luuHD_btn.IsEnabled = false;
            //xuatHD_btn.IsEnabled = false;
            //cancel_btn.IsEnabled = false;
            //maHoaDon_txt.IsReadOnly = true;
            //tenNhanVien_txt.IsReadOnly = true;
            //tenKhachHang_txt.IsReadOnly = true;
            //tenSanPham_txt.IsReadOnly = true;
            //donGia_txt.IsReadOnly = true;
            //thanhTien_txt.IsReadOnly = true;
            //phaiThanhToan_txt.IsReadOnly = true;
            //khuyenMai.Text = "0";
            //phaiThanhToan_txt.Text = "0";
            loadListHD();
            //Functions.FillCombo("SELECT MaKhach, TenKhach FROM tblKhach", cboMaKhach, "MaKhach", "MaKhach");
            //cboMaKhach.SelectedIndex = -1;
            //Functions.FillCombo("SELECT MaNhanVien, TenNhanVien FROM tblNhanVien", cboMaNhanVien, "MaNhanVien", "TenKhach");
            //cboMaNhanVien.SelectedIndex = -1;
            //Functions.FillCombo("SELECT MaHang, TenHang FROM tblHang", cboMaHang, "MaHang", "MaHang");
            //cboMaHang.SelectedIndex = -1;
        }
       
        void loadListHD()
        {
            String maSanPhamText = maSanPham.Text;
            String tenSanPhamText = tenSanPham_txt.Text;
            String donGiaText = donGia_txt.Text;
            String soLuongText = soLuong_txt.Text;
            String khuyenMaiText = khuyenMai.Text;
            String thanhTienText = thanhTien_txt.Text;


            //Thread thread = new Thread(new ThreadStart(() =>
            //{
                //List<SanPham> sanPhamList = new List<SanPham>();
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    String readString = "SELECT * FROM SANPHAM WHERE SANPHAM.MaSanPham = N'" + maSanPham.Text + "'";

                    //if (maSanPhamText.Length > 0) readString += " AND MaSanPham = N'" + maSanPhamText + "'";
                    //if (tenSanPhamText.Length > 0) readString += " AND TenSanPham Like N'%" + tenSanPhamText + "%'";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        chiTietHDList.Add(new ChiTietHoaDon(stt: count, maHoaDon: (String)reader["MaHoaDon"],  maSanPham: (String)reader["MaSanPham"],
                            soLuong: (Int32)reader["SoLuong"],
                            donGia: (Decimal)reader["DonGia"], giamGia: (float)reader["GiamGia"],
                            thanhTien: (decimal)reader["ThanhTien"]));
                    }
                    hoaDonTable.ItemsSource = chiTietHDList;
                //this.Dispatcher.BeginInvoke(new System.Action(() => {
                //        hoaDonTable.ItemsSource = chiTietHDList;
                //    }));

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            //thread.IsBackground = true;
            //thread.Start();
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
            loadListHD();
        }
    }
}
