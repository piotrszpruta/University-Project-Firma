using System;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace Firma
{
    [Table(Name = "Pracownicy")]
    class Pracownik
    {
        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)]
        public int Id;
        [Column(CanBeNull = false)]
        public string Imie;
        [Column(CanBeNull = false)]
        public string Nazwisko;
        [Column(CanBeNull = false)]
        public string Email;
        [Column(CanBeNull = false)]
        public string Telefon;
    }

    [Table(Name = "Kontrahenci")]
    class Kontrahent
    {
        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)]
        public int Id;
        [Column(CanBeNull = false)]
        public string Nazwa;
        [Column(CanBeNull = true)]
        public string Nip;
        [Column(CanBeNull = false)]
        public string Ulica;
        [Column(CanBeNull = false)]
        public string Miasto;
    }

    [Table(Name = "fakturySprzedazy")]
    class FakturaSprzedazy
    {
        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)] 
        public int Id;
        [Column(CanBeNull = false)]
        public string Numer;
        [Column(CanBeNull = false)]
        public double Netto;
        [Column(CanBeNull = false)]
        public double Vat;
        [Column(CanBeNull = false)]
        public double Zaplacono;
        [Column(CanBeNull = false)]
        public DateTime Data;
        [Column(CanBeNull = false)]
        public int KontrahentId;
        [Column(CanBeNull = false)]
        public int PracownikId;
    }
}
