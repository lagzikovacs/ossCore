using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Csoport
    {
        public Csoport()
        {
            Csoportfelhasznalo = new HashSet<Csoportfelhasznalo>();
            Csoportjog = new HashSet<Csoportjog>();
            Esemenynaplo = new HashSet<Esemenynaplo>();
            Session = new HashSet<Session>();
        }

        public int Csoportkod { get; set; }
        public int Particiokod { get; set; }
        public string Csoport1 { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Csoportfelhasznalo> Csoportfelhasznalo { get; set; }
        public virtual ICollection<Csoportjog> Csoportjog { get; set; }
        public virtual ICollection<Esemenynaplo> Esemenynaplo { get; set; }
        public virtual ICollection<Session> Session { get; set; }
    }
}
