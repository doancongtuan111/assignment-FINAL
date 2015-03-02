using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;


namespace App_Cloud_Tuandcpk00260_
{
    public partial class frm_main : Form
    {
        public frm_main()
        {
            InitializeComponent();
        }

        localhost.bus_data connectdata = new localhost.bus_data();
        public static string manv;
        public static string ngay;

        //Khai báo các biến User controll
        usc_banhang banhang = new usc_banhang();
        usc_hoadon hoadon = new usc_hoadon();
        usc_khachhang khachhang = new usc_khachhang();
        usc_nhanvien nhanvien = new usc_nhanvien();
        usc_sanpham sanpham = new usc_sanpham();

        private void frm_main_Load(object sender, EventArgs e)
        {
            //Hiển thị tên người đăng nhập
            timer1.Start();
            toolnguoidangnhap.Text = frm_login.nguoidangnhap;
            lblmsnv.Text = frm_login.manv;
            //manv = lblmsnv.Text;
            ngay = newToolStripMenuItem.Text;


            switch (frm_login.quyenhan)
            {
                case "BH":

                    loadbanhang();
                    toolbanhang.Enabled = true;////
                    toolqlhd.Enabled = false;
                    toolqlhd2.Enabled = false;
                    toolqlhh.Enabled = false;
                    toolqlhh2.Enabled = false;
                    toolqlkh.Enabled = false;
                    toolqlkh2.Enabled = false;
                    toolqlnv.Enabled = false;
                    toolqlnv2.Enabled = false;                      
                    break;
                case "HD":
                    loadhoadon();
                    toolbanhang.Enabled = false;
                    toolqlhd.Enabled = true;/////
                    toolqlhd2.Enabled = true;/////
                    toolqlhh.Enabled = false;
                    toolqlhh2.Enabled = false;
                    toolqlkh.Enabled = false;
                    toolqlkh2.Enabled = false;
                    toolqlnv.Enabled = false;
                    toolqlnv2.Enabled = false;                      
                    break;
                case "HH":
                    loadsanpham();
                    toolbanhang.Enabled = false;
                    toolqlhd.Enabled =  false;
                    toolqlhd2.Enabled = false;
                    toolqlhh.Enabled = true;/////
                    toolqlhh2.Enabled = true;/////
                    toolqlkh.Enabled = false;
                    toolqlkh2.Enabled = false;
                    toolqlnv.Enabled = false;
                    toolqlnv2.Enabled = false;
                    break;
                case "KH":
                    loadkhachhang();
                    toolbanhang.Enabled = false;
                    toolqlhd.Enabled = false;
                    toolqlhd2.Enabled = false;
                    toolqlhh.Enabled = false;
                    toolqlhh2.Enabled = false;
                    toolqlkh.Enabled = true;/////
                    toolqlkh2.Enabled = true;/////
                    toolqlnv.Enabled = false;
                    toolqlnv2.Enabled = false;
                    break;
                case "NV":
                    loadnhanvien();
                    toolbanhang.Enabled = false;
                    toolqlhd.Enabled = false;
                    toolqlhd2.Enabled = false;
                    toolqlhh.Enabled = false;
                    toolqlhh2.Enabled = false;
                    toolqlkh.Enabled = false;
                    toolqlkh2.Enabled = false;
                    toolqlnv.Enabled = true;/////
                    toolqlnv2.Enabled = true;/////
                    break;
                case "admin":
                    loadbanhang();
                    toolbanhang.Enabled = true;/////
                    toolqlhd.Enabled = true;/////
                    toolqlhd2.Enabled = true;/////
                    toolqlhh.Enabled = true;/////
                    toolqlhh2.Enabled = true;/////
                    toolqlkh.Enabled = true;/////
                    toolqlkh2.Enabled = true;/////
                    toolqlnv.Enabled = true;/////
                    toolqlnv2.Enabled = true;/////
                    break;
            }

        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Kết thúc ứng dụng và giải phóng bộ nhớ
            Application.Exit();
            Dispose();      
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            newToolStripMenuItem.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }


        private void frm_main_Resize(object sender, EventArgs e)
        {
            // Nếu Form đang Minimize thì ẩn luôn Form
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Hiển thị lại Form nếu doubleclick vào icon dưới System tray
            Show();
            WindowState = FormWindowState.Normal;
        }

        //Phần dành cho các menu ở Icon ẩn
        private void thoátToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Focus();
        }

        private void tắtỨngDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Dispose();
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadkhachhang();
        }

        private void quảnLýKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadkhachhang();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadsanpham();
        }

        private void quảnLýHàngHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadsanpham();
        }

        private void toolqlhd_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadhoadon();
        }

        private void toolqlnv_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadnhanvien();
        }

        private void toolqlhd2_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadhoadon();
        }

        private void toolqlnv2_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadnhanvien();
        }

        private void removecontrol()
        {
            this.Controls.Remove(banhang);
            this.Controls.Remove(hoadon);
            this.Controls.Remove(khachhang);
            this.Controls.Remove(nhanvien);
            this.Controls.Remove(sanpham);
        }

        private void toolbanhang_Click(object sender, EventArgs e)
        {
            removecontrol();
            loadbanhang();
        }

        private void loadbanhang()
        {
            splashScreenManager1.ShowWaitForm();
            banhang.Location = new Point(0,49);
            this.Controls.Add(banhang);
            splashScreenManager1.CloseWaitForm();
        }

        private void loadhoadon()
        {
            splashScreenManager1.ShowWaitForm();
            hoadon.Location = new Point(0, 49);
            this.Controls.Add(hoadon);
            splashScreenManager1.CloseWaitForm();
        }

        private void loadkhachhang()
        {
            splashScreenManager1.ShowWaitForm();
            khachhang.Location = new Point(0, 49);
            this.Controls.Add(khachhang);
            splashScreenManager1.CloseWaitForm();
        }

        private void loadnhanvien()
        {
            splashScreenManager1.ShowWaitForm();
            nhanvien.Location = new Point(0, 49);
            this.Controls.Add(nhanvien);
            splashScreenManager1.CloseWaitForm();
        }

        private void loadsanpham()
        {
            splashScreenManager1.ShowWaitForm();
            sanpham.Location = new Point(0, 49);
            this.Controls.Add(sanpham);
            splashScreenManager1.CloseWaitForm();
        }

        private void toolslogin_again_Click(object sender, EventArgs e)
        {
            frm_login lg = new frm_login();
            lg.Show();
            this.Hide();
            this.Dispose();

        }

        private void saoLưuDữLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string backupDIR = "D:\\BackupDB";
            string cmd = @"backup database DB_9BB044_tuandc to disk='" + backupDIR + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak' WITH FORMAT";
            string status;
            status = connectdata.excutedata(cmd);
            if (status == "OK")
            {
                MessageBox.Show("Backup thành công, File backup được lưu tại ổ đĩa D");
            }
        }

     }
}
