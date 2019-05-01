using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Projektkapcsolat
    {
        public int Projektkapcsolatkod { get; set; }
        public int Particiokod { get; set; }
        public int Projektkod { get; set; }
        public int? Iratkod { get; set; }
        public int? Bizonylatkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        public virtual Bizonylat BizonylatkodNavigation { get; set; }
        public virtual Irat IratkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Projekt ProjektkodNavigation { get; set; }
    }
}
