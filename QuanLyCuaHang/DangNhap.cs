using QuanLyCuaHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHang
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {     
            if (string.IsNullOrEmpty(txtTaiKhoan.Text) || string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Functions.Login(txtTaiKhoan.Text, txtMatKhau.Text))
            {
                MessageBox.Show("Đăng nhập thành công!");
                frmTrangChu frm = new frmTrangChu();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult tb = MessageBox.Show("Bạn có Muốn thoát hay không?","Thông Báo", MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if(tb == DialogResult.OK)
                Application.Exit();
        }
        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
           
        }
    }
}
