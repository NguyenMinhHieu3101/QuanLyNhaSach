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
using System.Xml.Linq;
using System.Windows.Markup;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemKhachHang.xaml
    /// </summary>
    public partial class ThemKhachHang : Page
    {
        List<KhachHang> khachHangList = new List<KhachHang>();
        int currentSelected = -1;

        public ThemKhachHang()
        {
            InitializeComponent();
            update.IsEnabled = false;
            delete.IsEnabled = false;
            updateMaKhachHang();
            ngaySinh.SelectedDate = DateTime.Now;
            loadData();
        }

        void reset()
        {
            name.Text = "";
            gioiTinh.SelectedIndex = 0;
            loaiKhachHang.Text = "";
            sdt.Text = "";
            ngaySinh.SelectedDate = DateTime.Now;
            updateMaKhachHang();
            hideError();
        }

        void hideError()
        {
            name_error.Visibility= Visibility.Hidden;
            sdt_error.Visibility= Visibility.Hidden;
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
                        maKH.Text = "KH" + count.ToString("000");
                    }));
                }
                catch { }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (checkDataInput(true))
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from KHACHHANG";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                    string insertString = "INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, NgaySinh, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) " +
                        "VALUES (@MaKhachHang, @TenKhachHang, @NgaySinh, @GioiTinh, @MaLoaiKhachHang, @SDT, @TrangThai)";
                    SqlCommand command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaKhachHang"].Value = "KH" + count.ToString("000");

                    command.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar);
                    command.Parameters["@TenKhachHang"].Value = name.Text;
                    
                    command.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime);
                    command.Parameters["@NgaySinh"].Value = ngaySinh.SelectedDate;

                    command.Parameters.Add("@GioiTinh", SqlDbType.NVarChar);
                    command.Parameters["@GioiTinh"].Value = gioiTinh.Text;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                        (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                    command.Parameters.Add("@SDT", SqlDbType.VarChar);
                    command.Parameters["@SDT"].Value = sdt.Text;

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    MessageBox.Show("Thêm thành công");
                    updateMaKhachHang();
                    reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        bool checkDataInput(bool check)
        {
            if (sdt.Text.Length == 0 || !Regex.IsMatch(sdt.Text, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$") || sdt.Text.Length !=10 && sdt.Text.Length != 11)
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }
            if (check)
            {
                foreach (KhachHang khachHang in khachHangList)
                {
                    if (khachHang.sdt == sdt.Text)
                    {
                        MessageBox.Show("Số điện thoại đã có trong hệ thống");
                        return false;
                    }
                }
            }
            return true;
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG, LOAIKHACHHANG WHERE KHACHHANG.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khachHangList.Add(new KhachHang(stt: count, maKhachHang: (String)reader["MaKhachHang"],
                            tenKhachHang: (String)reader["TenKhachHang"], ngaySinh: (DateTime)reader["NgaySinh"],
                            gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            sdt: (String)reader["SDT"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Không tồn tại" : "Còn sử dụng"));
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
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if (name.Text.Length==0)
            {
                name_error.Text = "Vui lòng nhập vào tên khách hàng";
                name_error.Visibility = Visibility.Visible;
            }    
            else
            {
                name_error.Visibility = Visibility.Hidden;
            }

            String text = name.Text;
            var list = text.Split(" ");
            text = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Length > 0)
                {
                    text += list[i][0].ToString().ToUpper() + list[i].Substring(1, list[i].Length - 1);                    
                }
                if (i < list.Length - 1) text += " ";
            }

            name.TextChanged -= name_TextChanged;
            name.Text = text;
            name.TextChanged += name_TextChanged;
            name.Select(name.Text.Length, 0);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            /*if (khachHangTable.SelectedIndex != -1)
            {
                var window = Window.GetWindow(this);
                ((Home)window).MainWindowFrame.Navigate(new CapNhatKhachHang(khachHangList[khachHangTable.SelectedIndex].maKhachHang));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa");
            }   */
            if (checkDataInput(false))
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string updateString = "UPDATE KHACHHANG SET TenKhachHang = @TenKhachHang, GioiTinh = @GioiTinh, MaLoaiKhachHang = @MaLoaiKhachHang, SDT = @SDT, TrangThai = @TrangThai " +
                        "WHERE MaKhachHang = @MaKhachHang";
                    SqlCommand command = new SqlCommand(updateString, connection);

                    command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaKhachHang"].Value = khachHangList[khachHangTable.SelectedIndex].maKhachHang;

                    command.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar);
                    command.Parameters["@TenKhachHang"].Value = name.Text;

                    command.Parameters.Add("@GioiTinh", SqlDbType.NVarChar);
                    command.Parameters["@GioiTinh"].Value = gioiTinh.Text;

                    command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                    command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                        (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                    command.Parameters.Add("@SDT", SqlDbType.VarChar);
                    command.Parameters["@SDT"].Value = sdt.Text;

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    loadData();
                    reset();
                    MessageBox.Show("Cập nhật thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (khachHangTable.SelectedIndex != -1)
            {
                var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();

                        string deleteString = "UPDATE KHACHHANG SET TrangThai = '0' Where MaKhachHang = @MaKhachHang";
                        SqlCommand command = new SqlCommand(deleteString, connection);
                        command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                        command.Parameters["@MaKhachHang"].Value = khachHangList[khachHangTable.SelectedIndex].maKhachHang;

                        command.ExecuteNonQuery();
                        connection.Close();
                        loadData();
                        reset();
                        MessageBox.Show("Xóa khách hàng thành công");
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa");
            }
        }

        private void khachHangTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (khachHangTable.SelectedIndex != -1)
            {
                if (currentSelected != -1)
                {
                    DataGridRow curentRow = (DataGridRow)khachHangTable.ItemContainerGenerator.ContainerFromIndex(currentSelected);
                    Setter normal = new Setter(TextBlock.FontWeightProperty, FontWeights.Normal, null);
                    Style normalStyle = new Style(curentRow.GetType());
                    normalStyle.Setters.Add(normal);
                    curentRow.Style = normalStyle;
                }
                DataGridRow row = (DataGridRow)khachHangTable.ItemContainerGenerator.ContainerFromIndex(khachHangTable.SelectedIndex);
                Setter bold = new Setter(TextBlock.FontWeightProperty, FontWeights.Bold, null);
                Style newStyle = new Style(row.GetType());
                newStyle.Setters.Add(bold);
                row.Style = newStyle;
                currentSelected = khachHangTable.SelectedIndex;

                update.IsEnabled = true;
                delete.IsEnabled = true;
                loadKH();
            }
            else
            {
                update.IsEnabled = false;
                delete.IsEnabled = false;
            }
        }

        void loadKH()
        {
            KhachHang khachHangData = khachHangList[khachHangTable.SelectedIndex];
            maKH.Text = khachHangData.maKhachHang;
            name.Text = khachHangData.tenKhachHang;
            gioiTinh.SelectedIndex = khachHangData.gioiTinh == "Nam" ? 0 : 1;
            loaiKhachHang.SelectedIndex = khachHangData.maLoaiKhachHang == "Vãng Lai" ? 0 :
            (khachHangData.maLoaiKhachHang == "Bạc" ? 1 : (khachHangData.maLoaiKhachHang == "Vàng" ? 2 : 3));
            sdt.Text = khachHangData.sdt;
        }

        private void sdt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(sdt.Text, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$") || sdt.Text.Length != 10 && sdt.Text.Length != 11)
            {
                sdt_error.Text = sdt.Text.Length > 0 ? "Số điện thoại không hợp lệ" : "Vui lòng nhập vào số điện thoại";
                sdt_error.Visibility= Visibility.Visible;
            }    
            else
            {
                sdt_error.Visibility = Visibility.Hidden;
            }
        }
    }
}
