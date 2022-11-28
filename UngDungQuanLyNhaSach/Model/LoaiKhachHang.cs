using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class LoaiKhachHang
    {
       
     

            [ColumnName("Tiền Tối Thiểu")]
            public double tienToiThieu { get; set; }


            [ColumnName("Tên Loại Khách Hàng")]
            public String tenLoaiKhachHang { get; set; }

            [ColumnName("Mã Loại Khách Hàng")]
            public String maLoaiKhachHang { get; set; }
            [ColumnName("STT")]
            public int stt { get; set; }

            public LoaiKhachHang(string maThuocTinh, string tenThuocTinh, double giaTri)
            {
                this.maLoaiKhachHang = maThuocTinh;
                this.tenLoaiKhachHang = tenThuocTinh;
                this.tienToiThieu = giaTri;
            }

            public LoaiKhachHang(int stt, string maThuocTinh, string tenThuocTinh, double giaTri)
            {
                this.stt = stt;
                this.maLoaiKhachHang = maThuocTinh;
                this.tenLoaiKhachHang = tenThuocTinh;
                this.tienToiThieu = giaTri;
            }
        
    }
}
