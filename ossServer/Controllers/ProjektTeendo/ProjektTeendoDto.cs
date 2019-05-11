using System;

namespace ossServer.Controllers.ProjektTeendo
{
    public class ProjektTeendoDto
    {
        public int Projektteendokod { get; set; }
        public int Projektkod { get; set; }
        public string Dedikalva { get; set; }
        public int Teendokod { get; set; }
        public string Teendo { get; set; }
        public DateTime Hatarido { get; set; }
        public DateTime? Elvegezve { get; set; }
        public string Leiras { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
