namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresDto
    {
        public int Ajanlatkereskod { get; set; }
        public int Particiokod { get; set; }
        public string Ugynoknev { get; set; }
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public decimal? Havifogyasztaskwh { get; set; }
        public decimal? Haviszamlaft { get; set; }
        public decimal? Napelemekteljesitmenyekw { get; set; }
        public string Megjegyzes { get; set; }


        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
