namespace ossServer.Controllers.Ajanlatkeres
{
    public class WebesAjanlatkeresParam
    {
        public int Particiokod { get; set; }
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public decimal? Havifogyasztaskwh { get; set; }
        public decimal? Haviszamlaft { get; set; }
        public decimal? Napelemekteljesitmenyekw { get; set; }
        public string Megjegyzes { get; set; }
        public string Ugynoknev { get; set; }
    }
}
