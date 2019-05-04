using System;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloDto
    {
        public int Felhasznalokod { get; set; }
        public string Azonosito { get; set; }
        public string Nev { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Statusz { get; set; }
        public DateTime Statuszkelte { get; set; }
        public bool Logonlog { get; set; }
        public bool Csoporttag { get; set; }


        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
