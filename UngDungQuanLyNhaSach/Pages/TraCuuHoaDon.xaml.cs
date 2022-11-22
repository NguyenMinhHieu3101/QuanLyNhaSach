
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DanhSachPhieuNhapSach.xaml
    /// </summary>
    public partial class TraCuuHoaDon : Page
    {
        List<HoaDon> hoaDonList = new List<HoaDon>();
        public TraCuuHoaDon()
        {
            InitializeComponent();
            loadData();
        }
        void loadData()
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from HOADON";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    hoaDonList.Add(new HoaDon (stt: count, 
                        maHoaDon: (String)reader["MaHoaDon"],
                        maNhanVien: (String)reader["MaNhanVien"],
                        maKhachHang: (String)reader["MaKhachHang"],
                        maKhuyenMai: (String)reader["MaKhuyenMai"],
                        ngayLapHD: (DateTime)reader["NgayLapHoaDon"],
                        tongTienHD: (decimal)reader["TongTienHoaDon"]
                        //nguoiLapHD: (String)reader["NguoiLapHoaDon"]
                        ));
                    danhSachHDTable.ItemsSource = hoaDonList;
                }
                connection.Close();
            }
            catch (Exception e1)
            {
                //MessageBox.Show("db error");
                MessageBox.Show(e1.Message);
                
            }
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
        private void chitietHDTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //var desc = e.PropertyDescriptor as PropertyDescriptor;
            //var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            //if (att != null)
            //{
            //    e.Column.Header = att.Name;
            //    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            //}
        }
    }
}

