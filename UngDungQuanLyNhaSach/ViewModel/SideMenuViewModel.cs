using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using UngDungQuanLyNhaSach.Pages;

namespace UngDungQuanLyNhaSach.ViewModel
{
    class SideMenuViewModel
    {
        //Our Source List for Menu Items
        public List<MenuItemsData> MenuList
        {
            get
            {
                return new List<MenuItemsData>
                {
                    new MenuItemsData()
                    {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/dashboard.png",
                        MenuText = "Quản Lý",
                        SubMenuList = new List<SubMenuItemsData>
                        {
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/book.png",
                                SubMenuText = "Sách",
                                Page = "Book"
                            },
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/books.png",
                                SubMenuText = "Dữ Liệu Sách",
                                Page = "BookData"
                            },
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/customer.png",
                                SubMenuText = "Khách Hàng",
                                Page = "Customer"
                            }
                        }
                    },

                    new MenuItemsData()
                    {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/business.png",
                        MenuText = "Kinh Doanh",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/import.png",
                                SubMenuText = "Nhập Sách",
                                Page = "ImportBook"
                            },
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/sell.png",
                                SubMenuText = "Bán Sách",
                                Page = "SellBook"
                            },
                            new SubMenuItemsData() {
                                PathData = "/UngDungQuanLyNhaSach;component/Images/collect_money.png",
                                SubMenuText = "Thu Tiền",
                                Page = "CollectMoney"
                            }
                        }
                    },

                    new MenuItemsData() {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/magnifying-glass.png",
                        MenuText ="Tìm Kiếm",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                PathData = "/UngDungQuanLyNhaSach;component/Images/research.png",
                                SubMenuText = "Tìm Kiếm Sách",
                                Page = "BookSearch"
                            },
                            new SubMenuItemsData(){
                                PathData = "/UngDungQuanLyNhaSach;component/Images/searching.png",
                                SubMenuText = "Tìm Kiếm Khách Hàng",
                                Page = "CustomerSearch"
                            }
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/report.png",
                        MenuText = "Báo cáo",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                PathData = "/UngDungQuanLyNhaSach;component/Images/inventory_report.png",
                                SubMenuText = "Báo Cáo Tồn",
                                Page = "InventoryReport"

                            },
                            new SubMenuItemsData(){
                                PathData = "/UngDungQuanLyNhaSach;component/Images/analysis.png",
                                SubMenuText = "Báo Cáo Công Nợ",
                                Page = "DebtReport"
                            }
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/gear.png",
                        MenuText = "Tùy Chỉnh",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Thay Đổi Quy Định",
                                Page = "RegulationChange"
                            }
                        }
                    }
                };
            }
        }
    }

    public class MenuItemsData
    {
        //Icon Data
        public string? PathData { get; set; }
        public string? MenuText { get; set; }
        public List<SubMenuItemsData>? SubMenuList { get; set; }
        public ICommand Command { get; }

        //To Add click event to our Buttons we will use ICommand here to switch pages when specific button is clicked
        public MenuItemsData()
        {
            Command = new CommandViewModel(Execute);
        }

        private void Execute()
        {
            //our logic comes here
            string MT = MenuText!.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(MT)) navigateToPage(MT);
        }

        private void navigateToPage(string Menu)
        {
            //We will search for our Main Window in open windows and then will access the frame inside it to set the navigation to desired page..
            //lets see how... ;)
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Home))
                {
                    //((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }

    public class SubMenuItemsData
    {
        public string? PathData { get; set; }
        public string? SubMenuText { get; set; }
        public ICommand SubMenuCommand { get; }
        public string? Page { get; set; }

        //To Add click event to our Buttons we will use ICommand here to switch pages when specific button is clicked
        public SubMenuItemsData()
        {
            SubMenuCommand = new CommandViewModel(Execute);
        }

        private void Execute()
        {
            //our logic comes here
            string SMT = Page!.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(SMT)) navigateToPage(SMT);
        }

        private void navigateToPage(string Menu)
        {
            //We will search for our Main Window in open windows and then will access the frame inside it to set the navigation to desired page..
            //lets see how... ;)
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Home))
                {
                   ((Home)window).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}
