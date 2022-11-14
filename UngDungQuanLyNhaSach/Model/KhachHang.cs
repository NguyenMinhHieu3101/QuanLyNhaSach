using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class KhachHang
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Mã Khách Hàng")]
        public String maKhachHang { get; set; }

        [ColumnName("Tên Khách Hàng")]
        public String tenKhachHang { get; set; }

        [ColumnName("Địa Chỉ")]
        public String diaChi { get; set; }

        [ColumnName("Giới Tính")]
        public String gioiTinh { get; set; }

        [ColumnName("Loại Khách Hàng")]
        public String maLoaiKhachHang { get; set; }

        [ColumnName("SĐT")]
        public String sdt { get; set; }

        [ColumnName("Email")]
        public String email { get; set; }

        [ColumnName("Trạng Thái")]
        public String trangThai { get; set; }

        public KhachHang(string maKhachHang, string tenKhachHang, 
            string diaChi, string gioiTinh, string maLoaiKhachHang, 
            string sdt, string email, string trangThai)
        {
            this.maKhachHang = maKhachHang;
            this.tenKhachHang = tenKhachHang;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.maLoaiKhachHang = maLoaiKhachHang;
            this.sdt = sdt;
            this.email = email;
            this.trangThai = trangThai;
        }
        
        public KhachHang(int stt, string maKhachHang, string tenKhachHang, 
            string diaChi, string gioiTinh, string maLoaiKhachHang, 
            string sdt, string email, string trangThai)
        {
            this.stt = stt;
            this.maKhachHang = maKhachHang;
            this.tenKhachHang = tenKhachHang;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.maLoaiKhachHang = maLoaiKhachHang;
            this.sdt = sdt;
            this.email = email;
            this.trangThai = trangThai;
        }
    }
}
