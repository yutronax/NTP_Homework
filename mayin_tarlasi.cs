using minesweeper;
using System.Collections;
using System.Drawing.Text;
using System.IO.Compression;
using System.Web;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            zorluk z = new zorluk();
            z.zorlukbutonlar(flowLayoutPanel1);
        }
    }

    public class zorluk
    {
        string[] dizi = { "kolay", "orta", "zor" };
        public int h = 0;
        FlowLayoutPanel panel;

        public void zorlukbutonlar(FlowLayoutPanel f)
        {
            panel = f;

            for (int i = 0; i < 3; i++)
            {
                Button btn = new Button();
                btn.Text = dizi[i];
                btn.Size = new Size(300, 300);
                f.Controls.Add(btn);
                btn.Click += zorlukbutontiklandi;
            }
        }

        public void zorlukbutontiklandi(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            for (int i = 0; i < 3; i++)
            {
                if (tiklanan.Text.Equals(dizi[i]))
                {
                    h = i;
                    panel.Controls.Clear();
                    Tahta t = new Tahta();
                    t.degerdondur(h);
                    t.butonlar(panel);
                    return;
                }
            }
        }
    }

    public class Tahta
    {
        protected FlowLayoutPanel panel = new FlowLayoutPanel();
        private Button btn;
        bool yerlestirme;
        protected int deger;
        protected int hucre_sayisi = 100;
        Button Btn { get { return btn; } set { btn = value; } }
        private Button bayrakbtn;

        int[] grid_genislikleri = { 8, 10, 15 }; 
        public int grid_genisligi;

        public Button Bayrakbtn
        {
            get { return bayrakbtn; }
            set { bayrakbtn = value; }
        }

        public void degerdondur(int h)
        {
            deger = h;
        }

        public void butonlar(FlowLayoutPanel f)
        {
            panel = f;
            int boyut = 120 - (deger * 30);

            grid_genisligi = grid_genislikleri[deger];
            hucre_sayisi = 9 * grid_genisligi; 

            Hucre h = new Hucre(f, deger, grid_genisligi);

            for (int i = 1; i <= hucre_sayisi; i++)
            {
                Btn = new Button();
                Btn.Text = $"";
                Btn.Name = $"Btn{i}";
                Btn.Size = new Size(boyut, 90);
                panel.Controls.Add(Btn);
                Btn.Click += h.buttonatıklandı;
                Btn.MouseDown += h.bayrak; 
            }
        }
    }

    class Hucre : Tahta
    {
        List<int> acildiMi = new List<int>();
        bool mayınlarsecildimi = false;
        public FlowLayoutPanel gamePanel;
        public List<int> button_saylari;
        zorluk z = new zorluk();
        private int toplam_hucre; 

        public Hucre(FlowLayoutPanel panel, int zorlukDegeri, int gridGenisligi)
        {
            gamePanel = panel;
            this.deger = zorlukDegeri;
            this.grid_genisligi = gridGenisligi;
            this.toplam_hucre = 9 * gridGenisligi;
        }

        public void bayrak(object sender, MouseEventArgs e)
        {
            Button bayrakBtn = sender as Button;
            if (e.Button == MouseButtons.Right)
            {
                if (bayrakBtn.Text == "")
                {
                    bayrakBtn.Text = "🚩";
                    bayrakBtn.Tag = "bayrak";
                }
                else if (bayrakBtn.Text == "🚩")
                {
                    bayrakBtn.Text = "";
                    bayrakBtn.Tag = null;
                }
            }
        }

        void sifirlarlistele(int sayi)
        {
            if (acildiMi.Contains(sayi))
            {
                return;
            }

            int mayinSayisi = mayinanla(sayi);
            List<int> komsular = komsugetir(sayi);

            if (mayinSayisi != 0)
            {
                return;
            }

            foreach (int komsu in komsular)
            {
                if (!acildiMi.Contains(komsu))
                {
                    sifirlarlistele(komsu);
                }
            }
        }

   
        private List<int> komsugetir(int sayi)
        {
            List<int> komsular = new List<int>();
            int satir = (sayi - 1) / grid_genisligi; 
            int sutun = (sayi - 1) % grid_genisligi; 

            for (int ilk = -1; ilk <= 1; ilk++)
            {
                for (int son = -1; son <= 1; son++)
                {
                    if (ilk == 0 && son == 0) continue; 

                    int yeniSatir = satir + ilk;
                    int yeniSutun = sutun + son;

                    if (yeniSatir >= 0 && yeniSatir < 9 &&
                        yeniSutun >= 0 && yeniSutun < grid_genisligi)
                    {
                        int yeniSayi = yeniSatir * grid_genisligi + yeniSutun + 1;
                        komsular.Add(yeniSayi);
                    }
                }
            }
            return komsular;
        }

        public void buttonatıklandı(object sender, EventArgs e)
        {
            Button tıklanan = sender as Button;
            if (tıklanan.Tag != null && tıklanan.Tag.ToString() == "bayrak")
            {
                return; 
            }

            string butonismi = tıklanan.Name;
            int sayi = int.Parse(string.Join("", tıklanan.Name.Where(char.IsDigit).Select(c => c.ToString())));

            if (mayınlarsecildimi == false)
            {
                button_saylari = mayinsec(sayi);
                mayınlarsecildimi = true;
            }

            if (tıklanan != null)
            {
                oyunbitti(tıklanan);
                sifirlarlistele(sayi);
            }
        }

        private void oyunbitti(object sender)
        {
            Button tıklanan = sender as Button;
            int sayi = int.Parse(string.Join("", tıklanan.Name.Where(char.IsDigit).Select(c => c.ToString())));

            if (button_saylari.Contains(sayi))
            {
                foreach (int item in button_saylari)
                {
                    string butonAdi = $"Btn{item}";
                    Control[] bulunan = tıklanan.Parent.Controls.Find(butonAdi, false);
                    if (bulunan.Length > 0 && bulunan[0] is Button btnn)
                    {
                        btnn.BackColor = Color.Red;
                    }
                }
                MessageBox.Show("OYUN BİTTİ!");
                Application.Exit();
            }
        }

        private int tarama(int say)
        {
            foreach (int i in button_saylari)
            {
                if (i == say)
                {
                    return 1; 
                }
            }
            return 0;
        }

        private int mayinanla(int sayi)
        {
            if (button_saylari.Contains(sayi))
            {
                return -1; 
            }

            List<int> komsular = komsugetir(sayi);
            int mayin_sayisi = 0;

            foreach (int komsu in komsular)
            {
                if (button_saylari.Contains(komsu))
                {
                    mayin_sayisi++;
                }
            }

            foreach (Control control in gamePanel.Controls)
            {
                int sayi2 = int.Parse(string.Join("", control.Name.Where(char.IsDigit).Select(c => c.ToString())));
                if (sayi2 == sayi)
                {
                    if (mayin_sayisi > 0)
                    {
                        control.Text = $"{mayin_sayisi}";
                        // Renk ayarlaması
                        if (mayin_sayisi == 1) control.BackColor = Color.Blue;
                        else if (mayin_sayisi == 2) control.BackColor = Color.Green;
                        else if (mayin_sayisi == 3) control.BackColor = Color.Brown;
                        else if (mayin_sayisi == 4) control.BackColor = Color.Purple;
                        else if (mayin_sayisi == 5) control.BackColor = Color.Orange;
                        else if (mayin_sayisi == 6) control.BackColor = Color.Pink;
                    }
                    else
                    {
                        control.BackColor = Color.LightGray;
                    }
                    break;
                }
            }

            acildiMi.Add(sayi);
            return mayin_sayisi;
        }

        public List<int> mayinsec(int x)
        {
            int mayinalgo = 10 + (20 * this.deger);
            Random r = new Random();
            List<int> mayinliste = new List<int>();

            List<int> korunanlar = new List<int> { x };
            korunanlar.AddRange(komsugetir(x));

            while (mayinliste.Count < mayinalgo)
            {
                int mayin = r.Next(1, toplam_hucre + 1);

                if (!mayinliste.Contains(mayin) && !korunanlar.Contains(mayin))
                {
                    mayinliste.Add(mayin);
                }
            }

            return mayinliste;
        }
    }
}
