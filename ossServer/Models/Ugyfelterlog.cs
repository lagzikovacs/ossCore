using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Ugyfelterlog
    {
        public int Ugyfelterlogkod { get; set; }
        public int Particiokod { get; set; }
        public int Ugyfelkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Ugyfel UgyfelkodNavigation { get; set; }
    }
}
