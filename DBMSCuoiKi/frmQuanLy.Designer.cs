namespace DBMSCuoiKi
{
    partial class frmQuanLy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTK = new System.Windows.Forms.Button();
            this.btnNV = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlQuanLy = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnTK
            // 
            this.btnTK.BackColor = System.Drawing.Color.White;
            this.btnTK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTK.Location = new System.Drawing.Point(32, 88);
            this.btnTK.Name = "btnTK";
            this.btnTK.Size = new System.Drawing.Size(112, 67);
            this.btnTK.TabIndex = 0;
            this.btnTK.Text = "Tài Khoản";
            this.btnTK.UseVisualStyleBackColor = false;
            this.btnTK.Click += new System.EventHandler(this.btnTK_Click);
            // 
            // btnNV
            // 
            this.btnNV.BackColor = System.Drawing.Color.White;
            this.btnNV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNV.Location = new System.Drawing.Point(32, 195);
            this.btnNV.Name = "btnNV";
            this.btnNV.Size = new System.Drawing.Size(112, 64);
            this.btnNV.TabIndex = 1;
            this.btnNV.Text = "Nhân Viên";
            this.btnNV.UseVisualStyleBackColor = false;
            this.btnNV.Click += new System.EventHandler(this.btnNV_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.Red;
            this.btnLogOut.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogOut.Location = new System.Drawing.Point(12, 613);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(124, 58);
            this.btnLogOut.TabIndex = 2;
            this.btnLogOut.Text = "Đăng Xuất";
            this.btnLogOut.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(528, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 46);
            this.label1.TabIndex = 3;
            this.label1.Text = "QUẢN LÝ NHÂN VIÊN";
            // 
            // pnlQuanLy
            // 
            this.pnlQuanLy.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlQuanLy.Location = new System.Drawing.Point(142, 88);
            this.pnlQuanLy.Name = "pnlQuanLy";
            this.pnlQuanLy.Size = new System.Drawing.Size(1171, 622);
            this.pnlQuanLy.TabIndex = 4;
            // 
            // frmQuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1334, 716);
            this.Controls.Add(this.pnlQuanLy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.btnNV);
            this.Controls.Add(this.btnTK);
            this.Name = "frmQuanLy";
            this.Text = "frmQuanLy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnTK;
        private Button btnNV;
        private Button btnLogOut;
        private Label label1;
        private Panel pnlQuanLy;
    }
}