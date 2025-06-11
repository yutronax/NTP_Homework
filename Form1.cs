using System;
using System.Drawing;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        int sayac = 0;
        Zorluk z = new Zorluk();
        MyTimerManager timerManager;
        SkorYoneticisi skorYoneticisi = new SkorYoneticisi();

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            timerManager = new MyTimerManager(this);

            // Skor tablosu butonu ekle
            Button skorBtn = new Button();
            skorBtn.Text = "Skor Tablosu";
            skorBtn.Size = new Size(120, 30);
            skorBtn.Location = new Point(10, 10);
            skorBtn.Click += (s, e) => skorYoneticisi.SkorTablosunuGoster();
            this.Controls.Add(skorBtn);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            Zorluk z = new Zorluk();
            z.zorlukbutonlar(flowLayoutPanel1, timerManager, skorYoneticisi);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }

}
