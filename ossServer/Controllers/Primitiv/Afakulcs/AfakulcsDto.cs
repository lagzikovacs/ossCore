namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsDto
    {
        public int AFAKULCSKOD { get; set; }
        public int PARTICIOKOD { get; set; }
        public string AFAKULCS1 { get; set; }
        public decimal AFAMERTEKE { get; set; }

        public System.DateTime LETREHOZVA { get; set; }
        public string LETREHOZTA { get; set; }
        public System.DateTime MODOSITVA { get; set; }
        public string MODOSITOTTA { get; set; }
    }
}
