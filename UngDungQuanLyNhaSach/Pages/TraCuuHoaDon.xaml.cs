
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachPhieuNhapSach.xaml
    /// </summary>
    public partial class TraCuuHoaDon : System.Windows.Controls.Page
    {
        List<HoaDon> hoaDonList = new List<HoaDon>();
        List<HoaDon> selectedHoaDon = new List<HoaDon>();

        public TraCuuHoaDon()
        {
            InitializeComponent();
            loadListHD();
            loadFilter();
            selectedHDTable.ItemsSource = new List<HoaDon>();
        }

        void loadListHD()
        {
            String maHDText = maHD.Text;
            String maKHText = maKH.Text;
            String ngayLapHDText = ngayLapHD.Text;
            String nguoiLapHDText = nguoiLapHD.Text;
            String tongTienText = tongTien.Text;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                hoaDonList = new List<HoaDon>();
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select * from HOADON, NHANVIEN, KHACHHANG where HOADON.MaNhanVien = NHANVIEN.MaNhanVien and HOADON.MaKhachHang = KHACHHANG.MaKhachHang";

                    if (maHDText.Length > 0) readString += " AND HOADON.MaHoaDon Like '%" + maHDText + "%'";
                    if (maKHText.Length > 0) readString += " AND KHACHHANG.TenKhachHang Like N'%" + maKHText + "%'";
                    if (ngayLapHDText.Length > 0) readString += " AND NgayLapHoaDon = '" + ngayLapHDText + "'";
                    if (nguoiLapHDText.Length > 0) readString += " AND NHANVIEN.HoTen Like N'%" + nguoiLapHDText + "%'";
                    if (tongTienText.Length > 0) readString += " AND TongTienHoaDon = " + Regex.Replace(tongTienText, "[^0-9]", "");

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        hoaDonList.Add(new HoaDon(stt: count,
                            maHoaDon: (String)reader["MaHoaDon"],
                            maKhachHang: (String)reader["MaKhachHang"],
                            ngayLapHD: (DateTime)reader["NgayLapHoaDon"],
                            maNhanVien: (String)reader["MaNhanVien"],
                            maKhuyenMai: (String)reader["MaKhuyenMai"],
                            tongTienHD: (double)reader["TongTienHoaDon"]
                            ));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        danhSachHDTable.ItemsSource = hoaDonList;
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

        void loadFilter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();

                    string readString = "SELECT * FROM HOADON";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<String> itemsMaHD = new List<String>();
                    List<String> itemsMaKH = new List<String>();
                    List<String> itemsMaNV = new List<String>();
                    List<DateTime> itemsNgayLapHD = new List<DateTime>();
                    List<double> itemsTongTien = new List<double>();

                    while (reader.Read())
                    {
                        itemsMaHD.Add((String)reader["MaHoaDon"]);
                        itemsMaKH.Add(getNameKHFromCode((String)reader["MaKhachHang"]));
                        itemsMaNV.Add(getNameNVFromCode((String)reader["MaNhanVien"]));
                        itemsNgayLapHD.Add((DateTime)reader["NgayLapHoaDon"]);
                        itemsTongTien.Add((double)reader["TongTienHoaDon"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maHD.ItemsSource = itemsMaHD;
                        maKH.ItemsSource = itemsMaKH.Distinct().OrderBy(e => e).ToList();
                        tongTien.ItemsSource = itemsTongTien.Distinct().OrderBy(e => e).Select(e => toMoney(e)).ToList();
                        nguoiLapHD.ItemsSource = itemsMaNV.Distinct().OrderBy(e => e).ToList();
                        ngayLapHD.ItemsSource = itemsNgayLapHD.Distinct().OrderBy(e => e).Select(date => date.ToString("MM/dd/yyyy")).ToList();
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

        String toMoney(double money)
        {
            String strMoney = money.ToString().Replace(".0000", "");
            return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(strMoney));
        }    

        String getNameNVFromCode(String code)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "SELECT * FROM NHANVIEN WHERE MaNhanVien = '" + code + "'";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                String name = (String)reader["HoTen"];
                connection.Close();
                return name;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }
        
        String getNameKHFromCode(String code)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "SELECT * FROM KHACHHANG WHERE MaKhachHang = '" + code + "'";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                String name = (String)reader["TenKhachHang"];
                connection.Close();
                return name;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }

        private void danhSachHDTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void selectedHDTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            selectedHDTable.ItemsSource = new List<HoaDon>();
            selectedHoaDon = new List<HoaDon>();
            loadListHD();
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            maHD.SelectedIndex = -1;
            maKH.SelectedIndex = -1;
            nguoiLapHD.SelectedIndex = -1;
            ngayLapHD.SelectedIndex = -1;
            tongTien.SelectedIndex = -1;
            selectedHDTable.ItemsSource = new List<HoaDon>();
            selectedHoaDon = new List<HoaDon>();            
            loadListHD();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {         
            if (danhSachHDTable.SelectedIndex != -1)
            {
                selectedHoaDon.RemoveAll(element => element.maHoaDon.CompareTo(hoaDonList[danhSachHDTable.SelectedIndex].maHoaDon) == 0);
                selectedHoaDon.Add(hoaDonList[danhSachHDTable.SelectedIndex]);
                List<HoaDon> showSelectedHoaDon = selectedHoaDon.OrderBy(e => e.maHoaDon).ToList();
                for (int i = 0; i < showSelectedHoaDon.Count; i++)
                {
                    showSelectedHoaDon[i].stt = i + 1;
                }
                selectedHDTable.ItemsSource = new List<HoaDon>();
                selectedHDTable.ItemsSource = showSelectedHoaDon;
            }
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (danhSachHDTable.SelectedIndex != -1)
            {
                selectedHoaDon.RemoveAll(element => element.maHoaDon.CompareTo(hoaDonList[danhSachHDTable.SelectedIndex].maHoaDon) == 0);
                List<HoaDon> showSelectedHoaDon = selectedHoaDon.OrderBy(e => e.maHoaDon).ToList();
                for (int i = 0; i < showSelectedHoaDon.Count; i++)
                {
                    showSelectedHoaDon[i].stt = i + 1;
                }
                selectedHDTable.ItemsSource = new List<HoaDon>();
                selectedHDTable.ItemsSource = showSelectedHoaDon;
            }
        }

        private void selectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            danhSachHDTable.ItemsSource = hoaDonList;
            selectedHoaDon = new List<HoaDon>();
            selectedHDTable.ItemsSource = selectedHoaDon;
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            selectedHoaDon = new List<HoaDon>();
            selectedHoaDon.AddRange(hoaDonList);
            selectedHDTable.ItemsSource = selectedHoaDon;
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (HoaDon hoaDon in selectedHoaDon)
            {
                Invoice invoice = new Invoice(hoaDon.maHoaDon);
                invoice.Show();
            }
        }
    }
}

