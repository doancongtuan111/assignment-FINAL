using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace App_Cloud_Tuandcpk00260_
{
    public partial class usc_banhang : UserControl
    {
        public usc_banhang()
        {
            InitializeComponent();
        }

        localhost.bus_data connectdata = new localhost.bus_data();

        public string gioitinh; //Khai báo biến chuỗi để chuyển đỗi giữa kiểu true thành nam

        //Bắt đầu các lệnh truy vấn

        //Thêm khách hàng
        private void btnThemKH_Click(object sender, EventArgs e) // Khi nhấn nút thêm khách hàng
        {
            string tenkh = txtTenKH.Text;
            string diachi = txtDiaChi.Text;
            string sdt = txtSoDT.Text;
            string email = txtEmail.Text;
            if (cbGioiTinh.Text == "Nam")
            {
                gioitinh = "True";
            }
            else if (cbGioiTinh.Text == "Nữ")
            {
                gioitinh = "False";
            }
            else if (cbGioiTinh.Text == "")
            {
                gioitinh = "";
            }
            string command = "";
            if (checkmail == true) //Kiểm tra hợp lệ Email
            {
                if (sdt.Length <= 12) //Kiểm tra số lượng kí tự nhập vào ô số điện thoại
                {
                    command = connectdata.excutedata("Insert into tbl_KhachHang (TENKH,GIOITINH,DIACHI,SDT,EMAIL) values (N'" + tenkh + "','" + gioitinh + "',N'" + diachi + "','" + sdt + "','" + email + "')");
                    if (command == "OK")
                    {
                        MessageBox.Show("Dữ liệu đã được thêm thành công !", "Thêm dữ liệu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtTenKH.Focus();
                        txtTenKH.Text = "";
                        cbGioiTinh.Text = "";
                        txtDiaChi.Text = "";
                        txtSoDT.Text = "";
                        txtEmail.Text = "";
                        loaddlKH();
                        dobanghetKH();
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu đã không được thêm thành công !", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Số điện quá dài, vui lòng chỉnh sửa", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Email của bạn vừa nhập không hợp lệ", "Email sai", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSoDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Chỉ cho phép nhập số
                e.Handled = true;
            }
        }

        public bool checkmail; //Khai báo kiểm tra mail
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            Regex regex;
            regex = new Regex(@"\S+@\S+\.\S+");
            Control ctrl = (Control)sender;
            if (regex.IsMatch(ctrl.Text))
            {
                checkmail = true;
            }
            else
            {
                checkmail = false;
            }
        }

        // Load dữ liệu khách hàng lên 
        DataTable dtkh = new DataTable();
        private void loaddlKH()
        {
            DataSet ds = new DataSet();
            ds = connectdata.getdata("select * from tbl_KhachHang where HideKH = 'false'");
            dtkh = ds.Tables[0];
        }
        // Do dữ liệu vào bảng nếu có sự lựa chọn
        private void dobanghetKH()
        {
            int i = 0;
            lstKH.Items.Clear();
            foreach (DataRow rows in dtkh.Rows)
            {
                lstKH.Items.Add(rows["MAKH"].ToString());
                lstKH.Items[i].SubItems.Add(rows["TENKH"].ToString());
                if (rows["GIOITINH"].ToString() == "True")
                {
                    lstKH.Items[i].SubItems.Add("Nam");
                }
                else if (rows["GIOITINH"].ToString() == "False")
                {
                    lstKH.Items[i].SubItems.Add("Nữ");
                }
                lstKH.Items[i].SubItems.Add(rows["DIACHI"].ToString());
                lstKH.Items[i].SubItems.Add(rows["SDT"].ToString());
                lstKH.Items[i].SubItems.Add(rows["EMAIL"].ToString());
                i++;
            }
        }


        private void dobangKH(string cmd)
        {
            int i = 0;
            lstKH.Items.Clear();
            DataRow[] rowss = dtkh.Select("TENKH=\'" + cmd + "\'");
            foreach (DataRow rows in rowss)
            {
                lstKH.Items.Add(rows["MAKH"].ToString());
                lstKH.Items[i].SubItems.Add(rows["TENKH"].ToString());
                if (rows["GIOITINH"].ToString() == "True")
                {
                    lstKH.Items[i].SubItems.Add("Nam");
                }
                else if (rows["GIOITINH"].ToString() == "False")
                {
                    lstKH.Items[i].SubItems.Add("Nữ");
                }
                lstKH.Items[i].SubItems.Add(rows["DIACHI"].ToString());
                lstKH.Items[i].SubItems.Add(rows["SDT"].ToString());
                lstKH.Items[i].SubItems.Add(rows["EMAIL"].ToString());
                i++;
            }
        }


        //Tìm kiếm trực tiếp trên ô nhập liệu
        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {
            if (txtTenKH.Text == "")
            {
                dobanghetKH();
            }
            else
            {
                dobangKH(txtTenKH.Text);
            }
        }

        private void lstKH_MouseClick(object sender, MouseEventArgs e) //thủ tục này dùng để bingding dữ liệu lên các đối tượng
        {
            foreach (ListViewItem lvItem in lstKH.SelectedItems)
            {
                lblMaKH.Text = lvItem.SubItems[0].Text;
                lblTenKH.Text = lvItem.SubItems[1].Text;
                txtDiaChi.Text = lvItem.SubItems[3].Text;
            }
        }
        private void btn_LapHD_Click(object sender, EventArgs e) //Thủ tục này để lập hóa đơn, bao gồm lập và đưa lên mã hóa đơn
        {
            string makh = lblMaKH.Text;
            string mota = txtGhiChu.Text;
            string manvs = lblMaNVs.Text;
            string ngaylap =  DateTime.Now.ToString();
            string command = "Insert into tbl_HoaDon (MAKH,MOTA,MANV) values ('" + makh + "',N'" + mota + "','" + manvs + "')";
            string thongbao = "";
            try
            {
                thongbao = connectdata.excutedata(command);
                if (thongbao == "OK")
                {
                    MessageBox.Show("Dữ liệu đã được thêm thành công !", "Thêm dữ liệu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Load lại mã
                    string command2 = "Select MAX(MAHD) MAHD from tbl_HoaDon";
                    DataSet ds = new DataSet();
                    ds = connectdata.getdata(command2);
                    DataTable tb = new DataTable();
                    tb = ds.Tables[0];
                    foreach (DataRow row in tb.Rows)
                    {
                        lblMaHD.Text = (row["MAHD"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Dữ liệu đã không được thêm thành công !", "Thêm dữ liệu lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Dữ liệu thêm vào không thành công !", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public static string masps;  //Khai báo một chuỗi để lưu mã sản phẩm
        public static string tensps; //Khai báo một tên sản phẩm kiểu chuỗi
        private void findsp() //khi chọn một tên, thông tin của sản phẩm đó sẽ hiện lên
        {
            string cmd = "select * from tbl_SanPham where TENSANPHAM = N'" + comboBox1.Text + "' and HideSP = 'False'";
            DataSet ds = new DataSet();
            ds = connectdata.getdata(cmd);
            DataTable tb = new DataTable();
            tb = ds.Tables[0];

            foreach (DataRow row in tb.Rows)
            {
                tensps = (row["TENSANPHAM"].ToString());
               // comboBox1.Items.Add(row["TENSANPHAM"].ToString());
                lblDonGia.Text = (row["DONGIA"].ToString());
                masps = (row["MASANPHAM"].ToString());
            }
        }

        private void loadsp()
        {
            string cmd = "select TENSANPHAM from tbl_SanPham where HideSP = 'False'";
            DataSet ds = new DataSet();
            ds = connectdata.getdata(cmd);
            DataTable tb = new DataTable();
            tb = ds.Tables[0];
            foreach (DataRow row in tb.Rows)
            {
                comboBox1.Items.Add(row["TENSANPHAM"].ToString());
            }
        }

        //Thực hiện tồng thời trên 2 thao tác và gõ và chọn
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.SelectAll();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // thủ tục này tương tự thủ tục trên nhưng được thực hiện thao tác khác
        {
            //comboBox1.Items.Clear();
            findsp();
            caculator();
        }


        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            caculator();
        }

        //chuyển đổi và tính toán tiền tệ theo số lượng
        private void caculator()
        {
            try //quá số lượng tối đa sẽ báo lỗi
            {
                int soluong = Convert.ToInt32(txtSoLuong.Text);
                int tong = int.Parse(lblDonGia.Text) * soluong;
                lblTongGia.Text = Convert.ToString(tong);
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập quá số lượng cho phép, vui lòng chỉnh sửa", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                //Chỉ cho phép nhập số
                e.Handled = true;
            }
        }
        public static DataTable das;
        private void loaddata2() //Load dữ liệu chi tiết hóa đơn theo hóa đơn hiện tại
        {
            string cmd2 = "select * from tbl_CTHD LEFT OUTER JOIN tbl_SanPham on tbl_CTHD.MASANPHAMM = tbl_SanPham.MASANPHAM  where tbl_CTHD.MAHDONs = '" + lblMaHD.Text + "' and HideCTHD = 'false'";
            DataSet ds = new DataSet();
            ds = connectdata.getdata(cmd2);
            DataTable tb3 = new DataTable();
            tb3 = ds.Tables[0];
            int i = 0;
            int tonggia;
            lsvcthd.Items.Clear();
            foreach (DataRow row in tb3.Rows)
            {
                int stt = i + 1;
                lsvcthd.Items.Add(stt.ToString());
                lsvcthd.Items[i].SubItems.Add(row["TENSANPHAM"].ToString());
                lsvcthd.Items[i].SubItems.Add(row["DONGIAs"].ToString());
                lsvcthd.Items[i].SubItems.Add(row["SOLUONG"].ToString());
                tonggia = Convert.ToInt32(row["SOLUONG"]) * Convert.ToInt32(row["DONGIAs"]);
                lsvcthd.Items[i].SubItems.Add(tonggia.ToString());
                lsvcthd.Items[i].SubItems.Add(row["MACTHD"].ToString());
                i++;
            }
        }

        // thủ tục thêm sản phẩm vào chi tiết hóa đơn
        private void btnThemCTHD_Click(object sender, EventArgs e)
        {
            string mahd = lblMaHD.Text;
            string soluong = txtSoLuong.Text;
            string dongia = lblDonGia.Text;
            string command = "Insert into tbl_CTHD (MAHDONs,MASANPHAMM,SOLUONG,DONGIAs) values (N'" + mahd + "','" + masps + "','" + soluong + "','" + dongia + "')";
            try //Bẩy lỗi
            {
                if (Convert.ToInt32(txtSoLuong.Text) >= 1)
                {
                    connectdata.excutedata(command);
                    comboBox1.Focus();
                    loaddata2();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số lượng lớn hơn hoặc bằng 1", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Dữ liệu thêm vào không thành công !", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //con.Close();
        }

        private void button6_Click(object sender, EventArgs e) //xóa dữ liệu trong bảng cthd (update lại trường hideCTHD)
        {
            if (lsvcthd.SelectedItems.Count == 1)
            {
                string command = "update tbl_CTHD set HideCTHD = 'true' where MACTHD = '" + idcthd + "'";
                connectdata.excutedata(command);
                comboBox1.Focus();
                loaddata2();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong chi tiết hóa đơn", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static string idcthd;
        private void lsvcthd_MouseClick(object sender, MouseEventArgs e) //Binding để thực hiện việc xóa hoặc sửa
        {
            foreach (ListViewItem lvItem in lsvcthd.SelectedItems)
            {
                comboBox1.Text = lvItem.SubItems[1].Text;
                txtSoLuong.Text = lvItem.SubItems[3].Text;
                idcthd = lvItem.SubItems[5].Text;
            }
        }

        private void btnSuaCTHD_Click(object sender, EventArgs e)
        {
            if (lsvcthd.SelectedItems.Count == 1)
            {
                string mahd = lblMaHD.Text;
                string soluong = txtSoLuong.Text;
                string dongia = lblDonGia.Text;
                string command = "update tbl_CTHD set MAHDONs = '" + mahd + "',MASANPHAMM = '" + masps + "',SOLUONG = '" + soluong + "',DONGIAs = '" + dongia + "' where MACTHD = '" + idcthd + "'";
                connectdata.excutedata(command);
                comboBox1.Focus();
                loaddata2();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong chi tiết hóa đơn", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string tenkh;
        public static string ngaylap;
        public static string diachikh;
        public static string nhanvien;
        public static string mahd;

        //Phần liên kết form//////////////////////////####################3#########################33#########
        private void button1_Click(object sender, EventArgs e)
        {
            
            tenkh = lblTenKH.Text;
            diachikh = txtDiaChi.Text;
            nhanvien = lblMaNVs.Text;
            mahd = lblMaHD.Text;

           frm_InHoaDon frint = new frm_InHoaDon();
           frint.Activate();
           frint.ShowDialog();
        }

        private void usc_banhang_Load(object sender, EventArgs e)
        {
            loadsp();
            loaddlKH();
            dobanghetKH();
            findsp();
            lblTenNVs.Text = frm_login.nguoidangnhap;
            lblMaNVs.Text = frm_login.manv;
            ngaylap = frm_main.ngay;
        }

        private void btn_KiemTra_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            findsp();
        }

       private MakeToString _mk;
       
        
        private void lblTongGia_TextChanged(object sender, EventArgs e)
        {
            var temp = lblTongGia.Text;
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

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {

        }

        private void grphanhang_Enter(object sender, EventArgs e)
        {
           
        }

    }
}
