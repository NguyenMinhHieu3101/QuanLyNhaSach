using System.Windows;
using System.Data.SqlClient;
using System.Data;
using UngDungQuanLyNhaSach.Model;
using System.Windows.Controls;
using System;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        public static DataRow Current_User;
        public DangNhap()
        {
            InitializeComponent();
        }
        private void Forget_Password_TextBlock(object sender, RoutedEventArgs e)
        {
            QuenMatKhau quenMatKhau = new QuenMatKhau();
            quenMatKhau.Show();    
            this.Hide();
        }
        private void Sign_In_Button(object sender, RoutedEventArgs e)
        {
            //Home home = new Home();
            //MessageBox.Show("Chào mừng!" + Current_User[2].ToString());
            //home.Show();
            //this.Hide();

            if (txtEmail.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Email và mật khẩu");
            }
            else

            if (getID(txtEmail.Text, txtMatKhau.Text))
            {
                NhanVienDangDangNhap.MaChucVu = Current_User[3].ToString();
                NhanVienDangDangNhap.HoTen = Current_User[2].ToString();

                Home home = new Home();
                MessageBox.Show("Chào mừng!" + Current_User[2].ToString());
                home.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng HOẶC NHÂN VIÊN CÓ TRẠNG THÁI ĐÃ NGHỈ VIỆC!");
            }
        }
    
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
       
        private Boolean getID(string username, string pass)
        {
            SqlConnection con = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM NHANVIEN WHERE Email ='" + username + "' and MatKhau='" + pass + "' and TrangThai <> '0'" , con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Current_User = dr;
                        return true;
                    }

                }
                return false;
            }
            catch (Exception)
            {

                MessageBox.Show("Lỗi xảy ra khi truy vấn dữ liệu hoặc kết nối với server thất bại !");
                return false;
            }
            finally
            {
                con.Close();
            }
  
        }


    }
}
