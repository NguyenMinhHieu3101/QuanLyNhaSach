using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for BaoCaoPdf.xaml
    /// </summary>
    public partial class BaoCaoPdf : Page
    {
        public BaoCaoPdf()
        {
            InitializeComponent();     
            BaoCaoDoanhThu baoCaoDoanhThu = new BaoCaoDoanhThu();
            baoCaoDoanhThu.RaiseCustomEvent += BaoCaoDoanhThu_RaiseCustomEvent;
        }

        private void BaoCaoDoanhThu_RaiseCustomEvent(object? sender, BaoCaoDoanhThu.CustomEventArgs e)
        {
            //throw new NotImplementedException();
            Date.Text = DateTime.Now.ToString();
            Author.Text = NhanVienDangDangNhap.HoTen;

        }
    }
}
