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
            ((Home)window).MainWindowFrame.Navigate(new ThemKhachHang());
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
                        tenKhachHang: (String)reader["TenKhachHang"], ngaySinh: (DateTime)reader["NgaySinh"],
                        gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                        sdt: (String)reader["SDT"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Không tồn tại" : "Còn sử dụng");
                    
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maKH.Text = khachHangData.maKhachHang;
                        name.Text = khachHangData.tenKhachHang;
                        gioiTinh.SelectedIndex = khachHangData.gioiTinh == "Nam" ? 0 : 1;
                        loaiKhachHang.SelectedIndex = khachHangData.maLoaiKhachHang == "VL" ? 0 :
                        (khachHangData.maLoaiKhachHang == "B" ? 1 : (khachHangData.maLoaiKhachHang == "V" ? 2 : 3));
                        sdt.Text = khachHangData.sdt;
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
            if (sdt.Text.Length == 0 || !Regex.IsMatch(sdt.Text, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }
            if (!Regex.IsMatch(email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                MessageBox.Show("Email không hợp lệ");
                return false;
            }
            if (totalMoney.Text.Length == 0)
            {
                MessageBox.Show("Lương không hợp lệ");
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

                    string updateString = "UPDATE KHACHHANG SET TenKhachHang = @TenKhachHang, DiaChi = @DiaChi, GioiTinh = @GioiTinh, MaLoaiKhachHang = @MaLoaiKhachHang, SDT = @SDT, Email = @Email, TrangThai = @TrangThai " +
                        "WHERE MaKhachHang = @MaKhachHang";
                    SqlCommand command = new SqlCommand(updateString, connection);

                    command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaKhachHang"].Value = data;

                    command.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar);
                    command.Parameters["@TenKhachHang"].Value = name.Text;

                    command.Parameters.Add("@DiaChi", SqlDbType.NVarChar);
                    command.Parameters["@DiaChi"].Value = diaChi.Text;

                    command.Parameters.Add("@GioiTinh", SqlDbType.NVarChar);
                    command.Parameters["@GioiTinh"].Value = gioiTinh.Text;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = "VL";

                    command.Parameters.Add("@SDT", SqlDbType.VarChar);
                    command.Parameters["@SDT"].Value = sdt.Text;

                    command.Parameters.Add("@Email", SqlDbType.VarChar);
                    command.Parameters["@Email"].Value = email.Text;

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    MessageBox.Show("Cập nhật thành công");
                    var window = Window.GetWindow(this);
                    ((Home)window).MainWindowFrame.Navigate(new ThemKhachHang());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
