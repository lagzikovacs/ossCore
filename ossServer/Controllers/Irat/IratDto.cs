namespace ossServer.Controllers.Irat
{
    public class IratDto
    {
        public int Iratkod { get; set; }
        public int Particiokod { get; set; }
        public string Irany { get; set; }
        public System.DateTime Keletkezett { get; set; }
        public int Irattipuskod { get; set; }
        public string Irattipus { get; set; }
        public int? Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Ugyfelcim { get; set; }
        public string Kuldo { get; set; }
        public string Targy { get; set; }

        public System.DateTime? Kikuldesikodidopontja { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
