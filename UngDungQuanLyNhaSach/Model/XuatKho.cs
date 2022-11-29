using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class XuatKho
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Xuất kho")]
        public string xuatKho { get; set; }

        [ColumnName("Số lượng")]
        public int soLuong { get; set; }

        public XuatKho(int stt, string xuatKho, int soLuong)
        {
            this.stt = stt;
            this.xuatKho = xuatKho;
            this.soLuong = soLuong;
        }
    }
}
