using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachNhanVien.xaml
    /// </summary>
    public partial class TraCuuNhanVien : Excel.Page
    {
        List<NhanVien> selectedNhanVien = new List<NhanVien>();
        List<NhanVien> nhanVienList = new List<NhanVien>();

        public TraCuuNhanVien()
        {
            InitializeComponent();
            loadListStaff();
            loadFilter();
            chooseNhanVienTable.ItemsSource = new List<NhanVien>();
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
            //DateTime? dateTime = ngaySinh.SelectedDate;
            String ngaySinhText = ngaySinh.Text;
            String emailText = email.Text;
            String chucVuText = chucVu.Text;
            String luongText = luong.Text;
            int index = trangThai.SelectedIndex;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                nhanVienList = new List<NhanVien>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from NHANVIEN, CHUCVU WHERE NHANVIEN.MaChucVu = CHUCVU.MaChucVu";
                    if (maNVText.Length > 0) readString += " AND MaNhanVien Like '%" + maNVText + "%'";
                    if (tenNV.Length > 0) readString += " AND HoTen Like N'%" + tenNV + "%'";
                    //if (dateTime != null) readString += " AND NgaySinh = '" + ((dateTime ?? DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (ngaySinhText.Length > 0) readString += " AND NgaySinh = '" + ngaySinhText + "'";
                    if (cccdText.Length > 0) readString += " AND CCCD Like '%" + cccdText + "%'";
                    if (emailText.Length > 0) readString += " AND Email Like '%" + emailText + "%'";
                    if (luongText.Length > 0) readString += " AND Luong = " + Regex.Replace(luongText, "[^0-9]", "");
                    if (index != -1) readString += " AND TrangThai = " + index;
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
                            diaChi: (String)reader["DiaChi"], luong: string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(reader["luong"].ToString())),
                            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Đã nghỉ việc" : "Còn hoạt động"));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
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

        void loadFilter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from NHANVIEN, CHUCVU WHERE NHANVIEN.MaChucVu = CHUCVU.MaChucVu";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<String> itemsMaNV = new List<String>();
                    List<String> itemsName = new List<String>();
                    List<String> itemsCCCD = new List<String>();
                    List<String> itemsEmail = new List<String>();
                    List<double> itemsLuong = new List<double>();
                    List<DateTime> itemsNgaySinh = new List<DateTime>();

                    while (reader.Read())
                    {
                        itemsMaNV.Add((String)reader["MaNhanVien"]);
                        itemsName.Add((String)reader["HoTen"]);
                        itemsCCCD.Add((String)reader["CCCD"]);
                        itemsEmail.Add((String)reader["Email"]);
                        itemsNgaySinh.Add((DateTime)reader["NgaySinh"]);
                        itemsLuong.Add(double.Parse(reader["luong"].ToString()));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maNV.ItemsSource = itemsMaNV;
                        name.ItemsSource = itemsName.Distinct().OrderBy(e => e).ToList();
                        cccd.ItemsSource = itemsCCCD.Distinct().OrderBy(e => e).ToList();
                        email.ItemsSource = itemsEmail.Distinct().OrderBy(e => e).ToList();
                        luong.ItemsSource = itemsLuong.Distinct().OrderBy(e => e).Select(luong => string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", luong)).ToList();
                        ngaySinh.ItemsSource = itemsNgaySinh.Distinct().OrderBy(e => e).Select(date => date.ToString("MM/dd/yyyy")).ToList();
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

        private void search_Click(object sender, RoutedEventArgs e)
        {
            loadListStaff();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maNV.SelectedIndex = -1;
            luong.SelectedIndex = -1;
            email.SelectedIndex = -1;
            trangThai.SelectedIndex = -1;
            chucVu.SelectedIndex = -1;
            ngaySinh.SelectedIndex = -1;
            cccd.SelectedIndex = -1;
            name.SelectedIndex = -1;
            chooseNhanVienTable.ItemsSource = new List<NhanVien>();
            loadListStaff();
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
                selectedNhanVien.Add(nhanVienList[resultNhanVienTable.SelectedIndex]);
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
                selectedNhanVien.Remove(nhanVienList[resultNhanVienTable.SelectedIndex]);
                List<NhanVien> showSelectedKhachHang = selectedNhanVien.OrderBy(e => e.maNhanVien).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseNhanVienTable.ItemsSource = new List<KhachHang>();
                chooseNhanVienTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true; 
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < chooseNhanVienTable.Columns.Count; j++) 
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; 
                sheet1.Columns[j + 1].ColumnWidth = 15; 
                myRange.Value2 = chooseNhanVienTable.Columns[j].Header;
            }
            for (int i = 0; i < chooseNhanVienTable.Columns.Count; i++)
            { 
                for (int j = 0; j < chooseNhanVienTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)chooseNhanVienTable.Columns[i].GetCellContent(chooseNhanVienTable.Items[j]);
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                foreach (NhanVien nhanVien in selectedNhanVien)
                {

                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();

                        string deleteString = "UPDATE NHANVIEN SET TrangThai = '0' Where MaNhanVien = @MaNhanVien";
                        SqlCommand command = new SqlCommand(deleteString, connection);
                        command.Parameters.Add("@MaNhanVien", SqlDbType.VarChar);
                        command.Parameters["@MaNhanVien"].Value = nhanVien.maNhanVien;

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
                selectedNhanVien = new List<NhanVien>();
                chooseNhanVienTable.ItemsSource = selectedNhanVien;
                loadListStaff();
                MessageBox.Show("Xóa nhân viên thành công");
            }
        }

        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();
    }
}
