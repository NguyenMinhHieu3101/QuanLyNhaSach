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
        List<KhuyenMai> selectedKhuyenMai = new List<KhuyenMai>();
        List<KhuyenMai> khuyenMaiList = new List<KhuyenMai>();

        public TraCuuKhuyenMai()
        {
            InitializeComponent();
            //ngayBatDau.SelectedDate= DateTime.Now;
            //ngayKetThuc.SelectedDate= DateTime.Now;
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
            String maKMText = maKM.Text;
            String soLuongText = soLuong.Text;
            String phanTramText = phanTram.Text;
            String loaiKH = loaiKhachHang.Text;
            int index = trangThai.SelectedIndex;
            DateTime? start = ngayBatDau.SelectedDate;
            DateTime? finish = ngayKetThuc.SelectedDate;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                khuyenMaiList = new List<KhuyenMai>();

                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "select * from KHUYENMAI, LOAIKHACHHANG WHERE KHUYENMAI.MaLoaiKhachHang = LOAIKHACHHANG.MaLoaiKhachHang";
                    if (maKMText.Length > 0) readString += " AND MaKhuyenMai Like '%" + maKMText + "%'";
                    if (soLuongText.Length > 0) readString += " AND SoLuongKhuyenMai = " + soLuongText;
                    if (phanTramText.Length > 0) readString += " AND PhanTram = " + phanTramText;
                    if (loaiKH.Length > 0) readString += " AND TenLoaiKhachHang = N'" + loaiKH + "'";
                    if (index != 2) readString += " AND TrangThai = '" + index + "'";
                    if (start != null) readString += " AND ThoiGianBatDau = '" + ((start ?? DateTime.Now).ToString("MM/dd/yyyy")) + "'";
                    if (finish != null) readString += " AND ThoiGianKetThuc = '" + ((finish ?? DateTime.Now).ToString("MM/dd/yyyy")) + "'";

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

        private void search_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            maKM.Text = "";
            soLuong.Text = "";
            phanTram.Text = "";
            ngayKetThuc.SelectedDate = DateTime.Now;
            ngayBatDau.SelectedDate = DateTime.Now;
            loaiKhachHang.SelectedIndex = 0;
            trangThai.SelectedIndex = 0;
        }

        private void resultKhuyenMaiTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Remove(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                selectedKhuyenMai.Add(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Remove(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
                for (int i = 0; i < showSelectedKhachHang.Count; i++)
                {
                    showSelectedKhachHang[i].stt = i + 1;
                }
                chooseKhuyenMaiTable.ItemsSource = new List<KhachHang>();
                chooseKhuyenMaiTable.ItemsSource = showSelectedKhachHang;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (resultKhuyenMaiTable.SelectedIndex != -1)
            {
                selectedKhuyenMai.Add(khuyenMaiList[resultKhuyenMaiTable.SelectedIndex]);
                List<KhuyenMai> showSelectedKhachHang = selectedKhuyenMai.OrderBy(e => e.maKhuyenMai).ToList();
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
