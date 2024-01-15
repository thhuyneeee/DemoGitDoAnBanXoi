using DevExpress.XtraEditors;
using QuanLyChuoiBanXoi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiBanXoi
{
    public partial class frmKhachHang : DevExpress.XtraEditors.XtraForm
    {
        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void LoadKhachHang()
        {
            dgvKhachHang.Rows.Clear();
            using (var dbContext = new QLBXDBcontext())
            {
                foreach (KhachHang khachHang in dbContext.KhachHang)
                {
                    int index = dgvKhachHang.Rows.Add();
                    dgvKhachHang.Rows[index].Cells[0].Value = khachHang.MaKH;
                    dgvKhachHang.Rows[index].Cells[1].Value = khachHang.TenKH;
                    dgvKhachHang.Rows[index].Cells[2].Value = khachHang.SDT;
                    dgvKhachHang.Rows[index].Cells[3].Value = khachHang.DiaChi;
                }
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var dbContext = new QLBXDBcontext())
            {
                int index = e.RowIndex;
                string maKH = dgvKhachHang.Rows[index].Cells[0].Value.ToString();
                KhachHang khachHang = dbContext.KhachHang.FirstOrDefault(x => x.MaKH == maKH);
                txtMaKH.Text = dgvKhachHang.Rows[index].Cells[0].Value.ToString();
                txtTenKH.Text = dgvKhachHang.Rows[index].Cells[1].Value.ToString();
                txtSDT.Text = dgvKhachHang.Rows[index].Cells[2].Value.ToString();
                txtDiaChi.Text = dgvKhachHang.Rows[index].Cells[3].Value.ToString();
                txtMaKH.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnKhongLuu.Enabled = true;
            }
        }
        private bool KiemTra()
        {
            if (string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtTenKH.Text) || string.IsNullOrEmpty(txtSDT.Text) || string.IsNullOrEmpty(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtMaKH.Text.Any(char.IsWhiteSpace))
            {
                MessageBox.Show("Mã khách hàng không được ghi dấu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(txtMaKH.Text.Length == 5))
            {
                MessageBox.Show("Mã khách hàng phải có độ dài 5 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTenKH.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên khách hàng phải chỉ chứa chữ ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!(txtTenKH.Text.Length >= 1 && txtTenKH.Text.Length <= 32))
            {
                MessageBox.Show("Tên khách hàng phải có độ dài từ 1 đến 32 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(txtSDT.Text.Length == 10) || !txtSDT.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải đúng 10 số và không phải là chữ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void CaiDatMacDinh()
        {
            txtMaKH.Text = txtTenKH.Text = txtTenKH.Text = txtSDT.Text = txtDiaChi.Text = string.Empty;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTra())
            {
                using (var dbContext = new QLBXDBcontext())
                {

                    if (dbContext.KhachHang.Any(KhachHang => KhachHang.MaKH == txtMaKH.Text))
                    {
                        MessageBox.Show("Mã khách hàng đã tồn tại. Vui lòng chọn mã khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Thêm nhân viên mới vào DataGridView
                    int index = dgvKhachHang.Rows.Add();
                    dgvKhachHang.Rows[index].Cells[0].Value = txtMaKH.Text.ToString();
                    dgvKhachHang.Rows[index].Cells[1].Value = txtTenKH.Text.ToString();
                    dgvKhachHang.Rows[index].Cells[2].Value = txtSDT.Text.ToString();
                    dgvKhachHang.Rows[index].Cells[3].Value = txtDiaChi.Text.ToString();

                    // Chọn dòng mới được thêm
                    dgvKhachHang.ClearSelection();
                    dgvKhachHang.Rows[index].Selected = true;
                    dgvKhachHang.FirstDisplayedScrollingRowIndex = index;

                    // Thiết lập trạng thái mặc định và kích hoạt nút "Lưu" và "Không Lưu"
                    CaiDatMacDinh();
                    btnLuu.Enabled = true;
                    btnKhongLuu.Enabled = true;

                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng muốn xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int selectedrow = GetSelectedRow(txtMaKH.Text);
                if (selectedrow == -1)
                {
                    throw new Exception("Không tìm thấy khách hàng cần xóa");
                }
                else
                {
                    DialogResult dg = MessageBox.Show("Bạn có chắc muốn xóa", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dg == DialogResult.Yes)
                    {
                        using (var dbContext = new QLBXDBcontext())
                        {
                            KhachHang khachHang = dbContext.KhachHang.FirstOrDefault(x => x.MaKH == txtMaKH.Text.ToString());
                            dbContext.KhachHang.Remove(khachHang);
                            dbContext.SaveChanges();
                        }
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                        CaiDatMacDinh();
                        LoadKhachHang();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSelectedRow(string MaKH)
        {
            for (int i = 0; i < dgvKhachHang.Rows.Count; i++)
            {
                if (dgvKhachHang.Rows[i].Cells[0].Value.ToString() == MaKH)
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTra())
            {
                using (var dbcontext = new QLBXDBcontext())
                {
                    KhachHang khachHang = new KhachHang()
                    {
                        MaKH = txtMaKH.Text.ToString(),
                        TenKH = txtTenKH.Text.ToString(),
                        SDT = txtSDT.Text.ToString(),
                        DiaChi = txtDiaChi.Text.ToString(),
                    };
                    dbcontext.KhachHang.AddOrUpdate(khachHang);
                    dbcontext.SaveChanges();
                }
                CaiDatMacDinh();
                LoadKhachHang();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenKH = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(TenKH))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dbContext = new QLBXDBcontext())
            {
                KhachHang kh = dbContext.KhachHang.FirstOrDefault(x => x.TenKH == TenKH);

                if (kh != null)
                {
                    dgvKhachHang.Rows.Clear();
                    int index = dgvKhachHang.Rows.Add();
                    dgvKhachHang.Rows[index].Cells[0].Value = kh.MaKH;
                    dgvKhachHang.Rows[index].Cells[1].Value = kh.TenKH;
                    dgvKhachHang.Rows[index].Cells[2].Value = kh.SDT;
                    dgvKhachHang.Rows[index].Cells[3].Value = kh.DiaChi;

                    MessageBox.Show("Đã tìm thấy khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng có tên " + TenKH, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnQL_Click(object sender, EventArgs e)
        {
            LoadKhachHang();
            CaiDatMacDinh();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (KiemTra())
            {
                using (var dbContext = new QLBXDBcontext())
                {

                    if (dbContext.KhachHang.Any(KhachHang => KhachHang.MaKH == txtMaKH.Text))
                    {
                        MessageBox.Show("Mã khách hàng đã tồn tại. Vui lòng chọn mã khách hàng khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    KhachHang khachHang = new KhachHang()
                    {
                        MaKH = txtMaKH.Text.ToString(),
                        TenKH = txtTenKH.Text.ToString(),
                        SDT = txtSDT.Text.ToString(),
                        DiaChi = txtDiaChi.Text.ToString(),
                    };
                    dbContext.KhachHang.Add(khachHang);
                    dbContext.SaveChanges();
                }
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnKhongLuu.Enabled = false;
                CaiDatMacDinh();
                LoadKhachHang();
            }

        }

        private void btnKhongLuu_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                dgvKhachHang.Rows.Remove(dgvKhachHang.SelectedRows[0]);
                CaiDatMacDinh();
            }
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                this.Close();
            }
        }

        
    }
}