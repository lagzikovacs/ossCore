using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatUtils
    {
        public static void BizonylattetelCalc(BizonylatTetelDto bizonylatTetelDto)
        {
            bizonylatTetelDto.Mennyiseg = Calc.RealRound(bizonylatTetelDto.Mennyiseg, 0.01m);
            bizonylatTetelDto.Egysegar = Calc.RealRound(bizonylatTetelDto.Egysegar, 0.01m);
            bizonylatTetelDto.Netto = Calc.RealRound(bizonylatTetelDto.Mennyiseg * bizonylatTetelDto.Egysegar, 0.01m);
            bizonylatTetelDto.Afa = Calc.RealRound(bizonylatTetelDto.Netto * bizonylatTetelDto.Afamerteke / 100m, 0.01m);
            bizonylatTetelDto.Brutto = Calc.RealRound(bizonylatTetelDto.Netto + bizonylatTetelDto.Afa, 0.01m);

            bizonylatTetelDto.Ossztomegkg = Calc.RealRound(bizonylatTetelDto.Mennyiseg * bizonylatTetelDto.Tomegkg, 0.01m);

            if (bizonylatTetelDto.Termekdijas)
            {
                if (bizonylatTetelDto.Termekdijkod == null || bizonylatTetelDto.Termekdijkt == null ||
                      bizonylatTetelDto.Termekdijmegnevezes == null || bizonylatTetelDto.Termekdijegysegar == null)
                    throw new Exception("A termékdíj adatok hibásak!");

                bizonylatTetelDto.Termekdij =
                  Calc.RealRound(bizonylatTetelDto.Ossztomegkg * (decimal)bizonylatTetelDto.Termekdijegysegar, 0.01m);
            }
            else
            {
                bizonylatTetelDto.Termekdij = null;
            }
        }

        public static void Bruttobol(BizonylatTetelDto bizonylatTetelDto, decimal brutto)
        {
            if (bizonylatTetelDto.Mennyiseg == 0)
                throw new Exception("A mennyiség nem lehet nulla!");

            bizonylatTetelDto.Egysegar = Calc.RealRound(brutto / (100 + bizonylatTetelDto.Afamerteke) * 100 / bizonylatTetelDto.Mennyiseg, 0.01m);
            BizonylattetelCalc(bizonylatTetelDto);
        }

        public static void SumEsAfaEsTermekdij(BizonylatDto bizonylatDto, List<BizonylatTetelDto> lstBizonylatTetelDto,
          List<BizonylatAfaDto> lstBizonylatAfaDto, List<BizonylatTermekdijDto> lstBizonylatTermekdijDto)
        {
            lstBizonylatAfaDto.Clear();
            lstBizonylatTermekdijDto.Clear();

            var mire = bizonylatDto.Penznem == "HUF" ? 1m : 0.01m;

            var afaBontas = lstBizonylatTetelDto.GroupBy(s =>
                new { s.Afakulcskod, s.Afakulcs, s.Afamerteke })
              .Select(t => new
              {
                  t.Key.Afakulcskod,
                  t.Key.Afakulcs,
                  t.Key.Afamerteke,
                  NETTO = t.Sum(k => k.Netto),
                  AFA = t.Sum(k => k.Afa)
              });
            foreach (var ab in afaBontas.OrderBy(s => s.Afakulcs))
            {
                var bizonylatAfaDto = new BizonylatAfaDto
                {
                    Afakulcskod = ab.Afakulcskod,
                    Afakulcs = ab.Afakulcs,
                    Afamerteke = ab.Afamerteke,
                    Netto = Calc.RealRound(ab.NETTO, mire),
                    Afa = Calc.RealRound(ab.AFA, mire)
                };
                bizonylatAfaDto.Brutto = bizonylatAfaDto.Netto + bizonylatAfaDto.Afa;

                lstBizonylatAfaDto.Add(bizonylatAfaDto);
            }

            var termekdijBontas = lstBizonylatTetelDto
              .Where(s => s.Termekdijas)
              .GroupBy(s => new { s.Termekdijkod, s.Termekdijkt, s.Termekdijmegnevezes, s.Termekdijegysegar })
              .Select(t => new
              {
                  OSSZTOMEGKG = t.Sum(k => k.Ossztomegkg),

                  t.Key.Termekdijkod,
                  t.Key.Termekdijkt,
                  t.Key.Termekdijmegnevezes,
                  t.Key.Termekdijegysegar,

                  TERMEKDIJ = t.Sum(k => k.Termekdij)
              });
            foreach (var tb in termekdijBontas.OrderBy(s => s.Termekdijkt))
            {
                lstBizonylatTermekdijDto.Add(new BizonylatTermekdijDto
                {
                    Ossztomegkg = tb.OSSZTOMEGKG,

                    Termekdijkod = (int)tb.Termekdijkod,
                    Termekdijkt = tb.Termekdijkt,
                    Termekdijmegnevezes = tb.Termekdijmegnevezes,
                    Termekdijegysegar = (decimal)tb.Termekdijegysegar,
                    //TERMEKDIJ = Calc.RealRound((decimal) tb.TERMEKDIJ, mire) //navfeltöltés miatt
                    Termekdij = (decimal)tb.TERMEKDIJ
                });
            }

            bizonylatDto.Netto = lstBizonylatAfaDto.Sum(s => s.Netto);
            bizonylatDto.Afa = lstBizonylatAfaDto.Sum(s => s.Afa);
            bizonylatDto.Brutto = lstBizonylatAfaDto.Sum(s => s.Brutto);
            bizonylatDto.Termekdij = lstBizonylatTermekdijDto.Sum(s => s.Termekdij);

            bizonylatDto.Azaz = Azaz.Szovegge(bizonylatDto.Brutto);
        }
    }
}
