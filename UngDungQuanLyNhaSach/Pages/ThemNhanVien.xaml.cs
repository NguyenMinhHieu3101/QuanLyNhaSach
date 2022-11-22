using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static System.Net.Mime.MediaTypeNames;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemNhanVien.xaml
    /// </summary>
    public partial class ThemNhanVien : Page
    {
        public ThemNhanVien()
        {
            InitializeComponent();
            loadListStaff();
        }

        void resetData()
        {
            name.Text = "";
            chucVu.SelectedIndex = 0;
            cccd.Text = "";
            sdt.Text = "";
            diaChi.Text = "";
            luong.Text = "";
        }

        void loadListStaff()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<NhanVien> nhanVienList = new List<NhanVien>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from NHANVIEN";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;

                        nhanVienList.Add(new NhanVien(stt: count, maNhanVien: (String)reader["MaNhanVien"],
                            hoTen: (String)reader["HoTen"], maChucVu: (String)reader["MaChucVu"],
                            ngaySinh: (DateTime)reader["NgaySinh"],  //DateTime.ParseExact(reader["NgaySinh"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            cccd: (String)reader["CCCD"], gioiTinh: (String)reader["GioiTinh"], sdt: (String)reader["SDT"],
                            diaChi: (String)reader["DiaChi"], luong: double.Parse(reader["Luong"].ToString()),
                            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Đã nghỉ việc" : "Còn hoạt động"));                       
                    }
                    this.Dispatcher.BeginInvoke(new Action(() => {
                        nhanVienTable.ItemsSource = nhanVienList;
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

        private void nhanVienTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                string readString = "select Count(*) from NHANVIEN";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                string insertString = "INSERT INTO NHANVIEN(MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) " +
                    "VALUES(@MaNhanVien, @MatKhau, @HoTen, @MaChucVu, @NgaySinh, @Email, @CCCD, @GioiTinh, @SDT, @DiaChi, @Luong, @TrangThai)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaNhanVien", SqlDbType.VarChar);
                command.Parameters["@MaNhanVien"].Value = "NV" + count.ToString();

                command.Parameters.Add("@MatKhau", SqlDbType.VarChar);
                command.Parameters["@MatKhau"].Value = "12345678";

                command.Parameters.Add("@HoTen", SqlDbType.NVarChar);
                command.Parameters["@HoTen"].Value = name.Text;

                command.Parameters.Add("@MaChucVu", SqlDbType.VarChar);
                command.Parameters["@MaChucVu"].Value = chucVu.SelectedIndex == 0 ? "Admin" :
                    (chucVu.SelectedIndex == 1 ? "NVBH" : "NVK");

                command.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime);
                command.Parameters["@NgaySinh"].Value = DateTime.Now;

                //MessageBox.Show(ngaySinh.SelectedDate.Value.Date.ToString());
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

                MessageBox.Show("Thêm thành công");
                connection.Close();
                loadListStaff();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
