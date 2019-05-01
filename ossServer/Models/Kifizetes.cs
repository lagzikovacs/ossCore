using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Kifizetes
    {
        public int Kifizeteskod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylatkod { get; set; }
        public DateTime Datum { get; set; }
        public decimal Osszeg { get; set; }
        public int Penznemkod { get; set; }
        public int Fizetesimodkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Bizonylat BizonylatkodNavigation { get; set; }
        public virtual Fizetesimod FizetesimodkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penznem PenznemkodNavigation { get; set; }
    }
}
