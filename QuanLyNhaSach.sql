DROP DATABASE QUANLYNHASACH
GO
CREATE DATABASE QUANLYNHASACH
GO
USE QUANLYNHASACH
GO

CREATE TABLE NHANVIEN
(
	MaNhanVien VARCHAR(20) CONSTRAINT PK_MANV PRIMARY KEY,
	MatKhau VARCHAR(100),
	HoTen NVARCHAR(100),
	MaChucVu VARCHAR(20),
	NgaySinh SMALLDATETIME,
	CCCD VARCHAR(15),
	Email VARCHAR(100),
	GioiTinh NVARCHAR(3),
	SDT VARCHAR(20),
	DiaChi NVARCHAR(100),
	Luong MONEY,
	TrangThai VARCHAR(1),
	ThoiGianLayOTP  SMALLDATETIME DEFAULT NULL,
	OTP VARCHAR(100) DEFAULT NULL
)

CREATE TABLE PHANQUYEN
(
	MaChucVu VARCHAR(20) NOT NULL,
	MaQuyen VARCHAR(20) NOT NULL
)

CREATE TABLE QUYEN
(
	MaQuyen VARCHAR(20) CONSTRAINT PK_MAQUYEN PRIMARY KEY,
	TenQuyen NVARCHAR(100)
)

CREATE TABLE CHUCVU
(
	MaChucVu VARCHAR(20) CONSTRAINT PK_MACV PRIMARY KEY,
	TenChucVu NVARCHAR(20)
)
CREATE TABLE HOADON
(
	MaHoaDon VARCHAR(20) CONSTRAINT PK_MAHD PRIMARY KEY,
	MaNhanVien VARCHAR(20),
	MaKhachHang VARCHAR(20),
	MaKhuyenMai VARCHAR(20),
	NgayLapHoaDon SMALLDATETIME,
	TongTienHoaDon MONEY
)
CREATE TABLE CHITIETHOADON
(
	MaHoaDon VARCHAR(20) NOT NULL,
	MaSanPham VARCHAR(20) NOT NULL,
	DichVuGoiQua VARCHAR(1),
	SoLuong INT	
)
CREATE TABLE KHACHHANG
(
	MaKhachHang VARCHAR(20) CONSTRAINT PK_MAKH PRIMARY KEY,
	TenKhachHang NVARCHAR(100),
	GioiTinh NVARCHAR(3),
	MaLoaiKhachHang VARCHAR(20),
	SDT VARCHAR(20),
	TrangThai VARCHAR(1)
)
CREATE TABLE LOAIKHACHHANG
(
	MaLoaiKhachHang VARCHAR(20) CONSTRAINT PK_MALKH PRIMARY KEY,
	TenLoaiKhachHang NVARCHAR(20),
	TienToiThieu FLOAT
)
CREATE TABLE KHUYENMAI
(
	MaKhuyenMai VARCHAR(20) CONSTRAINT PK_MAKM PRIMARY KEY,
	ThoiGianBatDau SMALLDATETIME,
	ThoiGianKetThuc SMALLDATETIME,
	MaLoaiKhachHang VARCHAR(20),
	SoLuongKhuyenMai INT,
	PhanTram INT,
	TrangThai VARCHAR(1)
)
CREATE TABLE PHIEUNHAP
(
	MaPhieuNhap VARCHAR(20) CONSTRAINT PK_MAPN PRIMARY KEY,
	MaNhanVien VARCHAR(20),
	MaKho VARCHAR(20),
	NhaCungCap NVARCHAR(100),
	NgayNhap SMALLDATETIME,
	TongTien MONEY    
)
CREATE TABLE CHITIETPHIEUNHAP
(
	MaPhieuNhap VARCHAR(20) NOT NULL,
	MaSanPham VARCHAR(20) NOT NULL,
	SoLuong INT,
	DonGia MONEY
)
CREATE TABLE CHITIETBAOCAODOANHTHU
(
	MaChiTietBaoCao VARCHAR(20) CONSTRAINT PK_MACTBCDT PRIMARY KEY,
	TuNgay SMALLDATETIME  NOT NULL,
	DenNgay SMALLDATETIME  NOT NULL,
	MaNVBC VARCHAR(20),
	DoanhThu MONEY,
	ChiPhi MONEY
)
CREATE TABLE CHITIETBAOCAOKHO
(
	MaChiTietBaoCao VARCHAR(20) CONSTRAINT PK_MACTBCK PRIMARY KEY,
	TuNgay SMALLDATETIME  NOT NULL,
	DenNgay SMALLDATETIME  NOT NULL,
	MaNVBC VARCHAR(20),
	MaKho VARCHAR(20)
)
CREATE TABLE CHITIETBAOCAOSANPHAM
(
	MaChiTietBaoCao VARCHAR(20) NOT NULL ,
	MaSanPham VARCHAR(20) NOT NULL,
	MaNVBC VARCHAR(20),
	TuNgay SMALLDATETIME NOT NULL,
	DenNgay SMALLDATETIME NOT NULL,
	SoLuongDaBan INT
)
CREATE TABLE KHO
(
	MaKho VARCHAR(20) CONSTRAINT PK_MAK PRIMARY KEY,
	TenKho NVARCHAR(20),
	SucChua INT,
	DaChua INT
)
CREATE TABLE SANPHAM
(
	MaSanPham VARCHAR(20) CONSTRAINT PK_MASP PRIMARY KEY,
	TenSanPham NVARCHAR(100),
	TacGia NVARCHAR(100),
	TheLoai NVARCHAR(100),
	NXB NVARCHAR(100),
	GiaNhap MONEY,
	NamXB INT,
	MaKho VARCHAR(20),
	TrangThai VARCHAR(1)
)


CREATE TABLE THAMSO
(
	MaThuocTinh VARCHAR(20) CONSTRAINT PK_MATT PRIMARY KEY,
	TenThuocTinh NVARCHAR(100),
	GiaTri FLOAT
)


SET DATEFORMAT DMY


INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT01', '01/01/2022', '02/01/2022', 'NV01', '2500000', '750000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT02', '02/01/2022', '03/01/2022', 'NV02', '2800000', '1750000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT03', '03/01/2022', '04/01/2022', 'NV03', '2500000', '1350000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT04', '04/01/2022', '05/01/2022', 'NV04', '2500000', '1750000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT05', '05/01/2022', '20/01/2022', 'NV04', '2500000', '1950000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT06', '06/01/2022', '21/01/2022', 'NV04', '3000000', '1250000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT07', '07/01/2022', '22/01/2022', 'NV04', '3250000', '850000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT08', '08/01/2022', '23/01/2022', 'NV05', '2860000', '770000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT09', '09/01/2022', '24/01/2022', 'NV06', '3220000', '1250000');
INSERT INTO CHITIETBAOCAODOANHTHU (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, DoanhThu, ChiPhi) VALUES ('DT10', '10/01/2022', '25/01/2022', 'NV07', '4000000', '950000');

INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK01', '01/01/2022', '02/01/2022', 'NV01', 'K01');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK02', '07/01/2022', '08/01/2022', 'NV02', 'K02');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK03', '08/01/2022', '09/01/2022', 'NV03', 'K03');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK04', '09/01/2022', '10/01/2022', 'NV04', 'K04');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK05', '10/01/2022', '20/01/2022', 'NV04', 'K03');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK06', '11/01/2022', '21/01/2022', 'NV04', 'K04');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK07', '12/01/2022', '22/01/2022', 'NV04', 'K01');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK08', '13/01/2022', '23/01/2022', 'NV05', 'K03');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK09', '14/01/2022', '24/01/2022', 'NV06', 'K03');
INSERT INTO CHITIETBAOCAOKHO (MaChiTietBaoCao, TuNgay, DenNgay, MaNVBC, MaKho) VALUES ('BCK10', '15/01/2022', '25/01/2022', 'NV07', 'K04');

INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP01', 'SP01', 'NV08', '01/01/2022', '02/01/2022', '25');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP01', 'SP02', 'NV09', '07/01/2022', '08/01/2022', '26');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP03', 'SP03', 'NV09', '08/01/2022', '09/01/2022', '27');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP04', 'SP04', 'NV09', '09/01/2022', '10/01/2022', '28');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP04', 'SP05', 'NV09', '10/01/2022', '20/01/2022', '32');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP04', 'SP06', 'NV02', '11/01/2022', '21/01/2022', '33');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP07', 'SP07', 'NV03', '12/01/2022', '22/01/2022', '34');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP08', 'SP08', 'NV04', '13/01/2022', '23/01/2022', '17');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP09', 'SP09', 'NV05', '14/01/2022', '24/01/2022', '18');
INSERT INTO CHITIETBAOCAOSANPHAM (MaChiTietBaoCao, MaSanPham, MaNVBC, TuNgay, DenNgay, SoLuongDaBan) VALUES ('BCSP10', 'SP10', 'NV06', '15/01/2022', '25/01/2022', '19');



INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD01', 'SP01', '1', '3');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD02', 'SP02', '1', '3');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD03', 'SP03', '0', '2');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD04', 'SP04', '1', '4');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD05', 'SP05', '0', '8');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD05', 'SP06', '0', '5');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD03', 'SP07', '1', '7');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD08', 'SP08', '1', '5');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD09', 'SP09', '0', '4');
INSERT INTO CHITIETHOADON (MaHoaDon, MaSanPham, DichVuGoiQua, SoLuong) VALUES ('HD10', 'SP10', '1', '3');

INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD01', 'NV04', 'KH01', 'KM01', '02/01/2022', '125000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD02', 'NV06', 'KH02', 'KM02', '08/01/2022', '300000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD03', 'NV07', 'KH03', 'KM09', '09/01/2022', '50000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD04', 'NV08', 'KH04', 'KM04', '10/01/2022', '50000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD05', 'NV09', 'KH05', 'KM05', '20/01/2022', '23000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD06', 'NV04', 'KH06', 'KM06', '21/01/2022', '23000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD07', 'NV06', 'KH07', 'KM06', '22/01/2022', '23000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD08', 'NV07', 'KH08', 'KM08', '23/01/2022', '20000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD09', 'NV08', 'KH09', 'KM08', '24/01/2022', '200000');
INSERT INTO HOADON (MaHoaDon, MaNhanVien, MaKhachHang, MaKhuyenMai, NgayLapHoaDon, TongTienHoaDon) VALUES ('HD10', 'NV09', 'KH10', 'KM10', '25/01/2022', '245000');

INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN01', 'SP03', '100', '1500000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN01', 'SP04', '150', '97500000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN01', 'SP05', '200', '3400000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN01', 'SP06', '300', '48000000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN01', 'SP07', '500', '10500000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN02', 'SP08', '400', '2000000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN02', 'SP09', '350', '1750000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN02', 'SP10', '500', '10000000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN02', 'SP11', '500', '9500000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN02', 'SP12', '500', '27500000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN03', 'SP13', '1000', '2000000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN03', 'SP14', '550', '2200000');
INSERT INTO CHITIETPHIEUNHAP (MaPhieuNhap, MaSanPham, SoLuong, DonGia) VALUES ('PN03', 'SP15', '450', '6750000');

INSERT INTO CHUCVU (MaChucVu, TenChucVu) VALUES ('ADMIN', 'Admin');
INSERT INTO CHUCVU (MaChucVu, TenChucVu) VALUES ('NVK', 'Nhân viên kho');
INSERT INTO CHUCVU (MaChucVu, TenChucVu) VALUES ('NVBH', 'Nhân viên bán hàng');
INSERT INTO CHUCVU (MaChucVu, TenChucVu) VALUES ('NVKT', 'Nhân viên kế toán');

INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH01', N'Nguyễn Ngọc Trinh', N'Nữ', 'VL', '0354161123', '1');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH02', N'Huỳnh Thế Vĩ', N'Nam', 'B', '0354161124', '1');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH03', N'Bùi Lê Hoài An', N'Nữ', 'V', '0354161125', '0');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH04', N'Phạm Nhật Minh', N'Nữ', 'KC', '0354161126', '0');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH05', N'Phan Xuân Quang', N'Nam', 'VL', '0354161127', '0');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH06', N'Tôn Nữ Hoài Thương', N'Nam', 'B', '0354161128', '1');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH07', N'Nguyễn Viết Đức', N'Nữ', 'V', '0354161129', '0');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH08', N'Dín Hiền Dũng', N'Nữ', 'KC', '0354161130', '0');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH09', N'Lê Sỹ Hội', N'Nữ', 'VL', '0354161131', '1');
INSERT INTO KHACHHANG (MaKhachHang, TenKhachHang, GioiTinh, MaLoaiKhachHang, SDT, TrangThai) VALUES ('KH10', N'Ngô Quang Khoa', N'Nữ', 'B', '0354161132', '1');

INSERT INTO KHO (MaKho, TenKho, SucChua, DaChua) VALUES ('K01', N'Quận 1', '10000', '1250');
INSERT INTO KHO (MaKho, TenKho, SucChua, DaChua) VALUES ('K02', N'Quận 9', '20000', '2250');
INSERT INTO KHO (MaKho, TenKho, SucChua, DaChua) VALUES ('K03', N'Quận Bình Thạnh', '5000', '3250');
INSERT INTO KHO (MaKho, TenKho, SucChua, DaChua) VALUES ('K04', N'Quận Gò Vấp', '10000', '2400');

INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM01', '01/01/2022', '02/01/2022', 'B', '100', '10', '1');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM02', '07/01/2022', '08/01/2022', 'VL', '150', '5', '1');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM03', '08/01/2022', '09/01/2022', 'KC', '50', '15', '0');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM04', '09/01/2022', '10/01/2022', 'V', '50', '20', '1');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM05', '19/01/2022', '20/01/2022', 'B', '100', '30', '0');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM06', '20/01/2022', '21/01/2022', 'KC', '50', '10', '1');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM07', '21/01/2022', '22/01/2022', 'VL', '150', '15', '1');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM08', '22/01/2022', '23/01/2022', 'KC', '50', '5', '0');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM09', '23/01/2022', '24/01/2022', 'B', '150', '15', '0');
INSERT INTO KHUYENMAI (MaKhuyenMai, ThoiGianBatDau, ThoiGianKetThuc, MaLoaiKhachHang, SoLuongKhuyenMai, PhanTram, TrangThai) VALUES ('KM10', '24/01/2022', '25/01/2022', 'V', '50', '10', '0');

INSERT INTO LOAIKHACHHANG (MaLoaiKhachHang, TenLoaiKhachHang, TienToiThieu) VALUES ('VL', N'Vãng Lai', '0');
INSERT INTO LOAIKHACHHANG (MaLoaiKhachHang, TenLoaiKhachHang, TienToiThieu) VALUES ('B', N'Bạc', '5000000');
INSERT INTO LOAIKHACHHANG (MaLoaiKhachHang, TenLoaiKhachHang, TienToiThieu) VALUES ('V', N'Vàng', '15000000');
INSERT INTO LOAIKHACHHANG (MaLoaiKhachHang, TenLoaiKhachHang, TienToiThieu) VALUES ('KC', N'Kim Cương', '30000000');

INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV01', '123456', N'Trần Ngọc Tiến', 'ADMIN', '02/01/2002', 'ngoctien@gmail.com', '312503551', 'Nam', '0334161123', N'Ninh Hòa, Khánh Hòa', '10000000', '1');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV02', '123456', N'Trần Tử Thiên', 'NVK', '03/01/2001', 'tuthien@gmail.com', '312503552', N'Nữ', '0334161124', N'Quận 1, Hồ Chí Minh', '8000000', '1');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV03', '123456', N'Trần Hoàng Thiên Long', 'NVK', '04/01/2000', 'thienlong@gmail.com', '312503553', 'Nam', N'0334161125', N'Quận Hoàn Kiếm, Hà Nội', '5000000', '0');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV04', '123456', N'Trần Thanh Phong', 'NVBH', '05/01/1999', 'thanhphong@gmail.com', '312503554', 'Nam', N'0334161126', N'Quận Hải Châu, Đà Nẵng', '6000000', '0');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV05', '123456', N'Lê Anh Quốc', 'NVK', '20/01/2002', 'anhquoc@gmail.com', '312503555', N'Nữ', '0334161127', N'Ninh Hải, Ninh Thuận', '7000000', '1');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV06', '123456', N'Nguyễn Minh Hiếu', 'NVBH', '21/01/1999', 'hieu31012002@gmail.com', '312503556', N'Nam', '0334161128', N'Rạch Giá, Kiên Giang', '10000000', '0');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV07', '123456', N'Dương Hoàng Mai', 'NVBH', '22/01/2001', 'hoangmai@gmail.com', '312503557', N'Nữ', '0334161129', N'Tân Thành, Cà Mau', '13000000', '1');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV08', '123456', N'Lê Thị Phương Quyên', 'NVBH', '23/01/2000', 'phuongquyen@gmail.com', '312503558', N'Nữ', '0334161130', N'Hương Trà, Huế', '12000000', '0');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV09', '123456', N'Trần Thị Thủy Tiên', 'NVBH', '24/01/2000', 'thuytien@gmail.com', '312503559', N'Nữ', '0334161131', N'An Dương, Hải Phòng', '9000000', '0');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV10', '123456', N'Nguyễn Lê Ngọc Văn', 'NVK', '25/01/2000', 'ngocvan@gmail.com', '312503560', N'Nam', '0334161132', N'Tuy Hòa, Phú Yên', '7000000', '1');
INSERT INTO NHANVIEN (MaNhanVien, MatKhau, HoTen, MaChucVu, NgaySinh, Email, CCCD, GioiTinh, SDT, DiaChi, Luong, TrangThai) VALUES ('NV11', '123456', N'Mai', 'ADMIN', '22/01/2001', 'mai@gmail.com', '312503557', N'Nữ', '0334161129', N'Tân Thành, Cà Mau', '13000000', '1');

INSERT INTO PHIEUNHAP (MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien) VALUES ('PN01', 'NV02', 'K01', N'VPP Hoàng Hà', '10/01/2022', '2500000');
INSERT INTO PHIEUNHAP (MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien) VALUES ('PN02', 'NV03', 'K02', N'VPP Phượng Vũ', '25/02/2022', '3000000');
INSERT INTO PHIEUNHAP (MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien) VALUES ('PN03', 'NV10', 'K03', N'VPP Hà Nội', '03/12/2021', '2400000');
INSERT INTO PHIEUNHAP (MaPhieuNhap, MaNhanVien, MaKho, NhaCungCap, NgayNhap, TongTien) VALUES ('PN04', 'NV05', 'K04', N'VPP Ánh Dương Xanh', '13/11/2021', '2120000');

INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS01', N'Hệ số lợi nhuận sách', '1.5');
INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS02', N'Hệ số lợi nhuận văn phòng phẩm', '1.8');
INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS03', N'Số lượng nhập tối đa', '500');
INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS04', N'Giá dịch vụ gói quà', '25000');

INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS05', N'Thuế', '25000');
INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS06', N'Mặt bằng', '3000000');
INSERT INTO THAMSO (MaThuocTinh, TenThuocTinh, GiaTri) VALUES ('TS07', N'Tuổi tối thiểu khách hàng', '10');

INSERT INTO SANPHAM  VALUES ('SP01', N'Vật Lý 12', N'Bộ Giáo Dục', N'Sách thiếu nhi', N'NXB Giáo Dục Việt Nam', '17000', '2022', 'K01', '1');
INSERT INTO SANPHAM  VALUES ('SP02', N'Thám tử lừng danh Conan', N'Aoyama Gōshō', N'Sách tham khảo', N'NXB Kim Đồng', '20000', '2020', 'K02', '0');
INSERT INTO SANPHAM  VALUES ('SP03', N'Chú mèo máy Doraemon', N'Fujiko Fujio', N'Sách thiếu nhi', N'NXB Kim Đồng', '22000', '2018', 'K03', '0');
INSERT INTO SANPHAM  VALUES ('SP04', N'TỰ HỌC TOÁN HỌC - TẬP 1 - HÌNH KHÔNG GIAN', N'Lê Văn Tuấn', N'Sách giáo khoa', N'NXB Hồng Đức', '160000', '2022', 'K04', '1');
INSERT INTO SANPHAM  VALUES ('SP05', N'TỰ HỌC TOÁN HỌC - TẬP 2 - HÌNH KHÔNG GIAN', N'Lê Văn Tuấn', N'Sách giáo khoa', N'NXB Hồng Đức', '160000', '2022', 'K01', '0');
INSERT INTO SANPHAM  VALUES ('SP06', N'Hóa Học 12', N'Bộ Giáo Dục', N'Sách giáo khoa', N'NXB Giáo Dục Việt Nam', '19000', '2022', 'K02', '1');
INSERT INTO SANPHAM  VALUES ('SP07', N'Đắc nhân tâm', N'Dale Carnegie', N'Sách tiểu thuyết', N'NXB Simon & Schuster', '50000', '2020', 'K03', '0');
INSERT INTO SANPHAM  VALUES ('SP08', N'Tôi tài giỏi – Bạn cũng thế', N'Adam Khoo', N'Sách kỹ năng mềm', N'NXB Phụ Nữ Việt Nam', '50000', '2021', 'K04', '1');
INSERT INTO SANPHAM  VALUES ('SP09', N'One Piece', N'Oda Eiichiro', N'Sách kỹ năng mềm', N'NXB Kim Đồng', '21000', '2020', 'K01', '1');
INSERT INTO SANPHAM  VALUES ('SP10', N'Chiến Thắng Con Quỷ Trong Bạn', N'Napoleon Hill', N'Sách kỹ năng mềm', N'NXB Lao Động Xã Hội', '55000', '2021', 'K02', '0');
INSERT INTO SANPHAM  VALUES ('SP11', N'Bút bi', '', N'Văn phòng phẩm', '', '2000', '', 'K03', '0');
INSERT INTO SANPHAM  VALUES ('SP12', N'Bút chì', '', N'Văn phòng phẩm', '', '2000', '', 'K04', '1');
INSERT INTO SANPHAM  VALUES ('SP13', N'Bút xóa', '', N'Văn phòng phẩm', '', '15000', '', 'K01', '0');
INSERT INTO SANPHAM  VALUES ('SP14', N'Cặp hồ sơ', '', N'Văn phòng phẩm', '', '5000', '', 'K02', '0');
INSERT INTO SANPHAM  VALUES ('SP15', N'Cục tẩy', '', N'Văn phòng phẩm', '', '4000', '', 'K03', '1');
INSERT INTO SANPHAM  VALUES ('SP16', N'Giấy in', '', N'Văn phòng phẩm', '', '42500', '', 'K04', '1');
INSERT INTO SANPHAM  VALUES ('SP17', N'Máy tính casio fx-580', '', N'Văn phòng phẩm', '', '650000', '', 'K01', '1');
INSERT INTO SANPHAM  VALUES ('SP18', N'Keo dán', '', N'Văn phòng phẩm', '', '5000', '', 'K02', '0');
INSERT INTO SANPHAM  VALUES ('SP19', N'Bút dạ quang', '', N'Văn phòng phẩm', '', '15000', '', 'K03', '0');
INSERT INTO SANPHAM  VALUES ('SP20', N'Vở', '', N'Văn phòng phẩm', '', '5000', '', 'K04', '0');

INSERT INTO PHANQUYEN (MaChucVu, MaQuyen) VALUES ('ADMIN', '1');
INSERT INTO PHANQUYEN (MaChucVu, MaQuyen) VALUES ('NVK', '2');
INSERT INTO PHANQUYEN (MaChucVu, MaQuyen) VALUES ('NVBH', '2');
INSERT INTO PHANQUYEN (MaChucVu, MaQuyen) VALUES ('NVKT', '3');

INSERT INTO QUYEN (MaQuyen, TenQuyen) VALUES ('1', N'Quản lý báo cáo');
INSERT INTO QUYEN (MaQuyen, TenQuyen) VALUES ('2', N'Quản lý kho');
INSERT INTO QUYEN (MaQuyen, TenQuyen) VALUES ('3', N'Quản lý khách hàng');
INSERT INTO QUYEN (MaQuyen, TenQuyen) VALUES ('4', N'Quản lý hóa đơn');


ALTER TABLE PHANQUYEN ADD CONSTRAINT PK_PQ PRIMARY KEY(MaChucVu, MaQuyen)
ALTER TABLE PHANQUYEN ADD CONSTRAINT FK_PQ_CV FOREIGN KEY(MaChucVu) REFERENCES CHUCVU(MaChucVu)
ALTER TABLE PHANQUYEN ADD CONSTRAINT FK_PQ_Q FOREIGN KEY(MaQuyen) REFERENCES QUYEN(MaQuyen)

ALTER TABLE NHANVIEN ADD CONSTRAINT FK_MACV_NV_CV FOREIGN KEY(MaChucVu) REFERENCES CHUCVU(MaChucVu)

ALTER TABLE HOADON ADD CONSTRAINT FK_MANV_HD_NV FOREIGN KEY(MaNhanVien) REFERENCES NHANVIEN(MaNhanVien)
ALTER TABLE HOADON ADD CONSTRAINT FK_MAKH_HD_KH FOREIGN KEY(MaKhachHang) REFERENCES KHACHHANG(MaKhachHang)
ALTER TABLE HOADON ADD CONSTRAINT FK_MAKM_HD_KM FOREIGN KEY(MaKhuyenMai) REFERENCES KHUYENMAI(MaKhuyenMai)

ALTER TABLE CHITIETHOADON ADD CONSTRAINT FK_MAHD_CTHD_HD FOREIGN KEY(MaHoaDon) REFERENCES HOADON(MaHoaDon)
ALTER TABLE CHITIETHOADON ADD CONSTRAINT FK_MASP_CTHD_SP FOREIGN KEY(MaSanPham) REFERENCES SANPHAM(MaSanPham)
ALTER TABLE CHITIETHOADON ADD CONSTRAINT PK_CTHD PRIMARY KEY(MaHoaDon,MaSanPham)

ALTER TABLE KHACHHANG ADD CONSTRAINT FK_MALKH_KH_LKH FOREIGN KEY(MaLoaiKhachHang) REFERENCES LOAIKHACHHANG(MaLoaiKhachHang)

ALTER TABLE KHUYENMAI ADD CONSTRAINT FK_MALKH_KM_LKH FOREIGN KEY(MaLoaiKhachHang) REFERENCES LOAIKHACHHANG(MaLoaiKhachHang)

ALTER TABLE PHIEUNHAP ADD CONSTRAINT FK_MANV_PN_NV FOREIGN KEY(MaNhanVien) REFERENCES NHANVIEN(MaNhanVien)
ALTER TABLE PHIEUNHAP ADD CONSTRAINT FK_MAK_PN_K FOREIGN KEY(MaKho) REFERENCES KHO(MaKho)

ALTER TABLE CHITIETPHIEUNHAP ADD CONSTRAINT FK_MAPN_CTPN_PN FOREIGN KEY(MaPhieuNhap) REFERENCES PHIEUNHAP(MaPhieuNhap)
ALTER TABLE CHITIETPHIEUNHAP ADD CONSTRAINT FK_MASP_CTPN_SP FOREIGN KEY(MaSanPham) REFERENCES SANPHAM(MaSanPham)

ALTER TABLE CHITIETPHIEUNHAP ADD CONSTRAINT PK_CTPN PRIMARY KEY(MaPhieuNhap, MaSanPham)

ALTER TABLE CHITIETBAOCAODOANHTHU ADD CONSTRAINT FK_MANVBC_CTBCDT_NV FOREIGN KEY(MaNVBC) REFERENCES NHANVIEN(MaNhanVien)

ALTER TABLE CHITIETBAOCAOKHO ADD CONSTRAINT FK_MANVBC_CTBCK_NV FOREIGN KEY(MaNVBC) REFERENCES NHANVIEN(MaNhanVien)
ALTER TABLE CHITIETBAOCAOKHO ADD CONSTRAINT FK_MAK_CTBCK_K FOREIGN KEY(MaKho) REFERENCES KHO(MaKho)

ALTER TABLE CHITIETBAOCAOSANPHAM ADD CONSTRAINT FK_MANVBC_CTBCSP_NV FOREIGN KEY(MaNVBC) REFERENCES NHANVIEN(MaNhanVien)
ALTER TABLE CHITIETBAOCAOSANPHAM ADD CONSTRAINT FK_MASP_CTBCSP_SP FOREIGN KEY(MaSanPham) REFERENCES SANPHAM(MaSanPham)
ALTER TABLE CHITIETBAOCAOSANPHAM ADD CONSTRAINT PK_CTBCSP PRIMARY KEY(MaChiTietBaoCao, MaSanPham, TuNgay, DenNgay)


ALTER TABLE SANPHAM ADD CONSTRAINT FK_MAK_S_K FOREIGN KEY(MaKho) REFERENCES KHO(MaKho)
