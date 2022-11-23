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
using System.Text.RegularExpressions;

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
            updateMaKhachHang();
            loadData();
        }

        void updateMaKhachHang()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from KHACHHANG";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maKH.Text = "NV" + count.ToString();
                    }));
                }
                catch (Exception ex) { }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (checkDataInput())
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

                    connection.Close();
                    loadData();
                    updateMaKhachHang();
                    MessageBox.Show("Thêm thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void totalMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            //String text = name.Text;
            //var list = text.Split(" ");
            //text = "";
            //for (int i =0;i<list.Length;i++)
            //{
            //    text += list[i][0].ToString().ToUpper() + list[i].Substring(1, list[i].Length-1);
            //    if (i < list.Length - 1) text += " ";
            //}    
            //name.Text = text;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "CapNhatKhachHang", ".xaml"), UriKind.RelativeOrAbsolute));
        }
    }
}
