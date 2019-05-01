using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Bizonylatkapcsolat
    {
        public int Bizonylatkapcsolatkod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylatkod { get; set; }
        public int Iratkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        public virtual Bizonylat BizonylatkodNavigation { get; set; }
        public virtual Irat IratkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
