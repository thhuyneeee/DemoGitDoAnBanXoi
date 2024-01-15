using System;
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
    public partial class cmd5 : Form
    {
        public cmd5()
        {
            InitializeComponent();
        }
        MD5 md = MD5.Create();
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] inputstr = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);
            byte[] hash = md.ComputeHash(inputstr);
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));

            }
            textBox2.Text = sb.ToString();
        }
    }
}
