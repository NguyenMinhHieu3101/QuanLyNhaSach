
using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for QuenMatKhau.xaml
    /// </summary>
    public partial class QuenMatKhau : Window
    {
        
        public QuenMatKhau()
        {
            InitializeComponent();
        }
    

        private void OTP_Button(object sender, RoutedEventArgs e)
        {
            GetOTP(txt_email.Text);
        }


        private void Confirm_Button(object sender, RoutedEventArgs e)
        {
            ResetPasswordExecute(txt_OTP.Text, txt_email.Text);
        }
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
            this.Hide();
        }


        private string GenerateOTP()
        {
            int length = 6;
            const string valid = "1234567890";
            //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        public string GetOTP(string email)
        {
       
            string otp = GenerateOTP();
            if (SetOTPForNhanVien(otp, email))
            {
                Mailer mail = new Mailer();
                string bodyMail = "<h2>Chào bạn</h2><p>Mã OTP của bạn là:" + otp + "</p>";
                if (mail.Send(email, "Mã OTP", bodyMail) == "OK")
                {
                    MessageBox.Show("Thành công");
                    return "true";
                }
                else
                {
                    MessageBox.Show("Lỗi máy chủ");
                    return "Lỗi máy chủ. Vui lòng thử lại.";
                }
            }
            else
            {
                MessageBox.Show("Email không tồn tại");
                return "Email sai hoặc không tồn tại";
            }
        }

        public string ResetPasswordExecute(string otp, string email)
        {
           
 
            int resultStatus = ResetPassword(otp, email);

            if (resultStatus == 1)
            {
                MessageBox.Show("Hết hạn");
                return "Mã OTP hết hạn";
            }
            if (resultStatus == 2)
            {
                MessageBox.Show("Thành công");
                return "true";
            }
            if (resultStatus == 3)
            {
                MessageBox.Show("OTP không hợp lệ");
                return "Mã OTP không hợp lệ";
            }
            if (resultStatus == 5)
            {
                MessageBox.Show("Lỗi máy chủ gửi mật khẩu mới không thành công");
                return "Lỗi máy chủ gửi mật khẩu mới qua email không thành công";
            }
            MessageBox.Show("Email không hợp lệ");
            return "Địa chỉ email không hợp lệ";
        }
        public bool SetOTPForNhanVien(string otp, string email)
        {
            using (SqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "UPDATE nhanvien " +
                    "SET OTP=@otp, ThoiGianLayOTP=@now " +
                    "WHERE Email=@email and TrangThai<>'0';";

                SqlCommand cmd = new SqlCommand(query, connectioncheck);
                cmd.Parameters.AddWithValue("otp", otp);
                cmd.Parameters.AddWithValue("now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("email", email);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                return true;
            }
        }
        private SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            return con;
        }
       /* private string GeneratePassword()
        {
            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }*/

        public int ResetPassword(string otp, string email)
        {
            using (SqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string str = "SELECT ThoiGianLayOTP, OTP FROM nhanvien WHERE Email=@email";
                SqlCommand cmd = new SqlCommand(str, connectioncheck);
                cmd.Parameters.AddWithValue("email", email);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        DateTime OTPTime = Convert.ToDateTime(reader["ThoiGianLayOTP"]);
                        DateTime _now = DateTime.Now;
                        if ((int)(_now - OTPTime).TotalSeconds > 180)
                        {
                            return 1;
                        }
                        else
                        {
                            string otpInDatabase = reader["OTP"].ToString();
                            string matKhau =/* GeneratePassword()*/ txt_newpass.Text.ToString();
                            if (otpInDatabase == otp)
                            {
                                reader.Close();
                                string queryUpdate = "UPDATE nhanvien " +
                                                    "SET MatKhau=@matKhau " +
                                                    "WHERE Email=@email;";
                                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, connectioncheck);
                                cmdUpdate.Parameters.AddWithValue("matKhau", matKhau);
                                cmdUpdate.Parameters.AddWithValue("email", email);
                                if (cmdUpdate.ExecuteNonQuery() > 0)
                                {
                                    Mailer mail = new Mailer();
                                    string bodyMail = "<h2>Chào bạn</h2><p>Mật khẩu mới của của bạn là:" + matKhau + "</p>";
                                    if (mail.Send(email, "Mã OTP", bodyMail) == "OK")
                                    {
                                        return 2;
                                    }
                                    else
                                    {
                                        return 5;
                                    }
                                }
                            }
                            return 3;
                        }
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
    }
}

