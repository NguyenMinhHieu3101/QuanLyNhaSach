using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class DoanhThu
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Chi Trả")]
        public String chiTra { get; set; }

        [ColumnName("Số Tiền")]
        public decimal soTien { get; set; }

        public DoanhThu(int stt, string chiTra, decimal soTien)
        {
            this.stt = stt;
            this.chiTra = chiTra;
            this.soTien = soTien;
        }
    }
}
