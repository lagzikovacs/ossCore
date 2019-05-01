using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Termekdij
    {
        public Termekdij()
        {
            Bizonylattermekdij = new HashSet<Bizonylattermekdij>();
            Bizonylattetel = new HashSet<Bizonylattetel>();
            Cikk = new HashSet<Cikk>();
        }

        public int Termekdijkod { get; set; }
        public int Particiokod { get; set; }
        public string Termekdijkt { get; set; }
        public string Termekdijmegnevezes { get; set; }
        public decimal Termekdijegysegar { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Bizonylattermekdij> Bizonylattermekdij { get; set; }
        public virtual ICollection<Bizonylattetel> Bizonylattetel { get; set; }
        public virtual ICollection<Cikk> Cikk { get; set; }
    }
}
