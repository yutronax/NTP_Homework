using minesweeper;
using System.Collections;
using System.Drawing.Text;
using System.IO.Compression;
using System.Web;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace minesweeper
{
    public class SkorVerisi
    {
        public string Zorluk { get; set; }
        public int Sure { get; set; }
        public DateTime Tarih { get; set; }
        public string OyuncuAdi { get; set; }
    }

    public class SkorYoneticisi
    {
        private string dosyaYolu = "skorlar.json";

        public List<SkorVerisi> SkorlariOku()
        {
            try
            {
                if (File.Exists(dosyaYolu))
                {
                    string json = File.ReadAllText(dosyaYolu);
                    return JsonSerializer.Deserialize<List<SkorVerisi>>(json) ?? new List<SkorVerisi>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skorlar okunurken hata oluştu:");
            }
            return new List<SkorVerisi>();
        }

        public void SkorKaydet(SkorVerisi yeniSkor)
        {
            try
            {
                List<SkorVerisi> skorlar = SkorlariOku();
                skorlar.Add(yeniSkor);

                skorlar = skorlar.OrderBy(s => s.Zorluk).ThenBy(s => s.Sure).ToList();

                string json = JsonSerializer.Serialize(skorlar, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dosyaYolu, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skor kaydedilirken hata oluştu: {ex.Message}");
            }
        }

        public void SkorTablosunuGoster()
        {
            List<SkorVerisi> skorlar = SkorlariOku();

            Form skorForm = new Form();
            skorForm.Text = "Skor Tablosu";
            skorForm.Size = new Size(600, 400);
            skorForm.StartPosition = FormStartPosition.CenterParent;

            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Zorluk", HeaderText = "Zorluk", DataPropertyName = "Zorluk" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sure", HeaderText = "Süre (saniye)", DataPropertyName = "Sure" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "OyuncuAdi", HeaderText = "Oyuncu", DataPropertyName = "OyuncuAdi" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Tarih", HeaderText = "Tarih", DataPropertyName = "Tarih" });

            
            dgv.Columns["Tarih"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            dgv.DataSource = skorlar;

           
            

            skorForm.Controls.Add(dgv);
            skorForm.ShowDialog();
        }
    }

    public partial class Form1 : Form
    {
        int sayac = 0;
        zorluk z = new zorluk();
        MyTimerManager timerManager;
        SkorYoneticisi skorYoneticisi = new SkorYoneticisi();

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            timerManager = new MyTimerManager(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            zorluk z = new zorluk();
            z.zorlukbutonlar(flowLayoutPanel1, timerManager, skorYoneticisi);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }

    public class MyTimerManager
    {
        private System.Windows.Forms.Timer timer;
        private Label label;
        private Form form;
        int sayac = 0;
        Tahta t = new Tahta();

        public MyTimerManager(Form parentForm)
        {
            form = parentForm;

            label = new Label();
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(form.ClientSize.Width - label.Width - 10, 10);
            label.Text = "Süre: 0";
            form.Controls.Add(label);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 saniye
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            label.Text = "Süre: " + sayac.ToString();
            sayac++;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop() => timer.Stop();

        public int GetSure() => sayac; 
    }

    public class zorluk
    {
        string[] dizi = { "Kolay", "Orta", "Zor" };
        public int h = 0;
        FlowLayoutPanel panel;
        public bool basildi = false;
        MyTimerManager timerManager;
        SkorYoneticisi skorYoneticisi;

        public void zorlukbutonlar(FlowLayoutPanel f, MyTimerManager timer, SkorYoneticisi skor)
        {
            panel = f;
            timerManager = timer;
            skorYoneticisi = skor;

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

                    timerManager.Start();

                    Tahta t = new Tahta();
                    t.degerdondur(h);
                    t.butonlar(panel, timerManager, skorYoneticisi, dizi[i]);

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
        protected int hucre_sayisi;
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

        public void butonlar(FlowLayoutPanel f, MyTimerManager timer, SkorYoneticisi skor, string zorlukAdi)
        {
            panel = f;

            int boyut = 120 - (deger * 30);

            grid_genisligi = grid_genislikleri[deger];
            hucre_sayisi = 9 * grid_genisligi;

            Hucre h = new Hucre(f, deger, grid_genisligi, hucre_sayisi, timer, skor, zorlukAdi);

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
        int mayinolmayanlar;
        MyTimerManager timerManager;
        SkorYoneticisi skorYoneticisi;
        string zorlukAdi;

        public Hucre(FlowLayoutPanel panel, int zorlukDegeri, int gridGenisligi, int hucre_sayisi, MyTimerManager timer, SkorYoneticisi skor, string zorluk)
        {
            gamePanel = panel;
            this.deger = zorlukDegeri;
            this.grid_genisligi = gridGenisligi;
            this.toplam_hucre = 9 * gridGenisligi;
            this.hucre_sayisi = hucre_sayisi;
            this.timerManager = timer;
            this.skorYoneticisi = skor;
            this.zorlukAdi = zorluk;
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
                oyunkazandi();
                sifirlarlistele(sayi);
            }
        }

        private void oyunbitti(object sender)
        {
            Button tıklanan = sender as Button;
            int sayi = int.Parse(string.Join("", tıklanan.Name.Where(char.IsDigit).Select(c => c.ToString())));

            if (button_saylari.Contains(sayi))
            {
                timerManager.Stop();

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

        private void oyunkazandi()
        {
            if (mayinolmayanlar - 1 == acildiMi.Count)
            {
                timerManager.Stop();

                string oyuncuAdi = Microsoft.VisualBasic.Interaction.InputBox(
                    "Tebrikler! Oyunu kazandınız!\nAdınızı girin:",
                    "Oyun Kazanıldı",
                    "Oyuncu");

                if (string.IsNullOrWhiteSpace(oyuncuAdi))
                    oyuncuAdi = "Anonim";

                SkorVerisi yeniSkor = new SkorVerisi
                {
                    Zorluk = zorlukAdi,
                    Sure = timerManager.GetSure(),
                    Tarih = DateTime.Now,
                    OyuncuAdi = oyuncuAdi
                };

                skorYoneticisi.SkorKaydet(yeniSkor);

                MessageBox.Show($"OYUNU KAZANDIN!\nSüreniz: {yeniSkor.Sure} saniye\nSkorunuz kaydedildi!");

                // Skor tablosunu göster
                skorYoneticisi.SkorTablosunuGoster();

                Application.Exit();
            }
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
            mayinolmayanlar = hucre_sayisi - mayinliste.Count;
            return mayinliste;
        }
    }
}
