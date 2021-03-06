﻿using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Helyseg
    {
        public Helyseg()
        {
            Bizonylat = new HashSet<Bizonylat>();
            Ugyfel = new HashSet<Ugyfel>();
        }

        public int Helysegkod { get; set; }
        public int Particiokod { get; set; }
        public string Helysegnev { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Bizonylat> Bizonylat { get; set; }
        public virtual ICollection<Ugyfel> Ugyfel { get; set; }
    }
}
