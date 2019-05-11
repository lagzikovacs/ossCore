using ossServer.Enums;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTipusLeiro
    {
        public BizonylatTipus BizonylatTipus { get; set; }
        public string BizonylatNev { get; set; }
        public string BizonylatFejlec { get; set; }
        public KodNev KodNev { get; set; }
        public bool GenBizonylatszam { get; set; }
        public bool Stornozhato { get; set; }
        public JogKod JogKod { get; set; }
        public bool FizetesiModIs { get; set; }
        public string Elotag { get; set; }
    }
}
