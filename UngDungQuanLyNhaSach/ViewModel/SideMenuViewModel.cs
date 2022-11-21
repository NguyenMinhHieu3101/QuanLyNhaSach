﻿using System;
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
                                SubMenuText = "Tra Cứu Nhân Viên",
                                Page = "TraCuuNhanVien"
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
                                SubMenuText = "Tra Cứu Khuyến Mãi",
                                Page = "TraCuuKhuyenMai"
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
                                SubMenuText = "Tra Cứu Khách Hàng",
                                Page = "TraCuuKhachHang"
                            },
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
                                SubMenuText = "Tra Cưu Phiếu Nhập Sách",
                                Page = "TraCuuPhieuNhapSach"
                            },
                        }
                    },

                    new MenuItemsData(){
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText = "Quản Lý Sản Phẩm",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/settings.png",
                                SubMenuText = "Tra Cứu Sản Phẩm",
                                Page = "TraCuuSanPham"
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
                                SubMenuText = "Tra Cứu Hóa Đơn",
                                Page = "TraCuuHoaDon"
                            }
                        }
                    },

                    new MenuItemsData() {
                        PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                        MenuText ="Quản Lý Báo Cáo",
                        SubMenuList = new List<SubMenuItemsData>{
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/research.png",
                                SubMenuText = "Báo Cáo Doanh Thu",
                                Page = "BaoCaoDoanhThu"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/analysis.png",
                                SubMenuText = "Báo Cáo Kho",
                                Page = "BaoCaoKho"
                            },
                            new SubMenuItemsData(){
                                //PathData = "/UngDungQuanLyNhaSach;component/Images/searching.png",
                                SubMenuText = "Báo Cáo Sản Phẩm",
                                Page = "BaoCaoSanPham"
                            }
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
