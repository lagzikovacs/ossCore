using System;

namespace ossServer.Controllers.Cikk
{
    public class CikkMozgasTetelDto
    {
        public string Bizonylatszam { get; set; }
        public string Ugyfelnev { get; set; }
        public DateTime Teljesiteskelte { get; set; }
        public decimal Mennyiseg { get; set; }
        public string Me { get; set; }
        public decimal Egysegar { get; set; }
        public string Penznem { get; set; }
        public decimal Netto { get; set; }
        public decimal Nettoft { get; set; }
    }
}
