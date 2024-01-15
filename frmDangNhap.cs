﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace QuanLyChuoiBanXoi
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            frmDangKy dangKy = new frmDangKy();
            dangKy.ShowDialog();
        }

        private void linkLabel_QuenMk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmQuenMatKhau quenMatKhau = new frmQuenMatKhau();  
            quenMatKhau.ShowDialog();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        Modify modify = new Modify();

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tentk = txtTenTaiKhoan.Text;
            string matkhau = MaHoaMD5(txtMatKhau.Text); 
            if (tentk.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTaiKhoan.Focus();
            }
            else if (matkhau.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
            }
            else
            {
                string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "' and MatKhau = '" + matkhau + "' ";
                if (modify.TaiKhoans(query).Count > 0)
                {
                    this.Hide();
                    frmGiaoDienChinh giaoDienChinh = new frmGiaoDienChinh();
                    giaoDienChinh.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không chính xác!", "Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    txtTenTaiKhoan.Focus(); // cho con trỏ chuột quay lại ô tk 
                }
            }          
        }
        private void An_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '*')
            {
                Hien.BringToFront();
                txtMatKhau.PasswordChar = '\0';
            }
        }

        private void Hien_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '\0')
            {
                An.BringToFront();
                txtMatKhau.PasswordChar = '*';
            }
        }

        private string MaHoaMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi đầu vào thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Mã hóa mảng byte
                byte[] hashedBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte đã mã hóa thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
