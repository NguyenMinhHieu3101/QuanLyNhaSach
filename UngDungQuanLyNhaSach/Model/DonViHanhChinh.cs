using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UngDungQuanLyNhaSach.Model
{
    public class DonViHanhChinh
    {
        public String code { get; set; }
        public String name { get; set; }

        public DonViHanhChinh(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
    }
}
