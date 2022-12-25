using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTietHoaDon
    {
        [ColumnName("#")]
        public int stt { get; set; }
        [ColumnName("Mã Hóa Đơn")]
        public String maHoaDon { get; set; }

        [ColumnName("Mã Sản Phẩm")]
        public String maSanPham { get; set; }

        [ColumnName("Số Lượng")]
        public Int32 soLuong { get; set; }
        [ColumnName("Đơn Giá")]
        public Decimal donGia { get; set; }
        [ColumnName("Giảm Giá")]
        public float giamGia { get; set; }
        [ColumnName("Thành Tiền")]
        public Decimal thanhTien { get; set; }



        public ChiTietHoaDon(string maHoaDon, string maSanPham, int soLuong, decimal donGia, float giamGia, decimal thanhTien)
        {
            this.maHoaDon = maHoaDon;
            this.maSanPham = maSanPham;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.giamGia = giamGia; 
            this.thanhTien = thanhTien; 

        }
        public ChiTietHoaDon(int stt, string maHoaDon, string maSanPham, int soLuong, decimal donGia, float giamGia, decimal thanhTien)
        {
            this.stt = stt;
            this.maHoaDon = maHoaDon;
            this.maSanPham = maSanPham;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.giamGia = giamGia;
            this.thanhTien = thanhTien;

        }
    }

}



