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
using UngDungQuanLyNhaSach.Model;
using System.ComponentModel;
using System.Threading;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemKhachHang.xaml
    /// </summary>
    public partial class ThemKhachHang : Page
    {
        public ThemKhachHang()
        {
            InitializeComponent();
            loadData();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                string readString = "select Count(*) from KHACHHANG";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                string insertString = "INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, DiaChi, GioiTinh, MaLoaiKhachHang, SDT, Email, TrangThai) " +
                    "VALUES (@MaKhachHang, @TenKhachHang, @DiaChi, @GioiTinh, @MaLoaiKhachHang, @SDT, @Email, @TrangThai)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                command.Parameters["@MaKhachHang"].Value = "NV" + count.ToString();

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

                MessageBox.Show("Thêm thành công");
                connection.Close();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<KhachHang> khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khachHangList.Add(new KhachHang(stt: count, maKhachHang: (String)reader["MaKhachHang"],
                            tenKhachHang: (String)reader["TenKhachHang"], diaChi: (String)reader["DiaChi"],
                            gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                            sdt: (String)reader["SDT"], email: (String)reader["Email"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Không tồn tại" : "Còn sử dụng"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        khachHangTable.ItemsSource = khachHangList;
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

        private void khachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
