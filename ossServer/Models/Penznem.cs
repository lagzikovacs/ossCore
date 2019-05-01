using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Penznem
    {
        public Penznem()
        {
            Bizonylat = new HashSet<Bizonylat>();
            Kifizetes = new HashSet<Kifizetes>();
            Penztar = new HashSet<Penztar>();
            Projekt = new HashSet<Projekt>();
            Szamlazasirend = new HashSet<Szamlazasirend>();
        }

        public int Penznemkod { get; set; }
        public int Particiokod { get; set; }
        public string Penznem1 { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Bizonylat> Bizonylat { get; set; }
        public virtual ICollection<Kifizetes> Kifizetes { get; set; }
        public virtual ICollection<Penztar> Penztar { get; set; }
        public virtual ICollection<Projekt> Projekt { get; set; }
        public virtual ICollection<Szamlazasirend> Szamlazasirend { get; set; }
    }
}
