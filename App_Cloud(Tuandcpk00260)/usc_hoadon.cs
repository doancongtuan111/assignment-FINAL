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
    public partial class usc_hoadon : UserControl
    {
        public usc_hoadon()
        {
            InitializeComponent();
        }

        localhost.bus_data sevice_data = new localhost.bus_data();
        private void usc_hoadon_Load(object sender, EventArgs e)
        {
            getData_HD();
            getData_CTHD();
            loadchart();


        }



        private void loadchart()
        {
            chartHD.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chartHD.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chartHD.Series["Số Hóa đơn"].Points.AddXY("0", 0);
            DataSet ds = new DataSet();
            ds = sevice_data.getdata("Select Count (*) as count,TENNV from tbl_HoaDon left join tbl_NhanVien on tbl_HoaDon.MANV = tbl_NhanVien.MANV Group by TENNV");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];  
            int i = 1;
            foreach (DataRow rows in dt.Rows)
            {

                chartHD.Series["Số Hóa đơn"].Points.AddXY(rows["TENNV"].ToString(), rows["count"].ToString());
                chartHD.Series["Số Hóa đơn"].Points[i].Label = rows["count"].ToString();
                i++;
            }
        }


        private void getData_HD()
        {
            DataSet ds = new DataSet();
            ds = sevice_data.getdata("select MAHD,TENKH,TENNV,NGAYLAP,MOTA,HideHD from tbl_HoaDon left join tbl_NhanVien on tbl_HoaDon.MANV = tbl_NhanVien.MANV left join tbl_KhachHang on tbl_KhachHang.MAKH = tbl_HoaDon.MAKH");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            int i = 0;
            lsvHD.Items.Clear();
            foreach (DataRow rows in dt.Rows)
            {
                lsvHD.Items.Add(rows["MAHD"].ToString());
                lsvHD.Items[i].SubItems.Add(rows["TENKH"].ToString());
                lsvHD.Items[i].SubItems.Add(rows["TENNV"].ToString());
                lsvHD.Items[i].SubItems.Add(rows["NGAYLAP"].ToString());
                lsvHD.Items[i].SubItems.Add(rows["MOTA"].ToString());
                if (rows["HideHD"].ToString() == "True")
                {
                    lsvHD.Items[i].SubItems.Add("Đã xóa");
                }
                else
                {
                    lsvHD.Items[i].SubItems.Add("Kích hoạt"); 
                }
                i++;
            }
            lblTongHD.Text = i.ToString();


        }

        int tong = 0;
        DataTable dtCTHD = new DataTable();

        private void getData_CTHD()
        {
            DataSet ds = new DataSet();
            ds = sevice_data.getdata("select * from tbl_CTHD");
            dtCTHD = ds.Tables[0];
        }

        private void CTHD(string cmd)
        {
            int i = 0;
            tong = 0;

            foreach (DataRow rows in dtCTHD.Rows)
            {
                if (rows["MAHDONs"].ToString() == cmd)
                {
                    lsvCTHD.Items.Add(rows["MACTHD"].ToString());
                    lsvCTHD.Items[i].SubItems.Add(rows["MAHDONs"].ToString());
                    lsvCTHD.Items[i].SubItems.Add(rows["MASANPHAMM"].ToString());
                    lsvCTHD.Items[i].SubItems.Add(rows["SOLUONG"].ToString());
                    lsvCTHD.Items[i].SubItems.Add(rows["DONGIAs"].ToString());
                    tong += int.Parse(rows["SOLUONG"].ToString()) * int.Parse(rows["DONGIAs"].ToString());

                    if (rows["HideCTHD"].ToString() == "True")
                    {
                        lsvCTHD.Items[i].SubItems.Add("Đã xóa");
                    }
                    else
                    {
                        lsvCTHD.Items[i].SubItems.Add("Kích hoạt");
                    }
                    i++;
                }
            }
            lblSoLuong.Text = i.ToString();
            lblTongTien.Text = tong.ToString() + " VNĐ";
        }


        private void lsvHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsvCTHD.Items.Clear();
            string cmd = "";
            foreach (ListViewItem item in lsvHD.SelectedItems)
            {
                lblMaHD.Text = item.SubItems[0].Text;
                lblKH.Text = item.SubItems[1].Text;
                lblNV.Text = item.SubItems[2].Text;
                lblNgayLap.Text = item.SubItems[3].Text;
                lblMoTa.Text = item.SubItems[4].Text;
            }
            cmd = lblMaHD.Text;
            CTHD(cmd);

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string tinhtrang = "";
            string tinhtrang2 = "";
            string mahd = lblMaHD.Text;
            tinhtrang = sevice_data.excutedata("update tbl_HoaDon set HideHD = 'true' where MAHD ='" + mahd + "'");
            tinhtrang2 = sevice_data.excutedata("update tbl_CTHD set HideCTHD = 'true' where MAHDONs ='" + mahd + "'");
            if (tinhtrang == "OK" && tinhtrang2 == "OK")
            {
                MessageBox.Show("Đã xóa hóa đơn thành công", "xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getData_HD();
                getData_CTHD();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra !", "xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private MakeToString _mk;
        private void lblTongTien_Click(object sender, EventArgs e)
        {
           
        }

        private void lblTongTien_TextChanged(object sender, EventArgs e)
        {
            var temp = Convert.ToString(tong);
            var check = false;
            for (var i = 0; i < temp.Length; i++)
            {
                check = Char.IsLetter(temp, i);
                break;
            }
            if (!check & temp.Length <= 15)
            {
                _mk = new MakeToString(Convert.ToDouble(temp));
                _mk.BlockProcessing();

                lblBangChu.Text = _mk.ReadThis() + " " + "đồng";
            }
            else
            {
                lblBangChu.Text = "Dãy vừa nhập không phải là số hoặc bạn đã nhập quá 15 chữ số !";
            }
        }
    }
}
