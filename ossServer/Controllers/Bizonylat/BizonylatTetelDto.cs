namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTetelDto
    {
        public int Bizonylattetelkod { get; set; }
        public int Bizonylatkod { get; set; }
        public int Cikkkod { get; set; }
        public string Megnevezes { get; set; }
        public string Megjegyzes { get; set; }
        public decimal Tomegkg { get; set; }
        public decimal Ossztomegkg { get; set; }
        public int Mekod { get; set; }
        public string Me { get; set; }
        public int Afakulcskod { get; set; }
        public string Afakulcs { get; set; }
        public decimal Afamerteke { get; set; }
        public decimal Egysegar { get; set; }
        public decimal Mennyiseg { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }
        public bool Kozvetitettszolgaltatas { get; set; }
        public bool Ezeloleg { get; set; }

        public bool Termekdijas { get; set; }
        public int? Termekdijkod { get; set; }
        public string Termekdijkt { get; set; }
        public string Termekdijmegnevezes { get; set; }
        public decimal? Termekdijegysegar { get; set; }
        public decimal? Termekdij { get; set; }
    }
}
