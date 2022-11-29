using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class NhapKho
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Nhập kho")]
        public string maPhieuNhap { get; set; }

        [ColumnName("Số lượng")]
        public int soLuongSP { get; set; }

        public NhapKho(int stt, string maPhieuNhap, int soLuongSP)
        {
            this.stt = stt;
            this.maPhieuNhap = maPhieuNhap;
            this.soLuongSP = soLuongSP;
        }
    }
}
