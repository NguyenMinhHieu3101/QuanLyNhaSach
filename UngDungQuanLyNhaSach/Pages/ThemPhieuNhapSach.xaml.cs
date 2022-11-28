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
        List<SanPham> sanPhamList = new List<SanPham>();
        public ThemPhieuNhapSach()
        {
            InitializeComponent();
            ngaylapphieu.SelectedDate = DateTime.Now;
            ngaynhapsach.SelectedDate = DateTime.Now;
            loadListProduct();
        }
        void resetData()
        {
            name.Text = "";
            nxb.Text = "";
            number.Text = "";
            cost.Text = "";
            year.Text = "";
            author.Text = "";
            category.Text = "";

        }
        
        void loadListProduct()
        {
            //Thread thread = new Thread(new ThreadStart(() =>
            //{
                sanPhamList = new List<SanPham>();

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

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        sanPhamList.Add(new SanPham(stt: count, maSanPham: (String)reader["MaSanPham"],
                                    tenSanPham: (String)reader["TenSanPham"], tacGia: (String)reader["TacGia"],
                                    theLoai: (String)reader["TheLoai"], nXB: (String)reader["NXB"],
                                    giaNhap: (Decimal)reader["GiaNhap"], namXB: (Int32)reader["NamXB"], maKho: (String)reader["MaKho"],
                                    trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Còn Hàng" : "Hết Hàng"));

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            phieuNhapSachTable.ItemsSource = sanPhamList;
                        }));
                    }
                    connection.Close();
                }
                catch (Exception e1)
                {
                    //MessageBox.Show("db error");
                    MessageBox.Show(e1.Message);

                }
           
            ////}/*));*/
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

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                string readString = "select Count(*) from SANPHAM";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;

                string insertString = "INSERT INTO SANPHAM(MaSanPham, TenSanPham, TacGia, TheLoai, NXB, GiaNhap, NamXB, MaKho, TrangThai)"
                    + "VALUES(@MaSanPham, @TenSanPham, @TacGia, @TheLoai, @NXB, @GiaNhap, @NamXB, @MaKho, @TrangThai)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaSanPham", SqlDbType.VarChar);
                command.Parameters["@MaSanPham"].Value = "SP" + count.ToString();

                command.Parameters.Add("@TenSanPham", SqlDbType.NVarChar);
                command.Parameters["@TenSanPham"].Value = name.Text;

                command.Parameters.Add("@TacGia", SqlDbType.NVarChar);
                command.Parameters["@TacGia"].Value = author.Text;

                command.Parameters.Add("@TheLoai", SqlDbType.NVarChar);
                command.Parameters["@TheLoai"].Value = category.Text;

                command.Parameters.Add("@NXB", SqlDbType.NVarChar);
                command.Parameters["@NXB"].Value = nxb.Text;

                command.Parameters.Add("@GiaNhap", SqlDbType.Money);
                command.Parameters["@GiaNhap"].Value = cost.Text;

                command.Parameters.Add("@NamXB", SqlDbType.Int);
                command.Parameters["@NamXB"].Value = year.Text;

                //command.Parameters.Add("@MaKho", SqlDbType.VarChar);
                //command.Parameters["@MaKho"].Value = "K" + count.ToString();
                command.Parameters.Add("@MaKho", SqlDbType.VarChar);
                command.Parameters["@MaKho"].Value = "K03";

                command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                command.Parameters["@TrangThai"].Value = "1";

                command.ExecuteNonQuery();

                connection.Close();
                loadListProduct();
                MessageBox.Show("Thêm thành công");
                resetData();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Lỗi òi nè");
            }
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
       
    }
}
