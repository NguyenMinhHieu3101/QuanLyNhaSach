﻿using System;
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
using System.Xml.Linq;
using UngDungQuanLyNhaSach.Model;
using System.ComponentModel;
using System.Threading;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Markup;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemKhuyenMai.xaml
    /// </summary>
    public partial class ThemKhuyenMai : Page
    {
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public ThemKhuyenMai()
        {
            InitializeComponent();
            update.IsEnabled = false;
            delete.IsEnabled = false;
            ngayBatDau.SelectedDate = DateTime.Now;
            ngayKetThuc.SelectedDate = DateTime.Now;
            updateMaKhuyenMai();
            loadData();
        }

        void reset()
        {
            soLuong.Text = "";
            phanTram.Text = "";
            loaiKhachHang.SelectedIndex = 0;
            ngayBatDau.SelectedDate = DateTime.Now;
            ngayKetThuc.SelectedDate = DateTime.Now;
            updateMaKhuyenMai();
        }

        bool checkDataInput()
        {
            if (soLuong.Text.Length == 0)
            {
                MessageBox.Show("Số lượng không hợp lệ");
                return false;
            }   
            if (phanTram.Text.Length == 0)
            {
                MessageBox.Show("Phần trăm không hợp lệ");
                return false;
            }
            DateTime start = ngayBatDau.SelectedDate ?? DateTime.Now;
            DateTime end = ngayKetThuc.SelectedDate ?? DateTime.Now;
            if (end.Subtract(start).TotalSeconds < 0)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc không hợp lệ!");
                return false;
            }    
            return true;
        }

        void updateMaKhuyenMai()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from KHUYENMAI";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maKM.Text = "KM" + count.ToString();
                    }));
                }
                catch { }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (checkDataInput())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from KHUYENMAI";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;


                    string insertString = "INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, TrangThai) " +
                        "VALUES (@MaKhuyenMai, @ThoiGianBatDau, @ThoiGianKetThuc, @MaLoaiKhachHang, @SoLuongKhuyenMai, @TrangThai)";
                    SqlCommand command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                    command.Parameters["@MaKhuyenMai"].Value = "KM" + count.ToString();

                    command.Parameters.Add("@ThoiGianBatDau", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianBatDau"].Value = ngayBatDau.SelectedDate;

                    command.Parameters.Add("@ThoiGianKetThuc", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianKetThuc"].Value = ngayKetThuc.SelectedDate;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                        (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                    command.Parameters.Add("@SoLuongKhuyenMai", SqlDbType.Int);
                    command.Parameters["@SoLuongKhuyenMai"].Value = int.Parse(soLuong.Text);

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    MessageBox.Show("Thêm thành công");
                    reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void khuyenMaiTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                khuyenMaiList = new List<KhuyenMai>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khuyenMaiList.Add(new KhuyenMai(stt: count, maKhuyenMai: (String)reader["MaKhuyenMai"],
                            batDau: (DateTime)reader["ThoiGianBatDau"], //DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            ketThuc: (DateTime)reader["ThoiGianKetThuc"], //DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            soLuong: (int)reader["SoLuongKhuyenMai"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        khuyenMaiTable.ItemsSource = khuyenMaiList;

                    }));
                    connection.Close();
                }
                catch
                {
                    MessageBox.Show("db error");
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            /*if (khuyenMaiTable.SelectedIndex != -1)
            {
                var window = Window.GetWindow(this);
                ((Home)window).MainWindowFrame.Navigate(new CapNhatKhuyenMai(khuyenMaiList[khuyenMaiTable.SelectedIndex].maKhuyenMai));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khuyến mãi để chỉnh sửa");
            }*/
            if (checkDataInput())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string updateString = "UPDATE KHUYENMAI SET ThoiGianBatDau = @ThoiGianBatDau, ThoiGianKetThuc = @ThoiGianKetThuc, MaLoaiKhachHang = @MaLoaiKhachHang, SoLuongKhuyenMai = @SoLuongKhuyenMai, TrangThai = @TrangThai " +
                        "WHERE MaKhuyenMai = @MaKhuyenMai";
                    SqlCommand command = new SqlCommand(updateString, connection);

                    command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                    command.Parameters["@MaKhuyenMai"].Value = khuyenMaiList[khuyenMaiTable.SelectedIndex].maKhuyenMai;

                    command.Parameters.Add("@ThoiGianBatDau", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianBatDau"].Value = ngayBatDau.SelectedDate;

                    command.Parameters.Add("@ThoiGianKetThuc", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianKetThuc"].Value = ngayKetThuc.SelectedDate;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                        (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                    command.Parameters.Add("@SoLuongKhuyenMai", SqlDbType.Int);
                    command.Parameters["@SoLuongKhuyenMai"].Value = int.Parse(soLuong.Text);

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    reset();
                    MessageBox.Show("Cập nhật thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (khuyenMaiTable.SelectedIndex != -1)
            { 
                var result =  MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();

                        string deleteString = "Delete From KHUYENMAI Where MaKhuyenMai = @MaKhuyenMai";
                        SqlCommand command = new SqlCommand(deleteString, connection);
                        command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                        command.Parameters["@MaKhuyenMai"].Value = khuyenMaiList[khuyenMaiTable.SelectedIndex].maKhuyenMai;

                        command.ExecuteNonQuery();
                        connection.Close();
                        loadData();
                        MessageBox.Show("Xóa khuyến mãi thành công");
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khuyến mãi để xóa");
            }
        }

        void loadKM()
        {
            KhuyenMai khuyenMai = khuyenMaiList[khuyenMaiTable.SelectedIndex];
            maKM.Text = khuyenMai.maKhuyenMai;
            soLuong.Text = khuyenMai.soLuong.ToString();
            loaiKhachHang.SelectedIndex = khuyenMai.maLoaiKhachHang == "Vãng Lai" ? 0 :
            (khuyenMai.maLoaiKhachHang == "Bạc" ? 1 : (khuyenMai.maLoaiKhachHang == "Vàng" ? 2 : 3));
            ngayBatDau.SelectedDate = khuyenMai.batDau;
            ngayKetThuc.SelectedDate = khuyenMai.ketThuc;
        }

        private void khuyenMaiTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (khuyenMaiTable.SelectedIndex != -1)
            {
                update.IsEnabled = true;
                loadKM();
            }
            else
            {
                update.IsEnabled = false;
            }
        }
    }
}
