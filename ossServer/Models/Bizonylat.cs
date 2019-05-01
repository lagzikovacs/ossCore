using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Bizonylat
    {
        public Bizonylat()
        {
            Bizonylatafa = new HashSet<Bizonylatafa>();
            Bizonylatkapcsolat = new HashSet<Bizonylatkapcsolat>();
            Bizonylattermekdij = new HashSet<Bizonylattermekdij>();
            Bizonylattetel = new HashSet<Bizonylattetel>();
            InverseStornozobizonylatkodNavigation = new HashSet<Bizonylat>();
            InverseStornozottbizonylatkodNavigation = new HashSet<Bizonylat>();
            Kifizetes = new HashSet<Kifizetes>();
            Navfeltoltes = new HashSet<Navfeltoltes>();
            Projektkapcsolat = new HashSet<Projektkapcsolat>();
        }

        public int Bizonylatkod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylattipuskod { get; set; }
        public string Bizonylatszam { get; set; }
        public string Szallitonev { get; set; }
        public string Szallitoiranyitoszam { get; set; }
        public string Szallitohelysegnev { get; set; }
        public string Szallitoutcahazszam { get; set; }
        public string Szallitobankszamla1 { get; set; }
        public string Szallitobankszamla2 { get; set; }
        public string Szallitoadotorzsszam { get; set; }
        public string Szallitoadoafakod { get; set; }
        public string Szallitoadomegyekod { get; set; }
        public int Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Ugyfeliranyitoszam { get; set; }
        public int? Ugyfelhelysegkod { get; set; }
        public string Ugyfelhelysegnev { get; set; }
        public string Ugyfelkozterulet { get; set; }
        public string Ugyfelkozterulettipus { get; set; }
        public string Ugyfelhazszam { get; set; }
        public string Ugyfeladoszam { get; set; }
        public DateTime Bizonylatkelte { get; set; }
        public DateTime Teljesiteskelte { get; set; }
        public int? Fizetesimodkod { get; set; }
        public string Fizetesimod { get; set; }
        public DateTime Fizetesihatarido { get; set; }
        public DateTime? Szallitasihatarido { get; set; }
        public bool? Kifizetesrendben { get; set; }
        public bool? Kiszallitva { get; set; }
        public string Megjegyzesfej { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }
        public decimal Termekdij { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public decimal Arfolyam { get; set; }
        public string Azaz { get; set; }
        public int Nyomtatottpeldanyokszama { get; set; }
        public bool Ezstornozo { get; set; }
        public bool Ezstornozott { get; set; }
        public int? Stornozobizonylatkod { get; set; }
        public int? Stornozottbizonylatkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Fizetesimod FizetesimodkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penznem PenznemkodNavigation { get; set; }
        public virtual Bizonylat StornozobizonylatkodNavigation { get; set; }
        public virtual Bizonylat StornozottbizonylatkodNavigation { get; set; }
        public virtual Helyseg UgyfelhelysegkodNavigation { get; set; }
        public virtual Ugyfel UgyfelkodNavigation { get; set; }
        public virtual ICollection<Bizonylatafa> Bizonylatafa { get; set; }
        public virtual ICollection<Bizonylatkapcsolat> Bizonylatkapcsolat { get; set; }
        public virtual ICollection<Bizonylattermekdij> Bizonylattermekdij { get; set; }
        public virtual ICollection<Bizonylattetel> Bizonylattetel { get; set; }
        public virtual ICollection<Bizonylat> InverseStornozobizonylatkodNavigation { get; set; }
        public virtual ICollection<Bizonylat> InverseStornozottbizonylatkodNavigation { get; set; }
        public virtual ICollection<Kifizetes> Kifizetes { get; set; }
        public virtual ICollection<Navfeltoltes> Navfeltoltes { get; set; }
        public virtual ICollection<Projektkapcsolat> Projektkapcsolat { get; set; }
    }
}
