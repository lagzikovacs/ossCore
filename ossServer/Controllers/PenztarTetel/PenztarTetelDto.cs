using System;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelDto
    {
        public int Penztartetelkod { get; set; }
        public int Penztarkod { get; set; }
        public string Penztarbizonylatszam { get; set; }
        public System.DateTime Datum { get; set; }
        public string Jogcim { get; set; }
        public Nullable<int> Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Bizonylatszam { get; set; }
        public decimal Bevetel { get; set; }
        public decimal Kiadas { get; set; }
        public string Megjegyzes { get; set; }

        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
