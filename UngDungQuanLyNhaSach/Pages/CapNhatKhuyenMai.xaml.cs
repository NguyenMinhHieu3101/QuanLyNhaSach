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
    /// Interaction logic for CapNhatKhuyenMai.xaml
    /// </summary>
    public partial class CapNhatKhuyenMai : Page
    {
        String data;

        public CapNhatKhuyenMai(string data)
        {
            InitializeComponent();
            this.data = data;
            loadData();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "ThemKhuyenMai", ".xaml"), UriKind.RelativeOrAbsolute));
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void phanTram_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void soLuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                    string readString = "select * from KHUYENMAI Where MaKhuyenMai = @maKM";
                    SqlCommand command = new SqlCommand(readString, connection);
                    command.Parameters.Add("@maKM", SqlDbType.VarChar);
                    command.Parameters["@maKM"].Value = data;

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    KhuyenMai khuyenMai = new KhuyenMai(stt: 0, maKhuyenMai: (String)reader["MaKhuyenMai"],
                        batDau: (DateTime)reader["ThoiGianBatDau"], //DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        ketThuc: (DateTime)reader["ThoiGianKetThuc"], //DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                        soLuong: (int)reader["SoLuongKhuyenMai"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực");

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maKM.Text = khuyenMai.maKhuyenMai;
                        soLuong.Text = khuyenMai.soLuong.ToString();
                        loaiKhachHang.SelectedIndex = khuyenMai.maLoaiKhachHang == "VL" ? 0 :
                        (khuyenMai.maLoaiKhachHang == "B" ? 1 : (khuyenMai.maLoaiKhachHang == "V" ? 2 : 3));
                        ngayBatDau.SelectedDate = khuyenMai.batDau;
                        ngayKetThuc.SelectedDate = khuyenMai.ketThuc;
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
    }
}
