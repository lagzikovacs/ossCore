using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Irattipus
    {
        public Irattipus()
        {
            Irat = new HashSet<Irat>();
        }

        public int Irattipuskod { get; set; }
        public int Particiokod { get; set; }
        public string Irattipus1 { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Irat> Irat { get; set; }
    }
}
