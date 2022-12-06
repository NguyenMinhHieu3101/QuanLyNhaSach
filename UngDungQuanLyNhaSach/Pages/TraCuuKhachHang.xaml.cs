using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachKhachHang.xaml
    /// </summary>
    public partial class TraCuuKhachHang : Page
    {
        List<KhachHang> selectedKhachHang = new List<KhachHang>();
        List<KhachHang> khachHangList = new List<KhachHang>();

        public TraCuuKhachHang()
        {
            InitializeComponent();
            //ngaySinh.SelectedDate = DateTime.Now;
            loadData();
        }

        void loadData()
        {
            String maKHText = maKH.Text;
            String loaiKHText = loaiKH.Text;
            String tenKHText = name.Text;
            DateTime? dateTime = ngaySinh.SelectedDate;
            String sdtText = sdt.Text;
            int index = trangThai.SelectedIndex;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG, LOAIKHACHHANG WHERE KHACHHANG.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    if (maKHText.Length > 0) readString += " AND MaKhachHang Like '%" + maKHText + "%'";
                    if (loaiKHText.Length > 0) readString += " AND TenLoaiKhachHang = N'" + loaiKHText + "'";
                    if (tenKHText.Length > 0) readString += " AND TenKhachHang Like N'%" + tenKHText + "%'";
                    if (dateTime != null) readString += " AND NgaySinh = '" + ((dateTime??DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (sdtText.Length > 0) readString += " AND SDT Like '%" + sdtText + "%'";
                    if (index != 2) readString += " AND TrangThai = '" + index + "'";

                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khachHangList.Add(new KhachHang(stt: count, maKhachHang: (String)reader["MaKhachHang"],
                            tenKhachHang: (String)reader["TenKhachHang"], ngaySinh: (DateTime)reader["NgaySinh"],
                            gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            sdt: (String)reader["SDT"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() => {
                        resultKhachHangTable.ItemsSource = khachHangList;
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

        private void resultKhachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.IsReadOnly = true;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKH.Text = "";
            loaiKH.SelectedIndex = 0;
            name.Text = "";
            sdt.Text = "";
            trangThai.SelectedIndex = 0;
            ngaySinh.SelectedDate = DateTime.Now;
            selectedKhachHang = new List<KhachHang>();
            chooseKhachHangTable.ItemsSource = new List<KhachHang>();
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (resultKhachHangTable.SelectedIndex != -1)
            {
                selectedKhachHang.Add(khachHangList[resultKhachHangTable.SelectedIndex]);
                List<KhachHang> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhachHang).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhachHangTable.ItemsSource = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (resultKhachHangTable.SelectedIndex != -1)
            {
                selectedKhachHang.Remove(khachHangList[resultKhachHangTable.SelectedIndex]);
                List<KhachHang> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhachHang).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhachHangTable.ItemsSource = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource = showSelectedKhachHang;
            }
        }
    }
}
