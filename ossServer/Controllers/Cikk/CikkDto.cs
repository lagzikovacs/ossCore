namespace ossServer.Controllers.Cikk
{
    public class CikkDto
    {
        public int Cikkkod { get; set; }
        public int Particiokod { get; set; }
        public string Megnevezes { get; set; }
        public int Mekod { get; set; }
        public string Me { get; set; }
        public int Afakulcskod { get; set; }
        public string Afakulcs { get; set; }
        public decimal Afamerteke { get; set; }
        public decimal Egysegar { get; set; }
        public bool Keszletetkepez { get; set; }
        public decimal Tomegkg { get; set; }
        public int? Termekdijkod { get; set; }
        public string Termekdijkt { get; set; }
        public string Termekdijmegnevezes { get; set; }
        public decimal? Termekdijegysegar { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
