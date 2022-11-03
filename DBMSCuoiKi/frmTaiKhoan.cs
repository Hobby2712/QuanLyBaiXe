using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DBMSCuoiKi
{
    public partial class frmTaiKhoan : Form
    {
        SqlConnection conn;
        string connStr = "Data Source=(local)" + ";Initial Catalog=QLBaiXe" + ";Integrated Security=True";

        public frmTaiKhoan()
        {
            conn = new SqlConnection(connStr);
            InitializeComponent();
        }

        private Boolean existsMaNV(int maNV)
        {
            conn.Open();
            String sql = $"select cast(count(*) as bit) from dbo.NhanVien where MaNV = {maNV}";
            SqlCommand command = new SqlCommand(sql, conn);
            Boolean exists = (Boolean)command.ExecuteScalar();
            conn.Close();
            return exists;
        }

        private Boolean existsTaiKhoan(String username)
        {
            conn.Open();
            String sql = $"select cast(count(*) as bit) from dbo.TaiKhoan where Username = '{username}'";
            SqlCommand command = new SqlCommand(sql, conn);
            Boolean exists = (Boolean)command.ExecuteScalar();
            conn.Close();
            return exists;
        }

        private Boolean existsMaNVInTaiKhoan(int maNV, int quyen)
        {
            
            conn.Open();
            String sql = $"select cast(count(*) as bit) from dbo.TaiKhoan where MaNV = {maNV} and Quyen = {quyen}";
            SqlCommand command = new SqlCommand(sql, conn);
            Boolean exists = (Boolean)command.ExecuteScalar();
            conn.Close();
            return exists;
        }

        private Boolean existsNewRole(String username, int quyen)
        {
            conn.Open();
            String sql = $"select cast(count(*) as bit) from dbo.TaiKhoan where MaNV in (select MaNV from dbo.TaiKhoan where Username = '{username}') and quyen = {quyen} and Username <> '{username}'";
            SqlCommand command = new SqlCommand(sql, conn);
            Boolean exists = (Boolean)command.ExecuteScalar();
            conn.Close();
            return exists;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                String maNhanVien = txtMaNV.Text;
                String taiKhoan = txtTK.Text;
                String matKhau = txtMK.Text;
                Object chucVu = comboBox1.SelectedValue;

                if (maNhanVien == null || taiKhoan == null || matKhau == null || chucVu == null)
                {
                    MessageBox.Show("Xin hãy nhập mã nhân viên, tài khoản, mật khẩu và chức vụ để thêm tài khoản mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(maNhanVien, out _))
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!existsMaNV(int.Parse(maNhanVien)))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (existsMaNVInTaiKhoan(int.Parse(maNhanVien), (int) chucVu))
                {
                    MessageBox.Show("Đã tồn tại tài khoản với mã nhân viên hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (existsTaiKhoan(taiKhoan))
                {
                    MessageBox.Show("Tên tài khoản đã được sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_addAccount";
                cmd.Parameters.AddWithValue("@UserName", taiKhoan);
                cmd.Parameters.AddWithValue("@Password", matKhau);
                cmd.Parameters.AddWithValue("@Quyen", (int)chucVu);
                cmd.Parameters.AddWithValue("@MaNV", int.Parse(maNhanVien));
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNV.Text = "";
                txtMK.Text = "";
                txtTK.Text = "";
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                String username = txtTK.Text;
                String password = txtMK.Text;
                int quyen = (int)comboBox1.SelectedValue;

                if (username == null)
                {
                    MessageBox.Show("Xin hãy nhập tên tài khoản để sửa thông tin tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!existsTaiKhoan(username))
                {
                    MessageBox.Show("Tài khoản không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (existsNewRole(username, quyen))
                {
                    MessageBox.Show("Tồn tại tài khoản cho nhân viên với chức vụ hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_updateAccount";
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Quyen", quyen);
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                String username = txtTK.Text;

                if (username == null)
                {
                    MessageBox.Show("Xin hãy nhập tên tài khoản để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_deleteAccount";
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            try
            {
                String username = txtTK.Text;

                if (username == null)
                {
                    MessageBox.Show("Xin hãy nhập tên tài khoản để xem thông tin tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                conn.Open();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnXemToanBo_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                String username = txtTK.Text;
                String sql = "SELECT * FROM dbo.XemTaiKhoan(@Username)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserName", username);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dgvTaiKhoan.DataSource = dataTable;
                dgvTaiKhoan.AutoGenerateColumns = false;
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
}
