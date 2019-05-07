using System;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumDto
    {
        public int Dokumentumkod { get; set; }
        public int Volumekod { get; set; }
        public int Konyvtar { get; set; }
        public int Meret { get; set; }
        public string Ext { get; set; }
        public string Hash { get; set; }
        public int Iratkod { get; set; }
        public string Megjegyzes { get; set; }

        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
