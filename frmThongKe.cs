using DevExpress.XtraEditors;
using QuanLyChuoiBanXoi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace QuanLyChuoiBanXoi
{
    public partial class frmThongKe : DevExpress.XtraEditors.XtraForm
    {
        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadThongKe();
            using (var dbContext = new QLBXDBcontext())
            {
                tongTien = dbContext.HoaDon
                                         .SelectMany(hd => dbContext.CTHD.Where(ct => ct.MaHD == hd.MaHD))
                                         .Sum(ct => (ct.Gia) * (ct.SoLuong));
                txtTongTien.Text = tongTien.ToString();
            }
        }
        private void LoadThongKe()
        {
            dgvThongKe.Rows.Clear();
            using (var dbContext = new QLBXDBcontext())
            {
                foreach (HoaDon hd in dbContext.HoaDon)
                {
                    int index = dgvThongKe.Rows.Add();
                    dgvThongKe.Rows[index].Cells[0].Value = hd.MaHD;
                    dgvThongKe.Rows[index].Cells[1].Value = hd.MaDB;
                    dgvThongKe.Rows[index].Cells[2].Value = hd.MaNV;
                    dgvThongKe.Rows[index].Cells[3].Value = hd.MaKH;
                    dgvThongKe.Rows[index].Cells[4].Value = hd.NgayLap;
                    var CTHD = dbContext.CTHD.FirstOrDefault(x => x.MaHD == hd.MaHD);
                    if (CTHD != null)
                    {
                        decimal gia = CTHD.Gia; // Nếu giá là null thì giả sử là 0
                        int soLuong = CTHD.SoLuong; // Nếu số lượng là null thì giả sử là 0
                        int giaInt = (int)gia;
                        // Tính thành tiền
                        int thanhTien = giaInt * soLuong;
                        dgvThongKe.Rows[index].Cells[5].Value = thanhTien;
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            daNhanTimKiem = true;
            string keyword = dtpNgayLap.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập thông tin cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime searchDate;
            if (!DateTime.TryParse(keyword, out searchDate))
            {
                MessageBox.Show("Ngày không hợp lệ. Vui lòng nhập ngày đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime nextDate = searchDate.AddDays(1);
            using (var dbContext = new QLBXDBcontext())
            {
                var hds = dbContext.HoaDon.Where(x => x.NgayLap >= searchDate && x.NgayLap < nextDate).ToList();
                if (hds.Any())
                {
                    dgvThongKe.Rows.Clear();
                    foreach (var hd in hds)
                    {
                        int index = dgvThongKe.Rows.Add();
                        dgvThongKe.Rows[index].Cells[0].Value = hd.MaHD;
                        dgvThongKe.Rows[index].Cells[1].Value = hd.MaDB;
                        dgvThongKe.Rows[index].Cells[2].Value = hd.MaNV;
                        dgvThongKe.Rows[index].Cells[3].Value = hd.MaKH;
                        dgvThongKe.Rows[index].Cells[4].Value = hd.NgayLap;
                        //dgvThongKe.Rows[index].Cells[5].Value = hd.ThanhTien;
                        var CTHD = dbContext.CTHD.FirstOrDefault(x => x.MaHD == hd.MaHD);
                        if (CTHD != null)
                        {
                            decimal gia = CTHD.Gia; // Nếu giá là null thì giả sử là 0
                            int soLuong = CTHD.SoLuong; // Nếu số lượng là null thì giả sử là 0

                            int giaInt = (int)gia;

                            // Tính thành tiền
                            int thanhTien = giaInt * soLuong;
                            dgvThongKe.Rows[index].Cells[5].Value = thanhTien;
                        }
                        btnTongTien.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn nào trong ngày " + searchDate.ToString("dd/MM/yyyy"), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTongTien.Enabled = false;
                    txtTongTien.Text = string.Empty;
                }
            }
        }

        private bool daNhanTimKiem = false;
        decimal tongTien = 0;
        private void btnTongTien_Click(object sender, EventArgs e)
        {


            using (var dbContext = new QLBXDBcontext())
            {
                decimal tongTien = 0;

                if (!daNhanTimKiem)
                {
                    // Tính tổng thành tiền cho tất cả các hóa đơn
                    tongTien = dbContext.HoaDon
                                         .SelectMany(hd => dbContext.CTHD.Where(ct => ct.MaHD == hd.MaHD))
                                         .Sum(ct => (ct.Gia) * (ct.SoLuong));
                }
                else
                {
                    string keyword = dtpNgayLap.Text.Trim();
                    if (string.IsNullOrWhiteSpace(keyword))
                    {
                        MessageBox.Show("Vui lòng nhập thông tin cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!DateTime.TryParse(keyword, out DateTime searchDate))
                    {
                        MessageBox.Show("Ngày không hợp lệ. Vui lòng nhập ngày đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DateTime nextDate = searchDate.AddDays(1);

                    // Tính tổng thành tiền cho các hóa đơn trong khoảng thời gian cụ thể
                    tongTien = dbContext.HoaDon
                                         .Where(x => x.NgayLap >= searchDate && x.NgayLap < nextDate)
                                         .SelectMany(hd => dbContext.CTHD.Where(ct => ct.MaHD == hd.MaHD))
                                         .Sum(ct => (ct.Gia) * (ct.SoLuong));
                }

                txtTongTien.Text = tongTien.ToString();
            }
        }

        private void btnQL_Click_2(object sender, EventArgs e)
        {
            LoadThongKe();
            dtpNgayLap.Value = DateTime.Now;
            daNhanTimKiem = false;
            btnTongTien.Enabled = true;
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