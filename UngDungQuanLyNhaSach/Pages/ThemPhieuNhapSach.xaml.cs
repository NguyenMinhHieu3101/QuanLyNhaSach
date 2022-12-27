using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemPhieuNhapSach.xaml
    /// </summary>
    public partial class ThemPhieuNhapSach : Page
    {
        List<PhieuNhapSach> phieuNhapList = new List<PhieuNhapSach>();
        List<ChiTietPhieuNhapSach> chiTietPhieuNhapSachList = new List<ChiTietPhieuNhapSach>();
        List<SanPham> topSPList = new List<SanPham>();
        public ThemPhieuNhapSach()
        {
            InitializeComponent();
            ngayNhap.SelectedDate = DateTime.Now;
            loadListPhieuNhap();
            loadListChiTietPhieuNhap();
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

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from SACH";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        count++;

                        chiTietPhieuNhapSachList.Add(new ChiTietPhieuNhapSach(stt: count, maSanPham: (String)reader["MaSanPham"],
                            tenSanPham: (String)reader["TenSanPham"], tacGia: (String)reader["TacGia"],
                            theLoai: (String)reader["TheLoai"], nXB: (String)reader["NXB"],
                            giaNhap: (Decimal)reader["GiaNhap"], namXB: (Int32)reader["NamXB"], maKho: (String)reader["MaKho"]));
                            //trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Còn Hàng" : "Hết Hàng"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        chitietphieuNhapSachTable.ItemsSource = chiTietPhieuNhapSachList;
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

        void loadListPhieuNhap()
        {
            Thread thread = new Thread(new ThreadStart(() =>
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
                                    tongTien: decimal.Parse(reader["TongTien"].ToString())));

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            phieuNhapSachTable.ItemsSource = phieuNhapList;
                           //GoiYSachTable.ItemsSource = phieuNhapList;
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
        { }
        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
        //        connection.Open();
        //        string readString = "select Count(*) from SANPHAM";
        //        SqlCommand commandReader = new SqlCommand(readString, connection);
        //        Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

        //        string insertString = "INSERT INTO PHIEUNHAP(MaSanPham, TenSanPham, TacGia, TheLoai, NXB, GiaNhap, NamXB, MaKho)"
        //            + "VALUES(@MaSanPham, @TenSanPham, @TacGia, @TheLoai, @NXB, @GiaNhap, @NamXB, @MaKho)";
        //        SqlCommand command = new SqlCommand(insertString, connection);

        //        command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
        //        command.Parameters["@MaSanPham"].Value = "SP" + count.ToString("000");

        //        command.Parameters.Add("@TenSanPham", SqlDbType.NVarChar);
        //        command.Parameters["@TenSanPham"].Value = name.Text;

        //        command.Parameters.Add("@TacGia", SqlDbType.NVarChar);
        //        command.Parameters["@TacGia"].Value = author.Text;

        //        command.Parameters.Add("@TheLoai", SqlDbType.NVarChar);
        //        command.Parameters["@TheLoai"].Value = category.Text;

        //        command.Parameters.Add("@NXB", SqlDbType.NVarChar);
        //        command.Parameters["@NXB"].Value = nxb.Text;

        //        command.Parameters.Add("@GiaNhap", SqlDbType.Money);
        //        command.Parameters["@GiaNhap"].Value = cost.Text;

        //        command.Parameters.Add("@NamXB", SqlDbType.Int);
        //        command.Parameters["@NamXB"].Value = int.Parse(year.Text);

        //        command.Parameters.Add("@MaKho", SqlDbType.NVarChar);
        //        command.Parameters["@MaKho"].Value = "K001";

        //        command.ExecuteNonQuery();

        //        connection.Close();
        //        loadListChiTietPhieuNhap();
        //        MessageBox.Show("Thêm thành công");
        //        resetData();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select Count(*) from PHIEUNHAP";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                string insertString = "INSERT INTO PHIEUNHAP(MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien)"
                    + "VALUES(@MaPhieuNhap, @MaNhanVien, @MaKho, @NhaCungCap, @NgayNhap, @TongTien)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaPhieuNhap", SqlDbType.VarChar);
                command.Parameters["@MaPhieuNhap"].Value = "PN" + count.ToString();

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
                loadListPhieuNhap();
                MessageBox.Show("Thêm thành công");
                resetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                                    tacGia: (String)reader["TacGia"], nXB: (String)reader["NXB"], giaNhap: (Decimal)reader["GiaNhap"],
                                    namXB: (int)reader["NamXB"], maKho: (String)reader["MaKho"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hàng" : "Còn hàng"));

                        this.Dispatcher.BeginInvoke(new Action(() =>
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
    }
}
