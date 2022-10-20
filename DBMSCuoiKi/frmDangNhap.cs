using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DBMSCuoiKi
{
    public partial class frmDangNhap : Form
    {
        SqlConnection conn;
        string connStr = "Data Source=(local)" + ";Initial Catalog=QLBaiXe" + ";Integrated Security=True";
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


        private void btnLogin_Click(object sender, EventArgs e)
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
                int a;
                if (rdoQuanLy.Checked == true)
                {
                    a = 2;
                }
                else a = 1;
                SqlConnection conn = new SqlConnection();
                try
                {
                    conn = new SqlConnection(connStr);
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_login";
                    cmd.Parameters.AddWithValue("@UserName", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("@Password", txtMatKhau.Text);
                    cmd.Parameters.AddWithValue("@ChkQuyen", a);
                    cmd.Connection = conn;
                    object kq = cmd.ExecuteScalar();
                    int code = Convert.ToInt32(kq);
                    if (code == 0)
                    {
                        MessageBox.Show("Chào mừng Admin đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frmQuanLy frmQL = new frmQuanLy();
                        frmQL.ShowDialog();

                    }
                    else if (code == 1)
                    {
                        MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frmHome frmNV = new frmHome();
                        frmNV.ShowDialog();
                    }
                    else if (code == 2)
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không đúng !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtMatKhau.Text = "";
                        txtTaiKhoan.Text = "";
                        txtTaiKhoan.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản không tồn tại !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtMatKhau.Text = "";
                        txtTaiKhoan.Text = "";
                        txtTaiKhoan.Focus();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
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
    }
}