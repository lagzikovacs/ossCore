using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private static async Task UpdateNyomtatottPeldanyAsync(ossContext context, int bizonylatKod, int peldanyszam)
        {
            // már lockolva van
            var entity = await BizonylatDal.GetAsync(context, bizonylatKod);
            entity.Nyomtatottpeldanyokszama = peldanyszam;
            await BizonylatDal.UpdateAsync(context, entity);
        }

        public static async Task<byte[]> NyomtatasAsync(IConfiguration config, ossContext context, string sid,
            int bizonylatKod, BizonylatNyomtatasTipus nyomtatasTipus)
        {
            const string minta = "!!! MINTA !!!";

            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var entityBizonylat = await BizonylatDal.GetComplexAsync(context, bizonylatKod);
            await BizonylatDal.Lock(context, bizonylatKod, entityBizonylat.Modositva);

            var entityParticio = await ParticioDal.GetAsync(context);
            var bc = JsonConvert.DeserializeObject<BizonylatConf>(entityParticio.Bizonylat);
            var iratKod = bc.BizonylatkepIratkod ?? throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatkepIratkod"));
            var peldanyszam = bc.EredetipeldanyokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "EredetipeldanyokSzama"));
            if (peldanyszam <= 0 || peldanyszam > 3)
                throw new Exception($"EredetipeldanyokSzama: Hibás érték, legyen 1, 2 vagy 3, most {peldanyszam} !");
            var masolat = bc.MasolatokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "MasolatokSzama"));
            if (masolat <= 0 || masolat > 3)
                throw new Exception($"MasolatokSzama: Hibás érték, legyen 1, 2 vagy 3, most {masolat} !");

            var szamlakep = await IratBll.LetoltesAsync(context, sid, iratKod);
            var v = await VerzioDal.GetAsync(context);

            var fejlec = BizonylatBll.Bl[entityBizonylat.Bizonylattipuskod].BizonylatFejlec;
            if (nyomtatasTipus == BizonylatNyomtatasTipus.Minta)
                fejlec = minta + " - " + fejlec;

            var printer = new BizonylatPrinter();
            printer.Setup(entityBizonylat, szamlakep.b, fejlec, v);

            switch (nyomtatasTipus)
            {
                case BizonylatNyomtatasTipus.Minta:
                    printer.UjPeldany("1", minta);
                    break;
                case BizonylatNyomtatasTipus.Eredeti:
                    for (var i = 1; i <= peldanyszam; i++)
                        printer.UjPeldany(i.ToString(), "Eredeti");

                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
                        await UpdateNyomtatottPeldanyAsync(context, bizonylatKod, peldanyszam);
                    break;
                case BizonylatNyomtatasTipus.Másolat:
                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
                        throw new Exception("Még nem készült eredeti példány!");

                    var sorszamTol = entityBizonylat.Nyomtatottpeldanyokszama + 1;
                    var sorszamIg = sorszamTol + masolat - 1;
                    for (var i = sorszamTol; i <= sorszamIg; i++)
                        printer.UjPeldany(i.ToString(), "Másolat");

                    await UpdateNyomtatottPeldanyAsync(context, bizonylatKod, sorszamIg);
                    break;
            }

            return printer.Print(config);
        }
    }
}
