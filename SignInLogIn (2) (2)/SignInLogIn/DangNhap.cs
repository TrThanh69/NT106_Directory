
using System;
using System.Windows.Forms;

namespace TuDien
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string Email = richTextBox1.Text;
            string MK = textBox1.Text;
            if (Email.Trim() == " ") { MessageBox.Show("Vui lòng nhập tên tài khoản"); }
            else if (MK.Trim() == " ") { MessageBox.Show("Vui lòng nhập tên tài khoản"); }
            else
            {
                string query = "Select * from  TaiKhoan where Email ='" + Email + "' and MatKhau ='" + MK + "'";
                if (modify.TaiKhoans(query).Count != 0)
                {
                    MessageBox.Show("Đăng nhập thành công ");
                    TuDien td = new TuDien();
                    td.Show();
                }
                else
                {
                    MessageBox.Show("Tên tài khoản hoặc Mật khẩu không chính xác!");
                }
            }

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            DangKy dk = new DangKy();
            dk.Show();
        }
        Modify modify = new Modify();

        private void btnGuest_Click(object sender, EventArgs e)
        {
            SignInLogIn.Text_translator obj = new SignInLogIn.Text_translator();
            obj.Show();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = true;  
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
