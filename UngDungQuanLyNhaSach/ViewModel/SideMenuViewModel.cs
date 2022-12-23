using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using UngDungQuanLyNhaSach.Pages;
using UngDungQuanLyNhaSach.Model;
using System.Data.SqlClient;
using System.Data;

namespace UngDungQuanLyNhaSach.ViewModel
{
    class SideMenuViewModel
    {
        //Our Source List for Menu Items
        public List<MenuItemsData> MenuList
        {
            get
            {
                if (NhanVienDangDangNhap.MaChucVu == "ADMIN")
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

                                SubMenuText = "Thêm Nhân Viên",
                                Page = "ThemNhanVien"
                            },
                            new SubMenuItemsData() {

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

                                SubMenuText = "Thêm Khuyến Mãi",
                                Page = "ThemKhuyenMai"
                            },
                            new SubMenuItemsData() {

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

                                SubMenuText = "Thêm Khách Hàng",
                                Page = "ThemKhachHang"
                            },
                            new SubMenuItemsData(){

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
                                  //SubMenuText = "Thêm Phiếu Nhập Sách",
                                  SubMenuText = "Thêm Phiếu Nhập Sản Phẩm",
                                Page = "ThemPhieuNhapSach"

                            },
                            new SubMenuItemsData(){

                                //SubMenuText = "Tra Cứu Phiếu Nhập Sách",
                                SubMenuText = "Tra Cứu Phiếu Nhập Sản Phẩm",
                                Page = "TraCuuPhieuNhapSach"
                            },
                            new SubMenuItemsData(){

                                SubMenuText = "Tra Cứu Sản Phẩm",
                                Page = "TraCuuSanPham"
                            },
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
                else
                {
                    List<Model.PhanQuyen> phanquyenlist = new List<Model.PhanQuyen>();
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

                    connection.Open();
                    string readString = "SELECT * FROM PHANQUYEN WHERE MaChucVu = '" +NhanVienDangDangNhap.MaChucVu +"'";
                    SqlCommand command = new SqlCommand(readString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                        phanquyenlist.Add(new Model.PhanQuyen(maChucVu: (string)reader["MaChucVu"], maQuyen: (string)reader["Maquyen"]));
                    }

                    connection.Close();
                 
                    MenuItemsData dataBaoCao = new MenuItemsData()  {
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
                                    
                                     };
                    MenuItemsData dataKho = new MenuItemsData()
                                        {
                                            PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                                            MenuText = "Quản Lý Kho",
                                            SubMenuList = new List<SubMenuItemsData>{
                                                                new SubMenuItemsData(){
                                                                    SubMenuText = "Thêm Phiếu Nhập Sách",
                                                                    Page = "ThemPhieuNhapSach"

                                                                },
                                                                new SubMenuItemsData(){

                                                                    SubMenuText = "Tra Cứu Phiếu Nhập Sách",
                                                                    Page = "TraCuuPhieuNhapSach"
                                                                },
                                                            }
                                        };

                    MenuItemsData dataHoaDon = new MenuItemsData()
                                        {
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
                                        };

                    MenuItemsData dataKhachHang = new MenuItemsData()
                                        {
                                            PathData = "/UngDungQuanLyNhaSach;component/Images/drop-down-arrow.png",
                                            MenuText = "Quản Lý Khách Hàng",
                                            SubMenuList = new List<SubMenuItemsData>{
                                                                new SubMenuItemsData(){

                                                                    SubMenuText = "Thêm Khách Hàng",
                                                                    Page = "ThemKhachHang"
                                                                },
                                                                new SubMenuItemsData(){

                                                                    SubMenuText = "Tra Cứu Khách Hàng",
                                                                    Page = "TraCuuKhachHang"
                                                                },
                                                            }
                                        };

                    List<MenuItemsData> dataList = new List<MenuItemsData>() { dataBaoCao, dataKho, dataHoaDon, dataKhachHang };
                    List<MenuItemsData> dataListShow = new List<MenuItemsData>();
                   
                
                        if (phanquyenlist.Count > 0)
                            for (int k = 0; k < phanquyenlist.Count; k++)
                            {
                                dataListShow.Add(dataList[int.Parse(phanquyenlist[k].MaQuyen) - 1]);
                            }
                        else
                            return null;
         
                    return dataListShow;
                }    
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
