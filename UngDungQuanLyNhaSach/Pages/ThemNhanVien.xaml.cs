using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                            cccd: (String)reader["CCCD"], gioiTinh: (String)reader["GioiTinh"], sdt: (String)reader["SDT"], email: (String)reader["Email"],
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (checkDataInput())
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

                    //MessageBox.Show(ngaySinh.SelectedDate.ToString());

                    command.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime);
                    command.Parameters["@NgaySinh"].Value = DateTime.Now;

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
                    loadListStaff();
                    MessageBox.Show("Thêm thành công");
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

        private void sdt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void cccd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }
    }
}
