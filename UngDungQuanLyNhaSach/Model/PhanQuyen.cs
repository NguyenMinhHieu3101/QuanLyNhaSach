using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class PhanQuyen
    {
        private string maChucVu;
        public string MaChucVu
        {
            get { return maChucVu; }
            set { maChucVu = value; }
        }
        private string tenChucVu;
        public string TenChucVu
        {
            get { return tenChucVu; }
            set { tenChucVu = value; }
        }
        private string maQuyen;
        public string MaQuyen
        {
            get { return maQuyen; }
            set { maQuyen = value; }
        }
        private string tenQuyen;
        public string TenQuyen
        {
            get { return tenQuyen; }
            set { tenQuyen = value; }
        }
    }
}
