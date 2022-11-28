using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UngDungQuanLyNhaSach.Model
{
    public class ThamSo
    {
      
        [ColumnName("Giá Trị")]
        public double giaTri { get; set; }


        [ColumnName("Tên Thuộc Tính")]
        public String tenThuocTinh { get; set; }

        [ColumnName("Mã Thuộc Tính")]
        public String maThuocTinh { get; set; }
        [ColumnName("STT")]
        public int stt { get; set; }

        public ThamSo(string maThuocTinh, string tenThuocTinh,double giaTri)
        {
            this.maThuocTinh = maThuocTinh;
            this.tenThuocTinh = tenThuocTinh;
            this.giaTri = giaTri;
        }

        public ThamSo(int stt, string maThuocTinh, string tenThuocTinh, double giaTri)
        {
            this.stt = stt;
            this.maThuocTinh = maThuocTinh;
            this.tenThuocTinh = tenThuocTinh;
            this.giaTri = giaTri;

        }
    }
}
