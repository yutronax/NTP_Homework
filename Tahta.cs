using minesweeper;
namespace minesweeper
{
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
}
