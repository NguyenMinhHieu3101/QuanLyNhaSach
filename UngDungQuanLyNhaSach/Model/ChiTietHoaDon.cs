﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UngDungQuanLyNhaSach.Model
{
    public class ChiTietHoaDon
    {
        [ColumnName("#")]
        public int stt { get; set; }
        [ColumnName("Tên Sản Phẩm")]
        public String MaSanPham
        {
            get
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
                    connection.Open();
                    string readString = "select * from SANPHAM WHERE MaSanPham = '" + _maSanPham + "'";
                    SqlCommand command = new SqlCommand(readString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string name = (String)reader["TenSanPham"];
                    connection.Close();
                    return name;
                }
                catch
                {
                    return _maSanPham;
                }
            }
        }
        private String _maSanPham { get; set; }
        [ColumnName("Số Lượng")]
        public Int32 soLuong { get; set; }
        [ColumnName("Đơn Giá")]
        public String donGia
        {
            get
            {
                return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", _donGia);
            }
        }
        private Decimal _donGia { get; set; }

        [ColumnName("Thành Tiền")]
        public String thanhTien
        {
            get
            {
                return string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:C0}", _thanhTien);
            }
        }
        private Decimal _thanhTien { get; set; }


        public ChiTietHoaDon(string maSanPham, int soLuong, decimal donGia, decimal thanhTien)
        {
            this._maSanPham = maSanPham;
            this.soLuong = soLuong;
            this._donGia = donGia;
            this._thanhTien = thanhTien; 

        }

        public ChiTietHoaDon(int stt, string maSanPham, int soLuong, decimal donGia, decimal thanhTien)
        {
            this.stt = stt;
            this._maSanPham = maSanPham;
            this.soLuong = soLuong;
            this._donGia = donGia;
            this._thanhTien = thanhTien;
        }

        public String getMaSanPham()
        {
            return _maSanPham;
        }
        
        public Decimal getDonGia()
        {
            return _donGia;
        }
        
        public Decimal getThanhTien()
        {
            return _thanhTien;
        }
    }

}



