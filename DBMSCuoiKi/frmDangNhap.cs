namespace DBMSCuoiKi
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult exit = MessageBox.Show("Bạn có muốn thoát không?", "Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (exit == DialogResult.Yes)
                Application.Exit();
        }

        private void chkShowPass_Click(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
            {
                txtMatKhau.PasswordChar = (char)0;
            }
            else
            {
                txtMatKhau.PasswordChar = '*';
            }
        }

        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaiKhoan.Focus();
            }
            else if (txtMatKhau.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
            }
            else
            {
                string a;
                if (rdoQuanLy.Checked == true)
                {
                    a = "Admin";
                }
                else a = "User";
                TaiKhoan check = db.TaiKhoans.SingleOrDefault(n => n.TK.Equals(txt_TaiKhoan.Text.Trim()) && n.MK.Equals(txt_MatKhau.Text.Trim()) && n.Quyen.Equals(a));
                if (check == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    if (rdoNhanVien.Checked == true)
                    {
                        //Model.maTT = check.MaTT;
                        MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frm_Main frmNV = new frm_Main();
                        frmTT.ShowDialog();
                    }
                    if (rdoQuanLy.Checked == true)
                    {
                        MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frm_QuanLy frmQL = new frm_QuanLy();
                        frmQL.ShowDialog();

                    }
                }
            }
        }
}