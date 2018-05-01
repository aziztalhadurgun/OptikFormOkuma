using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptikForm
{
    public partial class frmEskiKayitlar : Form
    {
        public frmEskiKayitlar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEskiKayitlar_Load(object sender, EventArgs e)
        {
            cOkunanlar o = new cOkunanlar();
            o.eskiKayitlariGetir(lvKayitlar);
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            cOkunanlar o = new cOkunanlar();
            o.eskiKayitlariGetir(lvKayitlar, txtArama.Text);
        }

        private void lvKayitlar_DoubleClick(object sender, EventArgs e)
        {
            cOkunanlar._kontrol = 1;
            cOkunanlar._str = lvKayitlar.SelectedItems[0].SubItems[7].Text;
            frmOptikOkuma frm = new frmOptikOkuma();
            this.Close();
            frm.Show();
        }




        /// <summary>
        /// dsdsdsdsdsdsdsdsdsdsd
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form_Surukle(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void Form_TamEkran(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
        private void Red_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void Red_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_red_pressed.png");
        }
        private void Red_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_red.png");
        }
        private void Orange_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Normal)
                this.WindowState = FormWindowState.Normal;
            else this.WindowState = FormWindowState.Maximized;
        }
        private void Orange_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_orange_pressed.png");
        }
        private void Orange_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_orange.png");
        }
        private void Green_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.WindowState = FormWindowState.Minimized;
            else this.WindowState = FormWindowState.Maximized;
        }
        private void Green_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_green_pressed.png");
        }
        private void Green_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Image.FromFile(Application.StartupPath + "\\images\\duz_green.png");
        }
    }

}
