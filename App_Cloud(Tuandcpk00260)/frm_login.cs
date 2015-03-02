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
using System.Data.SqlClient;
using System.Xml;
using System.Web.Services;


namespace App_Cloud_Tuandcpk00260_
{
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();

        }
        localhost.bus_data datas = new localhost.bus_data();

        private void frm_login_Load(object sender, EventArgs e)
        {
        
        }
                public string address1;
                public string dataname1;
                public string username1;
                public string password1;

                
        public static string nguoidangnhap;
        public static string manv;
        public static string quyenhan;
  
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Regex regex;
            regex = new Regex(@"\S+@\S+\.\S+");
            Control ctrl = (Control)sender;
            if (textBox1.TextLength >= 1)
            {
                lblCheck1.Visible = true;
            }
            else
            {
                lblCheck1.Visible = false;
            }

            if (regex.IsMatch(ctrl.Text))
            {
                lblCheck1.Text = "Email hợp lệ !";
                lblCheck1.ForeColor = Color.Green;
            }
            else
            {
                lblCheck1.Text = "Email không hợp lệ !";
                lblCheck1.ForeColor = Color.Red;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength >= 1)
            {
                lblCheck2.Visible = true;
            }
            else
            {
                lblCheck2.Visible = false;
            }

            if (textBox2.TextLength < 6)
            {
                lblCheck2.Text = "Vui lòng nhập mật mã lớn hơn 6 ký tự";
                lblCheck2.ForeColor = Color.Red;
            }
            else
            {
                lblCheck2.Text = "Mật khẩu đủ độ dài !";
                lblCheck2.ForeColor = Color.Green;
            }
        }
        private void dowork()
        {
            localhost.bus_data getdata = new localhost.bus_data();
            DataSet ds = new DataSet();
            string user = textBox1.Text;
            string pass = "";
            string changes = "";
            string hide = "";
            pass = md5(textBox2.Text);
            splashScreenManager1.ShowWaitForm();
            ds = getdata.getdata("select * from tbl_NhanVien where EMAIL like '%" + user + "%' and PASSWORDS like '%" + pass + "%'");
            DataTable dt = new DataTable();
            splashScreenManager1.CloseWaitForm();
            dt = ds.Tables[0];
            if (string.Compare(lblCheck1.Text, "Email hợp lệ !") == 0 && string.Compare(lblCheck2.Text, "Mật khẩu đủ độ dài !") == 0)
            {
                try
                {
                    if (dt.Rows.Count == 1)
                    {
                        int i = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            hide = (row["HideNV"].ToString());
                            manv = (row["MANV"].ToString());
                            nguoidangnhap = (row["TENNV"].ToString());
                            quyenhan = (row["QH"].ToString());
                            changes = (row["CHANGPAS"].ToString());
                            i++;
                        }
                        if (changes == "True")
                        {
                            MessageBox.Show("Bạn được yêu cầu đổi mật khẩu, vui lòng nhấn OK để thay đổi", "Yêu cầu đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            frm_changesPass frm = new frm_changesPass();
                            frm.Activate();
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            if (hide == "True")
                            {
                                MessageBox.Show("Tài khoản đã bị khóa, vui lòng hệ Admin", "Tài khoản đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                frm_loading frm3 = new frm_loading();
                                frm3.Activate();
                                frm3.Show();
                                this.Hide();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không đăng nhập được, vui lòng kiểm tra email hoặc mật khẩu.", "Không đăng nhập được", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Có lỗi khi tải dữ liệu, vui lòng kiểm tra lại", "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lỗi đăng nhập", "Vui lòng kiểm tra lại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                  
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            dowork();        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Dispose();
        }


    }
}
