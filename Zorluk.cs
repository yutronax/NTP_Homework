using System;
using System.Drawing;
using System.Windows.Forms;

namespace minesweeper
{
    public class Zorluk
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
}
