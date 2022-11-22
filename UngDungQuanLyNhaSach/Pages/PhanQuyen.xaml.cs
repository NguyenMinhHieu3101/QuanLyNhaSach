using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UngDungQuanLyNhaSach.Model;

namespace UngDungQuanLyNhaSach.Pages
{
    /// <summary>
    /// Interaction logic for PhanQuyen.xaml
    /// </summary>
    /// 

    public partial class PhanQuyen : Page
    {
     

        public PhanQuyen()
        {
            InitializeComponent();
        }
        public SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(@"Server=(local);Database=QUANLYNHASACH;Trusted_Connection=Yes;");
            return con;
        }

        public List<Model.PhanQuyen> GetPhanQuyen()
        {
            using (SqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT pq.MaQuyen, quyen.TenQuyen, cv.MaChucVu, cv.TenChucVu " +
                    "FROM (phanquyen pq INNER JOIN chucvu cv on pq.MaChucVu = cv.MaChucVu) " +
                            "INNER JOIN quyen ON quyen.MaQuyen = pq.MaQuyen " +
                    "ORDER BY `cv`.`MaChucVu` ASC, `pq`.`MaQuyen` ASC";

                SqlCommand cmd = new SqlCommand(query, connectioncheck);
                List<Model.PhanQuyen> phanQuyens = new List<Model.PhanQuyen>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Model.PhanQuyen ph = new Model.PhanQuyen();
                           
                            ph.MaQuyen = (reader["MaQuyen"]).ToString();
                            ph.TenQuyen = reader["TenQuyen"].ToString();
                            ph.MaChucVu = (reader["MaChucVu"]).ToString();
                            ph.TenChucVu = reader["TenChucVu"].ToString();
                            phanQuyens.Add(ph);
                        }
                        return phanQuyens;
                    }
                    else return null;
                }
            }
        }

        public List<Quyen> GetQuyen()
        {
            using (SqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT * FROM `quyen` ORDER BY `quyen`.`MaQuyen` ASC;";

                SqlCommand cmd = new SqlCommand(query, connectioncheck);
                List<Quyen> quyens = new List<Quyen>();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Quyen quyen = new Quyen();
                           
                            quyen.MaQuyen = reader["MaQuyen"].ToString();
                            quyen.TenQuyen = reader["TenQuyen"].ToString();
                            quyens.Add(quyen);
                        }
                        return quyens;
                    }
                    else return null;
                }
            }
        }

        public bool Decentralization(string stringPermission)
        {
            List<string> permissions = new List<string>();
            permissions = stringPermission.Split("@@@").ToList();
            permissions.RemoveAt(permissions.Count - 1);
            int length = permissions.Count;
            bool needInsert = (length > 0) ? true : false;

            string submit = "INSERT INTO phanquyen VALUES ";
            for (int i = 0; i < length; i++)
            {
                List<string> str = permissions[i].Split("$$").ToList();
                string maChucVu = str[0];
                List<string> listQuyen = str[1].Split("#").ToList();
                listQuyen.RemoveAt(listQuyen.Count - 1);
                int lengthListQuyen = listQuyen.Count;
                for (int j = 0; j < lengthListQuyen; j++)
                {
                    if (listQuyen[j] != "0")
                    {
                        submit += "(" + maChucVu + "," + (j + 1) + "),";
                    }
                }
            }
            submit = submit.Remove(submit.Length - 1);
            string deleteOldPermission = "DELETE FROM phanquyen WHERE MaChucVu <> 1;";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "Decentralization";

                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@deleteOldPermission", deleteOldPermission);
                cmd.Parameters["@deleteOldPermission"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@insertNewPermision", submit);
                cmd.Parameters["@insertNewPermision"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@needInsert", needInsert);
                cmd.Parameters["@needInsert"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@isSucccess", SqlDbType.Int);
                cmd.Parameters["@isSucccess"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(cmd.Parameters["@isSucccess"].Value);
            }
        }
        public string GetPhanQuyenForSession(int maChucVu)
        {
            using (SqlConnection connectioncheck = this.GetConnection())
            {
                connectioncheck.Open();

                string query = "SELECT pq.MaQuyen " +
                    "FROM phanquyen pq INNER JOIN chucvu cv on pq.MaChucVu = cv.MaChucVu " +
                    "WHERE pq.MaChucVu = @maChucVu " +
                    "ORDER BY pq.`MaQuyen` ASC";

                SqlCommand cmd = new SqlCommand(query, connectioncheck);
                cmd.Parameters.AddWithValue("maChucVu", maChucVu);
                string permissionPerNhanVien = "";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            permissionPerNhanVien += reader["MaQuyen"].ToString();
                        }
                    }
                    return permissionPerNhanVien;
                }
            }
        }
    }
}
