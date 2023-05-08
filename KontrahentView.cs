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
    public partial class KontrahentView : Form
    {
        static DataContext bazaDanychFirma = new DataContext(connectionString);
        static Table<Kontrahent> listaKontrahentow = bazaDanychFirma.GetTable<Kontrahent>();

        public KontrahentView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nazwa = textBoxNazwa.Text;
            string miasto = textBoxMiasto.Text;
            string ulica = textBoxUlica.Text;
            string nip = textBoxNip.Text;

            int noweId = listaKontrahentow.Max(Kontrahent => Kontrahent.Id) + 1;
            Kontrahent nowyKontrahent = new Kontrahent
            {
                Id = noweId,
                Nazwa = nazwa,
                Nip = nip,
                Miasto = miasto,
                Ulica = ulica,
            };
            listaKontrahentow.InsertOnSubmit(nowyKontrahent);
            bazaDanychFirma.SubmitChanges(); // zapisywanie zmian
            MessageBox.Show("Faktura wystawiona");
        }
    }
}
