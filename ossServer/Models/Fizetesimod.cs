using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Fizetesimod
    {
        public Fizetesimod()
        {
            Bizonylat = new HashSet<Bizonylat>();
            Kifizetes = new HashSet<Kifizetes>();
        }

        public int Fizetesimodkod { get; set; }
        public int Particiokod { get; set; }
        public string Fizetesimod1 { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Bizonylat> Bizonylat { get; set; }
        public virtual ICollection<Kifizetes> Kifizetes { get; set; }
    }
}
