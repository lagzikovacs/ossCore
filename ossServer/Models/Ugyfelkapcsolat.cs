using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Ugyfelkapcsolat
    {
        public int Ugyfelkapcsolatkod { get; set; }
        public int Particikod { get; set; }
        public int Fromugyfelkod { get; set; }
        public int Tougyfelkod { get; set; }
        public string Minoseg { get; set; }
        public string Leiras { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Ugyfel FromugyfelkodNavigation { get; set; }
        public virtual Particio ParticikodNavigation { get; set; }
        public virtual Ugyfel TougyfelkodNavigation { get; set; }
    }
}
