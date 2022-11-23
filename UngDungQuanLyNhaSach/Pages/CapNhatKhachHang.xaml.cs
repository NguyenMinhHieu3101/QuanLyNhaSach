using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for CapNhatKhachHang.xaml
    /// </summary>
    public partial class CapNhatKhachHang : Page
    {
        public CapNhatKhachHang()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "ThemKhachHang", ".xaml"), UriKind.RelativeOrAbsolute));
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private void totalMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }
    }
}
