namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegDto
    {
        public int Helysegkod { get; set; }
        public int Particiokod { get; set; }
        public string Helysegnev { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
