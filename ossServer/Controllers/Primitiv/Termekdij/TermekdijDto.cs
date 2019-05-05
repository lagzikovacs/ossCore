namespace ossServer.Controllers.Primitiv.Termekdij
{
    public class TermekdijDto
    {
        public int Termekdijkod { get; set; }
        public int Particiokod { get; set; }
        public string Termekdijkt { get; set; }
        public string Termekdijmegnevezes { get; set; }
        public decimal Termekdijegysegar { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
