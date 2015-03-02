using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace App_Cloud_Tuandcpk00260_
{
    public partial class frm_loading : Form
    {
        public frm_loading()
        {
            InitializeComponent();
            timer1.Start();
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            string a = " %";
            lblProgress.Text = progressBar1.Value.ToString() + a;
           if (string.Compare(lblProgress.Text, "100 %") == 0)
            {
                timer1.Stop();
                timer2.Stop();
                frm_main frm2 = new frm_main();
                frm2.Activate();
                frm2.Show();
                this.Hide();       
            }
        }




       [DllImport("user32.dll", CharSet = CharSet.Unicode)]
       static extern uint SendMessage(IntPtr hWnd,uint Msg,uint wParam, uint lParam);

        private void timer2_Tick(object sender, EventArgs e)
        {
            progressBar2.Value = (int)performanceCounter1.NextValue();
            label1.Text = progressBar2.Value.ToString() + " %";
            if ((int)progressBar2.Value < 25)
                SendMessage(progressBar2.Handle,
                0x400 + 16, 
                0x0000, 
                0);
            if ((int)progressBar2.Value < 55)
                SendMessage(progressBar2.Handle,
                0x400 + 16, 
                0x0003, 
                0);
            if ((int)progressBar2.Value >= 85)
                SendMessage(progressBar2.Handle,
                0x400 + 16, 
                0x0002, 
                0);

            progressBar3.Value = (int)performanceCounter2.NextValue();
            label6.Text = progressBar3.Value.ToString() + " %";

            if ((int)progressBar3.Value < 25)

                SendMessage(progressBar3.Handle,
                0x400 + 16,
                0x0004, 
                0);
            if ((int)progressBar3.Value < 55)
                SendMessage(progressBar3.Handle,
                0x400 + 16, 
                0x0003, 
                0);
            if ((int)progressBar3.Value >= 85)
                SendMessage(progressBar3.Handle,
                0x400 + 16, 
                0x0002, 
                0);
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
