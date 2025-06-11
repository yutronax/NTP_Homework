using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace minesweeper
{
    
   partial class  Hucre : Tahta
    {
        List<int> acildiMi = new List<int>();
        bool mayınlarsecildimi = false;
        public FlowLayoutPanel gamePanel;
        public List<int> button_saylari;
        Zorluk z = new Zorluk();
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

                // Oyuncu adını sor
                string oyuncuAdi = Microsoft.VisualBasic.Interaction.InputBox(
                    "Tebrikler! Oyunu kazandınız!\nAdınızı girin:",
                    "Oyun Kazanıldı",
                    "Oyuncu");

                if (string.IsNullOrWhiteSpace(oyuncuAdi))
                    oyuncuAdi = "Anonim";

                // Skoru kaydet
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
