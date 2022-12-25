using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
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
    /// Interaction logic for DanhSachSachHienCo.xaml
    /// </summary>
    public partial class TraCuuSanPham : Page
    {
        List<SanPham> sanPhamList = new List<SanPham>();
        public TraCuuSanPham()
        {
            InitializeComponent();
            loadData();
            //updateBtn.IsEnabled = false;
            //deleteBtn.IsEnabled = false;
            loadFilter();
        }

        void loadFilter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                //sanPhamList = new List<SanPham>();

                //try
                //{
                //    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                //    connection.Open();
                //    string readString = "select * from SANPHAM";
                //    SqlCommand command = new SqlCommand(readString, connection);

                //    SqlDataReader reader = command.ExecuteReader();

                //    int count = 0;
                //    while (reader.Read())
                //    {
                //        count++;

                //        sanPhamList.Add(new SanPham(stt: count, maSanPham: (String)reader["MaSanPham"],
                //            tenSanPham: (String)reader["TenSanPham"], tacGia: (String)reader["TacGia"],
                //            theLoai: (String)reader["TheLoai"],nXB: (String)reader["NXB"], 
                //            giaNhap: (Decimal)reader["GiaNhap"], namXB: (Int32)reader["NamXB"], maKho: (String)reader["MaKho"],
                //            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Còn Hàng" : "Hết Hàng"));
                //    }
                //    this.Dispatcher.BeginInvoke(new Action(() => {
                //        phieuNhapSachTable.ItemsSource = sanPhamList;
                //    }));
                //    connection.Close();
                //}
                //catch (Exception e1)
                //{
                //    //MessageBox.Show("db error");
                //    MessageBox.Show(e1.Message);

                //}

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from SANPHAM";

                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    //List<String> itemsMaSP = new List<String>();
                    List<String> itemsTenSP = new List<String>();
                    List<String> itemsTheLoai = new List<String>();
                    List<String> itemsTacGia = new List<String>();
                    List<decimal> itemsDonGia = new List<decimal>();
                    List<String> itemsNXB = new List<String>();
                    List<int> itemsNamXB = new List<int>();


                    while (reader.Read())
                    {
                        //sanPhamList.Add(new SanPham(maSanPham: (String)reader["MaSanPham"],
                        //            tenSanPham: (String)reader["TenSanPham"], tacGia: (String)reader["TacGia"],
                        //            theLoai: (String)reader["TheLoai"], nXB: (String)reader["NXB"],
                        //            giaNhap: (Decimal)reader["GiaNhap"], namXB: (Int32)reader["NamXB"], maKho: (String)reader["MaKho"],
                        //            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Còn Hàng" : "Hết Hàng"));
                        itemsTenSP.Add((String)reader["TenSanPham"]);
                        itemsTheLoai.Add((String)reader["TheLoai"]);
                        itemsTacGia.Add((String)reader["TacGia"]);
                        itemsDonGia.Add((decimal)reader["GiaNhap"]);
                        itemsNXB.Add((String)reader["NXB"]);
                        itemsNamXB.Add((int)reader["NamXB"]);

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            name.ItemsSource = itemsTenSP;
                            category.ItemsSource = itemsTheLoai;
                            author.ItemsSource = itemsTacGia;
                            cost.ItemsSource = itemsDonGia;
                            nxb.ItemsSource = itemsNXB;
                            year.ItemsSource = itemsNamXB;
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


        //private void updateBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
        //        connection.Open();
        //        string updateString = "Update SANPHAM Set TenSanPham = @TenSanPham, TacGia = @TacGia, TheLoai = @TheLoai, NXB = @NXB, GiaNhap = @GiaNhap, NamXB = @NamXB, MaKho = @MaKho, TrangThai = @TrangThai "
        //            + "Where MaSanPham = @MaSanPham";
        //        SqlCommand command = new SqlCommand(updateString, connection);

        //        command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
        //        command.Parameters["@MaSanPham"].Value = sanPhamList[phieuNhapSachTable.SelectedIndex].maSanPham;

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
        //        command.Parameters["@NamXB"].Value = year.Text;

        //        //command.Parameters.Add("@MaKho", SqlDbType.VarChar);
        //        //command.Parameters["@MaKho"].Value = "K" + count.ToString();
        //        command.Parameters.Add("@MaKho", SqlDbType.VarChar);
        //        command.Parameters["@MaKho"].Value = "K03";

        //        command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
        //        command.Parameters["@TrangThai"].Value = "1";

        //        command.ExecuteNonQuery();
        //        connection.Close();
        //        loadListProduct();
        //        resetData();
        //        MessageBox.Show("Cập nhật thành công");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Cập nhật không thành công");
        //    }
        //}



        //private void deleteBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (phieuNhapSachTable.SelectedIndex != -1)
        //    {
        //        var result = MessageBox.Show("Bạn thật sự muốn xóa?", "Thông báo!", MessageBoxButton.OKCancel);
        //        if (result == MessageBoxResult.OK)
        //        {
        //            try
        //            {
        //                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
        //                connection.Open();

        //                string deleteString = "Delete From SANPHAM Where MaSanPham = @MaSanPham";
        //                SqlCommand command = new SqlCommand(deleteString, connection);
        //                command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
        //                command.Parameters["@MaSanPham"].Value = sanPhamList[phieuNhapSachTable.SelectedIndex].maSanPham;

        //                command.ExecuteNonQuery();
        //                connection.Close();
        //                loadListProduct();
        //                MessageBox.Show("Xóa sản phẩm thành công");
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Xóa không thành công");
        //            }
        //        }
        //    }
        //}

        void loadData()
        {
            ////try
            ////{
            //SanPham sanPham = sanPhamList[sanPhamTable.SelectedIndex];
            //name.Text = sanPham.tenSanPham;
            //nxb.Text = sanPham.nXB;
            ////number.Text = sanPham.soLuong;
            //cost.Text = sanPham.giaNhap.ToString();
            //year.Text = sanPham.namXB.ToString();
            //author.Text = sanPham.tacGia;
            //category.Text = sanPham.theLoai;
            ////}
            ////catch (Exception ex)
            ////{
            ////    MessageBox.Show(ex.Message);
            ////}
            
            String tenSP = name.Text;
            String theLoai = category.Text;
            String donGia = cost.Text;
            String NXB = nxb.Text;
            String namXB = year.Text;
            String tacGia = author.Text;
            int index = status.SelectedIndex;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                sanPhamList = new List<SanPham>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from SANPHAM WHERE MaSanPham IS NOT NULL";
                    if (tenSP.Length > 0) readString += " AND TenSanPham Like N'%" + tenSP + "%'";
                    if (theLoai.Length > 0) readString += " AND TheLoai Like N'%" + theLoai + "%'";
                    if (donGia.Length > 0) readString += " AND GiaNhap = '" + donGia + "'";
                    if (NXB.Length > 0) readString += " AND NXB Like N'%" + NXB + "%'";
                    if (namXB.Length > 0) readString += " AND NamXB = '" + namXB + "'";
                    if (tacGia.Length > 0) readString += " AND TacGia Like N'%" + tacGia + "%'";
                    if (index != -1) readString += " AND TrangThai = '" + index + "'";
                    //if (soLuong.Length > 0) readString += " AND TongTien = '" + tongTienText + "'";

                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        sanPhamList.Add(new SanPham(stt: count,
                            maSanPham: (String)reader["MaSanPham"],
                            tenSanPham: (String)reader["TenSanPham"],
                            tacGia: (String)reader["TacGia"],
                            theLoai: (String)reader["TheLoai"],
                            nXB: (String)reader["NXB"],
                            giaNhap: (decimal)reader["GiaNhap"],
                            namXB: (Int32)reader["NamXB"],
                            maKho: (String)reader["MaKho"],
                            trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hàng" : "Còn hàng")); 
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        sanPhamTable.ItemsSource = sanPhamList;
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
        private void sanPhamTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            sanPhamList = new List<SanPham>();
            sanPhamTable.ItemsSource = sanPhamList;
            loadData();
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            name.SelectedIndex = -1;
            category.SelectedIndex = -1;
            cost.SelectedIndex = -1;
            nxb.SelectedIndex = -1;
            year.SelectedIndex = -1;
            author.SelectedIndex = -1;
            status.SelectedIndex = -1;
            //number.Text = "";
            sanPhamTable.ItemsSource = new List<SanPham>();
            //loadListProduct();
            loadData();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
