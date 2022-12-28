using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTietPhieuNhapSach
    {
        [ColumnName("#")]
        public int stt { get; set; }
        [ColumnName("Tên Sản Phẩm")]
        public String MaSanPham
        {
            get
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from SANPHAM WHERE MaSanPham = '" + _maSanPham + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string name = (String)reader["TenSanPham"];
                    connection.Close();
                    return name;
                }
                catch
                {
                    return _maSanPham;
                }
            }
        }
        private String _maSanPham { get; set; }
        [ColumnName("Số Lượng")]
        public Int32 soLuong { get; set; }
        [ColumnName("Đơn Giá")]
        public String donGia
        {
            get
            {
                return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", _donGia);
            }
        }
        private double _donGia { get; set; }

        [ColumnName("Thành Tiền")]
        public String thanhTien
        {
            get
            {
                return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", _thanhTien);
            }
        }
        private double _thanhTien { get; set; }


        public ChiTietPhieuNhapSach(string maSanPham, int soLuong, double donGia, double thanhTien)
        {
            _maSanPham = maSanPham;
            this.soLuong = soLuong;
            _donGia = donGia;
            _thanhTien = thanhTien;
        }

        public ChiTietPhieuNhapSach(int stt, string maSanPham, int soLuong, double donGia, double thanhTien)
        {
            this.stt = stt;
            _maSanPham = maSanPham;
            this.soLuong = soLuong;
            _donGia = donGia;
            _thanhTien = thanhTien;
        }

        public String getMaSanPham()
        {
            return _maSanPham;
        }

        public double getDonGia()
        {
            return _donGia;
        }

        public double getThanhTien()
        {
            return _thanhTien;
        }

        public void setThanhTien(double value)
        {
            this._thanhTien = value;
        }

    }
}




