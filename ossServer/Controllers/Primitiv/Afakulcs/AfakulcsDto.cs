using System.Runtime.Serialization;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsDto
    {
        public int Afakulcskod { get; set; }
        public int Particiokod { get; set; }
        public string Afakulcs1 { get; set; }
        public decimal Afamerteke { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
