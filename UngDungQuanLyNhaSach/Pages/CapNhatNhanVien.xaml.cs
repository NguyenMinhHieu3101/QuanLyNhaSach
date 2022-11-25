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
    /// Interaction logic for CapNhatNhanVien.xaml
    /// </summary>
    public partial class CapNhatNhanVien : Page
    {
        String data;
        public CapNhatNhanVien(string data)
        {
            InitializeComponent();
            this.data = data;
            loadData();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new ThemNhanVien());
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from NHANVIEN Where MaNhanVien = @maNv";
                    SqlCommand command = new SqlCommand(readString, connection);
                    command.Parameters.Add("@maNv", SqlDbType.VarChar);
                    command.Parameters["@maNv"].Value = data;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    NhanVien nhanVien = new NhanVien(stt: 0, maNhanVien: (String)reader["MaNhanVien"],
                        hoTen: (String)reader["HoTen"], maChucVu: (String)reader["MaChucVu"],
                        ngaySinh: (DateTime)reader["NgaySinh"],  //DateTime.ParseExact(reader["NgaySinh"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        cccd: (String)reader["CCCD"], gioiTinh: (String)reader["GioiTinh"], sdt: (String)reader["SDT"], email: (String)reader["Email"],
                        diaChi: (String)reader["DiaChi"], luong: double.Parse(reader["Luong"].ToString()),
                        trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Đã nghỉ việc" : "Còn hoạt động");

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ngaySinh.SelectedDate = nhanVien.ngaySinh;
                        name.Text = nhanVien.hoTen;
                        cccd.Text = nhanVien.cccd;
                        sdt.Text = nhanVien.sdt;
                        email.Text = nhanVien.email;
                        diaChi.Text = nhanVien.diaChi;
                        gioiTinh.SelectedIndex = nhanVien.gioiTinh == "Nam" ? 0 : 1;
                        luong.Text = nhanVien.luong.ToString();
                        chucVu.SelectedIndex = nhanVien.maChucVu == "NVBH" ? 1 : (nhanVien.maChucVu == "NVK" ? 2 : 0);
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
            double distance;
            if (cccd.Text.Length == 0 || cccd.Text.Length < 10 || !double.TryParse(cccd.Text, out distance))
            {
                MessageBox.Show("CCCD không hợp lệ");
                return false;
            }
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
            if (!double.TryParse(luong.Text, out distance))
            {
                MessageBox.Show("Lương không hợp lệ");
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (checkDataInput())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string updateString = "UPDATE NHANVIEN SET HoTen = @HoTen, MaChucVu = @MaChucVu, NgaySinh = @NgaySinh, Email = @Email, CCCD = @CCCD, GioiTinh = @GioiTinh, SDT = @SDT, DiaChi = @DiaChi, Luong = @Luong, TrangThai = @TrangThai " +
                        "Where MaNhanVien = @MaNhanVien";
                    SqlCommand command = new SqlCommand(updateString, connection);

                    command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar);
                    command.Parameters["@MaNhanVien"].Value = data;
                    
                    command.Parameters.Add("@HoTen", SqlDbType.NVarChar);
                    command.Parameters["@HoTen"].Value = name.Text;

                    command.Parameters.Add("@MaChucVu", SqlDbType.VarChar);
                    command.Parameters["@MaChucVu"].Value = chucVu.SelectedIndex == 0 ? "Admin" :
                        (chucVu.SelectedIndex == 1 ? "NVBH" : "NVK");

                    command.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime);
                    command.Parameters["@NgaySinh"].Value = ngaySinh.SelectedDate;

                    command.Parameters.Add("@Email", SqlDbType.VarChar);
                    command.Parameters["@Email"].Value = email.Text;

                    command.Parameters.Add("@CCCD", SqlDbType.VarChar);
                    command.Parameters["@CCCD"].Value = cccd.Text;

                    command.Parameters.Add("@GioiTinh", SqlDbType.NVarChar);
                    command.Parameters["@GioiTinh"].Value = gioiTinh.Text;

                    command.Parameters.Add("@SDT", SqlDbType.VarChar);
                    command.Parameters["@SDT"].Value = sdt.Text;

                    command.Parameters.Add("@DiaChi", SqlDbType.NVarChar);
                    command.Parameters["@DiaChi"].Value = diaChi.Text;

                    command.Parameters.Add("@Luong", SqlDbType.Money);
                    command.Parameters["@Luong"].Value = luong.Text;

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    MessageBox.Show("Sửa thành công");
                    var window = Window.GetWindow(this);
                    ((Home)window).MainWindowFrame.Navigate(new ThemNhanVien());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void luong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }
    }
}
