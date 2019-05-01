using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Projektteendo
    {
        public int Projektteendokod { get; set; }
        public int Particiokod { get; set; }
        public int Projektkod { get; set; }
        public string Dedikalva { get; set; }
        public int Teendokod { get; set; }
        public DateTime Hatarido { get; set; }
        public DateTime? Elvegezve { get; set; }
        public string Leiras { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Projekt ProjektkodNavigation { get; set; }
        public virtual Teendo TeendokodNavigation { get; set; }
    }
}
