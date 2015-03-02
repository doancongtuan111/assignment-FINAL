using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Cloud_Tuandcpk00260_
{
    public partial class usc_nhanvien : UserControl
    {
        public usc_nhanvien()
        {
            InitializeComponent();
        }

        //Khởi tạo webservices  ############################################################
        localhost.bus_data webservices = new localhost.bus_data();

        private void usc_nhanvien_Load(object sender, EventArgs e)
        {
            laydulieu();
            loaddata();
            loadchart();
        }


        //Lấy dữ liệu từ services   ############################################################
        DataSet ds = new DataSet();
        private void laydulieu()
        {
            ds = webservices.getdata("Select * from tbl_NhanVien");
        }

        //Đổ dữ liệu lên listview   ############################################################
        DataTable dt = new DataTable();
        private void loaddata()
        {

            dt = ds.Tables[0];
            int i = 0;
            lsvNV.Items.Clear();
            foreach (DataRow rows in dt.Rows)
            {
                lsvNV.Items.Add(rows["MANV"].ToString());
                lsvNV.Items[i].SubItems.Add(rows["TENNV"].ToString());
                if (rows["GIOITINH"].ToString() == "True")
                {
                    lsvNV.Items[i].SubItems.Add("Nam");
                }
                else if (rows["GIOITINH"].ToString() == "False")
                {
                    lsvNV.Items[i].SubItems.Add("Nữ");
                }
                lsvNV.Items[i].SubItems.Add(rows["NAMSINH"].ToString());
                lsvNV.Items[i].SubItems.Add(rows["EMAIL"].ToString());
                lsvNV.Items[i].SubItems.Add(rows["PASSWORDS"].ToString());
                if (rows["QH"].ToString() == "BH")
                {
                    lsvNV.Items[i].SubItems.Add("Bán hàng");
                }
                else if (rows["QH"].ToString() == "HD")
                {
                    lsvNV.Items[i].SubItems.Add("Hóa đơn");
                }
                else if (rows["QH"].ToString() == "HH")
                {
                    lsvNV.Items[i].SubItems.Add("Hàng hóa");
                }
                else if (rows["QH"].ToString() == "KH")
                {
                    lsvNV.Items[i].SubItems.Add("Khách hàng");
                }
                else if (rows["QH"].ToString() == "NV")
                {
                    lsvNV.Items[i].SubItems.Add("Nhân viên");
                }
                else if (rows["QH"].ToString() == "admin")
                {
                    lsvNV.Items[i].SubItems.Add("Admin");
                }
                if (rows["HideNV"].ToString() == "True")
                {
                    lsvNV.Items[i].SubItems.Add("Đã xóa");
                }
                else if (rows["HideNV"].ToString() == "False")
                {
                    lsvNV.Items[i].SubItems.Add("Kích hoạt");
                }
                i++;
            }
        }
        string gt = "";
        string quyenhan = "";
        string changepass = "";
        private void bochuyendoi()
        {
            //Chuyển đổi giới tính
            if (cbxGT.Text == "Nam")
            {
                gt = "True";
            }
            else if (cbxGT.Text == "Nữ")
            {
                gt = "False";
            }
            //chuyển đổi quyền hạn
            if (cbxQuyenHan.Text == "Bán hàng")
            {
                quyenhan = "BH";
            }
            else if (cbxQuyenHan.Text == "Hóa đơn")
            {
                quyenhan = "HD";
            }
            else if (cbxQuyenHan.Text == "Hàng hóa")
            {
                quyenhan = "HH";
            }
            else if (cbxQuyenHan.Text == "Khách hàng")
            {
                quyenhan = "KH";
            }
            else if (cbxQuyenHan.Text == "Nhân viên")
            {
                quyenhan = "NV";
            }
            else if (cbxQuyenHan.Text == "Admin")
            {
                quyenhan = "admin";
            }
            //Chuyển đổi bộ thay đổi password.

            if (ckbDoiMK.Checked == true)
            {
                changepass = "True";
            }
            else
            {
                changepass = "False";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tennv = txtTenNV.Text;
            string namsinh = datetime.Text;
            string email = txtEmail.Text;
            string matkhau = "";
            matkhau = md5(txtMatKhau.Text);
            bochuyendoi();

            string cmd = "insert into tbl_NhanVien (TENNV,GIOITINH,NAMSINH,EMAIL,PASSWORDS,QH,CHANGPAS) values (N'" + tennv + "','" + gt + "','" + namsinh + "','" + email + "','" + matkhau + "','" + quyenhan + "','" + changepass + "')";
            string trangthai = "";

            trangthai = webservices.excutedata(cmd);
            if (trangthai == "OK")
            {
                MessageBox.Show("Đã thêm nhân viên tên " + tennv + "", "Thêm thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                laydulieu();
                loaddata();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm nhân viên " + tennv + "", "Thêm lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            string trangthai = "";
            string manv = txtMaNV.Text;
            trangthai = webservices.excutedata("update tbl_NhanVien set HideNV = 'True' where MANV = '" + manv + "'");
            if (trangthai == "OK")
            {
                MessageBox.Show("Đã xóa nhân viên thành công!", "xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                laydulieu();
                loaddata();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa nhân viên mã " + manv + "", "Xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string tennv = txtTenNV.Text;
            string namsinh = datetime.Text;
            string email = txtEmail.Text;
            string matkhau = "";
            matkhau = md5(txtMatKhau.Text);
            bochuyendoi();
            string trangthai = "";
            string manv = txtMaNV.Text;
            trangthai = webservices.excutedata("update tbl_NhanVien set TENNV= N'" + tennv + "',GIOITINH= '" + gt + "',NAMSINH= '" + namsinh + "',EMAIL= '" + email + "',PASSWORDS= '" + matkhau + "',QH= '" + quyenhan + "',CHANGPAS= '" + changepass + "' where MANV = '" + manv + "'");
            if (trangthai == "OK")
            {
                MessageBox.Show("Đã sửa nhân viên thành công!", "sửa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                laydulieu();
                loaddata();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa nhân viên mã " + manv + "", "Sửa bị lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lsvNV_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in lsvNV.SelectedItems)
            {
                txtMaNV.Text = item.SubItems[0].Text;
                txtTenNV.Text = item.SubItems[1].Text;
                cbxGT.Text = item.SubItems[2].Text;
                datetime.Text = item.SubItems[3].Text;
                txtEmail.Text = item.SubItems[4].Text;
                txtMatKhau.Text = item.SubItems[5].Text;
                cbxQuyenHan.Text = item.SubItems[6].Text;
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
        private void loadchart()
        {
            //Select count(*)as soluong,  GIOITINH from tbl_NhanVien where QH = 'BH'  group by GIOITINH

            chartNV.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chartNV.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chartNV.Series["Số lượng"].Points.AddXY("Số lượng", 0);
            DataSet ds = new DataSet();
            ds = webservices.getdata("Select Count (*) as count,QH from tbl_NhanVien Group by QH");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            int i = 1;
            foreach (DataRow rows in dt.Rows)
            {
                switch (rows["QH"].ToString())
                { 
                    case "BH":
                        chartNV.Series["Bán hàng"].Points.AddXY("Bán hàng", rows["count"].ToString());
                        chartNV.Series["Bán hàng"].Points[0].Label = rows["count"].ToString();
                        break;
                    case "HD":
                        chartNV.Series["Hóa đơn"].Points.AddXY("Hóa đơn", rows["count"].ToString());
                        chartNV.Series["Hóa đơn"].Points[0].Label = rows["count"].ToString();
                        break;
                    case "HH":
                        chartNV.Series["Hàng hóa"].Points.AddXY("Hàng hóa", rows["count"].ToString());
                        chartNV.Series["Hàng hóa"].Points[0].Label = rows["count"].ToString();
                        break;
                    case "KH":
                        chartNV.Series["Khách hàng"].Points.AddXY("Khách hàng", rows["count"].ToString());
                        chartNV.Series["Khách hàng"].Points[0].Label = rows["count"].ToString();
                        break;
                    case "NV":
                        chartNV.Series["Nhân viên"].Points.AddXY("Nhân viên", rows["count"].ToString());
                        chartNV.Series["Nhân viên"].Points[0].Label = rows["count"].ToString();
                        break;
                    case "admin":
                        chartNV.Series["Admin"].Points.AddXY("Admin", rows["count"].ToString());
                        chartNV.Series["Admin"].Points[0].Label = rows["count"].ToString();
                        break;
                }
                i++;
            }

        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
