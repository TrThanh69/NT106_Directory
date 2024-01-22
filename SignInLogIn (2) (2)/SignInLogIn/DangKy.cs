using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TuDien
{
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }
        Modify modify = new Modify();
        private void btnNext_Click(object sender, EventArgs e)
        {
            string tentk = richTextBox1.Text;
            string matkhau = textBox1.Text;  
            string email = richTextBox3.Text;
            if (!KiemTra(tentk)) 
            { 
                MessageBox.Show("Vui lòng nhập tên tài khoản dài 6-24 ký tự, với các ký tự chữ và số, chữ hoa và thường!");
                return;
            }
            if (!KiemTra(matkhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu dài 6-24 ký tự, với các ký tự chữ và số, chữ hoa và thường!");
                return;
            }
            if (!KiemTra(matkhau))
            {
                    MessageBox.Show("Vui lòng nhập mật khẩu dài 6-24 ký tự, với các ký tự chữ và số, chữ hoa và thường!");
                    return;
            }
            if (!KiemTraEMail(email))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng email!");
                return;
            }
            if(modify.TaiKhoans("Select * from TaiKhoan where Email = '" + email + "'").Count != 0)
            {
                MessageBox.Show("Email này đã được dùng!");
            }
            try
            {
                string query = "Insert into TaiKhoan values ('"+tentk+"','"+matkhau+"','"+email+"')";
                modify.Command(query);

            }
            catch
            {
                MessageBox.Show("Tên tài khoản này đã được đăng ký, vui lòng đăng ký mới!");
            }


        }
        public bool  KiemTra(string ac)
        {
            return Regex.IsMatch(ac, "^[a-zA-Z0-9]{6,24}$");
        }
        public bool KiemTraEMail(string em)
        {
            return Regex.IsMatch(em, @"^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = true;  
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
