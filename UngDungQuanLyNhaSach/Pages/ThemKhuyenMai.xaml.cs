using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
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
using System.Xml.Linq;
using UngDungQuanLyNhaSach.Model;
using System.ComponentModel;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThemKhuyenMai.xaml
    /// </summary>
    public partial class ThemKhuyenMai : Page
    {
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public ThemKhuyenMai()
        {
            InitializeComponent();
            loadData();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                string readString = "select Count(*) from KHUYENMAI";
                SqlCommand commandReader = new SqlCommand(readString, connection);
                Int32 count = (Int32)commandReader.ExecuteScalar() + 1;


                string insertString = "INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, TrangThai) " +
                    "VALUES (@MaKhuyenMai, @ThoiGianBatDau, @ThoiGianKetThuc, @MaLoaiKhachHang, @SoLuongKhuyenMai, @TrangThai)";
                SqlCommand command = new SqlCommand(insertString, connection);

                command.Parameters.Add("@MaKhuyenMai", SqlDbType.VarChar);
                command.Parameters["@MaKhuyenMai"].Value = "KM" + count.ToString();

                command.Parameters.Add("@ThoiGianBatDau", SqlDbType.SmallDateTime);
                command.Parameters["@ThoiGianBatDau"].Value = DateTime.Now;

                command.Parameters.Add("@ThoiGianKetThuc", SqlDbType.SmallDateTime);
                command.Parameters["@ThoiGianKetThuc"].Value = DateTime.Now;

                command.Parameters.Add("@MaLoaiKhachHang", SqlDbType.VarChar);
                command.Parameters["@MaLoaiKhachHang"].Value = loaiKhachHang.SelectedIndex == 0 ? "VL" :
                    (loaiKhachHang.SelectedIndex == 1 ? "B" : (loaiKhachHang.SelectedIndex == 2 ? "V" : "KC"));

                command.Parameters.Add("@SoLuongKhuyenMai", SqlDbType.Int);
                command.Parameters["@SoLuongKhuyenMai"].Value = int.Parse(soLuong.Text);

                command.Parameters.Add("@TrangThai", SqlDbType.VarChar);
                command.Parameters["@TrangThai"].Value = "1";

                command.ExecuteNonQuery();

                MessageBox.Show("Thêm thành công");
                connection.Close();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void khuyenMaiTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from KHUYENMAI";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    khuyenMaiList.Add(new KhuyenMai(stt: count, maKhuyenMai: (String)reader["MaKhuyenMai"],
                        batDau: (DateTime)reader["ThoiGianBatDau"], //DateTime.ParseExact(reader["ThoiGianBatDau"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        ketThuc: (DateTime)reader["ThoiGianKetThuc"], //DateTime.ParseExact(reader["ThoiGianKetThuc"].ToString(), "M/d/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                        soLuong: (int)reader["SoLuongKhuyenMai"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Hết hạn" : "Còn hiệu lực"));
                    khuyenMaiTable.ItemsSource = khuyenMaiList;
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("db error");
            }
        }
    }
}
