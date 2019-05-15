using System;

namespace ossServer.Controllers.Riport
{
    public class BeszerzesRiporttetelBizonylatDto
    {
        public string Bizonylatszam { get; set; }
        public DateTime Bizonylatkelte { get; set; }
        public DateTime Teljesiteskelte { get; set; }
        public string Ugyfelnev { get; set; }
    }
}