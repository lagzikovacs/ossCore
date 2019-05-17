namespace ossServer.Controllers.Kifizetes
{
    public class KifizetesDto
    {
        public int Kifizeteskod { get; set; }
        public int Bizonylatkod { get; set; }
        public System.DateTime Datum { get; set; }
        public decimal Osszeg { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public int Fizetesimodkod { get; set; }
        public string Fizetesimod { get; set; }


        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
