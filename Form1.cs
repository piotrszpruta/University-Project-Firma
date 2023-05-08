using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firma
{
    public partial class Form1 : Form
    {
        static DataContext bazaDanychFirma = new DataContext(connectionString);
        static Table<Pracownik> listaPracownikow = bazaDanychFirma.GetTable<Pracownik>();
        static Table<FakturaSprzedazy> listaFakturSprzedazy = bazaDanychFirma.GetTable<FakturaSprzedazy>();

        static Table<Kontrahent> listaKontrahentow = bazaDanychFirma.GetTable<Kontrahent>();
        public Form1()
        {
            InitializeComponent();
        }

        private void listaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // pobieranie kolekcji
            var lp = from Pracownik in listaPracownikow
                     orderby Pracownik.Nazwisko
                     select new
                     {
                         Pracownik.Id,
                         Pracownik.Imie,
                         Pracownik.Nazwisko,
                         Pracownik.Telefon,
                         Pracownik.Email
                     };
            dataGridView1.DataSource = lp;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            // Poprawienie funkcjonalności
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int wiersz = Convert.ToInt32(dataGridView1.CurrentRow.Index);
                string imie = dataGridView1.Rows[wiersz].Cells[1].Value.ToString();
                string nazwisko = dataGridView1.Rows[wiersz].Cells[2].Value.ToString();
                string telefon = dataGridView1.Rows[wiersz].Cells[3].Value.ToString();
                string email = dataGridView1.Rows[wiersz].Cells[4].Value.ToString();
                int idPracownik = Convert.ToInt32(dataGridView1.Rows[wiersz].Cells[0].Value.ToString());
                textBoxImie.Text = imie;
                textBoxNazwisko.Text = nazwisko;
                textBoxEmail.Text = email;
                textBoxTelefon.Text = telefon;

                var ls = from FakturaSprzedazy in listaFakturSprzedazy
                         where (FakturaSprzedazy.PracownikId == idPracownik)
                         select new
                         {
                             FakturaSprzedazy.Id,
                             FakturaSprzedazy.Numer,
                             FakturaSprzedazy.Netto,
                             FakturaSprzedazy.Vat,
                             FakturaSprzedazy.Data,
                             FakturaSprzedazy.Zaplacono,
                         };

                dataGridView2.DataSource = ls;
            }
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dodawanie osoby do tabeli
            int noweId = listaPracownikow.Max(Pracownik => Pracownik.Id) + 1;
            Pracownik nowyPracownik = new Pracownik
            {
                Id = noweId,
                Imie = textBoxImie.Text,
                Nazwisko = textBoxNazwisko.Text,
                Email = textBoxEmail.Text,
                Telefon = textBoxTelefon.Text,
            };
            listaPracownikow.InsertOnSubmit(nowyPracownik);
            bazaDanychFirma.SubmitChanges(); // zapisywanie zmian
            listaToolStripMenuItem_Click(this, null); // odświeżenie siatki
        }

        private void usunięcieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // odczytujemy wiersz, w którym jest pozycja do usunięcia
            int wiersz = Convert.ToInt32(dataGridView1.CurrentRow.Index);
            // odczytujemy id osoby do usunięcia
            int idPracownik = Convert.ToInt32(dataGridView1.Rows[wiersz].Cells[0].Value.ToString());
            // wybieranie elementów do usunięcia i ich oznaczanie
            IEnumerable<Pracownik> doSkasowania = from pracownik in listaPracownikow
                                                  where pracownik.Id == idPracownik
                                                  select pracownik;
            listaPracownikow.DeleteAllOnSubmit(doSkasowania);
            // zapisywanie zmian
            bazaDanychFirma.SubmitChanges();
            // wyświetlanie tabeli
            listaToolStripMenuItem_Click(this, null);
        }

        private void edycjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int wiersz = Convert.ToInt32(dataGridView1.CurrentRow.Index);
            int idPracownik = Convert.ToInt32(dataGridView1.Rows[wiersz].Cells[0].Value.ToString());
            foreach (Pracownik pracownik in listaPracownikow)
            {
                if (pracownik.Id == idPracownik)
                {
                    pracownik.Imie = textBoxImie.Text;
                    pracownik.Nazwisko = textBoxNazwisko.Text;
                    pracownik.Email = textBoxEmail.Text;
                    pracownik.Telefon = textBoxTelefon.Text;
                }
            }
            // zapisywanie zmian
            bazaDanychFirma.SubmitChanges();
            listaToolStripMenuItem_Click(this, null);
        }

        private void listaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // pobieranie kolekcji kontahentow
            var lp = from Kontrahent in listaKontrahentow
                     orderby Kontrahent.Nazwa
                     select new
                     {
                         Kontrahent.Id,
                         Kontrahent.Nazwa,
                         Kontrahent.Nip,
                         Kontrahent.Ulica
                     };
            dataGridView1.DataSource = lp;
        }

        private void nowaFakturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KontrahentView nowyKontrahent = new KontrahentView();
            nowyKontrahent.Show();
        }

        private void pokażNieuregulowaneFakturyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string Status = "Nie uregulowana";
            var fv = from FakturaSprzedazy in listaFakturSprzedazy
                     where FakturaSprzedazy.Vat + FakturaSprzedazy.Netto > FakturaSprzedazy.Zaplacono
                     select new
                     {
                         Status,
                         FakturaSprzedazy.Numer,
                         FakturaSprzedazy.Data,
                         FakturaSprzedazy.Netto,
                         FakturaSprzedazy.Vat,
                         FakturaSprzedazy.Zaplacono,
                         FakturaSprzedazy.KontrahentId,
                         FakturaSprzedazy.PracownikId,
                     };

            dataGridView2.DataSource = fv;
        }
    }
}
