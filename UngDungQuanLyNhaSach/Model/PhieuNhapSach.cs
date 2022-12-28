using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class PhieuNhapSach
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Mã Phiếu Nhập")]
        public String maPhieuNhap { get; set; }

        [ColumnName("Mã Nhân Viên")]
        public String maNhanVien { get; set; }
        [ColumnName("Mã Kho")]
        public String maKho { get; set; }
        [ColumnName("Nhà Cung Cấp")]
        public String nhaCungCap { get; set; }
        [ColumnName("Ngày Nhập")]
        public DateTime ngayNhap { get; set; }

        [ColumnName("Tổng Tiền")]
        public double tongTien { get; set; }

        //[ColumnName("Người lập hóa đơn")]
        //public String nguoiLapHD { get; set; }

        public PhieuNhapSach(string maPhieuNhap, string maNhanVien,
            string maKho, string nhaCungCap, DateTime ngayNhap, double tongTien)
        {
            this.maPhieuNhap = maPhieuNhap;
            this.maNhanVien = maNhanVien;
            this.maKho = maKho;
            this.nhaCungCap = nhaCungCap;
            this.ngayNhap = ngayNhap;
            this.tongTien = tongTien;
        }
        public PhieuNhapSach(int stt,string maPhieuNhap, string maNhanVien,
            string maKho, string nhaCungCap, DateTime ngayNhap, double tongTien)
        {
            this.stt = stt;
            this.maPhieuNhap = maPhieuNhap;
            this.maNhanVien = maNhanVien;
            this.maKho = maKho;
            this.nhaCungCap = nhaCungCap;
            this.ngayNhap = ngayNhap;
            this.tongTien = tongTien;
        }
    }

}

