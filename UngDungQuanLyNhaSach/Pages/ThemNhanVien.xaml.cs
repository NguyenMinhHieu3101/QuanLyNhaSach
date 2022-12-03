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
using System.Windows.Markup;
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
        List<NhanVien> nhanVienList = new List<NhanVien>();
        int currentSelected = -1;

        public ThemNhanVien()
        {
            InitializeComponent();
            ngaySinh.SelectedDate = DateTime.Now;
            loadListStaff();
            updateMaNhanVien();
            update.IsEnabled = false;
            delete.IsEnabled = false;
        }

        void resetData()
        {
            name.Text = "";
            chucVu.SelectedIndex = 0;
            cccd.Text = "";
            sdt.Text = "";
            diaChi.Text = "";
            luong.Text = "";
            gioiTinh.SelectedIndex = 0;
            email.Text = "";
            ngaySinh.SelectedDate = DateTime.Now;
            updateMaNhanVien();
        }

        void loadListStaff()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                nhanVienList = new List<NhanVien>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from NHANVIEN, CHUCVU WHERE NHANVIEN.MaChucVu = CHUCVU.MaChucVu";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;

                        nhanVienList.Add(new NhanVien(stt: count, maNhanVien: (String)reader["MaNhanVien"],
                            hoTen: (String)reader["HoTen"], maChucVu: (String)reader["TenChucVu"],
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

        void updateMaNhanVien()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from NHANVIEN";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        maNv.Text = "NV" + count.ToString("000");
                    }));
                }
                catch (Exception e) {
                    MessageBox.Show(e.ToString());
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
            if (cccd.Text.Length == 0 || cccd.Text.Length < 10 || !double.TryParse(cccd.Text, out distance) || cccd.Text.Length!=9 || cccd.Text.Length != 12)
            {
                MessageBox.Show("CCCD không hợp lệ");
                return false;
            }   
            if (sdt.Text.Length == 0 || !Regex.IsMatch(sdt.Text, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$") || sdt.Text.Length != 10 || sdt.Text.Length != 11)
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
            if (DateTime.Now.AddYears(-18) < ngaySinh.SelectedDate)
            {
                MessageBox.Show("Ngày sinh chưa đủ 18 tuổi");
                return false;
            }    
            foreach (NhanVien nhanVien in nhanVienList)
            {
                if (nhanVien.sdt == sdt.Text || nhanVien.cccd == cccd.Text || nhanVien.email == email.Text)
                {
                    if (nhanVien.sdt == sdt.Text)
                    {
                        MessageBox.Show("Số điện thoại đã có trong hệ thống");
                    }
                    else if (nhanVien.cccd == cccd.Text)
                    {
                        MessageBox.Show("CCCD đã có trong hệ thống");
                    }
                    else if (nhanVien.email == email.Text)
                    {
                        MessageBox.Show("Email đã có trong hệ thống");
                    }
                    return false;
                }    
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
                        (chucVu.SelectedIndex == 1 ? "NVBH" : (chucVu.SelectedIndex == 2 ? "NVK" : "NVKT") );

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
                    loadListStaff();
                    MessageBox.Show("Thêm thành công");
                    resetData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ngaySinh_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            MessageBox.Show(date.ToString());
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            /*if (nhanVienTable.SelectedIndex != -1)
            {
                var window = Window.GetWindow(this);
                ((Home)window).MainWindowFrame.Navigate(new CapNhatNhanVien(nhanVienList[nhanVienTable.SelectedIndex].maNhanVien));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để chỉnh sửa");
            }*/
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
                    command.Parameters["@MaNhanVien"].Value = nhanVienList[nhanVienTable.SelectedIndex].maNhanVien;

                    command.Parameters.Add("@HoTen", SqlDbType.NVarChar);
                    command.Parameters["@HoTen"].Value = name.Text;

                    command.Parameters.Add("@MaChucVu", SqlDbType.VarChar);
                    command.Parameters["@MaChucVu"].Value = chucVu.SelectedIndex == 0 ? "Admin" :
                        (chucVu.SelectedIndex == 1 ? "Nhân Viên Bán Hàng" : "Nhân Viên Kho");

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
                    loadListStaff();
                    resetData();
                    MessageBox.Show("Sửa thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (nhanVienTable.SelectedIndex != -1)
            {
                var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();

                        string deleteString = "UPDATE NHANVIEN SET TrangThai = '0' Where MaNhanVien = @MaNhanVien";
                        SqlCommand command = new SqlCommand(deleteString, connection);
                        command.Parameters.Add("@MaNhanVien", SqlDbType.VarChar);
                        command.Parameters["@MaNhanVien"].Value = nhanVienList[nhanVienTable.SelectedIndex].maNhanVien;

                        command.ExecuteNonQuery();
                        connection.Close();
                        loadListStaff();
                        MessageBox.Show("Xóa nhân viên thành công");
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa");
            }    
        }

        void loadNV()
        {
            NhanVien nhanVien = nhanVienList[nhanVienTable.SelectedIndex];
            ngaySinh.SelectedDate = nhanVien.ngaySinh;
            name.Text = nhanVien.hoTen;
            cccd.Text = nhanVien.cccd;
            sdt.Text = nhanVien.sdt;
            email.Text = nhanVien.email;
            diaChi.Text = nhanVien.diaChi;
            gioiTinh.SelectedIndex = nhanVien.gioiTinh == "Nam" ? 0 : 1;
            luong.Text = nhanVien.luong.ToString();
            chucVu.SelectedIndex = nhanVien.maChucVu == "NVBH" ? 1 : (nhanVien.maChucVu == "NVK" ? 2 : 0);
            maNv.Text = nhanVien.maNhanVien;
        }

        private void nhanVienTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nhanVienTable.SelectedIndex != -1)
            {
                if (currentSelected != -1)
                {
                    DataGridRow curentRow = (DataGridRow)nhanVienTable.ItemContainerGenerator.ContainerFromIndex(currentSelected);
                    Setter normal = new Setter(TextBlock.FontWeightProperty, FontWeights.Normal, null);
                    Style normalStyle = new Style(curentRow.GetType());
                    normalStyle.Setters.Add(normal);
                    curentRow.Style = normalStyle;
                }    
                DataGridRow row = (DataGridRow)nhanVienTable.ItemContainerGenerator.ContainerFromIndex(nhanVienTable.SelectedIndex);
                Setter bold = new Setter(TextBlock.FontWeightProperty, FontWeights.Bold, null);
                Style newStyle = new Style(row.GetType());
                newStyle.Setters.Add(bold);
                row.Style = newStyle;
                currentSelected = nhanVienTable.SelectedIndex;

                update.IsEnabled = true;
                delete.IsEnabled = true;
                loadNV();
            }
            else
            {
                update.IsEnabled = false;
                delete.IsEnabled = false;
            }
        }    
    }
}
