
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachPhieuNhapSach.xaml
    /// </summary>
    public partial class TraCuuHoaDon : Excel.Page
    {
        List<HoaDon> hoaDonList = new List<HoaDon>();
        List<HoaDon> selectedHoaDon = new List<HoaDon>();
        public TraCuuHoaDon()
        {
            InitializeComponent();
            loadListHD();
            loadFilter();
            selectedHDTable.ItemsSource = new List<HoaDon>();
            ngayLapHD.SelectedDate = DateTime.Now;
        }
       
        void loadListHD()
        {
            String maHDText = maHD.Text;
            String maKHText = maKH.Text;
            String ngayLapHDText = ngayLapHD.Text;
            String nguoiLapHDText = nguoiLapHD.Text;
            String tongTienText = tongTien.Text;

            //Thread thread = new Thread(new ThreadStart(() =>
            //{
                hoaDonList = new List<HoaDon>();
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();

                    string readString = "SELECT * FROM HOADON, CHITIETHOADON WHERE HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon";

                    if (maHDText.Length > 0) readString += " AND MaHoaDon Like '%" + maHDText + "%'";
                    if (maKHText.Length > 0) readString += " AND MaKhachHang Like '%" + maKHText + "%'";
                    if (ngayLapHDText.Length > 0) readString += " AND NgayLapHoaDon = '" + ngayLapHDText + "'";
                    if (nguoiLapHDText.Length > 0) readString += " AND MaNhanVien Like '%" + nguoiLapHDText + "%'";
                    if (tongTienText.Length > 0) readString += " AND TongTienHoaDon Like '%" + tongTienText + "%'";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        hoaDonList.Add(new HoaDon(stt: count,
                            maHoaDon: (String)reader["MaHoaDon"],
                            maKhachHang: (String)reader["MaKhachHang"],
                            ngayLapHD: (DateTime)reader["NgayLapHoaDon"],
                            maNhanVien: (String)reader["MaNhanVien"],
                            maKhuyenMai: (String)reader["MaKhuyenMai"],
                            tongTienHD: (decimal)reader["TongTienHoaDon"]
                            ));
                        danhSachHDTable.ItemsSource = hoaDonList;
                    }
                    //this.Dispatcher.BeginInvoke(new System.Action(() =>
                    //{
                        danhSachHDTable.ItemsSource = hoaDonList;
                    //}));
                    connection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            //}));

            //thread.IsBackground = true;
            //thread.Start();

        }
        void loadFilter()
        {
            //Thread thread = new Thread(new ThreadStart(() =>
            //{
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();

                string readString = "SELECT * FROM HOADON, CHITIETHOADON WHERE HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon";
                SqlCommand command = new SqlCommand(readString, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<String> itemsMaHD = new List<String>();
                List<String> itemsMaKH = new List<String>();
                List<String> itemsMaNV = new List<String>();
                List<DateTime> itemsNgayLapHD = new List<DateTime>();
                List<Decimal> itemsTongTien = new List<Decimal>();



                while (reader.Read())
                {
                    itemsMaHD.Add((String)reader["MaHoaDon"]);
                    itemsMaKH.Add((String)reader["MaKhachHang"]);
                    itemsMaNV.Add((String)reader["MaNhanVien"]);
                    itemsNgayLapHD.Add((DateTime)reader["NgayLapHoaDon"]);
                    itemsTongTien.Add((Decimal)reader["TongTienHoaDon"]);
                }
                //this.Dispatcher.BeginInvoke(new System.Action(() =>
                //{
                maHD.ItemsSource = itemsMaHD;
                maKH.ItemsSource = itemsMaKH;
                nguoiLapHD.ItemsSource = itemsMaNV;
                //}));
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //}));

            //thread.IsBackground = true;
            //thread.Start();
        }
        private void danhSachHDTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void selectedHDTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            selectedHDTable.ItemsSource = new List<HoaDon>();
            selectedHoaDon = new List<HoaDon>();
            loadListHD();
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            maHD.SelectedIndex = -1;
            maKH.SelectedIndex = -1;
            nguoiLapHD.SelectedIndex = -1;
            tongTien.Text = "";
            ngayLapHD.SelectedDate = DateTime.Now;
            selectedHDTable.ItemsSource = new List<HoaDon>();
            selectedHoaDon = new List<HoaDon>();
            
            loadListHD();

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (danhSachHDTable.SelectedIndex != -1)
            {
                selectedHoaDon.RemoveAll(element => element.maHoaDon.CompareTo(hoaDonList[danhSachHDTable.SelectedIndex].maHoaDon) == 0);
                selectedHoaDon.Add(hoaDonList[danhSachHDTable.SelectedIndex]);
                List<HoaDon> showSelectedHoaDon = selectedHoaDon.OrderBy(e => e.maHoaDon).ToList();
                for (int i = 0; i < showSelectedHoaDon.Count; i++)
                {
                    showSelectedHoaDon[i].stt = i + 1;
                }
                selectedHDTable.ItemsSource = new List<HoaDon>();
                selectedHDTable.ItemsSource = showSelectedHoaDon;
            }
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (danhSachHDTable.SelectedIndex != -1)
            {
                selectedHoaDon.RemoveAll(element => element.maHoaDon.CompareTo(hoaDonList[danhSachHDTable.SelectedIndex].maNhanVien) == 0);
                List<HoaDon> showSelectedHoaDon = selectedHoaDon.OrderBy(e => e.maHoaDon).ToList();
                for (int i = 0; i < showSelectedHoaDon.Count; i++)
                {
                    showSelectedHoaDon[i].stt = i + 1;
                }
                selectedHDTable.ItemsSource = new List<HoaDon>();
                selectedHDTable.ItemsSource = showSelectedHoaDon;
            }
        }
        private void selectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            danhSachHDTable.ItemsSource = hoaDonList;
            selectedHoaDon = new List<HoaDon>();
            selectedHoaDon.AddRange(hoaDonList);
            selectedHDTable.ItemsSource = selectedHoaDon;
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            //danhSachHDTable.ItemsSource = hoaDonList;
            selectedHoaDon = new List<HoaDon>();
            selectedHoaDon.AddRange(hoaDonList);
            selectedHDTable.ItemsSource = selectedHoaDon;
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            //var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
            //if (result == MessageBoxResult.OK)
            //{
            //    foreach (HoaDon hoaDon in selectedHoaDon)
            //    {

            //        try
            //        {
            //            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            //            connection.Open();

            //            string deleteString = "UPDATE HOADON SET ";
            //            SqlCommand command = new SqlCommand(deleteString, connection);
            //            command.Parameters.Add("@MaHoaDon", SqlDbType.VarChar);
            //            command.Parameters["@MaHoaDon"].Value = hoaDon.maHoaDon;

            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }
            //        catch
            //        {
            //            MessageBox.Show("Xóa không thành công");
            //        }
            //    }
            //    selectedHoaDon = new List<HoaDon>();
            //    selectedHDTable.ItemsSource = selectedHoaDon;
            //    loadListHD();
            //    MessageBox.Show("Xóa hóa đơn thành công");
            //}
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < selectedHDTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = selectedHDTable.Columns[j].Header;
            }
            for (int i = 0; i < selectedHDTable.Columns.Count; i++)
            {
                for (int j = 0; j < selectedHDTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)selectedHDTable.Columns[i].GetCellContent(selectedHDTable.Items[j]);
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
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

