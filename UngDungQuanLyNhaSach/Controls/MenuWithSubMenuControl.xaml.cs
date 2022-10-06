using System.Windows;
using System.Windows.Controls;
using UngDungQuanLyNhaSach.ViewModel;

namespace UngDungQuanLyNhaSach.Controls
{
    /// <summary>
    /// Interaction logic for MenuWithSubMenuControl.xaml
    /// </summary>
    public partial class MenuWithSubMenuControl : UserControl
    {
        public MenuWithSubMenuControl()
        {
            InitializeComponent();

            DataContext = new SideMenuViewModel();
        }

        public Thickness SubMenuPadding
        {
            get { return (Thickness)GetValue(SubMenuPaddingProperty); }
            set { SetValue(SubMenuPaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubMenuPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubMenuPaddingProperty =
            DependencyProperty.Register("MySubMenuPadding", typeof(Thickness), typeof(MenuWithSubMenuControl));



        public bool HasIcon
        {
            get { return (bool)GetValue(HasIconProperty); }
            set { SetValue(HasIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasIconProperty =
            DependencyProperty.Register("MyHasIcon", typeof(bool), typeof(MenuWithSubMenuControl));
    }
}
