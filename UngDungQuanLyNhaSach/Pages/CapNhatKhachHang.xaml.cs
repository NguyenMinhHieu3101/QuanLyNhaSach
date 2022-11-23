using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CapNhatKhachHang.xaml
    /// </summary>
    public partial class CapNhatKhachHang : Page
    {
        String data;

        public CapNhatKhachHang(String data)
        {
            InitializeComponent();
            this.data = data;
            loadData();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "ThemKhachHang", ".xaml"), UriKind.RelativeOrAbsolute));
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void totalMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG WHERE MaKhachHang = @MaKH";
                    SqlCommand command = new SqlCommand(readString, connection);
                    command.Parameters.Add("@MaKH", SqlDbType.VarChar);
                    command.Parameters["@MaKH"].Value = data;

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    KhachHang khachHangData = new KhachHang(stt: 0, maKhachHang: (String)reader["MaKhachHang"],
                        tenKhachHang: (String)reader["TenKhachHang"], diaChi: (String)reader["DiaChi"],
                        gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                        sdt: (String)reader["SDT"], email: (String)reader["Email"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Không tồn tại" : "Còn sử dụng");
                    
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maKH.Text = khachHangData.maKhachHang;
                        name.Text = khachHangData.tenKhachHang;
                        gioiTinh.SelectedIndex = khachHangData.gioiTinh == "Nam" ? 0 : 1;
                        diaChi.Text = khachHangData.diaChi;
                        loaiKhachHang.SelectedIndex = khachHangData.maLoaiKhachHang == "VL" ? 0 :
                        (khachHangData.maLoaiKhachHang == "B" ? 1 : (khachHangData.maLoaiKhachHang == "V" ? 2 : 3));
                        sdt.Text = khachHangData.sdt;
                        email.Text = khachHangData.email;
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

        private void update_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
