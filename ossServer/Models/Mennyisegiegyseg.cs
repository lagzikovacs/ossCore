using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Mennyisegiegyseg
    {
        public Mennyisegiegyseg()
        {
            Cikk = new HashSet<Cikk>();
        }

        public int Mekod { get; set; }
        public int Particiokod { get; set; }
        public string Me { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Cikk> Cikk { get; set; }
    }
}
