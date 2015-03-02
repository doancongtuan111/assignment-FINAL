using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Printing;
using Microsoft.VisualBasic.PowerPacks.Printing;

namespace App_Cloud_Tuandcpk00260_
{
    public partial class frm_InHoaDon : Form
    {
        public frm_InHoaDon()
        {
            InitializeComponent();
        }
        localhost.bus_data sevice_data = new localhost.bus_data();
        private void frm_InHoaDon_Load(object sender, EventArgs e)
        {
            lblTenKH.Text = usc_banhang.tenkh;
            lblDiaChi.Text = usc_banhang.diachikh;
            lblMaHD.Text = usc_banhang.mahd;
            lblNgayBan.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblNhanVien.Text = usc_banhang.nhanvien;
            CTHD(usc_banhang.mahd);

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintImage);
            PrintDialog pdi = new PrintDialog();
            pdi.Document = pd;
            if (pdi.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                
            }
            else
            {
                MessageBox.Show("Print Cancelled");
            }

            //printForm1.Print(this, PrintForm.PrintOption.ClientAreaOnly);
        }
        //Code in       
        void PrintImage(object o, PrintPageEventArgs e)
        {
            int x = SystemInformation.WorkingArea.X;
            int y = SystemInformation.WorkingArea.Y;
            int width = this.Width;
            int height = this.Height;

            Rectangle bounds = new Rectangle(x, y, width, height);

            Bitmap img = new Bitmap(width, height);

            this.DrawToBitmap(img, bounds);
            Point p = new Point(100, 100);
            e.Graphics.DrawImage(img, p);
        }
        //kết thúc code in


        int tong = 0;
        private void CTHD(string id)
        {
            //int tong = 0;
            DataTable dtCTHD = new DataTable();
            int i = 0;
            tong = 0;
            DataSet ds = new DataSet();
            ds = sevice_data.getdata("select * from tbl_CTHD left join tbl_SanPham on tbl_CTHD.MASANPHAMM = tbl_SanPham.MASANPHAM   where MAHDONs = '" + id + "'  ");
            dtCTHD = ds.Tables[0];
            foreach (DataRow rows in dtCTHD.Rows)
            {
                    lsvcthd.Items.Add(i+1.ToString());
                    lsvcthd.Items[i].SubItems.Add(rows["TENSANPHAM"].ToString());
                    lsvcthd.Items[i].SubItems.Add(rows["DONGIAs"].ToString());
                    lsvcthd.Items[i].SubItems.Add(rows["SOLUONG"].ToString());
                    tong += int.Parse(rows["SOLUONG"].ToString()) * int.Parse(rows["DONGIAs"].ToString());
                    lsvcthd.Items[i].SubItems.Add(tong.ToString());
                    i++;
            }
            lblTongTien.Text = tong.ToString() + " VNĐ";
        }

        private MakeToString _mk;


        private void lblTongTien_TextChanged(object sender, EventArgs e)
        {
            var temp = tong.ToString();
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

                lblTongTienChu.Text = _mk.ReadThis() + " " + "đồng";
            }
            else
            {
                lblTongTienChu.Text = "Dãy vừa nhập không phải là số hoặc bạn đã nhập quá 15 chữ số !";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }



    }
}
