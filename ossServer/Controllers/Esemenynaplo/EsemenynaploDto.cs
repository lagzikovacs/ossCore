using System;

namespace ossServer.Controllers.Esemenynaplo
{
    public class EsemenynaploDto
    {
        public int Esemenynaplokod { get; set; }
        public DateTime Idopont { get; set; }
        public string Particio { get; set; }
        public string Csoport { get; set; }
        public string Muvelet { get; set; }
    }
}
