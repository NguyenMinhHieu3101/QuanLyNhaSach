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
    public partial class TraCuuPhieuNhapSach : Page
    {
        List <PhieuNhapSach> phieuNhapSachList = new List <PhieuNhapSach>();
        public TraCuuPhieuNhapSach()
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
                string readString = "select * from PHIEUNHAP";
                SqlCommand command = new SqlCommand(readString, connection);

                SqlDataReader reader = command.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                    phieuNhapSachList.Add(new PhieuNhapSach(stt: count,
                        maPhieuNhap: (String)reader["MaPhieuNhap"],
                        maNhanVien: (String)reader["MaNhanVien"],
                        maKho: (String)reader["MaKho"],
                        nhaCungCap: (String)reader["NhaCungCap"],
                        ngayNhap: (DateTime)reader["NgayNhap"],
                        tongTien: (decimal)reader["TongTien"]
                        ));
                    danhSachPNSTable.ItemsSource = phieuNhapSachList;
                }
                connection.Close();
            }
            catch (Exception e1)
            {
                //MessageBox.Show("db error");
                MessageBox.Show(e1.Message);

            }
        }

        private void danhSachPNSTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void chiTietPNSTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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