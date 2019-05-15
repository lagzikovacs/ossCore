using System.Linq;

namespace ossServer.Controllers.Riport
{
    public class BeszerzesRiporttetelDto
    {
        public string Megnevezes { get; set; }
        public decimal Mennyiseg { get; set; }
        public string Me { get; set; }
        public decimal Nettoft { get; set; }
        public IOrderedEnumerable<BeszerzesRiporttetelBizonylatDto> BizonylatFej { get; set; }
    }
}