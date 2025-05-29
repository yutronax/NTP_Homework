using minesweeper;
using System.Collections;
using System.Drawing.Text;
using System.Web;
using System.Windows.Forms;

namespace minesweeper
{   
    public class Hucre2
    {
        List<int> acildiMi=new List<int>();
        bool mayınlarsecildimi = false;
        private Button btn;
        Button Btn { get { return btn; } set { btn = value; } }
        FlowLayoutPanel panel = new FlowLayoutPanel();

        public List<int> button_saylari;
        public void butonlar(FlowLayoutPanel f)
        {Button ilk = new Button();
            panel = f;
            for (int i = 1; i < 91; i++)
            {
                Btn = new Button();
                Btn.Text = $"";
                Btn.Name = $"Btn{i}";
                Btn.Size = new Size(90, 90);
                panel.Controls.Add(Btn);
                Btn.Click += buttonatıklandı;
            }
        }
        void sifirlarlistele(int sayi)
        {

            if (acildiMi.Contains(sayi))
            {
                return;
            }
            mayinanla(sayi);

            if (sayi < 10 && sayi > 1)
            {
               if  (mayinanla(sayi + 1)==0) sifirlarlistele(sayi+1);
                if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);
                if (mayinanla(sayi + 9) == 0) sifirlarlistele(sayi + 9);
                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);
                if (mayinanla(sayi + 11) == 0) sifirlarlistele(sayi + 11);


            }
            else if (sayi > 81 && sayi < 90)
            {
                if (mayinanla(sayi + 1) == 0) sifirlarlistele(sayi + 1);
                if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);
                if (mayinanla(sayi - 9) == 0) sifirlarlistele(sayi - 9);
                if (mayinanla(sayi - 10) == 0) sifirlarlistele(sayi - 10);
                if (mayinanla(sayi - 11) == 0) sifirlarlistele(sayi - 11);


            }

            else if (sayi == 1)
            {
                if (mayinanla(sayi + 1) == 0) sifirlarlistele(sayi +1);
                if (mayinanla(sayi + 11) == 0) sifirlarlistele(sayi + 11);
                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);




            }
            else if (sayi == 10)
            {
                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);
                
                    if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);
                    if (mayinanla(sayi + 9) == 0) sifirlarlistele(sayi + 9);

                }
          
            else if (sayi == 81)
            {
                if (mayinanla(sayi + 1) == 0) sifirlarlistele(sayi + 1);
                if (mayinanla(sayi - 9) == 0) sifirlarlistele(sayi - 9);
                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi - 10);



            }
            else if (sayi == 90)
            {
                if (mayinanla(sayi - 11) == 0) sifirlarlistele(sayi - 11);
                if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);

                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi - 10);



            }
            else if (sayi % 10 == 1)
            {
                if (mayinanla(sayi + 1) == 0) sifirlarlistele(sayi + 1);
                if (mayinanla(sayi - 9) == 0) sifirlarlistele(sayi - 9);
                if (mayinanla(sayi + 11) == 0) sifirlarlistele(sayi + 11);


                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);
                if (mayinanla(sayi - 10) == 0) sifirlarlistele(sayi - 10);


            }
            else if (sayi % 10 == 0)
            {
                if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);
                if (mayinanla(sayi + 9) == 0) sifirlarlistele(sayi + 9);
                if (mayinanla(sayi - 11) == 0) sifirlarlistele(sayi - 11);


                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);
                if (mayinanla(sayi - 10) == 0) sifirlarlistele(sayi - 10);


            }
            else
            {
                if (mayinanla(sayi + 1) == 0) sifirlarlistele(sayi + 1);
                if (mayinanla(sayi - 1) == 0) sifirlarlistele(sayi - 1);
                if (mayinanla(sayi - 9) == 0) sifirlarlistele(sayi - 9);
                if (mayinanla(sayi - 10) == 0) sifirlarlistele(sayi - 10);
                if (mayinanla(sayi - 11) == 0) sifirlarlistele(sayi - 11);
                if (mayinanla(sayi + 9) == 0) sifirlarlistele(sayi + 9);
                if (mayinanla(sayi + 10) == 0) sifirlarlistele(sayi + 10);
                if (mayinanla(sayi + 11) == 0) sifirlarlistele(sayi + 11);
            }
        }
        public void buttonatıklandı(object sender, EventArgs e)
        {
            Button tıklanan = sender as Button;
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
                foreach(int item in button_saylari)
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
            int sayac = 0;
            foreach (int i in button_saylari)
            {
                if (i == say)
                {
                    sayac++;
                    return sayac;
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

            int mayin_sayisi = 0;
            if (sayi < 10 && sayi > 1)
            {
                mayin_sayisi += tarama(sayi + 1);
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi + 10);
                mayin_sayisi += tarama(sayi + 9);
                mayin_sayisi += tarama(sayi + 11);


            }
            else if (sayi > 81 && sayi < 90)
            {
                mayin_sayisi += tarama(sayi + 1);
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi - 9);
                mayin_sayisi += tarama(sayi - 11);

            }
            else if (sayi % 10 == 0)
            {
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi + 9);
                mayin_sayisi += tarama(sayi - 11);
                mayin_sayisi += tarama(sayi + 10);

            }
            else if (sayi % 10 == 1)
            {

                mayin_sayisi += tarama(sayi + 1);
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi - 9);
                mayin_sayisi += tarama(sayi + 11);
                mayin_sayisi += tarama(sayi + 10);
            }
            else if (sayi == 1)
            {
                mayin_sayisi += tarama(sayi + 1);
                mayin_sayisi += tarama(sayi + 10);
                mayin_sayisi += tarama(sayi + 11);

            }
            else if (sayi == 10)
            {
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi + 10);
                mayin_sayisi += tarama(sayi + 9);

            }
            else if (sayi == 81)
            {
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi - 9);
                mayin_sayisi += tarama(sayi + 1);

            }
            else if (sayi == 90)
            {
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi - 9);

            }
            else
            {

                mayin_sayisi += tarama(sayi + 1);
                mayin_sayisi += tarama(sayi - 1);
                mayin_sayisi += tarama(sayi - 10);
                mayin_sayisi += tarama(sayi + 10);
                mayin_sayisi += tarama(sayi - 11);
                mayin_sayisi += tarama(sayi + 11);
                mayin_sayisi += tarama(sayi - 9);
                mayin_sayisi += tarama(sayi + 9);

            }
            foreach (Control tıklanan in panel.Controls)
            {
                int sayi2 = int.Parse(string.Join("", tıklanan.Name.Where(char.IsDigit).Select(c => c.ToString())));
                if (sayi2 == sayi)
                {
                    tıklanan.Text = $"{mayin_sayisi}";
                    if (mayin_sayisi == 1)
                    {
                        tıklanan.BackColor = Color.Blue;
                    }
                    else if (mayin_sayisi == 2)
                    {
                        tıklanan.BackColor = Color.Green;

                    }
                    else if (mayin_sayisi == 3)
                    {
                        tıklanan.BackColor = Color.Brown;

                    }
                    else if (mayin_sayisi == 4)
                    {
                        tıklanan.BackColor = Color.Purple;

                    }
                    else if (mayin_sayisi == 5)
                    {
                        tıklanan.BackColor = Color.Orange;

                    }
                    else if (mayin_sayisi == 6)
                    {
                        tıklanan.BackColor = Color.Pink;

                    }
                }

            }
            acildiMi.Add(sayi);
            return mayin_sayisi;
        }
        public List<int> mayinsec(int x)
        {
            Random r = new Random();

            List<int> mayinliste = new List<int>();
            int mayin = 0;
            bool varmi = false;
            for (int i = 0; i < 30; i++)
            {
                varmi = false;
                mayin = r.Next(0, 91);
                if (mayinliste.Count != 0)
                {
                    foreach (int j in mayinliste)
                    {
                        if (j == mayin || x==mayin)
                        {
                            varmi = true;
                            i--;
                            break;
                        }

                    }
                }
                if (varmi == false)
                {
                    mayinliste.Add(mayin);
                }



            }
            return mayinliste;
        }

    }


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
            
            Hucre2 h = new Hucre2();
            h.butonlar(flowLayoutPanel1);
            
        }
    }
}
