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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachNhanVien.xaml
    /// </summary>
    public partial class TraCuuNhanVien : Page
    {
        List<NhanVien> selectedNhanVien = new List<NhanVien>();
        List<NhanVien> khuyenMaiList = new List<NhanVien>();

        public TraCuuNhanVien()
        {
            InitializeComponent();
            //ngaySinh.SelectedDate = DateTime.Now;
            loadListStaff();
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

        void loadListStaff()
        {
            String maNVText = maNV.Text;
            String tenNV = name.Text;
            String cccdText = cccd.Text;
            DateTime? dateTime = ngaySinh.SelectedDate;
            String emailText = email.Text;
            String chucVuText = chucVu.Text;
            String luongText = luong.Text;
            int index = trangThai.SelectedIndex;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<NhanVien> nhanVienList = new List<NhanVien>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from NHANVIEN, CHUCVU WHERE NHANVIEN.MaChucVu = CHUCVU.MaChucVu";
                    if (maNVText.Length > 0) readString += " AND MaNhanVien Like '%" + maNVText + "%'";
                    if (tenNV.Length > 0) readString += " AND HoTen Like N'%" + tenNV + "%'";
                    if (dateTime != null) readString += " AND NgaySinh = '" + ((dateTime ?? DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (cccdText.Length > 0) readString += " AND CCCD Like '%" + cccdText + "%'";
                    if (emailText.Length > 0) readString += " AND Email Like '%" + emailText + "%'";
                    if (luongText.Length > 0) readString += " AND Luong = " + luongText;
                    if (index != 2) readString += " AND TrangThai = " + index;
                    if (chucVuText.Length > 0) readString += " AND TenChucVu = N'" + chucVuText + "'";

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
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        resultNhanVienTable.ItemsSource = nhanVienList;
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

        private void search_Click(object sender, RoutedEventArgs e)
        {
            loadListStaff();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maNV.Text = "";
            luong.Text = "";
            email.Text = "";
            trangThai.SelectedIndex = 2;
            chucVu.SelectedIndex = 3;
            ngaySinh.SelectedDate = DateTime.Now;
            cccd.Text = "";
            name.Text = "";
        }
        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (resultNhanVienTable.SelectedIndex != -1)
            {
                selectedNhanVien.Add(khuyenMaiList[resultNhanVienTable.SelectedIndex]);
                List<NhanVien> showSelectedKhachHang = selectedNhanVien.OrderBy(e => e.maNhanVien).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseNhanVienTable.ItemsSource = new List<KhachHang>();
                chooseNhanVienTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (resultNhanVienTable.SelectedIndex != -1)
            {
                selectedNhanVien.Remove(khuyenMaiList[resultNhanVienTable.SelectedIndex]);
                List<NhanVien> showSelectedKhachHang = selectedNhanVien.OrderBy(e => e.maNhanVien).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseNhanVienTable.ItemsSource = new List<KhachHang>();
                chooseNhanVienTable.ItemsSource = showSelectedKhachHang;
            }
        }
    }
}
