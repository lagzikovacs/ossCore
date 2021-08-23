using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Ugyfel
    {
        public Ugyfel()
        {
            Bizonylat = new HashSet<Bizonylat>();
            Irat = new HashSet<Irat>();
            Penztartetel = new HashSet<Penztartetel>();
            Projekt = new HashSet<Projekt>();
            Ugyfelterlog = new HashSet<Ugyfelterlog>();
        }

        public int Ugyfelkod { get; set; }
        public int Particiokod { get; set; }
        public int Csoport { get; set; }
        public string Nev { get; set; }
        public string Ceg { get; set; }
        public string Beosztas { get; set; }
        public string Iranyitoszam { get; set; }
        public int? Helysegkod { get; set; }
        public string Utcahazszam { get; set; }
        public string Kozterulet { get; set; }
        public string Kozterulettipus { get; set; }
        public string Hazszam { get; set; }
        public string Adoszam { get; set; }
        public string Euadoszam { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public int? Tevekenysegkod { get; set; }
        public string Egyeblink { get; set; }
        public string Ajanlotta { get; set; }
        public string Megjegyzes { get; set; }
        public bool Vasarolt { get; set; }
        public bool Hirlevel { get; set; }
        public string Kikuldesikod { get; set; }
        public DateTime? Kikuldesikodidopontja { get; set; }
        public int? Halox { get; set; }
        public int? Haloy { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Helyseg HelysegkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Tevekenyseg TevekenysegkodNavigation { get; set; }
        public virtual ICollection<Bizonylat> Bizonylat { get; set; }
        public virtual ICollection<Irat> Irat { get; set; }
        public virtual ICollection<Penztartetel> Penztartetel { get; set; }
        public virtual ICollection<Projekt> Projekt { get; set; }
        public virtual ICollection<Ugyfelterlog> Ugyfelterlog { get; set; }
    }
}
