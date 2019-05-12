using ossServer.Enums;

namespace ossServer.Controllers.Ajanlat
{
    public class AjanlatBuf
    {
        public AjanlatTetelTipus AjanlatTetelTipus { get; set; }
        public string TetelTipus { get; set; }
        public int CikkKod { get; set; }
        public string CikkNev { get; set; }
        public decimal Mennyiseg { get; set; }
        public decimal EgysegAr { get; set; }
        public decimal EgysegnyiTeljesitmeny { get; set; }
        public decimal AfaMerteke { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }
        public decimal OsszTeljesitmeny { get; set; }
        public int Garancia { get; set; }
    }
}
