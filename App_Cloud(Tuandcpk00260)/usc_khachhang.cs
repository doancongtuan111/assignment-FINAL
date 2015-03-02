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
    public partial class usc_khachhang : UserControl
    {
        public usc_khachhang()
        {
            InitializeComponent();
        }
        //Khai báo webservices #############################################################
        localhost.bus_data webservice = new localhost.bus_data();

        //Lấy dữ liệu từ webservices  #############################################################
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private void laydulieu()
        {
            ds = webservice.getdata("select * from tbl_KhachHang");
            dt = ds.Tables[0];
        }

        //Lấy dữ liệu vào Listview  #############################################################
        private void loaddata()
        {
            int i = 0;
            lsvKH.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                lsvKH.Items.Add(row["MAKH"].ToString());
                lsvKH.Items[i].SubItems.Add(row["TENKH"].ToString());
                if (row["GIOITINH"].ToString() == "True")
                {
                    lsvKH.Items[i].SubItems.Add("Nam");
                }
                else if (row["GIOITINH"].ToString() == "False")
                {
                    lsvKH.Items[i].SubItems.Add("Nữ");
                }
                lsvKH.Items[i].SubItems.Add(row["DIACHI"].ToString());
                lsvKH.Items[i].SubItems.Add(row["SDT"].ToString());
                lsvKH.Items[i].SubItems.Add(row["EMAIL"].ToString());
                if (row["HideKH"].ToString() == "True")
                {
                    lsvKH.Items[i].SubItems.Add("Đã xóa");
                }
                else if (row["HideKH"].ToString() == "False")
                {
                    lsvKH.Items[i].SubItems.Add("Kích hoạt");
                }
                i++;
            }
        }

        //Form load  #############################################################
        private void frm_QLKhachHang_Load(object sender, EventArgs e)
        {
            laydulieu();
            loaddata();
        }

        //Bingding  #############################################################
        private void lsvKH_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in lsvKH.SelectedItems)
            {
                txtMaKH.Text = item.SubItems[0].Text;
                txtTenKH.Text = item.SubItems[1].Text;
                txtGioiTinh.Text = item.SubItems[2].Text;
                txtDiaChi.Text = item.SubItems[3].Text;
                txtSoDT.Text = item.SubItems[4].Text;
                txtEmail.Text = item.SubItems[5].Text;
                txtTinhTrang.Text = item.SubItems[6].Text;
            }
        }

        //Xóa dữ liệu  #############################################################
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lsvKH.SelectedItems.Count == 1)
            {
                string command = "update tbl_KhachHang set HideKH = 'true' where MAKH = '" + txtMaKH.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    MessageBox.Show("Đã xóa khách hàng " + txtTenKH.Text + "", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    laydulieu();
                    loaddata();
                }
                else
                {
                    MessageBox.Show("Dữ liệu chưa được xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để thực hiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Sửa dữ liệu  #############################################################
        private void btnSua_Click(object sender, EventArgs e)
        {
            string gt = "";

            if (txtGioiTinh.Text == "Nam")
            {
                gt = "true";
            }
            else if (txtGioiTinh.Text == "Nữ")
            {
                gt = "false";
            }
            string ten = txtTenKH.Text;
            string diachi = txtDiaChi.Text;
            string sdt = txtSoDT.Text;
            string email = txtEmail.Text;
            if (lsvKH.SelectedItems.Count == 1)
            {
                string command = "update tbl_KhachHang set TENKH =N'" + ten + "',GIOITINH ='" + gt + "',DIACHI =N'" + diachi + "',SDT ='" + sdt + "',EMAIL ='" + email + "' where MAKH = '" + txtMaKH.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    MessageBox.Show("Đã sửa khách hàng " + txtTenKH.Text + "", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    laydulieu();
                    loaddata();
                }
                else
                {
                    MessageBox.Show("Dữ liệu chưa được sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để thực hiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // bỏ xóa   #############################################################
        private void btnBoXoa_Click(object sender, EventArgs e)
        {
            if (lsvKH.SelectedItems.Count == 1)
            {
                string command = "update tbl_KhachHang set HideKH = 'False' where MAKH = '" + txtMaKH.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    MessageBox.Show("Đã bỏ xóa khách hàng " + txtTenKH.Text + "", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    laydulieu();
                    loaddata();
                }
                else
                {
                    MessageBox.Show("Dữ liệu chưa được bỏ xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để thực hiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void usc_khachhang_Load(object sender, EventArgs e)
        {
            laydulieu();
            loaddata();
            loadchart();
        }

        private void loadchart()
        {
            chartKH.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chartKH.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chartKH.Series["Nam"].Points.AddXY("Nữ", 0);
            DataSet ds = new DataSet();
            ds = webservice.getdata("Select Count (*) as count,GIOITINH from tbl_KhachHang Group by GIOITINH");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            int i = 1;
            foreach (DataRow rows in dt.Rows)
            {
                if (rows["GIOITINH"].ToString() == "True")
                {
                    chartKH.Series["Nam"].Points.AddXY("Nam", rows["count"].ToString());
                    chartKH.Series["Nam"].Points[1].Label = rows["count"].ToString();
                }
                else if (rows["GIOITINH"].ToString() == "False")
                {
                    chartKH.Series["Nữ"].Points.AddXY("Nữ", rows["count"].ToString());
                    chartKH.Series["Nữ"].Points[0].Label = rows["count"].ToString();
                }
            }
        }

    }
}
