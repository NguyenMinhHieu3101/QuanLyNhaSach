using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTietHoaDon
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
        public Decimal donGia { get; set; }
        [ColumnName("Thành Tiền")]
        public Decimal thanhTien { get; set; }


        public ChiTietHoaDon(string maSanPham, int soLuong, decimal donGia, decimal thanhTien)
        {
            this._maSanPham = maSanPham;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.thanhTien = thanhTien; 

        }

        public ChiTietHoaDon(int stt, string maSanPham, int soLuong, decimal donGia, decimal thanhTien)
        {
            this.stt = stt;
            this._maSanPham = maSanPham;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.thanhTien = thanhTien;
        }

        public String getMaSanPham()
        {
            return _maSanPham;
        }
    }

}



