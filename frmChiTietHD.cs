using DevExpress.XtraEditors;
using QuanLyChuoiBanXoi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiBanXoi
{
    public partial class frmChiTietHD : DevExpress.XtraEditors.XtraForm
    {
        public frmChiTietHD()
        {
            InitializeComponent();
        }
        public frmChiTietHD(string mahoadon)
        {
            InitializeComponent();

            using (var dbContext = new QLBXDBcontext())
            {
                CTHD hd = dbContext.CTHD.FirstOrDefault(x => x.MaHD == mahoadon);
                if (hd != null)
                {
                    txtMaHD.Text = hd.MaHD;
                    txtMaCT.Text = hd.MaCT;
                    txtMaXoi.Text = hd.MaXoi;
                    txtGia.Text = hd.Gia.ToString();
                    txtTenXoi.Text = hd.TenXoi;
                    txtSL.Text = hd.SoLuong.ToString();
                    if (int.TryParse(txtGia.Text, out int gia) && int.TryParse(txtSL.Text, out int soLuong))
                    {
                        int thanhTien = gia * soLuong;
                        txtThanhTien.Text = thanhTien.ToString();
                    }
                }
            }
            txtThanhTien.Enabled = false;
            txtMaHD.Enabled = false;
            txtMaCT.Enabled = false;
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTraCTHD())
            {
                using (var dbcontext = new QLBXDBcontext())
                {
                    CTHD chiTietHoaDon = new CTHD()
                    {
                        MaHD = txtMaHD.Text.ToString(),
                        MaCT = txtMaCT.Text.ToString(),
                        MaXoi = txtMaXoi.Text.ToString(),
                        TenXoi = txtTenXoi.Text.ToString(),
                        Gia = decimal.Parse(txtGia.Text),
                        SoLuong = int.Parse(txtSL.Text),
                    };
                    dbcontext.CTHD.AddOrUpdate(chiTietHoaDon);
                    dbcontext.SaveChanges();
                }
                ResetCTHDFields();
            }
        }

        private void ResetCTHDFields()
        {
            txtMaHD.Text = string.Empty;
            txtMaCT.Text = string.Empty;
            txtMaXoi.Text = string.Empty;
            txtTenXoi.Text = string.Empty;
            txtGia.Text = string.Empty;
            txtSL.Text = string.Empty;
        }

        private bool KiemTraCTHD()
        {
            if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtMaCT.Text) || string.IsNullOrEmpty(txtMaXoi.Text) ||
                string.IsNullOrEmpty(txtTenXoi.Text) || string.IsNullOrEmpty(txtGia.Text) ||
                string.IsNullOrEmpty(txtSL.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cho các trường bắt buộc.", "Lỗi Kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!txtSL.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số lượng không phải là chữ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!txtGia.Text.All(char.IsDigit))
            {
                MessageBox.Show("Giá không phải là chữ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtTenXoi.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên món phải chỉ chứa chữ ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!(txtMaXoi.Text.Length >= 1 && txtMaXoi.Text.Length <= 6))
            {
                MessageBox.Show("Mã món phải có độ dài từ 1 đến 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
