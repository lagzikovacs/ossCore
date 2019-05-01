using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Ajanlatkeres
    {
        public int Ajanlatkereskod { get; set; }
        public int Particiokod { get; set; }
        public string Ugynoknev { get; set; }
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public decimal? Havifogyasztaskwh { get; set; }
        public decimal? Haviszamlaft { get; set; }
        public decimal? Napelemekteljesitmenyekw { get; set; }
        public string Megjegyzes { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
