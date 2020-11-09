using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Projekt
    {
        public Projekt()
        {
            Projektjegyzet = new HashSet<Projektjegyzet>();
            Projektkapcsolat = new HashSet<Projektkapcsolat>();
            Szamlazasirend = new HashSet<Szamlazasirend>();
        }

        public int Projektkod { get; set; }
        public int Particiokod { get; set; }
        public int Statusz { get; set; }
        public int Ugyfelkod { get; set; }
        public string Telepitesicim { get; set; }
        public string Projektjellege { get; set; }
        public string Muszakiallapot { get; set; }
        public string Inverter { get; set; }
        public string Inverterallapot { get; set; }
        public string Napelem { get; set; }
        public string Napelemallapot { get; set; }
        public decimal Dckw { get; set; }
        public decimal Ackva { get; set; }
        public decimal Vallalasiarnetto { get; set; }
        public int Penznemkod { get; set; }
        public string Munkalapszam { get; set; }
        public DateTime Keletkezett { get; set; }
        public DateTime? Megrendelve { get; set; }
        public DateTime? Kivitelezesihatarido { get; set; }
        public string Megjegyzes { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penznem PenznemkodNavigation { get; set; }
        public virtual Ugyfel UgyfelkodNavigation { get; set; }
        public virtual ICollection<Projektjegyzet> Projektjegyzet { get; set; }
        public virtual ICollection<Projektkapcsolat> Projektkapcsolat { get; set; }
        public virtual ICollection<Szamlazasirend> Szamlazasirend { get; set; }
    }
}
