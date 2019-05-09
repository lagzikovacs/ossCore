using System;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelDto
    {
        public int PENZTARTETELKOD { get; set; }
        public int PENZTARKOD { get; set; }
        public string PENZTARBIZONYLATSZAM { get; set; }
        public System.DateTime DATUM { get; set; }
        public string JOGCIM { get; set; }
        public Nullable<int> UGYFELKOD { get; set; }
        public string UGYFELNEV { get; set; }
        public string BIZONYLATSZAM { get; set; }
        public decimal BEVETEL { get; set; }
        public decimal KIADAS { get; set; }
        public string MEGJEGYZES { get; set; }

        public System.DateTime LETREHOZVA { get; set; }
        public string LETREHOZTA { get; set; }
        public System.DateTime MODOSITVA { get; set; }
        public string MODOSITOTTA { get; set; }
    }
}
