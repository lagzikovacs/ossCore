using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTetelBll
    {
        public static BizonylatTetelDto CreateNew(ossContext context, string sid, 
            BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            return new BizonylatTetelDto
            {
                Mennyiseg = 0,
                Egysegar = 0,
                Netto = 0,
                Afa = 0,
                Brutto = 0,

                Tomegkg = 0,
                Ossztomegkg = 0,
                Termekdijegysegar = 0,
                Termekdij = 0,

                Termekdijas = bizonylatTipus == BizonylatTipus.Szamla || 
                    bizonylatTipus == BizonylatTipus.ElolegSzamla ||
                    bizonylatTipus == BizonylatTipus.Szallito,
                Kozvetitettszolgaltatas = false,
                Ezeloleg = false
            };
        }
    }
}
