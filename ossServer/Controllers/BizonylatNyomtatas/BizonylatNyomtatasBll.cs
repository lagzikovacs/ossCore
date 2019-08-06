using Microsoft.Extensions.Configuration;
using ossServer.Controllers.Bizonylat;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Session;
using ossServer.Controllers.Verzio;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.BizonylatNyomtatas
{
    public class BizonylatNyomtatasBll
    {
        public static int GetBizonylatEredetiPeldany(ossContext context)
        {
            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.BizonylatEredetipeldanyokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatEredetipeldanyokSzama"));

            if (result <= 0 || result > 3)
                throw new Exception($"BizonylatEredetipeldanyokSzama: Hibás érték, legyen 1, 2 vagy 3, most {result} !");

            return result;
        }

        public static int GetBizonylatMasolat(ossContext context)
        {
            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.BizonylatMasolatokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatMasolatokSzama"));

            if (result <= 0 || result > 3)
                throw new Exception($"BizonylatMasolatokSzama: Hibás érték, legyen 1, 2 vagy 3, most {result} !");

            return result;
        }

        private static void UpdateNyomtatottPeldany(ossContext context, int bizonylatKod, int peldanyszam)
        {
            // már lockolva van
            var entity = BizonylatDal.Get(context, bizonylatKod);
            entity.Nyomtatottpeldanyokszama = peldanyszam;
            BizonylatDal.Update(context, entity);
        }

        public static async Task<byte[]> NyomtatasAsync(IConfiguration config, ossContext context, string sid,
            int bizonylatKod, BizonylatNyomtatasTipus nyomtatasTipus)
        {
            const string minta = "!!! MINTA !!!";

            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var entityBizonylat = BizonylatDal.GetComplex(context, bizonylatKod);
            await BizonylatDal.Lock(context, bizonylatKod, entityBizonylat.Modositva);

            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.BizonylatBizonylatkepIratkod != null ?
                (int)entityParticio.BizonylatBizonylatkepIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatBizonylatkepIratkod"));

            var szamlakep = IratBll.Letoltes(context, sid, iratKod);
            var v = VerzioDal.Get(context);

            var fejlec = BizonylatBll.Bl[entityBizonylat.Bizonylattipuskod].BizonylatFejlec;
            if (nyomtatasTipus == BizonylatNyomtatasTipus.Minta)
                fejlec = minta + " - " + fejlec;

            var printer = new BizonylatPrinter();
            printer.Setup(entityBizonylat, szamlakep.b, fejlec, v);

            int peldanyszam;

            switch (nyomtatasTipus)
            {
                case BizonylatNyomtatasTipus.Minta:
                    printer.UjPeldany("1", minta);
                    break;
                case BizonylatNyomtatasTipus.Eredeti:
                    peldanyszam = GetBizonylatEredetiPeldany(context);
                    for (var i = 1; i <= peldanyszam; i++)
                        printer.UjPeldany(i.ToString(), "Eredeti");

                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
                        UpdateNyomtatottPeldany(context, bizonylatKod, peldanyszam);
                    break;
                case BizonylatNyomtatasTipus.Másolat:
                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
                        throw new Exception("Még nem készült eredeti példány!");

                    peldanyszam = GetBizonylatMasolat(context);
                    var sorszamTol = entityBizonylat.Nyomtatottpeldanyokszama + 1;
                    var sorszamIg = sorszamTol + peldanyszam - 1;
                    for (var i = sorszamTol; i <= sorszamIg; i++)
                        printer.UjPeldany(i.ToString(), "Másolat");

                    UpdateNyomtatottPeldany(context, bizonylatKod, sorszamIg);
                    break;
            }

            return printer.Print(config);
        }
    }
}
