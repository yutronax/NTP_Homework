using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace minesweeper
{
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
                MessageBox.Show($"Skorlar okunurken hata oluştu: {ex.Message}");
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
                MessageBox.Show($"Skor kaydedilirken hata oluştu:");
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
}
