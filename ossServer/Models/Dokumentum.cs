using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Dokumentum
    {
        public int Dokumentumkod { get; set; }
        public int Particiokod { get; set; }
        public int Volumekod { get; set; }
        public int Konyvtar { get; set; }
        public int Meret { get; set; }
        public string Ext { get; set; }
        public string Hash { get; set; }
        public int Iratkod { get; set; }
        public string Megjegyzes { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Irat IratkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Volume VolumekodNavigation { get; set; }
    }
}
