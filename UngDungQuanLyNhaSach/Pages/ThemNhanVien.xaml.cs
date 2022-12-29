using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
        List<BaseNhanVien> baseNhanVienList = new List<BaseNhanVien>();
        List<DonViHanhChinh> provinces = new List<DonViHanhChinh>();
        List<DonViHanhChinh> districts = new List<DonViHanhChinh>();
        List<DonViHanhChinh> wards = new List<DonViHanhChinh>();
        String huyenText = "";
        int currentSelected = -1;

        public ThemNhanVien()
        {
            InitializeComponent();
            ngaySinh.SelectedDate = DateTime.Now.AddYears(-18);
            loadListStaff(false);
            loadTinh();
            updateMaNhanVien();
            ngaySinh.DisplayDateEnd = DateTime.Now.AddYears(-18);
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
            tinh.SelectedIndex = -1;
            huyen.SelectedIndex = -1;
            huyen.Text = "";
            phuong.SelectedIndex = -1;
            phuong.Text = "";
            updateMaNhanVien();
            hideError();
        }

        void hideError()
        {
            cccd_error.Visibility = Visibility.Hidden;
            email_error.Visibility = Visibility.Hidden;
            luong_error.Visibility = Visibility.Hidden;
            name_error.Visibility = Visibility.Hidden;
            sdt_error.Visibility = Visibility.Hidden;
            diaChi_error.Visibility = Visibility.Hidden;
        }

        private int getNumberItem()
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                string readString = "select Count(*) from NHANVIEN";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar();
                connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        void loadListStaff(bool check)
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
                    baseNhanVienList = new List<BaseNhanVien>();

                    int count = 0;
                    int number = getNumberItem();

                    while (reader.Read())
                    {
                        count++;

                        NhanVien nhanVien = new NhanVien(stt: count, maNhanVien: (String)reader["MaNhanVien"],
                            hoTen: (String)reader["HoTen"], maChucVu: (String)reader["TenChucVu"],
                            ngaySinh: (DateTime)reader["NgaySinh"],  //DateTime.ParseExact(reader["NgaySinh"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            cccd: (String)reader["CCCD"], gioiTinh: (String)reader["GioiTinh"], sdt: (String)reader["SDT"], email: (String)reader["Email"],
                            diaChi: (String)reader["DiaChi"], luong: string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(reader["luong"].ToString())),
                            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Đã nghỉ việc" : "Còn hoạt động");

                        if (count == number && check)
                        {
                            baseNhanVienList.Insert(0, nhanVien.toBase());
                            nhanVienList.Insert(0, nhanVien);
                            for (int i = 0; i < baseNhanVienList.Count; i++)
                            {
                                baseNhanVienList[i].stt = i + 1;
                                nhanVienList[i].stt = i + 1;
                            }
                        }
                        else
                        {
                            baseNhanVienList.Add(nhanVien.toBase());
                            nhanVienList.Add(nhanVien);
                        }
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        nhanVienTable.ItemsSource = baseNhanVienList;
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

        void loadTinh()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from provinces";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    List<DonViHanhChinh> temp = new List<DonViHanhChinh>();

                    while (reader.Read())
                    {
                        temp.Add(new DonViHanhChinh((String)reader["code"], (String)reader["name"]));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        provinces.AddRange(temp.OrderBy(e => e.name).ToList());
                        tinh.ItemsSource = provinces.Select(e => e.name).ToList();
                        huyen.ItemsSource = new List<String>();
                        phuong.ItemsSource = new List<String>();
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

        void loadHuyen()
        {
            if (tinh.SelectedIndex != -1)
            {
                String code = provinces[tinh.SelectedIndex].code;
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();
                        string readString = "select * from districts WHERE province_code = N'" + code + "'";
                        SqlCommand command = new SqlCommand(readString, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        List<DonViHanhChinh> temp = new List<DonViHanhChinh>();

                        while (reader.Read())
                        {
                            temp.Add(new DonViHanhChinh((String)reader["code"], (String)reader["full_name"]));
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            districts.Clear();
                            districts.AddRange(temp.OrderBy(e => e.name).ToList());
                            huyen.ItemsSource = districts.Select(e => e.name).ToList();
                            phuong.ItemsSource = new List<String>();
                            if (tinh.SelectedIndex != -1)
                            {
                                huyen.Text = huyenText;
                            }
                        }));
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));

                thread.IsBackground = true;
                thread.Start();
            }
        }

        void loadPhuong()
        {
            if (huyen.SelectedIndex != -1)
            {
                String code = districts[huyen.SelectedIndex].code;
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();
                        string readString = "select * from wards WHERE district_code = N'" + code + "'";
                        SqlCommand command = new SqlCommand(readString, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        List<DonViHanhChinh> temp = new List<DonViHanhChinh>();

                        while (reader.Read())
                        {
                            temp.Add(new DonViHanhChinh((String)reader["code"], (String)reader["full_name"]));
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            wards.Clear();
                            wards.AddRange(temp.OrderBy(e => e.name).ToList());
                            phuong.ItemsSource = wards.Select(e => e.name).ToList();
                        }));
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));

                thread.IsBackground = true;
                thread.Start();
            }
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
                catch (Exception e)
                {
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
            if (cccd.Text.Length == 0 || !double.TryParse(cccd.Text, out distance) || cccd.Text.Length != 9 && cccd.Text.Length != 12)
            {
                MessageBox.Show("CCCD không hợp lệ");
                return false;
            }
            if (sdt.Text.Length == 0 || !Regex.IsMatch(sdt.Text, "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$") || sdt.Text.Length != 10 && sdt.Text.Length != 11)
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }
            if (!Regex.IsMatch(email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                MessageBox.Show("Email không hợp lệ");
                return false;
            }
            if (!double.TryParse(Regex.Replace(luong.Text, "[^0-9]", ""), out distance))
            {
                MessageBox.Show("Lương không hợp lệ");
                return false;
            }
            if (DateTime.Now.AddYears(-18) < ngaySinh.SelectedDate)
            {
                MessageBox.Show("Ngày sinh chưa đủ 18 tuổi");
                return false;
            }
            if (tinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Tỉnh/Thành Phố");
                return false;
            }
            if (huyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Quận/Huyện");
                return false;
            }
            if (phuong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Xã/Phường");
                return false;
            }
            if (diaChi.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập vào địa chỉ");
                return false;
            }
            if (name.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập vào tên");
                return false;
            }
            foreach (NhanVien nhanVien in nhanVienList)
            {
                if (nhanVien.maNhanVien != maNv.Text && (nhanVien.sdt == sdt.Text || nhanVien.cccd == cccd.Text || nhanVien.email == email.Text))
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
            if (update.IsEnabled == false && delete.IsEnabled == false)
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
                        command.Parameters["@MaNhanVien"].Value = "NV" + count.ToString("000");

                        command.Parameters.Add("@MatKhau", SqlDbType.VarChar);
                        command.Parameters["@MatKhau"].Value = "12345678";

                        command.Parameters.Add("@HoTen", SqlDbType.NVarChar);
                        command.Parameters["@HoTen"].Value = name.Text;

                        command.Parameters.Add("@MaChucVu", SqlDbType.VarChar);
                        command.Parameters["@MaChucVu"].Value = chucVu.SelectedIndex == 0 ? "ADMIN" :
                            (chucVu.SelectedIndex == 1 ? "NVBH" : (chucVu.SelectedIndex == 2 ? "NVK" : "NVKT"));

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
                        command.Parameters["@DiaChi"].Value = diaChi.Text + ", " + phuong.Text + ", " + huyen.Text + ", " + tinh.Text;

                        command.Parameters.Add("@Luong", SqlDbType.Money);
                        command.Parameters["@Luong"].Value = Regex.Replace(luong.Text, "[^0-9]", "");

                        command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                        command.Parameters["@TrangThai"].Value = "1";

                        command.ExecuteNonQuery();

                        connection.Close();
                        loadListStaff(true);
                        MessageBox.Show("Thêm thành công");
                        resetData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                resetData();
                update.IsEnabled = false;
                delete.IsEnabled = false;
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
                    command.Parameters["@MaChucVu"].Value = chucVu.SelectedIndex == 0 ? "ADMIN" :
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
                    command.Parameters["@DiaChi"].Value = diaChi.Text + ", " + phuong.Text + ", " + huyen.Text + ", " + tinh.Text;

                    command.Parameters.Add("@Luong", SqlDbType.Money);
                    command.Parameters["@Luong"].Value = Regex.Replace(luong.Text, "[^0-9]", "");

                    command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();

                    connection.Close();
                    loadListStaff(false);
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
                        loadListStaff(false);
                        resetData();
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
            List<String> donVi = new List<String>();
            donVi.AddRange(nhanVien.diaChi.Split(','));
            String diaChiText = "";
            for (int i = 0; i < donVi.Count - 3; i++)
            {
                diaChiText += donVi[i].Trim();
                if (i < donVi.Count - 4) { diaChiText += ", "; }
            }
            diaChi.Text = diaChiText;
            phuong.Text = donVi[donVi.Count - 3].Trim();
            huyenText = donVi[donVi.Count - 2].Trim();
            tinh.Text = donVi[donVi.Count - 1].Trim();
            gioiTinh.SelectedIndex = nhanVien.gioiTinh == "Nam" ? 0 : 1;
            luong.Text = nhanVien.luong.ToString();
            chucVu.SelectedIndex = nhanVien.maChucVu == "Nhân viên bán hàng" ? 1 : (nhanVien.maChucVu == "Nhân viên kho" ? 2 : 0);
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

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (name.Text.Length == 0)
            {
                name_error.Text = "Vui lòng nhập tên";
                name_error.Visibility = Visibility.Visible;
            }
            else
            {
                name_error.Visibility = Visibility.Hidden;
            }

            name.TextChanged -= name_TextChanged;
            name.Text = formatText(name.Text);
            name.TextChanged += name_TextChanged;
            name.Select(name.Text.Length, 0);
        }

        private void cccd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cccd.Text.Length != 9 && cccd.Text.Length != 12)
            {
                cccd_error.Text = cccd.Text.Length == 0 ? "Vui lòng nhập vào CCCD" : "CCCD không hợp lệ";
                cccd_error.Visibility = Visibility.Visible;
            }
            else
            {
                cccd_error.Visibility = Visibility.Hidden;
            }
        }

        private void sdt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(sdt.Text, "^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$") || sdt.Text.Length != 10 && sdt.Text.Length != 11)
            {
                sdt_error.Text = sdt.Text.Length == 0 ? "Vui lòng nhập vào số điện thoại" : "Số điện thoại không hợp lệ";
                sdt_error.Visibility = Visibility.Visible;
            }
            else
            {
                sdt_error.Visibility = Visibility.Hidden;
            }
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                email_error.Text = email.Text.Length == 0 ? "Vui lòng nhập vào địa chỉ Email" : "Email không hợp lệ";
                email_error.Visibility = Visibility.Visible;
            }
            else
            {
                email_error.Visibility = Visibility.Hidden;
            }
        }

        private void luong_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = Regex.Replace(luong.Text, "[^0-9]", "");
            double ul;
            if (double.TryParse(value, out ul))
            {
                if (luong.Text.Length == 0 || ul == 0)
                {
                    luong_error.Text = "Số lượng không hợp lệ";
                    luong_error.Visibility = Visibility.Visible;
                }
                else
                {
                    luong_error.Visibility = Visibility.Hidden;
                }

                luong.TextChanged -= luong_TextChanged;
                luong.Text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", ul);
                luong.TextChanged += luong_TextChanged;
                luong.Select(luong.Text.Length, 0);
            }
        }

        private void diaChi_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (diaChi.Text.Length == 0)
            {
                diaChi_error.Text = "Vui lòng nhập địa chỉ";
                diaChi_error.Visibility = Visibility.Visible;
            }
            else
            {
                diaChi_error.Visibility = Visibility.Hidden;
            }
            diaChi.TextChanged -= diaChi_TextChanged;
            diaChi.Text = formatText(diaChi.Text);
            diaChi.TextChanged += diaChi_TextChanged;
            diaChi.Select(diaChi.Text.Length, 0);
        }

        String formatText(String text)
        {
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
            return text;
        }

        private void tinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadHuyen();
        }

        private void huyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadPhuong();
        }
    }
}
