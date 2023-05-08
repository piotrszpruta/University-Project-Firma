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
    public partial class FakturaView : Form
    {
        static DataContext bazaDanychFirma = new DataContext(connectionString);
        static Table<Kontrahent> listaKontrahentow = bazaDanychFirma.GetTable<Kontrahent>();
        static Table<Pracownik> listaPracownikow = bazaDanychFirma.GetTable<Pracownik>();
        public FakturaView()
        {
            InitializeComponent();
            // Załaduj potrzebne dane
            ZaladujDanePracownikow();
            ZaladujDaneKontrahentow();
        }

        private void ZaladujDanePracownikow()
        {
            var lp = from Pracownik in listaPracownikow
                     orderby Pracownik.Nazwisko
                     select new
                     {
                         Pracownik.Id,
                         Pracownik.Imie,
                         Pracownik.Nazwisko
                     };

            comboBoxPracownicy.DisplayMember = "danePracownika";
            comboBoxPracownicy.ValueMember = "idPracownika";
            comboBoxPracownicy.DataSource = lp.AsEnumerable().Select(t => new
            {
                idPracownika = t.Id,
                danePracownika = $"{t.Imie} {t.Nazwisko}"
            }).ToList();
        }

        public void ZaladujDaneKontrahentow()
        {
            var lp = from Kontrahent in listaKontrahentow
                     orderby Kontrahent.Nazwa
                     select new
                     {
                         Kontrahent.Id,
                         Kontrahent.Nazwa
                     };

            comboBoxKontrahenci.DisplayMember = "daneKontrahenta";
            comboBoxKontrahenci.ValueMember = "idKontrahenta";
            comboBoxKontrahenci.DataSource = lp.AsEnumerable().Select(t => new
            {
                idKontrahenta = t.Id,
                daneKontrahenta = t.Nazwa
            }).ToList();
        }

        private void zapiszDane_Click(object sender, EventArgs e)
        {

        }
    }
}
