using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTietPhieuNhapSach
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Mã Sản Phẩm")]
        public String maSanPham { get; set; }

        [ColumnName("Tên Sản Phẩm")]
        public String tenSanPham { get; set; }
        [ColumnName("Tác Giả")]
        public String tacGia { get; set; }
        [ColumnName("Thể Loại")]
        public String theLoai { get; set; }
        [ColumnName("NXB")]
        public String nXB { get; set; }

        [ColumnName("Giá Nhập")]
        public Decimal giaNhap { get; set; }
        [ColumnName("Năm XB")]
        public Int32 namXB { get; set; }
        [ColumnName("Mã Kho")]
        public String maKho { get; set; }


        public ChiTietPhieuNhapSach(string maSanPham, string tenSanPham, string theLoai,
            string tacGia, string nXB, decimal giaNhap, int namXB, string maKho)
        {
            this.maSanPham = maSanPham;
            this.tenSanPham = tenSanPham;
            this.tacGia = tacGia;
            this.theLoai = theLoai;
            this.nXB = nXB;
            this.giaNhap = giaNhap;
            this.namXB = namXB;
            this.maKho = maKho;

        }
        public ChiTietPhieuNhapSach(int stt, string maSanPham, string tenSanPham, string theLoai,
            string tacGia, string nXB, decimal giaNhap, int namXB, string maKho)
        {
            this.stt = stt;
            this.maSanPham = maSanPham;
            this.tenSanPham = tenSanPham;
            this.tacGia = tacGia;
            this.theLoai = theLoai;
            this.nXB = nXB;
            this.giaNhap = giaNhap;
            this.namXB = namXB;
            this.maKho = maKho;

        }
    }

}


