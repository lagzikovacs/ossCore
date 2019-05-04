using System;

namespace ossServer.Controllers.Session
{
    public class SessionDto
    {
        public string Sessionid { get; set; }
        public string Particio { get; set; }
        public string Csoport { get; set; }
        public int Felhasznalokod { get; set; }
        public string Felhasznalo { get; set; }
        public string Azonosito { get; set; }
        public bool Logol { get; set; }
        public string Ip { get; set; }
        public string Host { get; set; }
        public string Osuser { get; set; }
        public DateTime Letrehozva { get; set; }
        public DateTime Ervenyes { get; set; }
    }
}
