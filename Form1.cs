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
        }

    }
}
