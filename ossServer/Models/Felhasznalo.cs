using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Felhasznalo
    {
        public Felhasznalo()
        {
            Csoportfelhasznalo = new HashSet<Csoportfelhasznalo>();
            Esemenynaplo = new HashSet<Esemenynaplo>();
            Session = new HashSet<Session>();
        }

        public int Felhasznalokod { get; set; }
        public string Azonosito { get; set; }
        public string Jelszo { get; set; }
        public string Nev { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Statusz { get; set; }
        public DateTime Statuszkelte { get; set; }
        public bool Logonlog { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual ICollection<Csoportfelhasznalo> Csoportfelhasznalo { get; set; }
        public virtual ICollection<Esemenynaplo> Esemenynaplo { get; set; }
        public virtual ICollection<Session> Session { get; set; }
    }
}
