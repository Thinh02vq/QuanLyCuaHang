using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
namespace QuanLyCuaHang.Class
{
    class Functions
    {
        
        private static string ConnString = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
     
        // Hàm lấy dữ liệu
        public static DataTable GetDataToTable(string sql, SqlParameter[] parameters = null)
        {
            
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open(); 
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(table);
                }
            }
            return table;
        }
        //Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql, SqlParameter[] parameters = null)
        {
            DataTable table = GetDataToTable(sql, parameters);
            return table.Rows.Count > 0;
        }
        //Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static string GetFieldValues(string sql, SqlParameter[] parameters = null)
        {
            string ma = "";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    object value = cmd.ExecuteScalar(); // Dùng ExecuteScalar cho hàm trả về 1 giá trị duy nhất

                    if (value != null)
                        ma = value.ToString();
                }
            }
            return ma;
        }
        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            DataTable table = GetDataToTable(sql);
            cbo.DataSource = table;
            cbo.ValueMember = ma;
            cbo.DisplayMember = ten;
        }
        public static string CreateKey(string tiento)
        {
            return $"{tiento}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}";
        }
        
        public static DataTable GetDataToTableWithParams(string sql, SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString)) 
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(table);
                }
            }
            return table;
        }
        public static bool Login(string user, string pass)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE TenDangNhap = @user AND MatKhau = @pass";
            SqlParameter[] p = {
        new SqlParameter("@user", user),
        new SqlParameter("@pass", pass)
    };

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(p);
                        object result = cmd.ExecuteScalar();

                        // Kiểm tra: nếu result là số > 0 thì mới đăng nhập thành công
                        if (result != null && Convert.ToInt32(result) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return false;
            }
            return false; 
        }
        public static void Logout()
        {
            // Giả sử có lưu thông tin nhân viên đăng nhập ở biến static này
            // Cần reset lại chúng để người sau không vào được tài khoản cũ
            // Ví dụ: UserSession.MaNV = "";
            // Ví dụ: UserSession.TenNV = "";
        }
    }
}
