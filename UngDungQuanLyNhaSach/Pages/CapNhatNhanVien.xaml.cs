using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "ThemNhanVien", ".xaml"), UriKind.RelativeOrAbsolute));
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
                        name.Text = nhanVien.hoTen;
                        cccd.Text = nhanVien.cccd;
                        sdt.Text = nhanVien.sdt;
                        email.Text = nhanVien.email;
                        diaChi.Text = nhanVien.diaChi;
                        gioiTinh.SelectedIndex = nhanVien.gioiTinh == "Nam" ? 0 : 1;
                        luong.Text = nhanVien.luong.ToString();
                        chucVu.SelectedIndex = nhanVien.maNhanVien == "NVBH" ? 1 : (nhanVien.maNhanVien == "NVK" ? 2 : 0);
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
