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
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for ThayDoiQuyDinhChiTiet.xaml
    /// </summary>
    public partial class ThayDoiQuyDinhChiTiet : Window
    {

        public event EventHandler<List<ThamSo>> AfterClosingEvent;
        public event EventHandler<List<ThamSo>> AfterClosingEvent2;
        public ThayDoiQuyDinhChiTiet()
        {
            InitializeComponent();

              tenThuocTinh.Text = ThamSoDangDuocChinhSua.Ten;
              giaTri.Text = ThamSoDangDuocChinhSua.GiaTri ;
        }
        List<ThamSo> thamSoList = new List<ThamSo>();
        
        private void update_Click(object sender, RoutedEventArgs e)
        {
            ThamSoDangDuocChinhSua.GiaTri = giaTri.Text;
            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

            if (ThamSoDangDuocChinhSua.Loai == 0)
            {

                connection.Open();
                    string updateString = "UPDATE THAMSO SET GiaTri = @GiaTri, TenThuocTinh = @TenThuocTinh WHERE MaThuocTinh = @MaThuocTinh";
                    SqlCommand command = new SqlCommand(updateString, connection);
                    command.Parameters.AddWithValue("GiaTri", Convert.ToDouble(ThamSoDangDuocChinhSua.GiaTri));
                    command.Parameters.AddWithValue("TenThuocTinh", ThamSoDangDuocChinhSua.Ten);
                    command.Parameters.AddWithValue("MaThuocTinh", ThamSoDangDuocChinhSua.Ma);
                    command.ExecuteNonQuery();

                    string readString = "select * from THAMSO";
                    command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        thamSoList.Add(new ThamSo(stt: count,
                            maThuocTinh: (String)reader["MaThuocTinh"],
                            tenThuocTinh: (String)reader["TenThuocTinh"],
                            giaTri: (Double)reader["GiaTri"]));

                    }
                connection.Close();
                this.AfterClosingEvent?.Invoke(this, thamSoList);
            }
            else
            {
                connection.Open();
                   string  updateString = "UPDATE LOAIKHACHHANG SET TienToiThieu = @GiaTri, TenLoaiKhachHang = @TenThuocTinh WHERE MaLoaiKhachHang = @MaThuocTinh";
                   SqlCommand command = new SqlCommand(updateString, connection);
                    command.Parameters.AddWithValue("GiaTri", Convert.ToDouble(ThamSoDangDuocChinhSua.GiaTri));
                    command.Parameters.AddWithValue("TenThuocTinh", ThamSoDangDuocChinhSua.Ten);
                    command.Parameters.AddWithValue("MaThuocTinh", ThamSoDangDuocChinhSua.Ma);
                    command.ExecuteNonQuery();
                 
                    string readString = "select * from LOAIKHACHHANG";
                    command = new SqlCommand(readString, connection);
                thamSoList = new List<ThamSo>();
                SqlDataReader reader = command.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        thamSoList.Add(new ThamSo(stt: count,
                            maThuocTinh: (String)reader["MaLoaiKhachHang"],
                            tenThuocTinh: (String)reader["TenLoaiKhachHang"],
                            giaTri: (double)reader["TienToiThieu"]));
                    }
                connection.Close();
                this.AfterClosingEvent2?.Invoke(this, thamSoList);
            }

            MessageBox.Show("Update complete");
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
   
    }
}
