using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class NhanVien
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Mã Nhân Viên")]
        public String maNhanVien { get; set; }

        [ColumnName("Họ Tên")]
        public String hoTen { get; set; }

        [ColumnName("Mã Chức Vụ")]
        public String maChucVu { get; set; }

        [ColumnName("Ngày Sinh")]
        public DateTime ngaySinh { get; set; }

        [ColumnName("CCCD")]
        public String cccd { get; set; }

        [ColumnName("Giới Tính")]
        public String gioiTinh { get; set; }
        [ColumnName("Email")]
        public String email { get; set; }

        [ColumnName("SĐT")]
        public String sdt { get; set; }

        [ColumnName("Địa Chỉ")]
        public String diaChi { get; set; }

        [ColumnName("Lương")]
        public double luong { get; set; }

        [ColumnName("Trạng Thái")]
        public String trangThai { get; set; }

        public NhanVien(string maNhanVien, string hoTen, string maChucVu, 
            DateTime ngaySinh, string cccd, string gioiTinh, string sdt, string email, 
            string diaChi, double luong, string trangThai)
        {
            this.maNhanVien = maNhanVien;
            this.hoTen = hoTen;
            this.maChucVu = maChucVu;
            this.ngaySinh = ngaySinh;
            this.cccd = cccd;
            this.gioiTinh = gioiTinh;
            this.sdt = sdt;
            this.email = email;
            this.diaChi = diaChi;
            this.luong = luong;
            this.trangThai = trangThai;
        }
        
        public NhanVien(int stt,string maNhanVien, string hoTen, string maChucVu, 
            DateTime ngaySinh, string cccd, string gioiTinh, string sdt, string email, 
            string diaChi, double luong, string trangThai)
        {
            this.stt = stt;
            this.maNhanVien = maNhanVien;
            this.hoTen = hoTen;
            this.maChucVu = maChucVu;
            this.ngaySinh = ngaySinh;
            this.cccd = cccd;
            this.gioiTinh = gioiTinh;
            this.sdt = sdt;
            this.email = email;
            this.diaChi = diaChi;
            this.luong = luong;
            this.trangThai = trangThai;
        }
    }
}
