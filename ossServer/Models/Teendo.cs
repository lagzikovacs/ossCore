﻿using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Teendo
    {
        public Teendo()
        {
            Projektteendo = new HashSet<Projektteendo>();
        }

        public int Teendokod { get; set; }
        public int Particiokod { get; set; }
        public string Teendo1 { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Projektteendo> Projektteendo { get; set; }
    }
}
