using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.IO;
using System.Collections;
using LiveCharts;
using LiveCharts.Wpf;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for BaoCaoDoanhThu.xaml
    /// </summary>
    public partial class BaoCaoDoanhThu : Page
    {
        List<ChiTra> chiTraList = new List<ChiTra>();
        List<ThuNhap> thuNhapList = new List<ThuNhap>();

        //public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        public BaoCaoDoanhThu()
        {
            InitializeComponent();
            loadChart();
            DataContext = this;

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        //protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        //{
        //    EventHandler<CustomEventArgs> raiseEvent = RaiseCustomEvent;
        //    raiseEvent.Invoke(this, e);
        //}

        //public class CustomEventArgs : EventArgs
        //{
        //    public CustomEventArgs(string message) => Message = message;
        //    public string Message { get; set; }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chiTraList = new List<ChiTra>();
                thuNhapList = new List<ThuNhap>();

                decimal tongThu = 0;
                decimal tongChi = 0;
                decimal tongLuong = 0;
                decimal tongTienNhap = 0;
                double tienThue = 0;
                double tienMatBang = 0;

                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();

                if (dPickerTuNgay.SelectedDate == null || dPickerDenNgay.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn khoảng thời gian cần xuất báo cáo!");
                }
                else
                {
                    txtThoiGian.Text = "TỪ " + dPickerTuNgay.SelectedDate.Value.ToShortDateString() + " ĐẾN " + dPickerDenNgay.SelectedDate.Value.ToShortDateString();

                    //Xử lí số liệu thu nhập

                    string readString1 = "SELECT SUM(TongTienHoaDon) AS TongThu FROM HOADON WHERE NgayLapHoaDon BETWEEN @TuNgay AND @DenNgay";
                    SqlCommand command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongThu = 0;
                    else
                        tongThu = (decimal)command.ExecuteScalar();

                    txtTongThu.Text = "TỔNG THU: " + tongThu;

                    thuNhapList.Add(new ThuNhap(1, "Bán sách", tongThu));
                    thuNhapTable.ItemsSource = thuNhapList;

                    //Xử lí số liệu chi trả

                    string readString2 = "SELECT SUM(TongTien) AS TongChi FROM PHIEUNHAP WHERE NgayNhap BETWEEN @TuNgay AND @DenNgay";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongTienNhap = 0;
                    else
                        tongTienNhap = (decimal)command.ExecuteScalar();

                    chiTraList.Add(new ChiTra(1, "Tiền nhập sách", tongTienNhap));

                    string readString3 = "SELECT GiaTri FROM THAMSO WHERE TenThuocTinh = N'Thuế'";
                    command = new SqlCommand(readString3, connection);
                    tienThue = (double)command.ExecuteScalar();
                    chiTraList.Add(new ChiTra(2, "Thuế", (decimal)tienThue));

                    string readString4 = "SELECT GiaTri FROM THAMSO WHERE TenThuocTinh = N'Mặt bằng'";
                    command = new SqlCommand(readString4, connection);
                    tienMatBang = (double)command.ExecuteScalar();
                    chiTraList.Add(new ChiTra(3, "Tiền mặt bằng", (decimal)tienMatBang));

                    if (dPickerTuNgay.SelectedDate.Value.Day == 1 || dPickerDenNgay.SelectedDate.Value.Day == 1)
                    {
                        string readString5 = "SELECT SUM(Luong) AS TongLuong FROM NHANVIEN";
                        command = new SqlCommand(readString5, connection);
                        tongLuong = (decimal)command.ExecuteScalar();
                        chiTraList.Add(new ChiTra(4, "Lương nhân viên", tongLuong));
                    }

                    chiTraTable.ItemsSource = chiTraList;

                    tongChi = tongTienNhap + tongLuong;
                    txtTongChi.Text = "TỔNG CHI: " + tongChi;

                    decimal loiNhuan = tongThu - tongChi;
                    txtLoiNhuan.Text = "LỢI NHUẬN: " + loiNhuan;

                    //Insert chi tiết báo cáo vào database

                    string readString6 = "SELECT COUNT(*) FROM CHITIETBAOCAODOANHTHU";
                    command = new SqlCommand(readString6, connection);
                    Int32 count = (Int32)command.ExecuteScalar() + 1;

                    string insertString = "INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES (@MaChiTietBaoCao, @TuNgay, @DenNgay, @MaNVBC, @DoanhThu, @ChiPhi)";
                    command = new SqlCommand(insertString, connection);

                    command.Parameters.Add("@MaChiTietBaoCao", SqlDbType.VarChar);
                    command.Parameters["@MaChiTietBaoCao"].Value = "DT" + count.ToString();

                    command.Parameters.Add("@TuNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@TuNgay"].Value = dPickerTuNgay.SelectedDate;

                    command.Parameters.Add("@DenNgay", SqlDbType.SmallDateTime);
                    command.Parameters["@DenNgay"].Value = dPickerDenNgay.SelectedDate;

                    command.Parameters.Add("@MaNVBC", SqlDbType.VarChar);
                    command.Parameters["@MaNVBC"].Value = NhanVienDangDangNhap.MaNhanVien;

                    command.Parameters.Add("@DoanhThu", SqlDbType.Money);
                    command.Parameters["@DoanhThu"].Value = tongThu.ToString();

                    command.Parameters.Add("@ChiPhi", SqlDbType.Money);
                    command.Parameters["@ChiPhi"].Value = tongChi.ToString();

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //MessageBox.Show("Khoảng thời gian không hợp lệ!");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Button_Click(sender, e);

            //PdfPTable pdfTable = new PdfPTable(chiTraTable.Columns.Count);
            //pdfTable.DefaultCell.Padding = 3;
            //pdfTable.WidthPercentage = 90;
            //pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //pdfTable.DefaultCell.BorderWidth = 1;

            //foreach (DataGridColumn column in chiTraTable.Columns)
            //{
            //    PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString()));
            //    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
            //    pdfTable.AddCell(cell);
            //}
            //var rows = GetDataGridRowsForButtons(chiTraTable);

            //foreach (DataGridRow row in rows)
            //{
            //    foreach (DataGridCell cell in row.Resources)
            //    {
            //        pdfTable.AddCell(cell.Resources.ToString());
            //    }
            //}

            //var path = "C:\\Users\\Phuong Quyen\\Downloads\\";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph("Test");
            //using (FileStream stream = new FileStream(path + "report.pdf", FileMode.Create))
            //{
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            //    PdfWriter.GetInstance(pdfDoc, stream);
            //    pdfDoc.Open();

            //    pdfDoc.Add(p);
            //    pdfDoc.Add(pdfTable);
            //    pdfDoc.Close();
            //}

            //OnRaiseCustomEvent(new CustomEventArgs("Event triggered!"));

            BaoCaoPdf baoCaoPdf = new BaoCaoPdf();

            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(baoCaoPdf, "Báo cáo doanh thu");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        void loadChart()
        {
            //int thangHienTai = 5;
            int thangHienTai = DateTime.Now.Month;
            decimal[] tongThu = new decimal[5];
            decimal[] tongChi = new decimal[5];
            String[] label = new String[5];

            for (int i = 0; i < 5; i++)
            {
                label[i] = "Tháng " + (thangHienTai - i).ToString();
            }

            try
            {
                SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                connection.Open();
                SqlCommand command;

                for (int i = 0; i < 5; i++)
                {
                    string readString1 = "SELECT SUM(TongTienHoaDon) AS TongThu FROM HOADON WHERE MONTH(NgayLapHoaDon) = @Thang";
                    command = new SqlCommand(readString1, connection);

                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    command.Parameters["@Thang"].Value = thangHienTai - i;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongThu[i] = 0;
                    else
                        tongThu[i] = (decimal)command.ExecuteScalar();
                }

                for (int i = 0; i < 5; i++)
                {

                    string readString2 = "SELECT SUM(TongTien) AS TongChi FROM PHIEUNHAP WHERE MONTH(NgayNhap) = @Thang";
                    command = new SqlCommand(readString2, connection);

                    command.Parameters.Add("@Thang", SqlDbType.Int);
                    command.Parameters["@Thang"].Value = thangHienTai - i;

                    if (command.ExecuteScalar() == DBNull.Value)
                        tongChi[i] = 0;
                    else
                        tongChi[i] = (decimal)command.ExecuteScalar();

                    //string readString3 = "SELECT GiaTri FROM THAMSO WHERE TenThuocTinh = N'Thuế'";
                    //command = new SqlCommand(readString3, connection);
                    //tongChi[i] += (decimal)command.ExecuteScalar();

                    //string readString4 = "SELECT GiaTri FROM THAMSO WHERE TenThuocTinh = N'Mặt bằng'";
                    //command = new SqlCommand(readString4, connection);
                    //tongChi[i] += (decimal)command.ExecuteScalar();

                    string readString5 = "SELECT SUM(Luong) AS TongLuong FROM NHANVIEN";
                    command = new SqlCommand(readString5, connection);
                    tongChi[i] += (decimal)command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SeriesCollection = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                        Title = "Thu nhập",
                        Values = new ChartValues<double> {(double)tongThu[4], (double)tongThu[3], (double)tongThu[2], (double)tongThu[1], (double)tongThu[0]}
                    },
                    new ColumnSeries
                    {
                        Title = "Chi trả",
                        Values = new ChartValues<double> { (double)tongChi[4], (double)tongChi[3], (double)tongChi[2], (double)tongChi[1], (double)tongChi[0] }
                    }
                };

            Labels = new[] { label[4], label[3], label[2], label[1], label[0] };
            Formatter = value => value.ToString("N");
        }
    }
}
