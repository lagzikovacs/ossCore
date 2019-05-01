using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Afakulcs
    {
        public Afakulcs()
        {
            Bizonylatafa = new HashSet<Bizonylatafa>();
            Bizonylattetel = new HashSet<Bizonylattetel>();
            Cikk = new HashSet<Cikk>();
        }

        public int Afakulcskod { get; set; }
        public int Particiokod { get; set; }
        public string Afakulcs1 { get; set; }
        public decimal Afamerteke { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Bizonylatafa> Bizonylatafa { get; set; }
        public virtual ICollection<Bizonylattetel> Bizonylattetel { get; set; }
        public virtual ICollection<Cikk> Cikk { get; set; }
    }
}
