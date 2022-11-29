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
            loadData();
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG, LOAIKHACHHANG WHERE KHACHHANG.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khachHangList.Add(new KhachHang(stt: count, maKhachHang: (String)reader["MaKhachHang"],
                            tenKhachHang: (String)reader["TenKhachHang"],
                            gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            sdt: (String)reader["SDT"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() => {
                        resultKhachHangTable.ItemsSource = khachHangList;
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

        private void resultKhachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKH.Text = "";
            loaiKH.SelectedIndex = 0;
            name.Text = "";
            totalMoney.Text = "";
            sdt.Text = "";
            trangThai.SelectedIndex = 0;
            selectedKhachHang = new List<KhachHang>();
            chooseKhachHangTable.ItemsSource = new List<KhachHang>();
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void resultKhachHangTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultKhachHangTable.SelectedIndex != -1)
            {
                selectedKhachHang.Remove(khachHangList[resultKhachHangTable.SelectedIndex]);
                selectedKhachHang.Add(khachHangList[resultKhachHangTable.SelectedIndex]);
                List < KhachHang> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhachHang).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++) 
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }    
                chooseKhachHangTable.ItemsSource = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource = showSelectedKhachHang;
            }
        }

        bool checkSearch(KhachHang khachHang)
        {
            if (!khachHang.maKhachHang.ToLower().Contains(maKH.Text.ToLower())) return false;
            if (!khachHang.tenKhachHang.ToLower().Contains(name.Text.ToLower())) return false;
            if (!khachHang.sdt.Contains(sdt.Text)) return false;
            MessageBox.Show(khachHang.trangThai + " " + trangThai.SelectedIndex);
            if (trangThai.SelectedIndex != 0 && !trangThai.SelectedIndex.ToString().Contains(khachHang.trangThai)) 
                return false;
            return true;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            List<KhachHang> searchList = new List<KhachHang>();
            foreach (KhachHang kh in khachHangList)
            {
                if (checkSearch(kh))
                {
                    searchList.Add(kh);
                }    
            }
            resultKhachHangTable.ItemsSource = new List<KhachHang>();
            resultKhachHangTable.ItemsSource = searchList;
        }
    }
}
