using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Penztartetel
    {
        public int Penztartetelkod { get; set; }
        public int Particiokod { get; set; }
        public int Penztarkod { get; set; }
        public string Penztarbizonylatszam { get; set; }
        public DateTime Datum { get; set; }
        public string Jogcim { get; set; }
        public int? Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Bizonylatszam { get; set; }
        public decimal Bevetel { get; set; }
        public decimal Kiadas { get; set; }
        public string Megjegyzes { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Penztar PenztarkodNavigation { get; set; }
        public virtual Ugyfel UgyfelkodNavigation { get; set; }
    }
}
