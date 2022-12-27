using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class HoaDon
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Mã Hóa Đơn")]
        public String maHoaDon { get; set; }

        [ColumnName("Tên Nhân Viên")]
        public String maNhanVien
        {
            get
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from NHANVIEN WHERE MaNhanVien = '" + _maNhanVien + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string name = (String)reader["HoTen"];
                    connection.Close();
                    return name;
                }
                catch
                {
                    return _maNhanVien;
                }
            }
        }
        private String _maNhanVien { get; set; }

        [ColumnName("Tên Khách Hàng")]
        public String maKhachHang
        {
            get
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from KHACHHANG WHERE MaKhachHang = '" + _maKhachHang + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string name = (String)reader["TenKhachHang"];
                    connection.Close();
                    return name;
                }
                catch
                {
                    return _maKhachHang;
                }
            }
        }
        private String _maKhachHang { get; set; }

        [ColumnName("Ngày Lập Hóa Đơn")]
        public DateTime ngayLapHD { get; set; }

        [ColumnName("Tổng Tiền Hóa Đơn")]
        public String tongTienHD
        {
            get
            {
                String strMoney = _tongTienHD.ToString().Replace(".0000", "");
                return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", double.Parse(strMoney));
            }   
        }
        private Decimal _tongTienHD { get; set; }

        public HoaDon(string maHoaDon, string maNhanVien,
            string maKhachHang, DateTime ngayLapHD, decimal tongTienHD)
        {
            this.maHoaDon = maHoaDon;
            this._maNhanVien = maNhanVien;
            this._maKhachHang = maKhachHang;
            this.ngayLapHD = ngayLapHD;
            this._tongTienHD = tongTienHD;
        }

        public HoaDon(int stt,string maHoaDon, string maNhanVien,
            string maKhachHang, string maKhuyenMai, DateTime ngayLapHD, decimal tongTienHD)
        {
            this.stt = stt;
            this.maHoaDon = maHoaDon;
            this._maNhanVien = maNhanVien;
            this._maKhachHang = maKhachHang;
            this.ngayLapHD = ngayLapHD;
            this._tongTienHD = tongTienHD;
        }

        public String getMaNhanVien()
        {
            return _maNhanVien;
        }
        
        public String getMaKhachHang()
        {
            return _maKhachHang;
        }

        public decimal getTongTienHD()
        {
            return _tongTienHD;
        }
    }    
}
