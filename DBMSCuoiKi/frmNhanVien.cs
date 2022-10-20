using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSCuoiKi
{
    public partial class frmNhanVien : Form
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult tl = MessageBox.Show("Bạn muốn huỷ à??",
                        "Thông báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
            if (tl == DialogResult.Yes)
            {
                // Xóa trống các đối tượng trong Panel
                txtMaNV.ResetText();
                txtTenNV.ResetText();
                dtpNgaySinh.ResetText();
                txtGioiTinh.ResetText();
                txtSdt.ResetText();
                txtCCCD.ResetText();
                txtEmail.ResetText();
                txtDiaChi.ResetText();

                // Cho thao tác trên các nút Thêm / Sửa 
                btnThem.Enabled = true;
                btnSua.Enabled = true;

                // Không cho thao tác trên các nút Lưu / Hủy / Panel
                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                grbNhanVien.Enabled = false;
            }
        }
    }
}
