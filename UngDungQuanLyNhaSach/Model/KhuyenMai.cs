using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class KhuyenMai
    {
        [ColumnName("STT")]
        public int stt { get; set; }

        [ColumnName("Mã Khuyến Mãi")]
        public String maKhuyenMai { get; set; }

        [ColumnName("Bắt Đầu")]
        public DateTime batDau { get; set; }

        [ColumnName("Kết Thúc")]
        public DateTime ketThuc { get; set; }

        [ColumnName("Loại Khách Hàng")]
        public String maLoaiKhachHang { get; set; }

        [ColumnName("Số Lượng")]
        public int soLuong { get; set; }

        [ColumnName("Trạng Thái")]
        public String trangThai { get; set; }

        public KhuyenMai(string maKhuyenMai, DateTime batDau, DateTime ketThuc, 
            string maLoaiKhachHang, int soLuong, string trangThai)
        {
            this.maKhuyenMai = maKhuyenMai;
            this.batDau = batDau;
            this.ketThuc = ketThuc;
            this.maLoaiKhachHang = maLoaiKhachHang;
            this.soLuong = soLuong;
            this.trangThai = trangThai;
        }
        
        public KhuyenMai(int stt, string maKhuyenMai, DateTime batDau, DateTime ketThuc, 
            string maLoaiKhachHang, int soLuong, string trangThai)
        {
            this.stt = stt;
            this.maKhuyenMai = maKhuyenMai;
            this.batDau = batDau;
            this.ketThuc = ketThuc;
            this.maLoaiKhachHang = maLoaiKhachHang;
            this.soLuong = soLuong;
            this.trangThai = trangThai;
        }
    }
}
