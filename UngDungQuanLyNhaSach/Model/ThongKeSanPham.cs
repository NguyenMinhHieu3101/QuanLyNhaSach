using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class ThongKeSanPham
    {
        [ColumnName("#")]
        public int stt { get; set; }

        [ColumnName("Tên sản phẩm")]
        public String tenSP { get; set; }

        [ColumnName("Số lượng bán")]
        public int soLuongBan { get; set; }

        [ColumnName("Số lượng tồn")]
        public int soLuongTon { get; set; }

        public ThongKeSanPham(int stt, string tenSP, int soLuongBan, int soLuongTon)
        {
            this.stt = stt;
            this.tenSP = tenSP; 
            this.soLuongBan = soLuongBan;
            this.soLuongTon = soLuongTon;
        }
    }
}
