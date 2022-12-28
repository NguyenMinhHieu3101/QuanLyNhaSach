using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ThuNhap
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Thu nhập")]
        public String thuNhap { get; set; }

        [ColumnName("Số tiền")]
        public double soTien { get; set; }

        public ThuNhap(int stt, string thuNhap, double soTien)
        {
            this.stt = stt;
            this.thuNhap = thuNhap;
            this.soTien = soTien;
        }
    }
}
