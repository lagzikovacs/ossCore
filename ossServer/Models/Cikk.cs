using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Cikk
    {
        public Cikk()
        {
            Bizonylattetel = new HashSet<Bizonylattetel>();
        }

        public int Cikkkod { get; set; }
        public int Particiokod { get; set; }
        public string Megnevezes { get; set; }
        public int Mekod { get; set; }
        public int Afakulcskod { get; set; }
        public decimal Egysegar { get; set; }
        public bool Keszletetkepez { get; set; }
        public decimal Tomegkg { get; set; }
        public int? Termekdijkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Afakulcs AfakulcskodNavigation { get; set; }
        public virtual Mennyisegiegyseg MekodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Termekdij TermekdijkodNavigation { get; set; }
        public virtual ICollection<Bizonylattetel> Bizonylattetel { get; set; }
    }
}
