using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTra
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Chi trả")]
        public String chiTra { get; set; }

        [ColumnName("Số tiền")]
        public String soTien { get; set; }

        public ChiTra(int stt, string chiTra, String soTien)
        {
            this.stt = stt;
            this.chiTra = chiTra;
            this.soTien = soTien;
        }
    }
}
