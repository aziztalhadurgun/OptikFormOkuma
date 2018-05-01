using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace OptikForm
{
    public partial class frmOptikOkuma : Form
    {
        public frmOptikOkuma()
        {
            InitializeComponent();
        }
        Bitmap resim = new Bitmap(600, 849);
        List<string> dizi_harfler = new List<string>(new string[] {
                "A", "B", "C","Ç","D","E","F","G","Ğ","H","I","İ","J","K","L","M","N","O","Ö","P","R","S","Ş","T","U","Ü","V","Y","Z"
            });
        List<string> dizi_tc = new List<string>(new string[] {
                "0","1","2","3","4","5","6","7","8","9",""
            });
        List<string> cevap_anahtari = new List<string>(new string[] {
                "A","A","A","A","A","B","B","B","B","B","C","C","C","C","C","D","D","D","D","D"
            });

        string ResimYolu = Application.StartupPath.ToString() + "\\resimler\\res.jpg";
        private void Form1_Load(object sender, EventArgs e)
        {
            btnDegistir.Visible = false;
            if (cOkunanlar._kontrol == 1)
            {
                pictureBox1.Image = Image.FromFile(cOkunanlar._str);
                Graphics.FromImage(resim).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
                optik_oku.Visible = true;
            }
            /*else if (cOkunanlar._kontrol == 0)
            {
                pictureBox1.Image = Image.FromFile(ResimYolu);
                Graphics.FromImage(resim).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }*/
        }
        private void OptikOku_Click(object sender, EventArgs e)
        {
            /////////////////////////////////////////////////
            toplamDogru = 0;
            toplamYanlis = 0;
            toplamNet = 0;
            toplamBos = 0;

            SilSupur();
            lblFormSayisi.Visible = true;
            lblGosterilen.Visible = true;
            Oku_Fonk(61, 102, 29, 10, 102, dizi_harfler, txt_ad);                           // Ad okur
            Oku_Fonk(61, 451, 29, 10, 451, dizi_harfler, txt_soyad);                        // Soyad okur
            Oku_Fonk(225, 102, 10, 11, 102, dizi_tc, txt_tc);                               // TC okur
            Oku_Ders(279, 277, 20, 4, 279, cevap_anahtari, rich_tr_kisi, rich_tr_cevap, "TR");    // Türkçe dersini okur.
            Oku_Ders(356, 277, 20, 4, 356, cevap_anahtari, rich_mat_kisi, rich_mat_cevap, "Mat");  // Matematik dersini okur.
            Oku_Ders(433, 277, 20, 4, 433, cevap_anahtari, rich_ing_kisi, rich_ing_cevap, "Ing");  // İngilizce dersini okur.

            if (cOkunanlar._kontrol == 0)
            {
                cOkunanlar o = new cOkunanlar();
                o.AdSoyad = txt_ad.Text + " " + txt_soyad.Text;
                o.Dogru = toplamDogru;
                o.Yanlis = toplamYanlis;
                o.Bos = toplamBos;
                o.Net = toplamNet;
                o.DosyaAdi = listBox1.SelectedItem.ToString();
                o.formKaydet(o);
            }
        }
        private void Oku_Fonk(int x, int y, int tekrar_asagi, int tekrar_sag, int sifirlama, List<string> dizi, TextBox textBox)
        {
            int count = 0;
            int count_bosluk = 0;
            int oran = 11;
            Color c;
            int red, green, blue;      

            // Alttaki for'da count_bosluk yeni eklendi.

            for (int i = 0; i < tekrar_sag; i++)
            {
                for (int j = 0; j < tekrar_asagi; j++)
                {
                    c = resim.GetPixel(x, y);
                    red = c.R;
                    green = c.G;
                    blue = c.B;
                    int w = sifirlama;
                    if ((0 <= red && red < 127) || (0 <= green && green < 127) || (0 <= blue && blue < 127))
                    {
                        for (int a = 0; a < tekrar_sag; a++)
                        {
                            for (int b = 0; b < tekrar_asagi; b++)
                            {
                                if (y == w)
                                {
                                    textBox.Text += dizi[count];
                                }                                  
                                count++;
                                w += oran;
                            }
                            count = 0;
                        }
                        count_bosluk--;
                    }
                    else
                    {
                        count_bosluk++;
                    }
                    if (count_bosluk == 29)
                    {
                        textBox.Text += " ";
                        count_bosluk = 0;
                    }
                    if (j == 28)
                    {
                        count_bosluk = 0;
                    }
                    y += oran;
                }
                y = sifirlama;
                x += oran;
                pictureBox1.Image = resim;
                this.Refresh();
            }

            // Bu ifler de yeni eklendi.
            if(textBox == txt_tc)
            {
                Regex r = new Regex(@"\s+");
                string s = r.Replace(textBox.Text, @"");
                textBox.Text = s;
            }
            if (textBox == txt_ad || textBox == txt_soyad)
            {
                string s = textBox.Text;
                textBox.Text = s.TrimEnd();
            }

        }
        //////////////////////////////////////////////////
        /*float TRDogru = 0, TRYanlis = 0, TRBos = 0, TRNet = 0;
        float MatDogru = 0, MatYanlis = 0, MatBos = 0, MatNet = 0;
        float IngDogru = 0, IngYanlis = 0, IngBos = 0, IngNet = 0;*/
        float toplamDogru = 0, toplamYanlis = 0, toplamNet = 0, toplamBos = 0;

        string Dirpath;
        int imgindex;
        private void btnDosyaSec_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr != DialogResult.Cancel)
            {
                cOkunanlar._kontrol = 0;
                SilSupur();
                optik_oku.Visible = false;
                listBox1.Items.Clear();
                Dirpath = folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(Dirpath, "*.Jpg");
                foreach (string file in files)
                {
                    int pos = file.LastIndexOf("||");
                    string FName = file.Substring(pos + 1);
                    listBox1.Items.Add(FName);
                }
                lblFormSayisi.Visible = true;
                lblGosterilen.Visible = true;
                listBox1.SelectedIndex = imgindex = 0;
                lblFormSayisi.Text = "Form Sayısı : " + listBox1.Items.Count.ToString();
                lblGosterilen.Text = "Gösterilen Form : " + (imgindex + 1).ToString() + ". Form";
                btnDegistir.Visible = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SilSupur();
            pictureBox1.Image = Image.FromFile(listBox1.SelectedItem.ToString());
            Graphics.FromImage(resim).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Oku();
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if (imgindex < listBox1.Items.Count - 1)
            {
                imgindex++;
                listBox1.SelectedIndex = imgindex;
                lblGosterilen.Text = "Gösterilen Form : " + (imgindex + 1).ToString() + ". Form";
                if (imgindex == listBox1.Items.Count - 1)
                {
                    btnDegistir.Visible = false;
                    imgindex = 0;
                }
            }
            /*else
            {
                imgindex = 0;
                btnDegistir.Visible = false;
            }*/
        }
        
        private void Oku_Ders(int x, int y, int tekrar_asagi, int tekrar_sag, int sifirlama, List<string> cevap_anahtari, RichTextBox rich_kisi, RichTextBox rich_cevap, string yeni)
        {
            int oran = 11, count = 0, kontrol = 0, soru = 1;
            Color c;
            int red, green, blue;
            List<string> cevaplar = new List<string>();
            List<string> secenekler = new List<string>(new string[] {
                "A","B","C","D","BOS","GECERSIZ"
            });

            int a = 0;
            for (int i = 0; i < tekrar_asagi; i++)
            {
                int sonX = 0, sonY = 0;
                for (int j = 0; j < tekrar_sag; j++)
                {
                    c = resim.GetPixel(x, y);
                    red = c.R;
                    green = c.G;
                    blue = c.B;
                    if ((0 <= red && red < 127) || (0 <= green && green < 127) || (0 <= blue && blue < 127))
                    {
                        if (kontrol < 2)
                        {
                            kontrol++;
                            cevaplar.Add(secenekler[count]);
                            if (cevaplar.Count > 0)
                            {
                                if (kontrol != 2)
                                {
                                    if (cevaplar[a] == cevap_anahtari[a])
                                    {
                                        Graphics g;
                                        Pen kalemim = new Pen(Color.Green, 2);
                                        g = pictureBox1.CreateGraphics();
                                        g.DrawEllipse(kalemim, new Rectangle(x - 5, y - 5, 8, 8));
                                        g.Dispose();
                                        sonX = x - 5;
                                        sonY = y - 5;
                                    }
                                    else
                                    {
                                        Graphics g;
                                        Pen kalemim = new Pen(Color.Red, 2);
                                        g = pictureBox1.CreateGraphics();
                                        g.DrawEllipse(kalemim, new Rectangle(x - 5, y - 5, 8, 8));
                                        g.Dispose();
                                        sonX = x - 5;
                                        sonY = y - 5;
                                    }
                                }
                                else
                                {
                                    Graphics g;
                                    Pen kalemim = new Pen(Color.Red, 2);
                                    g = pictureBox1.CreateGraphics();
                                    if (sonX != 0)
                                    {
                                        g.DrawEllipse(kalemim, new Rectangle(sonX, sonY, 8, 8));
                                        g.DrawEllipse(kalemim, new Rectangle(x - 5, y - 5, 8, 8));
                                    }
                                    g.Dispose();
                                }
                            }
                        }
                        if (kontrol == 2)
                        {
                            cevaplar.RemoveAt(cevaplar.Count - 1);
                            cevaplar.RemoveAt(cevaplar.Count - 1);
                            cevaplar.Add(secenekler[5]);
                        }
                    }
                    if (j == 3)
                    {
                        if (kontrol == 0)
                        {
                            cevaplar.Add(secenekler[4]);
                        }
                    }
                    x += oran;
                    count++;
                }
                a++;
                kontrol = 0;
                x = sifirlama;
                y += oran;
                count = 0;
            }
            foreach (var item in cevaplar)
            {
                rich_kisi.Text += "\n" + soru + ". " + item;
                soru++;
            }
            soru = 1;
            foreach (var item in cevap_anahtari)
            {
                rich_cevap.Text += "\n" + soru + ". " + item;
                soru++;
            }
            ///////////////////////////////////////////////
            float dogru = 0, yanlis = 0, bos = 0, net = 0;
            int z = 0;
            foreach(var item in cevaplar)
            {
                if (item == "BOS")
                {
                    bos++;
                }
                else if (item == "GECERSIZ")
                {
                    yanlis++;
                }
                else if (item == cevap_anahtari[z])
                {
                    dogru++;
                }
                else if(item != cevap_anahtari[z])
                {
                    yanlis++;
                }
                z++;
            }
            net = dogru - (yanlis / 4);
            lblToplamBos.Visible = true;
            lblToplamDogru.Visible = true;
            lblToplamNet.Visible = true;
            lblToplamYanlis.Visible = true;
            if (yeni == "TR")
            {
                lblTurkceDogru.Text = "Türkçe Doğru : " + dogru.ToString();
                lblTurkceYanlis.Text = "Türkçe Yanlış : " + yanlis.ToString();
                lblTurkceBos.Text = "Türkçe Boş : " + bos.ToString();
                lblTurkceNet.Text = "Türkçe Net : " + net.ToString();
                lblTurkceBos.Visible = true;
                lblTurkceDogru.Visible = true;
                lblTurkceNet.Visible = true;
                lblTurkceYanlis.Visible = true;
                toplamDogru += dogru;
                toplamYanlis += yanlis;
                toplamBos += bos;
                toplamNet += net;
                /*TRDogru = dogru;
                TRYanlis = yanlis;
                TRBos = bos;
                TRNet = net;*/
            }
            else if (yeni == "Mat")
            {
                lblMatematikDogru.Text = "Matematik Doğru : " + dogru.ToString();
                lblMatematikYanlis.Text = "Matematik Yanlış : " + yanlis.ToString();
                lblMatematikBos.Text = "Matematik Boş : " + bos.ToString();
                lblMatematikNet.Text = "Matematik Net : " + net.ToString();
                lblMatematikBos.Visible = true;
                lblMatematikDogru.Visible = true;
                lblMatematikNet.Visible = true;
                lblMatematikYanlis.Visible = true;
                toplamDogru += dogru;
                toplamYanlis += yanlis;
                toplamBos += bos;
                toplamNet += net;
                /*MatDogru = dogru;
                MatYanlis = yanlis;
                MatBos = bos;
                MatNet = net;*/
            }
            else
            {
                lblIngDogru.Text = "İngilizce Doğru : " + dogru.ToString();
                lblIngYanlis.Text = "İngilizce Yanlış : " + yanlis.ToString();
                lblIngBos.Text = "İngilizce Boş : " + bos.ToString();
                lblIngNet.Text = "İngilizce Net : " + net.ToString();
                lblIngBos.Visible = true;
                lblIngDogru.Visible = true;
                lblIngNet.Visible = true;
                lblIngYanlis.Visible = true;
                toplamDogru += dogru;
                toplamYanlis += yanlis;
                toplamBos += bos;
                toplamNet += net;
                /*IngDogru = dogru;
                IngYanlis = yanlis;
                IngBos = bos;
                IngNet = net;*/
            }
            lblToplamDogru.Text = "Toplam Doğru : " + toplamDogru.ToString();
            lblToplamYanlis.Text = "Toplam Tanlış : " + toplamYanlis.ToString();
            lblToplamBos.Text = "Toplam Boş : " + toplamBos.ToString();
            lblToplamNet.Text = "Toplam Net : " + toplamNet.ToString();
        }

        private void Temizle_Click(object sender, EventArgs e)
        {
            //Form1_Load(sender, e);
            txt_ad.Clear();
            txt_soyad.Clear();
            txt_tc.Clear();
            rich_tr_kisi.Clear();
            rich_tr_cevap.Clear();
            rich_mat_kisi.Clear();
            rich_mat_cevap.Clear();
            rich_ing_kisi.Clear();
            rich_ing_cevap.Clear();
            lblTurkceBos.Visible = false;
            lblTurkceDogru.Visible = false;
            lblTurkceNet.Visible = false;
            lblTurkceYanlis.Visible = false;
            lblMatematikBos.Visible = false;
            lblMatematikDogru.Visible = false;
            lblMatematikNet.Visible = false;
            lblMatematikYanlis.Visible = false;
            lblIngBos.Visible = false;
            lblIngDogru.Visible = false;
            lblIngNet.Visible = false;
            lblIngYanlis.Visible = false;
            lblToplamBos.Visible = false;
            lblToplamDogru.Visible = false;
            lblToplamNet.Visible = false;
            lblToplamYanlis.Visible = false;
            lblFormSayisi.Visible = false;
            lblGosterilen.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void SilSupur()
        {
            txt_ad.Clear();
            txt_soyad.Clear();
            txt_tc.Clear();
            rich_tr_kisi.Clear();
            rich_tr_cevap.Clear();
            rich_mat_kisi.Clear();
            rich_mat_cevap.Clear();
            rich_ing_kisi.Clear();
            rich_ing_cevap.Clear();
            lblTurkceBos.Visible = false;
            lblTurkceDogru.Visible = false;
            lblTurkceNet.Visible = false;
            lblTurkceYanlis.Visible = false;
            lblMatematikBos.Visible = false;
            lblMatematikDogru.Visible = false;
            lblMatematikNet.Visible = false;
            lblMatematikYanlis.Visible = false;
            lblIngBos.Visible = false;
            lblIngDogru.Visible = false;
            lblIngNet.Visible = false;
            lblIngYanlis.Visible = false;
            lblToplamBos.Visible = false;
            lblToplamDogru.Visible = false;
            lblToplamNet.Visible = false;
            lblToplamYanlis.Visible = false;
            lblFormSayisi.Visible = false;
            lblGosterilen.Visible = false;
        }

        void Oku()
        {
            /////////////////////////////////////////////////
            toplamDogru = 0;
            toplamYanlis = 0;
            toplamNet = 0;
            toplamBos = 0;

            SilSupur();
            lblFormSayisi.Visible = true;
            lblGosterilen.Visible = true;
            Oku_Fonk(61, 102, 29, 10, 102, dizi_harfler, txt_ad);                           // Ad okur
            Oku_Fonk(61, 451, 29, 10, 451, dizi_harfler, txt_soyad);                        // Soyad okur
            Oku_Fonk(225, 102, 10, 11, 102, dizi_tc, txt_tc);                               // TC okur
            Oku_Ders(279, 277, 20, 4, 279, cevap_anahtari, rich_tr_kisi, rich_tr_cevap, "TR");    // Türkçe dersini okur.
            Oku_Ders(356, 277, 20, 4, 356, cevap_anahtari, rich_mat_kisi, rich_mat_cevap, "Mat");  // Matematik dersini okur.
            Oku_Ders(433, 277, 20, 4, 433, cevap_anahtari, rich_ing_kisi, rich_ing_cevap, "Ing");  // İngilizce dersini okur.

            if (cOkunanlar._kontrol == 0)
            {
                cOkunanlar o = new cOkunanlar();
                o.AdSoyad = txt_ad.Text + " " + txt_soyad.Text;
                o.Dogru = toplamDogru;
                o.Yanlis = toplamYanlis;
                o.Bos = toplamBos;
                o.Net = toplamNet;
                o.DosyaAdi = listBox1.SelectedItem.ToString();
                o.formKaydet(o);
            }
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
