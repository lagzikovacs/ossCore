using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Csoportjog
    {
        public int Csoportjogkod { get; set; }
        public int Particiokod { get; set; }
        public int Csoportkod { get; set; }
        public int Lehetsegesjogkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        public virtual Csoport CsoportkodNavigation { get; set; }
        public virtual Lehetsegesjog LehetsegesjogkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
