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
using System.Windows.Shapes;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for QuenMatKhau.xaml
    /// </summary>
    public partial class QuenMatKhau : Window
    {
        public QuenMatKhau()
        {
            InitializeComponent();
        }
        private void Confirm_Button(object sender, RoutedEventArgs e)
        {
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
            this.Hide();
        }
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
            this.Hide();
        }
    }
}
