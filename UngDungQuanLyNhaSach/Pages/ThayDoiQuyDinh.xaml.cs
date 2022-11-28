using System;
using System.Collections;
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
    /// Interaction logic for ThayDoiQuyDinh.xaml
    /// </summary>
    public partial class ThayDoiQuyDinh : Page
    {
      
        public ThayDoiQuyDinh()
        {
            InitializeComponent();
            loadData();
        }

        List<ThamSo> thamSoList = new List<ThamSo>();
       

        public void loadData()
        {
        
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                string readString = "select * from THAMSO";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    thamSoList.Add(new ThamSo(stt: count,
                        maThuocTinh: (String)reader["MaThuocTinh"],
                        tenThuocTinh: (String)reader["TenThuocTinh"],
                        giaTri: (double)reader["GiaTri"]

                        ));
                    danhSachThamSoTable.ItemsSource = thamSoList;
                }
                connection.Close();

                connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                connection.Open();
                thamSoList = new List<ThamSo>();
                readString = "select * from LOAIKHACHHANG";
                command = new SqlCommand(readString, connection);

                reader = command.ExecuteReader();

                count = 0;

                while (reader.Read())
                {
                    count++;
                    thamSoList.Add(new ThamSo(stt: count,
                        maThuocTinh: (String)reader["MaLoaiKhachHang"],
                        tenThuocTinh: (String)reader["TenLoaiKhachHang"],
                        giaTri: (double)reader["TienToiThieu"]

                        ));
                    danhSachKhachHangTable.ItemsSource = thamSoList;
                }
                connection.Close();
            
          
        }

        private void danhSachThamSoTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    
        private void Update_Button(object sender, RoutedEventArgs e)
        {

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var rows = GetDataGridRowsForButtons(danhSachThamSoTable);
                    foreach (DataGridRow dr in rows)
                    {
                        ThamSoDangDuocChinhSua.Ma = (dr.Item as ThamSo).maThuocTinh;
                        ThamSoDangDuocChinhSua.Ten = (dr.Item as ThamSo).tenThuocTinh;
                        ThamSoDangDuocChinhSua.GiaTri = (dr.Item as ThamSo).giaTri.ToString();
                

                        ThayDoiQuyDinhChiTiet chitiet = new ThayDoiQuyDinhChiTiet();

                    
                        chitiet.AfterClosingEvent += (o, s) => {
                            this.danhSachThamSoTable.ItemsSource = s;
                        };

                        ThamSoDangDuocChinhSua.Loai = 0;
                        chitiet.Show();
                        break;
                    }
                   break;
                }
            }
        }


        private void danhSachKhachHangTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Update_KH_Button(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var rows = GetDataGridRowsForButtons(danhSachKhachHangTable);
                    foreach (DataGridRow dr in rows)
                    {
                        ThamSoDangDuocChinhSua.Ma = (dr.Item as ThamSo).maThuocTinh;
                        ThamSoDangDuocChinhSua.Ten = (dr.Item as ThamSo).tenThuocTinh;
                        ThamSoDangDuocChinhSua.GiaTri = (dr.Item as ThamSo).giaTri.ToString();



                        ThayDoiQuyDinhChiTiet chitiet = new ThayDoiQuyDinhChiTiet();


                        chitiet.AfterClosingEvent2 += (o, s) => {
                            this.danhSachKhachHangTable.ItemsSource = s;
                        };
                        ThamSoDangDuocChinhSua.Loai = 1;
                        chitiet.Show();
                        break;
                    }
                    break;
                }
            }
        }

        private static IEnumerable<DataGridRow> GetDataGridRowsForButtons(DataGrid grid)
        {//IQueryable
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row & row.IsSelected) yield return row;
            }
        }
    }
}

