using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
    /// Interaction logic for DanhSachKhachHang.xaml
    /// </summary>
    public partial class TraCuuKhachHang : Page
    {
        public TraCuuKhachHang()
        {
            InitializeComponent();
            loadData();
        }

        void loadData()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<KhachHang> khachHangList = new List<KhachHang>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHACHHANG";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        khachHangList.Add(new KhachHang(stt: count, maKhachHang: (String)reader["MaKhachHang"],
                            tenKhachHang: (String)reader["TenKhachHang"], diaChi: (String)reader["DiaChi"],
                            gioiTinh: (String)reader["GioiTinh"], maLoaiKhachHang: (String)reader["MaLoaiKhachHang"],
                            sdt: (String)reader["SDT"], email: (String)reader["Email"], trangThai: ((String)reader["TrangThai"]).CompareTo("0") == 0 ? "Không tồn tại" : "Còn sử dụng"));
                    }
                    this.Dispatcher.BeginInvoke(new Action(() => {
                        resultKhachHangTable.ItemsSource = khachHangList;
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

        private void resultKhachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
