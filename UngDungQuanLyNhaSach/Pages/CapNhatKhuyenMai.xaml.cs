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
            ((Home)window).MainWindowFrame.Navigate(new ThemKhuyenMai());
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
            return true;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
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
                    command.Parameters["@MaKhuyenMai"].Value = data;

                    command.Parameters.Add("@ThoiGianBatDau", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianBatDau"].Value = DateTime.Now;

                    command.Parameters.Add("@ThoiGianKetThuc", SqlDbType.SmallDateTime);
                    command.Parameters["@ThoiGianKetThuc"].Value = DateTime.Now;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                        (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                    command.Parameters.Add("@SoLuongKhuyenMai", SqlDbType.Int);
                    command.Parameters["@SoLuongKhuyenMai"].Value = int.Parse(soLuong.Text);

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    MessageBox.Show("Cập nhật thành công");
                    var window = Window.GetWindow(this);
                    ((Home)window).MainWindowFrame.Navigate(new ThemKhuyenMai());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
