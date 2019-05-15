using System;

namespace ossServer.Controllers.Riport
{
    public class KovetelesTartozasRiporttetelDto
    {
        public int Bizonylatkod { get; set; }
        public decimal Afa { get; set; }
        public DateTime Bizonylatkelte { get; set; }
        public string Bizonylatszam { get; set; }
        public decimal Brutto { get; set; }
        public DateTime Fizetesihatarido { get; set; }
        public string Fizetesimod { get; set; }
        public decimal Netto { get; set; }
        public string Penznem { get; set; }
        public DateTime Teljesiteskelte { get; set; }
        public string Ugyfelnev { get; set; }
        public decimal Arfolyam { get; set; }
        public decimal Megfizetve { get; set; }
    }
}