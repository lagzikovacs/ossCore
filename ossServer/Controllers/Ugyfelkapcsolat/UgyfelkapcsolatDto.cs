using System;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public partial class UgyfelkapcsolatDto
    {
        public int Ugyfelkapcsolatkod { get; set; }
        public int Fromugyfelkod { get; set; }
        public int Tougyfelkod { get; set; }
        

        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Ceg { get; set; }
        public string Beosztas { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Tevekenyseg { get; set; }

        public string Leiras { get; set; }

        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
