namespace ossServer.Controllers.Particio
{
    public class ParticioDto
    {
        public int Particiokod { get; set; }
        public string Megnevezes { get; set; }

        public string Szallito { get; set; }
        public string Bizonylat { get; set; }
        public string Projekt { get; set; }
        public string Volume { get; set; }
        public string Emails { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
