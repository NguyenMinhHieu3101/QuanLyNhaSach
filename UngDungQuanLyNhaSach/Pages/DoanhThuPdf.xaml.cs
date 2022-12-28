using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for DoanhThuPdf.xaml
    /// </summary>
    public partial class DoanhThuPdf : Window
    {
        List<ChiTra> chiTraList = new List<ChiTra>();
        List<ThuNhap> thuNhapList = new List<ThuNhap>();

        public DoanhThuPdf()
        {
            InitializeComponent();
            loadInfo();
            loadDataGrid();
        }

        private void print_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Báo cáo doanh thu");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void loadInfo()
        {
            date.Text = "Ngày lập báo cáo: " + DateTime.Now.ToString();
            author.Text = "Người lập báo cáo: " + NhanVienDangDangNhap.HoTen;
        }

        void loadDataGrid()
        {
            SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            connection.Open();

            SqlCommand command = new SqlCommand();

            try
            {
                string readString = "SELECT COUNT(*) FROM CHITIETBAOCAODOANHTHU";
                command = new SqlCommand(readString, connection);
                Int32 count = (Int32)command.ExecuteScalar();

                string readString2 = "SELECT * FROM CHITIETBAOCAODOANHTHU WHERE MaChiTietBaoCao = @MaChiTietBaoCao";
                command = new SqlCommand(readString2, connection);

                command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                command.Parameters["@MaChiTietBaoCao"].Value = "DT" + count.ToString();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                from_to.Text = "TỪ NGÀY " + reader["TuNgay"] + " ĐẾN NGÀY " + reader["DenNgay"];
                txtTongChi.Text = "TỔNG CHI: " + string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(reader["ChiPhi"].ToString()));
                txtTongThu.Text = "TỔNG THU: " + string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(reader["DoanhThu"].ToString()));
                txtLoiNhuan.Text = "LỢI NHUẬN: " + string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(((double)reader["DoanhThu"] - (double)reader["ChiPhi"]).ToString()));

                thuNhapList.Add(new ThuNhap(1, "Bán sách", string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(reader["DoanhThu"].ToString()))));
                thuNhapTable.ItemsSource = thuNhapList;

                double tongTienNhap;
                double tongLuong;
                double tongChi = (double)reader["ChiPhi"];

                string tuNgay = reader["TuNgay"].ToString();
                string denNgay = reader["DenNgay"].ToString();

                reader.Close();

                string readString3 = "SELECT SUM(TongTien) AS TongChi FROM PHIEUNHAP WHERE NgayNhap BETWEEN @TuNgay AND @DenNgay";
                command = new SqlCommand(readString3, connection);

                command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                command.Parameters["@TuNgay"].Value = tuNgay;

                command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                command.Parameters["@DenNgay"].Value = denNgay;

                if (command.ExecuteScalar() == DBNull.Value)
                    tongTienNhap = 0;
                else
                    tongTienNhap = (double)command.ExecuteScalar();

                chiTraList.Add(new ChiTra(1, "Tiền nhập sách", string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(tongTienNhap.ToString()))));

                if (tongChi > tongTienNhap)
                {
                    string readString4 = "SELECT SUM(Luong) AS TongLuong FROM NHANVIEN";
                    command = new SqlCommand(readString4, connection);
                    tongLuong = (double)command.ExecuteScalar();
                    chiTraList.Add(new ChiTra(2, "Lương nhân viên", string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(tongLuong.ToString()))));
                }

                chiTraTable.ItemsSource = chiTraList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chiTraTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void thuNhapTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
