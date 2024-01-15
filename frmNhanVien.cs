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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyChuoiBanXoi
{
    public partial class frmNhanVien : DevExpress.XtraEditors.XtraForm
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void LoadNhanVien()
        {
            dgvNV.Rows.Clear();
            using (var dbContext = new QLBXDBcontext())
            {
                foreach (NhanVien nv in dbContext.NhanVien)
                {
                    int index = dgvNV.Rows.Add();
                    dgvNV.Rows[index].Cells[0].Value = nv.MaNV;
                    dgvNV.Rows[index].Cells[1].Value = nv.TenNV;
                    dgvNV.Rows[index].Cells[2].Value = nv.SDT;
                    dgvNV.Rows[index].Cells[3].Value = nv.DiaChi;
                }
            }
        }
        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var dbContext = new QLBXDBcontext())
            {
                int index = e.RowIndex;
                string maNV = dgvNV.Rows[index].Cells[0].Value.ToString();
                NhanVien nhanvien = dbContext.NhanVien.FirstOrDefault(x => x.MaNV == maNV);
                txtMaNV.Text = dgvNV.Rows[index].Cells[0].Value.ToString();
                txtTenNV.Text = dgvNV.Rows[index].Cells[1].Value.ToString();
                txtSDT.Text = dgvNV.Rows[index].Cells[2].Value.ToString();
                txtDiaChi.Text = dgvNV.Rows[index].Cells[3].Value.ToString();
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnKhongLuu.Enabled = true;
            }
        }
        private bool KiemTra()
        {
            if (string.IsNullOrEmpty(txtMaNV.Text) || string.IsNullOrEmpty(txtTenNV.Text) || string.IsNullOrEmpty(txtSDT.Text) || string.IsNullOrEmpty(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtMaNV.Text.Any(char.IsWhiteSpace))
            {
                MessageBox.Show("Mã nhân viên không được ghi dấu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(txtMaNV.Text.Length == 5))
            {
                MessageBox.Show("Mã nhân viên phải có độ dài 5 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTenNV.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên nhân viên phải chỉ chứa chữ ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ( !(txtTenNV.Text.Length >= 1 && txtTenNV.Text.Length <= 32))
            {
                MessageBox.Show("Tên nhân viên phải có độ dài từ 1 đến 32 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!(txtSDT.Text.Length == 10) || !txtSDT.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải đúng 10 số và không phải là chữ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTra())
            {
                using (var dbContext = new QLBXDBcontext())
                {
                    if (dbContext.NhanVien.Any(NhanVien => NhanVien.MaNV == txtMaNV.Text))
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng chọn mã nhân viên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Thêm nhân viên mới vào DataGridView
                    int index = dgvNV.Rows.Add();
                    dgvNV.Rows[index].Cells[0].Value = txtMaNV.Text.ToString();
                    dgvNV.Rows[index].Cells[1].Value = txtTenNV.Text.ToString();
                    dgvNV.Rows[index].Cells[2].Value = txtSDT.Text.ToString();
                    dgvNV.Rows[index].Cells[3].Value = txtDiaChi.Text.ToString();

                    // Chọn dòng mới được thêm
                    dgvNV.ClearSelection();
                    dgvNV.Rows[index].Selected = true;
                    dgvNV.FirstDisplayedScrollingRowIndex = index;

                    // Thiết lập trạng thái mặc định và kích hoạt nút "Lưu" và "Không Lưu"
                    CaiDatMacDinh();
                    btnLuu.Enabled = true;
                    btnKhongLuu.Enabled = true;
                }
            }

        }

        private void CaiDatMacDinh()
        {
            txtMaNV.Text = txtTenNV.Text = txtTenNV.Text = txtSDT.Text = txtDiaChi.Text = string.Empty;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên muốn xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int selectedrow = GetSelectedRow(txtMaNV.Text);
                if (selectedrow == -1)
                {
                    throw new Exception("Không tìm thấy nhân viên cần xóa");
                }
                else
                {
                    DialogResult dg = MessageBox.Show("Bạn có chắc muốn xóa", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dg == DialogResult.Yes)
                    {
                        using (var dbContext = new QLBXDBcontext())
                        {
                            NhanVien nhanVien = dbContext.NhanVien.FirstOrDefault(x => x.MaNV == txtMaNV.Text.ToString());
                            dbContext.NhanVien.Remove(nhanVien);
                            dbContext.SaveChanges();
                        }
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                        CaiDatMacDinh();
                        LoadNhanVien();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private int GetSelectedRow(string MaNV)
        {
            for (int i = 0; i < dgvNV.Rows.Count; i++)
            {
                if (dgvNV.Rows[i].Cells[0].Value.ToString() == MaNV)
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
                    NhanVien nhanVien = new NhanVien()
                    {
                        MaNV = txtMaNV.Text.ToString(),
                        TenNV = txtTenNV.Text.ToString(),
                        SDT = txtSDT.Text.ToString(),
                        DiaChi = txtDiaChi.Text.ToString(),
                    };
                    dbcontext.NhanVien.AddOrUpdate(nhanVien);
                    dbcontext.SaveChanges();
                }
                CaiDatMacDinh();
                LoadNhanVien();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var dbContext = new QLBXDBcontext())
            {
                NhanVien nv = dbContext.NhanVien.FirstOrDefault(x => x.MaNV == keyword);

                if (nv != null)
                {
                    dgvNV.Rows.Clear();
                    int index = dgvNV.Rows.Add();
                    dgvNV.Rows[index].Cells[0].Value = nv.MaNV;
                    dgvNV.Rows[index].Cells[1].Value = nv.TenNV;
                    dgvNV.Rows[index].Cells[2].Value = nv.SDT;
                    dgvNV.Rows[index].Cells[3].Value = nv.DiaChi;

                    MessageBox.Show("Đã tìm thấy nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên với mã " + keyword, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnQL_Click(object sender, EventArgs e)
        {
            LoadNhanVien();
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

                    if (dbContext.NhanVien.Any(NhanVien => NhanVien.MaNV == txtMaNV.Text))
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng chọn mã nhân viên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    NhanVien nv = new NhanVien()
                    {
                        MaNV = txtMaNV.Text.ToString(),
                        TenNV = txtTenNV.Text.ToString(),
                        SDT = txtSDT.Text.ToString(),
                        DiaChi = txtDiaChi.Text.ToString(),
                    };
                    dbContext.NhanVien.Add(nv);
                    dbContext.SaveChanges();
                }
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnKhongLuu.Enabled = false;
                CaiDatMacDinh();
                LoadNhanVien();
            }

        }

        private void btnKhongLuu_Click(object sender, EventArgs e)
        {
            if (dgvNV.SelectedRows.Count > 0)
            {
                dgvNV.Rows.Remove(dgvNV.SelectedRows[0]);
                CaiDatMacDinh();
            }
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
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