using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Szamlazasirend
    {
        public int Szamlazasirendkod { get; set; }
        public int Particiokod { get; set; }
        public int Projektkod { get; set; }
        public decimal Osszeg { get; set; }
        public int Penznemkod { get; set; }
        public string Megjegyzes { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penznem PenznemkodNavigation { get; set; }
        public virtual Projekt ProjektkodNavigation { get; set; }
    }
}
