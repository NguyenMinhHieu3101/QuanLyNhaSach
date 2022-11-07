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
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Nhân Viên",
                        SubMenuList = new List<SubMenuItemsData>
                        {
                            new SubMenuItemsData() {
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/list.png",
                                SubMenuText = "Thêm Nhân Viên",
                                Page = "ThemNhanVien"
                            },
                            new SubMenuItemsData() {
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/list.png",
                                SubMenuText = "Danh Sách Nhân Viên",
                                Page = "DanhSachNhanVien"
                            }
                        }
                    },

                    new MenuItemsData()
                    {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Khuyến Mãi",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData() {
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/import.png",
                                SubMenuText = "Thêm Khuyến Mãi",
                                Page = "ThemKhuyenMai"
                            },
                            new SubMenuItemsData() {
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/sell.png",
                                SubMenuText = "Danh Sách Khuyến Mãi",
                                Page = "DanhSachKhuyenMai"
                            }
                        }
                    },

                    new MenuItemsData() {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText ="Quản Lý Báo Cáo Doanh Thu",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/research.png",
                                SubMenuText = "Báo Cáo Doanh Thu",
                                Page = "BaoCaoDoanhThu"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/searching.png",
                                SubMenuText = "Biểu Đồ Doanh Thu",
                                Page = "BieuDoDoanhThu"
                            }
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Kho",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/inventory_report.png",
                                SubMenuText = "Thêm Phiếu Nhập Sách",
                                Page = "ThemPhieuNhapSach"

                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/analysis.png",
                                SubMenuText = "Báo Cáo Kho",
                                Page = "BaoCaoKho"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/analysis.png",
                                SubMenuText = "Biểu Đồ Kho Biến Động",
                                Page = "BieuDoKhoBienDong"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/analysis.png",
                                SubMenuText = "Danh Sách Phiếu Nhập Sách",
                                Page = "DanhSachPhieuNhapSach"
                            },
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Sách",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Danh Sách Sách Hiện Có",
                                Page = "DanhSachSachHienCo"
                            }
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Hóa Đơn",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Thêm Hóa Đơn",
                                Page = "ThemHoaDon"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Danh Sách Hóa Đơn",
                                Page = "DanhSachHoaDon"
                            }
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Khách Hàng",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Thêm Khách Hàng",
                                Page = "ThemKhachHang"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Danh Sách Khách Hàng",
                                Page = "DanhSachKhachHang"
                            },
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Hệ Thống",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Thay Đổi Quy Định",
                                Page = "ThayDoiQuyDinh"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Phân Quyền",
                                Page = "PhanQuyen"
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
