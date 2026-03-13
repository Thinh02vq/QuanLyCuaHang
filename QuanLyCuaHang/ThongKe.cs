using QuanLyCuaHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyCuaHang
{
    public partial class ThongKe : Form
    {
        public ThongKe()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            string query = @"SELECT hd.MaHoaDon, hd.NgayBan, hd.MaKhachHang, ct.MaHang, ct.SoLuong, ct.DonGia, ct.GiamGia, 
                     (ct.SoLuong * ct.DonGia * (1 - ct.GiamGia / 100.0)) AS ThanhTien
                     FROM HoaDon hd
                     INNER JOIN ChiTietHD ct ON hd.MaHoaDon = ct.MaHoaDon
                     WHERE hd.NgayBan BETWEEN @TuNgay AND @DenNgay";

            // Khai báo tham số
            SqlParameter[] p = {
        new SqlParameter("@TuNgay", dtpTuNgay.Value.Date),
        new SqlParameter("@DenNgay", dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1))
            };

            // Gọi hàm từ class Functions - Form không cần biết ConnectionString là gì!
            DataTable dt = Functions.GetDataToTableWithParams(query, p);

            dgvKetQua.DataSource = dt;

            // Tính tổng
            decimal tongDoanhThu = 0;
            foreach (DataRow row in dt.Rows)
            {
                // Kiểm tra nếu giá trị không phải null và không phải dbnull
                if (row["ThanhTien"] != DBNull.Value)
                {
                    // Chuyển sang chuỗi rồi ép kiểu decimal, cách này an toàn hơn nhiều
                    tongDoanhThu += Convert.ToDecimal(row["ThanhTien"]);
                }
            }
            lblKetQua.Text = $"Tổng doanh thu: {tongDoanhThu:N0} VNĐ";
        }
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvKetQua.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Không tìm thấy Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet xlWorkSheet = xlWorkBook.Sheets[1];
            xlWorkSheet = xlWorkBook.ActiveSheet;
            xlWorkSheet.Name = "ThongKeDoanhThu";

            // Header
            for (int i = 1; i <= dgvKetQua.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, i] = dgvKetQua.Columns[i - 1].HeaderText;
            }

            // Data
            for (int i = 0; i < dgvKetQua.Rows.Count; i++)
            {
                for (int j = 0; j < dgvKetQua.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[i + 2, j + 1] = dgvKetQua.Rows[i].Cells[j].Value?.ToString();
                }
            }

            // Định dạng tự động
            xlWorkSheet.Columns.AutoFit();

            // Lưu file
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveDialog.FileName = "ThongKeDoanhThu.xlsx";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                xlWorkBook.SaveAs(saveDialog.FileName);
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Giải phóng
            xlWorkBook.Close();
            xlApp.Quit();
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {

        }
    }
}
