using ossServer.Controllers.Bizonylat;

namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatParam
    {
        public int ProjektKod { get; set; }
        public int BizonylatKod { get; set; }
        public int IratKod { get; set; }
        public BizonylatDto Dto { get; set; }
    }
}
