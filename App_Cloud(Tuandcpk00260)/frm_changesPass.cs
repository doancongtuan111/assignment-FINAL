using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Cloud_Tuandcpk00260_
{
    public partial class frm_changesPass : Form
    {
        public frm_changesPass()
        {
            InitializeComponent();
        }

        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        string manv = frm_login.manv;
        private void frm_changesPass_Load(object sender, EventArgs e)
        {

        }

        localhost.bus_data webservices = new localhost.bus_data();
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string pass = "";
            pass = md5(txtpass2.Text);

            if (txtpass1.Text == txtpass2.Text)
            {
                if (txtpass1.Text.Length >= 6)
                {
                    string trangthai = "";
                    trangthai = webservices.excutedata("update tbl_NhanVien set PASSWORDS = '" + pass + "', CHANGPAS = 'False' where MANV = '" + manv + "'");
                    if (trangthai == "OK")
                    {
                        MessageBox.Show("OK");
                        frm_login frm = new frm_login();
                        frm.Activate();
                        frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi");
                    }
                }
                else
                {
                    MessageBox.Show("lớn hơn 6 ký tự");
                }
            }
            else
            {
                MessageBox.Show("Hai mật khẩu không khớp.");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
