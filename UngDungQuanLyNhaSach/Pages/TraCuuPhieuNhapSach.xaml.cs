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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachPhieuNhapSach.xaml
    /// </summary>
    public partial class TraCuuPhieuNhapSach : Excel.Page

    {
        List<PhieuNhapSach> selectedPNS = new List<PhieuNhapSach>();
        List<PhieuNhapSach> phieuNhapSachList = new List<PhieuNhapSach>();
        public TraCuuPhieuNhapSach()
        {
            InitializeComponent();
            loadListPhieuNhap();
            choosePNSTable.ItemsSource = new List<PhieuNhapSach>();
            loadData();
        }
        private void danhSachPNSTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        void loadListPhieuNhap()
        {
            String maPhieuNhapText = maPNS.Text;
            String maNhanVienText = maNV.Text;
            String maKhoText = maKho.Text;
            String nhaCungCapText = nhaCungCap.Text;
            DateTime? dateTime = ngayNhap.SelectedDate;
            String tongTienText = tongTien.Text;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                phieuNhapSachList = new List<PhieuNhapSach>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from PHIEUNHAP WHERE MaPhieuNhap IS NOT NULL";
                    if (maPhieuNhapText.Length > 0) readString += " AND MaPhieuNhap Like '%" + maPhieuNhapText + "%'";
                    if (maNhanVienText.Length > 0) readString += " AND MaNhanVien Like '%" + maNhanVienText + "%'";
                    if (maKhoText.Length > 0) readString += " AND MaKho Like '%" + maKhoText + "%'";
                    if (nhaCungCapText.Length > 0) readString += " AND NhaCungCap Like N'%" + nhaCungCapText + "%'";
                    if (dateTime != null) readString += " AND NgayNhap = '" + ((dateTime ?? DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (tongTienText.Length > 0) readString += " AND TongTien = '" + tongTienText + "'";

                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        phieuNhapSachList.Add(new PhieuNhapSach(stt: count,
                            maPhieuNhap: (String)reader["MaPhieuNhap"],
                            maNhanVien: (String)reader["MaNhanVien"],
                            maKho: (String)reader["MaKho"],
                            nhaCungCap: (String)reader["NhaCungCap"],
                            ngayNhap: (DateTime)reader["NgayNhap"],
                            tongTien: (double)reader["TongTien"]
                            ));
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        danhSachPNSTable.ItemsSource = phieuNhapSachList;
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


        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from PHIEUNHAP";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    List<String> itemsmaPhieuNhap = new List<String>();
                    List<String> itemsmaNhanVien = new List<String>();
                    List<String> itemsmaKho = new List<String>();
                    List<String> itemsnhaCungCap = new List<String>();
                    List<DateTime> itemsngayNhap = new List<DateTime>();
                    List<double> itemstongTien = new List<double>();

                    while (reader.Read())
                    {
                        itemsmaPhieuNhap.Add((String)reader["MaPhieuNhap"]);
                        itemsmaNhanVien.Add((String)reader["MaNhanVien"]);
                        itemsmaKho.Add((String)reader["MaKho"]);
                        itemsnhaCungCap.Add((String)reader["NhaCungCap"]);
                        ////itemsngayNhap.Add(DateTime.Parse(reader["NgayNhap"].ToString()));
                        //itemstongTien.Add(double.Parse(reader["TongTien"].ToString()));

                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maPNS.ItemsSource = itemsmaPhieuNhap;
                        maNV.ItemsSource = itemsmaNhanVien.Distinct().OrderBy(e => e).ToList();
                        maKho.ItemsSource = itemsmaKho;
                        nhaCungCap.ItemsSource = itemsnhaCungCap;
                        ////ngayNhap.ItemsSource = itemsngayNhap;
                        //itemstongTien.ItemsSource = itemstongTien.Distinct().OrderBy(e => e).ToList();


                    }));
                    connection.Close();
                }
                catch (Exception e1)
                {
                    //MessageBox.Show("db error");
                    MessageBox.Show(e1.Message);

                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maPNS.Text = "";
            maNV.Text = "";
            maKho.Text = "";
            nhaCungCap.Text = "";
            ngayNhap.SelectedDate = null;
            tongTien.Text = "";
            selectedPNS = new List<PhieuNhapSach>();
            choosePNSTable.ItemsSource = new List<PhieuNhapSach>();
            loadListPhieuNhap();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (danhSachPNSTable.SelectedIndex != -1)
            {
                selectedPNS.RemoveAll(element => element.maPhieuNhap.CompareTo(phieuNhapSachList[danhSachPNSTable.SelectedIndex].maPhieuNhap) == 0);
                selectedPNS.Add(phieuNhapSachList[danhSachPNSTable.SelectedIndex]);
                List<PhieuNhapSach> showSelectedPNS = selectedPNS.OrderBy(e => e.maPhieuNhap).ToList();
                for (int i = 0; i < showSelectedPNS.Count; i++)
                {
                    showSelectedPNS[i].stt = i + 1;
                }
                choosePNSTable.ItemsSource = new List<PhieuNhapSach>();
                choosePNSTable.ItemsSource = showSelectedPNS;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (danhSachPNSTable.SelectedIndex != -1)
            {
                selectedPNS.Remove(phieuNhapSachList[danhSachPNSTable.SelectedIndex]);
                List<PhieuNhapSach> showSelectedPNS = selectedPNS.OrderBy(e => e.maPhieuNhap).ToList();
                for (int i = 0; i < showSelectedPNS.Count; i++)
                {
                    showSelectedPNS[i].stt = i + 1;
                }
                choosePNSTable.ItemsSource = new List<KhachHang>();
                choosePNSTable.ItemsSource = showSelectedPNS;
            }
        }


        private void chiTietPNSTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();

        private void search_Click(object sender, RoutedEventArgs e)
        {
            selectedPNS = new List<PhieuNhapSach>();
            choosePNSTable.ItemsSource = new List<PhieuNhapSach>();
            loadListPhieuNhap();
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

            for (int j = 0; j < choosePNSTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = choosePNSTable.Columns[j].Header;
            }
            for (int i = 0; i < choosePNSTable.Columns.Count; i++)
            {
                for (int j = 0; j < choosePNSTable.Items.Count; j++)
                {
                    TextBlock b = (TextBlock)choosePNSTable.Columns[i].GetCellContent(choosePNSTable.Items[j]);
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
                foreach (PhieuNhapSach phieuNhap in selectedPNS)
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                        connection.Open();
                        string deleteString = "DELETE FROM PHIEUNHAP Where MaPhieuNhap = @MaPhieuNhap";
                        SqlCommand command = new SqlCommand(deleteString, connection);

                        command.Parameters.Add("@MaPhieuNhap", SqlDbType.VarChar);
                        command.Parameters["@MaPhieuNhap"].Value = phieuNhap.maPhieuNhap;
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch (Exception exception)
                    {
                        //MessageBox.Show("Xóa không thành công");
                        MessageBox.Show(exception.Message);
                    }
                }
                selectedPNS = new List<PhieuNhapSach>();
                choosePNSTable.ItemsSource = selectedPNS;
                loadData();
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            selectedPNS = new List<PhieuNhapSach>();
            selectedPNS.AddRange(phieuNhapSachList);
            choosePNSTable.ItemsSource = selectedPNS;
        }

        private void selectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedPNS = new List<PhieuNhapSach>();
            choosePNSTable.ItemsSource = selectedPNS;
        }
    }
}