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
    public partial class usc_sanpham : UserControl
    {
        public usc_sanpham()
        {
            InitializeComponent();
        }

        private void usc_sanpham_Load(object sender, EventArgs e)
        {
            laydulieuloaisp();
            laydulieusanpham();
            loaddataproduc();
            loaddataloai();
        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Chỉ cho phép nhập số
                e.Handled = true;
            }
        }
        //Khai báo webservices  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        localhost.bus_data webservice = new localhost.bus_data();

        //Lấy dữ liệu sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        DataSet dssp = new DataSet();
        private void laydulieusanpham()
        {
            dssp = webservice.getdata("select * from tbl_SanPham left join tbl_LoaiSanPham on tbl_SanPham.LOAISANPHAM = tbl_LoaiSanPham.MALOAI");
        }

        //Lấy dữ liệu loại sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        DataSet dslsp = new DataSet();
        private void laydulieuloaisp()
        {
            dslsp = webservice.getdata("select * from tbl_LoaiSanPham");
        }

        //Đưa dữ liệu bảng sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void loaddataproduc()
        {
            DataTable dt = new DataTable();
            dt = dssp.Tables[0];
            lsvSP.Items.Clear();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                lsvSP.Items.Add(row["MASANPHAM"].ToString());
                lsvSP.Items[i].SubItems.Add(row["TENSANPHAM"].ToString());
                lsvSP.Items[i].SubItems.Add(row["TENLOAI"].ToString());
                lsvSP.Items[i].SubItems.Add(row["DONGIA"].ToString());
                lsvSP.Items[i].SubItems.Add(row["MOTA"].ToString());
                if (row["HideSP"].ToString() == "True")
                {
                    lsvSP.Items[i].SubItems.Add("Đã xóa");
                }
                else if (row["HideSP"].ToString() == "False")
                {
                    lsvSP.Items[i].SubItems.Add("Kích hoạt");
                }
                i++;
            }
        }

        //Đưa dữ liệu vào bảng loại sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void loaddataloai()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = dslsp.Tables[0];
                lsvLoai.Items.Clear();
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    lsvLoai.Items.Add(row["MALOAI"].ToString());
                    lsvLoai.Items[i].SubItems.Add(row["TENLOAI"].ToString());
                    lsvLoai.Items[i].SubItems.Add(row["MOTALOAI"].ToString());
                    if (row["HideLSP"].ToString() == "True")
                    {
                        lsvLoai.Items[i].SubItems.Add("Đã xóa");
                    }
                    else if (row["HideLSP"].ToString() == "False")
                    {
                        lsvLoai.Items[i].SubItems.Add("Kích hoạt");
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xẩy ra khi tải dữ liệu, vui lòng kiểm tra lại", "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Bingding dữ liệu  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void lsvSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lsvSP.SelectedItems)
            {
                txtMaSP.Text = item.SubItems[0].Text;
                txtTenSP.Text = item.SubItems[1].Text;
                txtLoaiSP.Text = item.SubItems[2].Text;
                txtDonGia.Text = item.SubItems[3].Text;
                txtMoTaSP.Text = item.SubItems[4].Text;
                txtTinhTrangSP.Text = item.SubItems[5].Text;
            }
        }

        private void lsvLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem items in lsvLoai.SelectedItems)
            {
                txtMaLoai.Text = items.SubItems[0].Text;
                txtTenLoai.Text = items.SubItems[1].Text;
                txtLoaiSP.Text = items.SubItems[1].Text;
                txtMoTaLoai.Text = items.SubItems[2].Text;
                txtTinhTrangLoai.Text = items.SubItems[3].Text;
            }
        }

        //Xóa sản phẩm $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (lsvSP.SelectedItems.Count == 1)
            {
                string command = "update tbl_SanPham set HideSP = 'True' where MASANPHAM = '" + txtMaSP.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieusanpham();
                    loaddataproduc();
                    MessageBox.Show("Đã bỏ xóa thành công sản phẩm mã " + txtTenSP.Text + "", "Bỏ xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi bỏ xóa sản phẩm mã " + txtTenSP.Text + ", Vui lòng kiểm tra lại", "Bỏ xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Sửa sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (lsvSP.SelectedItems.Count == 1 && lsvLoai.SelectedItems.Count==1 )
            {
                string tensp = txtTenSP.Text;
                string loaisp = txtMaLoai.Text;
                string dongia = txtDonGia.Text;
                string mota = txtMoTaSP.Text;
                string masp = txtMaSP.Text;
                string command = "update tbl_SanPham set TENSANPHAM = N'" + tensp + "', LOAISANPHAM = '" + loaisp + "',DONGIA = '" + dongia + "', MOTA = N'" + mota + "' where MASANPHAM = '" + masp+ "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieusanpham();
                    loaddataproduc();
                    MessageBox.Show("Đã sửa thành công sản phẩm mã " + txtTenSP.Text + "", "Sửa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa sản phẩm mã " + txtTenSP.Text + ", Vui lòng kiểm tra lại", "sửa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Bỏ xóa sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnBoXoaSp_Click(object sender, EventArgs e)
        {
            if (lsvSP.SelectedItems.Count == 1)
            {
                string command = "update tbl_SanPham set HideSP = 'False' where MASANPHAM = '" + txtMaSP.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieusanpham();
                    loaddataproduc();
                    MessageBox.Show("Đã bỏ xóa thành công sản phẩm mã " + txtTenSP.Text + "", "Bỏ xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi bỏ xóa sản phẩm mã " + txtTenSP.Text + ", Vui lòng kiểm tra lại", "Bỏ xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Thêm sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (lsvLoai.SelectedItems.Count == 1)
            {
                string tensp = txtTenSP.Text;
                string loaisp = txtMaLoai.Text;
                string dongia = txtDonGia.Text;
                string mota = txtMoTaSP.Text;
                string them = "insert into tbl_SanPham (TENSANPHAM, LOAISANPHAM,DONGIA, MOTA) values (N'" + tensp + "','" + loaisp + "','" + dongia + "',N'" + mota + "')";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(them);
                if (tinhtrang == "OK")
                {
                    laydulieusanpham();
                    loaddataproduc();
                    MessageBox.Show("Thêm sản phẩm thành công", "Thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại", "Lỗi thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Thêm loai sản phẩm  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnThemLoai_Click(object sender, EventArgs e)
        {
            string tenloai = txtTenLoai.Text;
            string mota = txtMoTaLoai.Text;
            string them = "insert into tbl_LoaiSanPham (TENLOAI, MOTALOAI) values (N'" + tenloai + "',N'" + mota + "')";
            string tinhtrang = "";
            tinhtrang = webservice.excutedata(them);
            if (tinhtrang == "OK")
            {
                laydulieuloaisp();
                loaddataloai();
                MessageBox.Show("Thêm loại sản phẩm thành công", "Thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thêm loại sản phẩm thất bại", "Lỗi thêm sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Xóa loại sản phẩm   $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnXoaLoai_Click(object sender, EventArgs e)
        {
            if (lsvLoai.SelectedItems.Count == 1)
            {
                string command = "update tbl_LoaiSanPham set HideLSP = 'True' where MALOAI = '" + txtMaLoai.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieuloaisp();
                    loaddataloai();
                    MessageBox.Show("Đã xóa thành công loại sản phẩm mã " + txtMaLoai.Text + "", "Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại sản phẩm mã " + txtMaLoai.Text + ", Vui lòng kiểm tra lại", "Xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Sửa loại sản phẩm   $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnSuaLoai_Click(object sender, EventArgs e)
        {
            if (lsvLoai.SelectedItems.Count == 1)
            {
                string tenloai = txtTenLoai.Text;
                string mota = txtMoTaLoai.Text;
                string command = "update tbl_LoaiSanPham set TENLOAI =N'" + tenloai + "', MOTALOAI =N'" + mota + "' where MALOAI = '" + txtMaLoai.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieuloaisp();
                    loaddataloai();
                    MessageBox.Show("Đã sửa thành công loại sản phẩm mã " + txtMaLoai.Text + "", "Sửa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa loại sản phẩm mã " + txtMaLoai.Text + ", Vui lòng kiểm tra lại", "Xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Bỏ xóa loại sản phẩm   $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private void btnBoXoaLoai_Click(object sender, EventArgs e)
        {
            if (lsvLoai.SelectedItems.Count == 1)
            {
                string command = "update tbl_LoaiSanPham set HideLSP = 'False' where MALOAI = '" + txtMaLoai.Text + "'";
                string tinhtrang = "";
                tinhtrang = webservice.excutedata(command);
                if (tinhtrang == "OK")
                {
                    laydulieuloaisp();
                    loaddataloai();
                    MessageBox.Show("Đã bỏ xóa thành công loại sản phẩm mã " + txtMaLoai.Text + "", "bỏ xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi bỏ xóa loại sản phẩm mã " + txtMaLoai.Text + ", Vui lòng kiểm tra lại", "Bỏ xóa lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
