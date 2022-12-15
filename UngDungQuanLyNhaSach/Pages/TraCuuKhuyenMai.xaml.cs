using System;
using System.Collections;
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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachKhuyenMai.xaml
    /// </summary>
    public partial class TraCuuKhuyenMai : Excel.Page
    {
        List<KhuyenMai> selectedKhuyenMai = new List<KhuyenMai>();
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public TraCuuKhuyenMai()
        {
            InitializeComponent();
            loadData();
            chooseKhuyenMaiTable.ItemsSource = new List<KhuyenMai>();
            loadFilter();
        }

        private void resultKhuyenMaiTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        void loadData()
        {
            String maKMText = maKM.Text;
            String soLuongText = soLuong.Text;
            String phanTramText = phanTram.Text;
            String loaiKH = loaiKhachHang.Text;
            int index = trangThai.SelectedIndex;
            String start = ngayBatDau.Text;
            String finish = ngayKetThuc.Text;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                khuyenMaiList = new List<KhuyenMai>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    if (maKMText.Length > 0) readString += " AND MaKhuyenMai Like '%" + maKMText + "%'";
                    if (soLuongText.Length > 0) readString += " AND SoLuongKhuyenMai = " + soLuongText;
                    if (phanTramText.Length > 0) readString += " AND PhanTram = " + phanTramText;
                    if (loaiKH.Length > 0) readString += " AND TenLoaiKhachHang = N'" + loaiKH + "'";
                    if (index != -1) readString += " AND TrangThai = '" + index + "'";
                    if (start.Length > 0) readString += " AND ThoiGianBatDau = '" + start + "'";
                    if (finish.Length > 0) readString += " AND ThoiGianKetThuc = '" + finish + "'";

                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khuyenMaiList.Add(new KhuyenMai(stt: count, maKhuyenMai: (String)reader["MaKhuyenMai"],
                            batDau: (DateTime)reader["ThoiGianBatDau"], //DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            ketThuc: (DateTime)reader["ThoiGianKetThuc"], //DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            maLoaiKhachHang: (String)reader["TenLoaiKhachHang"],
                            phanTram: (int)reader["PhanTram"],
                            soLuong: (int)reader["SoLuongKhuyenMai"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực"));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        resultKhuyenMaiTable.ItemsSource = khuyenMaiList;
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
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<String> itemsMaKM = new List<String>();
                    List<int> itemsSoluong = new List<int>();
                    List<int> itemsPhanTram = new List<int>();
                    List<DateTime> itemsStart = new List<DateTime>();
                    List<DateTime> itemsFinish = new List<DateTime>();

                    while (reader.Read())
                    {
                        itemsMaKM.Add((String)reader["MaKhuyenMai"]);
                        itemsSoluong.Add((int)reader["SoLuongKhuyenMai"]);
                        itemsPhanTram.Add((int)reader["PhanTram"]);
                        itemsStart.Add((DateTime)reader["ThoiGianBatDau"]);
                        itemsFinish.Add((DateTime)reader["ThoiGianKetThuc"]);
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maKM.ItemsSource = itemsMaKM;
                        soLuong.ItemsSource = itemsSoluong.Distinct().OrderBy(e => e).ToList();
                        phanTram.ItemsSource = itemsPhanTram.Distinct().OrderBy(e => e).ToList();
                        ngayBatDau.ItemsSource = itemsStart.Distinct().OrderBy(e => e).Select(date => date.ToString("MM/dd/yyyy")).ToList();
                        ngayKetThuc.ItemsSource = itemsFinish.Distinct().OrderBy(e => e).Select(date => date.ToString("MM/dd/yyyy")).ToList();
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

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKM.SelectedIndex = -1;
            soLuong.SelectedIndex = -1;
            phanTram.SelectedIndex = -1;
            ngayKetThuc.SelectedIndex = -1;
            ngayBatDau.SelectedIndex = -1;
            loaiKhachHang.SelectedIndex = -1;
            trangThai.SelectedIndex = -1;
            chooseKhuyenMaiTable.ItemsSource = new List<KhuyenMai>();
            loadData();
        }

        private void resultKhuyenMaiTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Remove(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                selectedKhuyenMai.Add(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Remove(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Add(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                foreach (KhuyenMai khuyenMai in selectedKhuyenMai)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();

                        string deleteString = "UPDATE KHUYENMAI SET TrangThai = '0' Where MaKhuyenMai = @MaKhuyenMai";
                        SqlCommand command = new SqlCommand(deleteString, connection);
                        command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                        command.Parameters["@MaKhuyenMai"].Value = khuyenMai.maKhuyenMai;

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công");
                    }
                }
                selectedKhuyenMai = new List<KhuyenMai>();
                chooseKhuyenMaiTable.ItemsSource = selectedKhuyenMai;
                loadData();
                MessageBox.Show("Xóa khuyến mãi thành công");
            }
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < chooseKhuyenMaiTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = chooseKhuyenMaiTable.Columns[j].Header;
            }
            for (int i = 0; i < chooseKhuyenMaiTable.Columns.Count; i++)
            {
                for (int j = 0; j < chooseKhuyenMaiTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)chooseKhuyenMaiTable.Columns[i].GetCellContent(chooseKhuyenMaiTable.Items[j]);
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();
    }
}
