namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatDto
    {
        public int Projektkapcsolatkod { get; set; }
        public int Projektkod { get; set; }
        public int? Iratkod { get; set; }
        public int? Bizonylatkod { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }

        //CALC
        public string Kapcsolat { get; set; }
        public string Tipus { get; set; }
        public string Azonosito { get; set; }
        public System.DateTime Keletkezett { get; set; }
        public string Irany { get; set; }
        public string Kuldo { get; set; }
        public string Targy { get; set; }
    }
}
