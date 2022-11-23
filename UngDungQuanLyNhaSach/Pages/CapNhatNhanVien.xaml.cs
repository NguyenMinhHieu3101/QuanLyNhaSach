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

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for CapNhatNhanVien.xaml
    /// </summary>
    public partial class CapNhatNhanVien : Page
    {
        public CapNhatNhanVien()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", "ThemNhanVien", ".xaml"), UriKind.RelativeOrAbsolute));
        }
    }
}
