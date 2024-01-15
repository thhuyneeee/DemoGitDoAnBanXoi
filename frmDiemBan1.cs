﻿using DevExpress.XtraBars.Docking.Helpers;
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
    public partial class frmDiemBan1 : DevExpress.XtraEditors.XtraForm
    {
        public frmDiemBan1()
        {
            InitializeComponent();
        }

        private void frmDiemBan1_Load(object sender, EventArgs e)
        {
            LoadDiemBan1();
        }

        private void LoadDiemBan1()
        {
            dgvDiem1.Rows.Clear();
            using (var dbContext = new QLBXDBcontext())
            {
                foreach (HoaDon hd in dbContext.HoaDon.Where(h => h.MaDB == "DB01"))
                {
                    int index = dgvDiem1.Rows.Add();
                    dgvDiem1.Rows[index].Cells[0].Value = hd.MaHD;
                    dgvDiem1.Rows[index].Cells[1].Value = hd.MaDB;
                    dgvDiem1.Rows[index].Cells[2].Value = hd.MaNV;
                    dgvDiem1.Rows[index].Cells[3].Value = hd.MaKH;
                    dgvDiem1.Rows[index].Cells[4].Value = hd.NgayLap;
                    var CTHD = dbContext.CTHD.FirstOrDefault(x => x.MaHD == hd.MaHD);
                    if (CTHD != null)
                    {
                        decimal gia = CTHD.Gia; // Nếu giá là null thì giả sử là 0
                        int soLuong = CTHD.SoLuong; // Nếu số lượng là null thì giả sử là 0
                        int giaInt = (int)gia;
                        // Tính thành tiền
                        int thanhTien = giaInt * soLuong;
                        dgvDiem1.Rows[index].Cells[5].Value = thanhTien;
                    }
                }
            }
        }

        private void dgvDiem1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var dbContext = new QLBXDBcontext())
            {
                int index = e.RowIndex;
                string MaHD = dgvDiem1.Rows[index].Cells[0].Value?.ToString();
                string MaDB = dgvDiem1.Rows[index].Cells[1].Value?.ToString();
                string MaNV = dgvDiem1.Rows[index].Cells[2].Value?.ToString();
                string MaKhachHang = dgvDiem1.Rows[index].Cells[3].Value?.ToString();
                DateTime? NgayLap = dgvDiem1.Rows[index].Cells[4].Value as DateTime?;
                string ThanhTien = dgvDiem1.Rows[index].Cells[5].Value?.ToString();
                if (!string.IsNullOrEmpty(MaHD))
                {
                    HoaDon hd = dbContext.HoaDon.FirstOrDefault(x => x.MaHD == MaHD);
                    txtMaHD.Text = MaHD;
                    txtMaDB.Text = MaDB;
                    txtMaNV.Text = MaNV;
                    txtMaKhachHang.Text = MaKhachHang;
                    dtpNgayLap.Value = NgayLap ?? DateTime.Now;

                }
            }
            txtMaHD.Enabled = false;
            txtMaDB.Enabled = false;
        }
        private void CaiDatMacDinh()
        {
            txtMaDB.Text = txtMaHD.Text = txtMaKhachHang.Text = txtMaNV.Text  = string.Empty;
            dtpNgayLap.Value = DateTime.Now;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDiem1.SelectedRows.Count > 0)
            {

                string maHD = dgvDiem1.SelectedRows[0].Cells[0].Value.ToString();
                using (var dbContext = new QLBXDBcontext())
                {
                    var hoaDon = dbContext.HoaDon.FirstOrDefault(h => h.MaHD == maHD && h.MaDB == "DB01");
                    DialogResult dg = MessageBox.Show("Bạn có chắc muốn xóa", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (hoaDon != null && dg == DialogResult.Yes)
                    {
                        dbContext.HoaDon.Remove(hoaDon);
                        dbContext.SaveChanges();
                        MessageBox.Show("Xóa hóa đơn thành công", "Thông báo", MessageBoxButtons.OK);
                        CaiDatMacDinh();
                        LoadDiemBan1();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn muốn xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool KiemTra()
        {
            if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtMaDB.Text) || string.IsNullOrEmpty(txtMaNV.Text) || string.IsNullOrEmpty(dtpNgayLap.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin hoặc chọn hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTra())
            {
                using (var dbcontext = new QLBXDBcontext())
                {
                    string maHD = txtMaHD.Text.ToString();
                    var hoaDon = dbcontext.HoaDon.FirstOrDefault(hd => hd.MaHD == maHD);
                    if (hoaDon != null)
                    {
                        hoaDon.MaDB = txtMaDB.Text.ToString();
                        hoaDon.MaKH = txtMaKhachHang.Text.ToString();
                        hoaDon.NgayLap = dtpNgayLap.Value;
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn có mã " + maHD, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                CaiDatMacDinh();
                LoadDiemBan1();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string timKiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(timKiem))
            {
                MessageBox.Show("Vui lòng nhập thông tin cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dbContext = new QLBXDBcontext())
            {
                HoaDon hd = dbContext.HoaDon.FirstOrDefault(x => x.MaHD == timKiem && x.MaDB == "DB01");

                if (hd != null)
                {
                    dgvDiem1.Rows.Clear();
                    int index = dgvDiem1.Rows.Add();
                    dgvDiem1.Rows[index].Cells[0].Value = hd.MaHD;
                    dgvDiem1.Rows[index].Cells[1].Value = hd.MaDB;
                    dgvDiem1.Rows[index].Cells[2].Value = hd.MaNV;
                    dgvDiem1.Rows[index].Cells[3].Value = hd.MaKH;
                    dgvDiem1.Rows[index].Cells[4].Value = hd.NgayLap;
                    dgvDiem1.Rows[index].Cells[5].Value = hd.ThanhTien;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy " + timKiem, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnQL_Click(object sender, EventArgs e)
        {
            LoadDiemBan1();
        }

        private void btnChitiet_Click(object sender, EventArgs e)
        {
            if (dgvDiem1.SelectedRows.Count > 0)
            {
                int index = dgvDiem1.SelectedRows[0].Index;
                string MaHD = dgvDiem1.Rows[index].Cells[0].Value?.ToString();
                frmChiTietHD frmChiTietHD = new frmChiTietHD(MaHD);
                frmChiTietHD.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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