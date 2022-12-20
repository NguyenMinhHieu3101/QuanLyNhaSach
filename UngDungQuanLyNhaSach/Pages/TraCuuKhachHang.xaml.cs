using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachKhachHang.xaml
    /// </summary>
    public partial class TraCuuKhachHang : Excel.Page
    {
        List<KhachHang> selectedKhachHang = new List<KhachHang>();
        List<KhachHang> khachHangList = new List<KhachHang>();

        public TraCuuKhachHang()
        {
            InitializeComponent();
            loadData();
            chooseKhachHangTable.ItemsSource = new List<KhachHang>();
            loadFilter();
        }

        void loadData()
        {
            String maKHText = maKH.Text;
            String loaiKHText = loaiKH.Text;
            String tenKHText = name.Text;
            //DateTime? dateTime = ngaySinh.SelectedDate;
            String ngaySinhText = ngaySinh.Text;
            String sdtText = sdt.Text;
            int index = trangThai.SelectedIndex;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG, LOAIKHACHHANG WHERE KHACHHANG.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    if (maKHText.Length > 0) readString += " AND MaKhachHang Like '%" + maKHText + "%'";
                    if (loaiKHText.Length > 0) readString += " AND TenLoaiKhachHang = N'" + loaiKHText + "'";
                    if (tenKHText.Length > 0) readString += " AND TenKhachHang Like N'%" + tenKHText + "%'";
                    //if (dateTime != null) readString += " AND NgaySinh = '" + ((dateTime??DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (ngaySinhText.Length > 0) readString += " AND NgaySinh = '" + ngaySinhText + "'";
                    if (sdtText.Length > 0) readString += " AND SDT Like '%" + sdtText + "%'";
                    if (index != -1) readString += " AND TrangThai = '" + index + "'";

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
                    this.Dispatcher.BeginInvoke(new System.Action(() => {
                        resultKhachHangTable.ItemsSource = khachHangList;
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

        void loadFilter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from KHACHHANG, LOAIKHACHHANG WHERE KHACHHANG.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    List<String> itemsMaKH =  new List<String>();
                    List<String> itemsName =  new List<String>();
                    List<String> itemsSdt =  new List<String>();
                    List<DateTime> itemsNgaySinh =  new List<DateTime>();

                    while (reader.Read())
                    {
                        itemsMaKH.Add((String)reader["MaKhachHang"]);
                        itemsName.Add((String)reader["TenKhachHang"]);
                        itemsSdt.Add((String)reader["SDT"]);
                        itemsNgaySinh.Add((DateTime)reader["NgaySinh"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() => {
                        maKH.ItemsSource = itemsMaKH;
                        name.ItemsSource = itemsName.Distinct().ToList();
                        sdt.ItemsSource = itemsSdt.Distinct().OrderBy(e => e).ToList();
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

        private void resultKhachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.IsReadOnly = true;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKH.SelectedIndex = -1;
            loaiKH.SelectedIndex = -1;
            name.SelectedIndex = -1;
            sdt.SelectedIndex = -1;
            trangThai.SelectedIndex = -1;
            ngaySinh.SelectedIndex= -1;
            selectedKhachHang = new List<KhachHang>();
            chooseKhachHangTable.ItemsSource = new List<KhachHang>();            
            loadData();
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            selectedKhachHang = new List<KhachHang>();
            chooseKhachHangTable.ItemsSource = new List<KhachHang>();
            loadData();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (resultKhachHangTable.SelectedIndex != -1)
            {
                selectedKhachHang.RemoveAll(element => element.maKhachHang.CompareTo(khachHangList[resultKhachHangTable.SelectedIndex].maKhachHang) == 0);
                selectedKhachHang.Add(khachHangList[resultKhachHangTable.SelectedIndex]);
                List<KhachHang> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhachHang).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhachHangTable.ItemsSource = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (resultKhachHangTable.SelectedIndex != -1)
            {
                selectedKhachHang.RemoveAll(element => element.maKhachHang.CompareTo(khachHangList[resultKhachHangTable.SelectedIndex].maKhachHang) == 0);
                List<KhachHang> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhachHang).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhachHangTable.ItemsSource = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource = showSelectedKhachHang;
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

            for (int j = 0; j < chooseKhachHangTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = chooseKhachHangTable.Columns[j].Header;
            }
            for (int i = 0; i < chooseKhachHangTable.Columns.Count; i++)
            {
                for (int j = 0; j < chooseKhachHangTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)chooseKhachHangTable.Columns[i].GetCellContent(chooseKhachHangTable.Items[j]);
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
                foreach (KhachHang khachHang in selectedKhachHang)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();
                        string deleteString = "UPDATE KHACHHANG SET TrangThai = '0' Where MaKhachHang = @MaKhachHang";
                        SqlCommand command = new SqlCommand(deleteString, connection);

                        command.Parameters.Add("@MaKhachHang", SqlDbType.VarChar);
                        command.Parameters["@MaKhachHang"].Value = khachHang.maKhachHang;
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
                selectedKhachHang = new List<KhachHang>();
                chooseKhachHangTable.ItemsSource= selectedKhachHang;
                loadData();
                MessageBox.Show("Xóa khách hàng thành công");
            }
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            selectedKhachHang = new List<KhachHang>();
            selectedKhachHang.AddRange(khachHangList);
            chooseKhachHangTable.ItemsSource = selectedKhachHang;
        }

        private void selectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedKhachHang = new List<KhachHang>();
            chooseKhachHangTable.ItemsSource = selectedKhachHang;
        }

        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();        
    }
}
