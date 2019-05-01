using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Esemenynaplo
    {
        public int Esemenynaplokod { get; set; }
        public int Esemenyazonosito { get; set; }
        public DateTime Idopont { get; set; }
        public int? Particiokod { get; set; }
        public string Particio { get; set; }
        public int? Csoportkod { get; set; }
        public string Csoport { get; set; }
        public int Felhasznalokod { get; set; }
        public string Felhasznalo { get; set; }
        public string Azonosito { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
        public string Osuser { get; set; }

        public virtual Csoport CsoportkodNavigation { get; set; }
        public virtual Felhasznalo FelhasznalokodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
