using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Penztar
    {
        public Penztar()
        {
            Penztartetel = new HashSet<Penztartetel>();
        }

        public int Penztarkod { get; set; }
        public int Particiokod { get; set; }
        public string Penztar1 { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public bool Nyitva { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penznem PenznemkodNavigation { get; set; }
        public virtual ICollection<Penztartetel> Penztartetel { get; set; }
    }
}
