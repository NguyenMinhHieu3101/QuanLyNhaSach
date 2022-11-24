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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        bool check = false;

        public Home()
        {
            InitializeComponent();
            MainWindowFrame.Navigate(new Wellcome());           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!check) System.Windows.Application.Current.Shutdown();
        }

        private void MainWindowFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (info.SelectedIndex == 1)
            {
                check = true;
                DangNhap dangNhap = new DangNhap();
                dangNhap.Show();
                this.Close();
            }
            else
            {

            }    
        }
    }
}
