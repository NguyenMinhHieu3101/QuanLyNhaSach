using Microsoft.VisualBasic;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using UngDungQuanLyNhaSach.Model;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Data.Common;
using System.Windows.Markup;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using DataTable = System.Data.DataTable;
using Path = System.IO.Path;
using System.IO.Packaging;
using Page = System.Windows.Controls.Page;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemPhieuNhapSach.xaml
    /// </summary>
    public partial class ThemPhieuNhapSach : Page
    {
        List<PhieuNhapSach> phieuNhapList = new List<PhieuNhapSach>();
        List<ChiTietPhieuNhapSach> chiTietPhieuNhapSachList = new List<ChiTietPhieuNhapSach>();
        List<SanPham> sanPhamTrongPhieuNhap = new List<SanPham>();
        List<SanPham> sanPhamList = new List<SanPham>();
        List<SanPham> topSPList = new List<SanPham>();
        public ThemPhieuNhapSach()
        {
            InitializeComponent();
            ngayNhap.SelectedDate = DateTime.Now;
            loadListPhieuNhap();
            loadFilter();
            ngayNhap.SelectedDate = DateTime.Today;
            ngayNhap.IsEnabled = false;
           // loadListChiTietPhieuNhap();
            updateBtn.IsEnabled = false;
            loadGoiYList();
        }
        void resetData()
        {
            maNhanVien.Text = "";
            maPhieuNhap.Text = "";
            maKho.Text = "";
            nhaCungCap.Text = "";
            tongTien.Text = "";
            ngayNhap.Text = "";

        }
        void loadListChiTietPhieuNhap()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                chiTietPhieuNhapSachList = new List<ChiTietPhieuNhapSach>();

              
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from CHITIETPHIEUNHAP";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        count++;

                        chiTietPhieuNhapSachList.Add(new ChiTietPhieuNhapSach(stt: count, maSanPham: (String)reader["MaSanPham"],
                           
                            soLuong: (int)reader["SoLuong"], donGia: (double)reader["DonGia"], thanhTien: (int)reader["SoLuong"] * (double)reader["DonGia"]));
                            
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        chitietphieuNhapSachTable.ItemsSource = chiTietPhieuNhapSachList;
                    }));
                    connection.Close();

            }));

            thread.IsBackground = true;
            thread.Start();

        }

        void loadListPhieuNhap()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();

                    string readString = "select Count(*) from PHIEUNHAP";
                    SqlCommand commandReader = new SqlCommand(readString, connection);
                    Int32 count = (Int32)commandReader.ExecuteScalar() + 1;
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        maPhieuNhap.Text = "PN" + count.ToString("000");
                    }));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }));
            thread.IsBackground = true;
            thread.Start();
           
            
            thread = new Thread(new ThreadStart(() =>
            {
                phieuNhapList = new List<PhieuNhapSach>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from PHIEUNHAP";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        phieuNhapList.Add(new PhieuNhapSach(stt: count, maPhieuNhap: (String)reader["MaPhieuNhap"],
                                    maNhanVien: (String)reader["MaNhanVien"], maKho: (String)reader["MaKho"],
                                    nhaCungCap: (String)reader["NhaCungCap"],
                                    ngayNhap: (DateTime)reader["NgayNhap"],
                                    tongTien: double.Parse(reader["TongTien"].ToString())));

                        this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            phieuNhapSachTable.ItemsSource = phieuNhapList;
                         
                        }));
                    }
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
        void loadFilter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
  

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from SANPHAM";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    List<String> itemsMaSP = new List<String>();
                    List<String> itemsTenSP = new List<String>();
                    List<String> itemsTheLoai = new List<String>();
                    List<String> itemsTacGia = new List<String>();
                    List<double> itemsDonGia = new List<double>();
                    List<String> itemsNXB = new List<String>();
                    List<int> itemsNamXB = new List<int>();


                    while (reader.Read())
                    {
                        itemsMaSP.Add((String)reader["MaSanPham"]);                      
                        itemsTenSP.Add((String)reader["TenSanPham"]);
                        itemsTheLoai.Add((String)reader["TheLoai"]);
                        itemsTacGia.Add((String)reader["TacGia"]);
                        itemsDonGia.Add((double)reader["GiaNhap"]);
                        itemsNXB.Add((String)reader["NXB"]);
                        itemsNamXB.Add((int)reader["NamXB"]);

                        this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {

                            maSach.ItemsSource = itemsMaSP;
                            tenSach.ItemsSource = itemsTenSP;
                            theLoai.ItemsSource = itemsTheLoai;
                            tacGia.ItemsSource = itemsTacGia;
                            donGia.ItemsSource = itemsDonGia;
                            nhaXB.ItemsSource = itemsNXB;
                            namXB.ItemsSource = itemsNamXB;
                        }));
                    }
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
        private void phieuNhapSachTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            
            SanPham sanpham = new SanPham(maSach.Text, tenSach.Text, theLoai.Text, tacGia.Text, nhaXB.Text, double.Parse(donGia.Text), int.Parse(namXB.Text),"K001","1", int.Parse(soLuongNhap.Text));
            try
            {


                sanPhamTrongPhieuNhap.Add(sanpham);
                if (soLuongNhap.Text.Length > 0)
                {
                    int soLuong = int.Parse(soLuongNhap.Text);
                    if (soLuong == 0)
                    {
                        MessageBox.Show("Số lượng không hợp lệ");
                        return;
                    }
                    string value = Regex.Replace(donGia.Text, "[^0-9]", "");
                    double donGia1;
                    if (double.TryParse(value, out donGia1))
                    {
                        bool check = true;
                        for (int i = 0; i < chiTietPhieuNhapSachList.Count; i++)
                        {
                            if (chiTietPhieuNhapSachList[i].getMaSanPham().CompareTo(maSach.Text) == 0)
                            {
                                chiTietPhieuNhapSachList[i].soLuong += soLuong;
                                chiTietPhieuNhapSachList[i].setThanhTien(chiTietPhieuNhapSachList[i].soLuong * chiTietPhieuNhapSachList[i].getDonGia());
                                check = false;
                                break;
                            }
                        }
                        if (check)
                        {
                            chiTietPhieuNhapSachList.Add(new ChiTietPhieuNhapSach(chiTietPhieuNhapSachList.Count + 1, maSach.Text, soLuong, donGia1, donGia1 * soLuong));
                        }
                        chitietphieuNhapSachTable.ItemsSource = new List<ChiTietPhieuNhapSach>();
                        chitietphieuNhapSachTable.ItemsSource = chiTietPhieuNhapSachList;
                        resetAddProduct();
                        double sum = 0;
                        foreach (ChiTietPhieuNhapSach chiTiet in chiTietPhieuNhapSachList)
                        {
                            sum += chiTiet.getThanhTien();
                        }
                        tongTien.Text = sum.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn sản phẩm");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập vào số lượng");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);

            }
        }
        void resetAddProduct()
        {
            maSach.Text = "";
            tenSach.Text = "";
            soLuongNhap.Text = "";
            tacGia.Text = "";
            theLoai.Text = "";
            nhaXB.Text = "";
            namXB.Text = "";
            donGia.Text = "";
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select Count(*) from PHIEUNHAP";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 countPN = (Int32)commandReader.ExecuteScalar() + 1;



                string insertString = "INSERT INTO PHIEUNHAP(MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien)"
                    + "VALUES(@MaPhieuNhap, @MaNhanVien, @MaKho, @NhaCungCap, @NgayNhap, @TongTien)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaPhieuNhap", SqlDbType.VarChar);
                command.Parameters["@MaPhieuNhap"].Value = "PN" + countPN.ToString("000");

                command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar);
                command.Parameters["@MaNhanVien"].Value = maNhanVien.Text;

                command.Parameters.Add("@MaKho", SqlDbType.NVarChar);
                command.Parameters["@MaKho"].Value = "K001";

                command.Parameters.Add("@NhaCungCap", SqlDbType.NVarChar);
                command.Parameters["@NhaCungCap"].Value = nhaCungCap.Text;

                command.Parameters.Add("@NgayNhap", SqlDbType.SmallDateTime);
                command.Parameters["@NgayNhap"].Value = ngayNhap.SelectedDate;

                command.Parameters.Add("@TongTien", SqlDbType.Money);
                command.Parameters["@TongTien"].Value = tongTien.Text;


                command.ExecuteNonQuery();

                connection.Close();


                foreach (SanPham sp in sanPhamTrongPhieuNhap)
                {

                    connection.Open();
                    readString = "select Count(*) from SANPHAM";
                    commandReader = new SqlCommand(readString, connection);
                    Int32 countSP = (Int32)commandReader.ExecuteScalar() + 1;

                    
                    insertString = "INSERT INTO SANPHAM(MaSanPham, TenSanPham, TacGia, TheLoai, NXB, GiaNhap, NamXB, MaKho, TrangThai)"
                        + "VALUES(@MaSanPham, @TenSanPham, @TacGia, @TheLoai, @NXB, @GiaNhap, @NamXB, @MaKho, @TrangThai)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                    command.Parameters["@MaSanPham"].Value = "SP" + countSP.ToString("000");

                    command.Parameters.Add("@TenSanPham", SqlDbType.NVarChar);
                    command.Parameters["@TenSanPham"].Value = sp.tenSanPham;

                    command.Parameters.Add("@TacGia", SqlDbType.NVarChar);
                    command.Parameters["@TacGia"].Value = sp.tacGia;

                    command.Parameters.Add("@TheLoai", SqlDbType.NVarChar);
                    command.Parameters["@TheLoai"].Value = sp.theLoai;

                    command.Parameters.Add("@NXB", SqlDbType.NVarChar);
                    command.Parameters["@NXB"].Value = sp.nXB;

                    command.Parameters.Add("@GiaNhap", SqlDbType.Money);
                    command.Parameters["@GiaNhap"].Value = sp.giaNhap;

                    command.Parameters.Add("@NamXB", SqlDbType.Int);
                    command.Parameters["@NamXB"].Value = sp.namXB;

                    command.Parameters.Add("@MaKho", SqlDbType.NVarChar);
                    command.Parameters["@MaKho"].Value = "K001";

                    command.Parameters.Add("@TrangThai", SqlDbType.NVarChar);
                    command.Parameters["@TrangThai"].Value = "1";

                    command.ExecuteNonQuery();




                    insertString = "INSERT INTO CHITIETPHIEUNHAP(MaPhieuNhap, MaSanPham, SoLuong,NgayNhap, DonGia)"
                        + "VALUES(@MaPhieuNhap, @MaSanPham, @SoLuong,@NgayNhap, @DonGia)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaPhieuNhap", SqlDbType.VarChar);
                    command.Parameters["@MaPhieuNhap"].Value = "PN" + countPN.ToString("000");

                    command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                    command.Parameters["@MaSanPham"].Value = "SP" + countSP.ToString("000");

                    command.Parameters.Add("@SoLuong", SqlDbType.Int);
                    command.Parameters["@SoLuong"].Value = sp.soLuong;

                    command.Parameters.Add("@NgayNhap", SqlDbType.SmallDateTime);
                    command.Parameters["@NgayNhap"].Value = DateTime.Today;

                    command.Parameters.Add("@DonGia", SqlDbType.Money);
                    command.Parameters["@DonGia"].Value = sp.giaNhap;

                    command.ExecuteNonQuery();


                    connection.Close();
                    countSP++;
                }



                loadListPhieuNhap();
                MessageBox.Show("Thêm thành công");
                resetData();
            }
            catch (Exception e1)
            {
                //MessageBox.Show("db error");
                MessageBox.Show(e1.Message);

            }

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string updateString = "Update PHIEUNHAP Set MaNhanVien = @MaNhanVien, MaKho = @MaKho, NhaCungCap = @NhaCungCap, NgayNhap = @NgayNhap, TongTien = @TongTien "
                    + "Where MaPhieuNhap = @MaPhieuNhap";
                SqlCommand command = new SqlCommand(updateString, connection);

                command.Parameters.Add("@MaPhieuNhap", SqlDbType.VarChar);
                command.Parameters["@MaPhieuNhap"].Value = phieuNhapList[phieuNhapSachTable.SelectedIndex].maPhieuNhap;

                command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar);
                command.Parameters["@MaNhanVien"].Value = maNhanVien.Text;

                command.Parameters.Add("@MaKho", SqlDbType.NVarChar);
                command.Parameters["@MaKho"].Value = maKho.Text;

                command.Parameters.Add("@NhaCungCap", SqlDbType.NVarChar);
                command.Parameters["@NhaCungCap"].Value = nhaCungCap.Text;

                command.Parameters.Add("@NgayNhap", SqlDbType.SmallDateTime);
                command.Parameters["@NgayNhap"].Value = ngayNhap.SelectedDate;

                command.Parameters.Add("@TongTien", SqlDbType.Money);
                command.Parameters["@TongTien"].Value = tongTien.Text;


                command.ExecuteNonQuery();
                connection.Close();
                loadListPhieuNhap();
                resetData();
                MessageBox.Show("Cập nhật thành công");
            }
            catch
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        private void phieuNhapSachTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (phieuNhapSachTable.SelectedIndex != -1)
            {
                updateBtn.IsEnabled = true;

                loadPhieuNhap();
            }
            else
            {
                updateBtn.IsEnabled = false;
    
            }

        }
        void loadPhieuNhap()
        {
            //try
            //{
            PhieuNhapSach phieuNhapSach = phieuNhapList[phieuNhapSachTable.SelectedIndex];
            maPhieuNhap.Text = phieuNhapSach.maPhieuNhap;
            maNhanVien.Text = phieuNhapSach.maNhanVien;
            //number.Text = sanPham.soLuong;
            maKho.Text = phieuNhapSach.maKho;
            nhaCungCap.Text = phieuNhapSach.nhaCungCap.ToString();
            ngayNhap.Text = phieuNhapSach.ngayNhap.ToString();
            tongTien.Text = phieuNhapSach.tongTien.ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void phieuNhapSachTable_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (phieuNhapSachTable.SelectedIndex != -1)
            {
                updateBtn.IsEnabled = true;

                loadPhieuNhap();
            }
            else
            {
                updateBtn.IsEnabled = false;
    
            }
        }


        private void chitietphieuNhapSachTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }


        void loadGoiYList()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                topSPList = new List<SanPham>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "SELECT TOP(5) SANPHAM.MaSanPham, TenSanPham, TacGia, TheLoai, NXB, NamXB, MaKho, TrangThai, GiaNhap FROM HOADON, CHITIETHOADON, SANPHAM WHERE (HOADON.MaHoaDon = CHITIETHOADON.MaHoaDon) AND (SANPHAM.MaSanPham = CHITIETHOADON.MaSanPham) AND (MONTH(NgayLapHoaDon) = @Thang) GROUP BY SANPHAM.MaSanPham, TenSanPham, TacGia, TheLoai, NXB, NamXB, MaKho, TrangThai, GiaNhap ORDER BY COUNT(SoLuong) DESC";
                    SqlCommand command = new SqlCommand(readString, connection);
                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    //command.Parameters["@Thang"].Value = DateTime.Now.Month - 1;

                    //Dùng tạm sách bán chạy tháng 1, đợi thêm dữ liệu trong SQL
                    command.Parameters["@Thang"].Value = 1;

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        topSPList.Add(new SanPham(stt: count, maSanPham: (String)reader["MaSanPham"],
                                    tenSanPham: (String)reader["TenSanPham"], theLoai: (String)reader["TheLoai"], 
                                    tacGia: (String)reader["TacGia"], nXB: (String)reader["NXB"], giaNhap: (double)reader["GiaNhap"],
                                    namXB: (int)reader["NamXB"], maKho: (String)reader["MaKho"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hàng" : "Còn hàng"));

                        this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            GoiYSachTable.ItemsSource = topSPList;
                        }));
                    }
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
            

        private void importBtn_Click(object sender, RoutedEventArgs e)
        {



            string path = "";
            OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            if (openFile.ShowDialog() == true)
            {
                path = openFile.FileName;
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelBook = excelApp.Workbooks.Open(path, 0, true, 5, "", "", true,
                    Excel.XlPlatform.xlWindows, "/t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelSheet = excelBook.Worksheets.get_Item(1);
                Excel.Range excelRange = excelSheet.UsedRange;
                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                DataTable dt = new DataTable();
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Excel.Range).Value2;
                    dt.Columns.Add(strColumn, typeof(string));
                }
                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            strData += strCellData + "|";
                        }
                        catch
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }

                    }
                    strData = strData.Remove(strData.Length -1, 1);
                    dt.Rows.Add(strData.Split("|"));
                }
                chitietphieuNhapSachTable.ItemsSource = dt.DefaultView;
                excelBook.Close();
            }

        }

        private void qrBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void maSanPham_cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select count(*) from SANPHAM WHERE MaSanPham = '" + maSach.Text + "'";
                    SqlCommand command = new SqlCommand(readString, connection);


                    Int32 count = (Int32)command.ExecuteScalar();
                
                if (count > 0)
                {
                   
                    readString = "select * from SANPHAM WHERE MaSanPham = '" + maSach.Text + "'";
                    command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    tenSach.Text = (String)reader["TenSanPham"];
                    tacGia.Text = (String)reader["TacGia"];
                    theLoai.Text = (String)reader["TheLoai"];
                    nhaXB.Text = (String)reader["NXB"];
                    namXB.Text = ((Int32)reader["NamXB"]).ToString();

                    tenSach.IsEnabled = false;
                    tacGia.IsEnabled = false;
                    theLoai.IsEnabled = false;
                    namXB.IsEnabled = false;
                    nhaXB.IsEnabled =false;

                }
                else
                {
                    tenSach.Text = "";
                    tacGia.Text = "";
                    theLoai.Text = "";
                    nhaXB.Text ="";
                    namXB.Text = "";

                    tenSach.IsEnabled = true;
                    tacGia.IsEnabled = true;
                    namXB.IsEnabled = true;
                    theLoai.IsEnabled = true;
                    nhaXB.IsEnabled = true;
                }    
                connection.Close();

            }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (chitietphieuNhapSachTable.SelectedIndex != -1)
            {
                chiTietPhieuNhapSachList.RemoveAt(chitietphieuNhapSachTable.SelectedIndex);
                double sum = 0;

                for (int i = 0; i < chiTietPhieuNhapSachList.Count; i++)
                {
                    chiTietPhieuNhapSachList[i].stt = i + 1;
                    sum += chiTietPhieuNhapSachList[i].getThanhTien();
                }
             
                tongTien.Text = sum.ToString();
               
                chitietphieuNhapSachTable.ItemsSource = new List<ChiTietHoaDon>();
                chitietphieuNhapSachTable.ItemsSource = chiTietPhieuNhapSachList;
            }
        }
    }
    
  

}
