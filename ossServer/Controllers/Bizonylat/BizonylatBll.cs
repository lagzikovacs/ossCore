using ossServer.Enums;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatBll
    {
        public static BizonylatTipusLeiro[] Bl =
        {
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Szamla,
            BizonylatNev = "Számla",
            BizonylatFejlec = "Számla",
            KodNev = KodNev.Szamla,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.SZAMLA,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Szallito,
            BizonylatNev = "Szállító",
            BizonylatFejlec = "Szállítólevél",
            KodNev = KodNev.Szallito,
            GenBizonylatszam = true,
            Stornozhato = false,
            JogKod = JogKod.SZALLITO,
            FizetesiModIs = false,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.BejovoSzamla,
            BizonylatNev = "Bejövő számla",
            BizonylatFejlec = "Bejövő számla másolat",
            KodNev = KodNev.BejovoSzamla,
            GenBizonylatszam = false,
            Stornozhato = false,
            JogKod = JogKod.BEJOVOSZAMLA,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Megrendeles,
            BizonylatNev = "Megrendelés",
            BizonylatFejlec = "Megrendelés",
            KodNev = KodNev.Megrendeles,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.MEGRENDELES,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.DijBekero,
            BizonylatNev = "Díjbekérő",
            BizonylatFejlec = "Díjbekérő",
            KodNev = KodNev.DijBekero,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.DIJBEKERO,
            FizetesiModIs = true,
            Elotag = "DB"
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.ElolegSzamla,
            BizonylatNev = "Előlegszámla",
            BizonylatFejlec = "Előlegszámla",
            KodNev = KodNev.ElolegSzamla,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.ELOLEGSZAMLA,
            FizetesiModIs = true,
            Elotag = "E"
          }
        };
    }
}
