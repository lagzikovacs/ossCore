namespace ossServer.Controllers.Penztar
{
    public class PenztarDto
    {
        public int Penztarkod { get; set; }
        public string Penztar1 { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public bool Nyitva { get; set; }
        public decimal Egyenleg { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
