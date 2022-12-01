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
    /// Interaction logic for DanhSachKhuyenMai.xaml
    /// </summary>
    public partial class TraCuuKhuyenMai : Page
    {
        List<KhuyenMai> selectedKhachHang = new List<KhuyenMai>();
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public TraCuuKhuyenMai()
        {
            InitializeComponent();
            loadData();
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
            Thread thread = new Thread(new ThreadStart(() =>
            {
                khuyenMaiList = new List<KhuyenMai>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
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
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        resultKhuyenMaiTable.ItemsSource = khuyenMaiList;
                        //DataGridCheckBoxColumn checkBoxColumn = new DataGridCheckBoxColumn();
                        //checkBoxColumn.Header = "";
                        //resultKhuyenMaiTable.Columns.Add(checkBoxColumn);
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

        bool checkSearch(KhuyenMai khuyenMai)
        {
            if (!maKM.Text.ToLower().Contains(khuyenMai.maKhuyenMai)) return false;
            if (!soLuong.Text.Contains(khuyenMai.soLuong.ToString())) return false;
            return true;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            List<KhuyenMai> searchList = new List<KhuyenMai>();
            foreach (KhuyenMai km in khuyenMaiList)
                if (checkSearch(km))
                {
                    searchList.Add(km);
                }
            resultKhuyenMaiTable.ItemsSource = new List<KhuyenMai>();
            resultKhuyenMaiTable.ItemsSource = searchList;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKM.Text = "";
            soLuong.Text = "";
            phanTram.Text = "";
            loaiKhachHang.SelectedIndex = 0;
            trangThai.SelectedIndex = 0;
        }

        private void resultKhuyenMaiTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhachHang.Remove(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                selectedKhachHang.Add(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhachHang.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }
    }
}
