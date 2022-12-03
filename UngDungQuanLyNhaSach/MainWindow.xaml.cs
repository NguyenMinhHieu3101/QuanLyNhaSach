using System;
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
using UngDungQuanLyNhaSach.Pages;
using System.Threading;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            checkDataKM();
        }

        private void Button_SignIn_Click(object sender, RoutedEventArgs e)
        {
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
            this.Hide();
        }

        void checkDataKM()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        KhuyenMai khuyenMai = new KhuyenMai(stt: 0, maKhuyenMai: (String)reader["MaKhuyenMai"],
                            batDau: (DateTime)reader["ThoiGianBatDau"], //DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            ketThuc: (DateTime)reader["ThoiGianKetThuc"], //DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            soLuong: (int)reader["SoLuongKhuyenMai"], phanTram: (int)reader["PhanTram"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực");
                        
                        if (khuyenMai.ketThuc < DateTime.Now) updateKM(khuyenMai);
                    }
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

        void updateKM(KhuyenMai khuyenMai)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string deleteString = "UPDATE KHUYENMAI SET TrangThai = '0' Where MaKhuyenMai = @MaKhuyenMai";
                SqlCommand deletecommand = new SqlCommand(deleteString, connection);
                deletecommand.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                deletecommand.Parameters["@MaKhuyenMai"].Value = khuyenMai.maKhuyenMai;

                deletecommand.ExecuteNonQuery();
                connection.Close();

            }));
            thread.IsBackground = true;
            thread.Start();
        }    
    }
}
