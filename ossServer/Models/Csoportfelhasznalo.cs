using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Csoportfelhasznalo
    {
        public int Csoportfelhasznalokod { get; set; }
        public int Particiokod { get; set; }
        public int Csoportkod { get; set; }
        public int Felhasznalokod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        public virtual Csoport CsoportkodNavigation { get; set; }
        public virtual Felhasznalo FelhasznalokodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
