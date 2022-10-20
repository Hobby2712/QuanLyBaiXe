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
    public partial class frmQuanLy : Form
    {
        public frmQuanLy()
        {
            InitializeComponent();
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            frmTaiKhoan frm = new frmTaiKhoan() { TopLevel = false, TopMost = true };
            pnlQuanLy.Controls.Clear();
            Refresh();
            pnlQuanLy.Controls.Add(frm);
            frm.Show();
            btnTK.BackColor = Color.LemonChiffon;
            btnNV.BackColor = Color.White;
            btnBaiXe.BackColor = Color.White;
        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            frmNhanVien frm = new frmNhanVien() { TopLevel = false, TopMost = true };
            pnlQuanLy.Controls.Clear();
            Refresh();
            pnlQuanLy.Controls.Add(frm);
            frm.Show();
            btnNV.BackColor = Color.LemonChiffon;
            btnTK.BackColor = Color.White;
            btnBaiXe.BackColor = Color.White;
        }

        private void btnBaiXe_Click(object sender, EventArgs e)
        {
            frmBaiXe frm = new frmBaiXe() { TopLevel = false, TopMost = true };
            pnlQuanLy.Controls.Clear();
            Refresh();
            pnlQuanLy.Controls.Add(frm);
            frm.Show();
            btnNV.BackColor = Color.White;
            btnTK.BackColor = Color.White;
            btnBaiXe.BackColor = Color.LemonChiffon;
        }
    }
}
