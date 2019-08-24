using System;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public partial class UgyfelkapcsolatSimpleDto
    {
        public int Ugyfelkapcsolatkod { get; set; }
        public int Fromugyfelkod { get; set; }
        public int Tougyfelkod { get; set; }
        public string Minoseg { get; set; }
        public string Leiras { get; set; }

        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
