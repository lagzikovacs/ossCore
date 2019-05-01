using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Session
    {
        public string Sessionid { get; set; }
        public int? Particiokod { get; set; }
        public string Particio { get; set; }
        public int? Csoportkod { get; set; }
        public string Csoport { get; set; }
        public int Felhasznalokod { get; set; }
        public string Felhasznalo { get; set; }
        public string Azonosito { get; set; }
        public bool Logol { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
        public string Osuser { get; set; }
        public DateTime Letrehozva { get; set; }
        public DateTime Ervenyes { get; set; }

        public virtual Csoport CsoportkodNavigation { get; set; }
        public virtual Felhasznalo FelhasznalokodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
