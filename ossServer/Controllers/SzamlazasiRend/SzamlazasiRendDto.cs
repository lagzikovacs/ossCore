namespace ossServer.Controllers.SzamlazasiRend
{
    public class SzamlazasiRendDto
    {
        public int Szamlazasirendkod { get; set; }
        public int Projektkod { get; set; }
        public decimal Osszeg { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public string Megjegyzes { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
