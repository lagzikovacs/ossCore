using System;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatDto
    {
        public int Bizonylatkapcsolatkod { get; set; }
        public int Bizonylatkod { get; set; }
        public int Iratkod { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public string Tipus { get; set; }
        public string Azonosito { get; set; }
        public DateTime Keletkezett { get; set; }
        public string Irany { get; set; }
        public string Kuldo { get; set; }
        public string Targy { get; set; }
    }
}
