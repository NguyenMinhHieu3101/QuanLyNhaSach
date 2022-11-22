using System;
using System.Collections.Generic;
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

        [ColumnName("Mã Nhân Viên")]
        public String maNhanVien { get; set; }
        [ColumnName("Mã Khách Hàng")]
        public String maKhachHang { get; set; }
        [ColumnName("Mã Khuyến Mãi")]
        public String maKhuyenMai { get; set; }
        [ColumnName("Ngày Lập Hóa Đơn")]
        public DateTime ngayLapHD { get; set; }

        [ColumnName("Tổng Tiền Hóa Đơn")]
        public Decimal tongTienHD { get; set; }
        
        //[ColumnName("Người lập hóa đơn")]
        //public String nguoiLapHD { get; set; }

        public HoaDon(string maHoaDon, string maNhanVien,
            string maKhachHang, string maKhuyenMai, DateTime ngayLapHD, decimal tongTienHD)
        {
            this.maHoaDon = maHoaDon;
            this.maNhanVien = maNhanVien;
            this.maKhachHang = maKhachHang;
            this.maKhuyenMai = maKhuyenMai;
            this.ngayLapHD = ngayLapHD;
            this.tongTienHD = tongTienHD;
            //this.nguoiLapHD = nguoiLapHD;
        }
        public HoaDon(int stt,string maHoaDon, string maNhanVien,
            string maKhachHang, string maKhuyenMai, DateTime ngayLapHD, decimal tongTienHD)
        {
            this.stt = stt;
            this.maHoaDon = maHoaDon;
            this.maNhanVien = maNhanVien;
            this.maKhachHang = maKhachHang;
            this.maKhuyenMai = maKhuyenMai;
            this.ngayLapHD = ngayLapHD;
            this.tongTienHD = tongTienHD;
            //this.nguoiLapHD = nguoiLapHD;
        }
    }
    
}
