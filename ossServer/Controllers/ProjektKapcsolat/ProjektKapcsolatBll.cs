using ossServer.Controllers.Bizonylat;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatBll
    {
        private static ProjektKapcsolatDto KapcsolatCalc(Projektkapcsolat entity)
        {
            var dto = ObjectUtils.Convert<Projektkapcsolat, ProjektKapcsolatDto>(entity);
            if (entity.Iratkod != null)
            {
                dto.Kapcsolat = "Irat";
                dto.Tipus = entity.IratkodNavigation.IrattipuskodNavigation.Irattipus1;
                dto.Azonosito = entity.Iratkod.ToString();
                dto.Keletkezett = entity.IratkodNavigation.Keletkezett;
                dto.Irany = entity.IratkodNavigation.Irany;
                dto.Kuldo = entity.IratkodNavigation.Kuldo;
                dto.Targy = entity.IratkodNavigation.Targy;
            }
            else
            {
                dto.Kapcsolat = "Bizonylat";
                dto.Tipus = BizonylatBll.Bl[entity.BizonylatkodNavigation.Bizonylattipuskod].BizonylatNev;
                dto.Azonosito = entity.BizonylatkodNavigation.Bizonylatszam;
                dto.Keletkezett = entity.BizonylatkodNavigation.Bizonylatkelte;
            }

            return dto;
        }

        public static List<ProjektKapcsolatDto> Get(ossContext context, string sid, int projektkapcsolatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ProjektKapcsolatDal.Get(context, projektkapcsolatKod);
            return new List<ProjektKapcsolatDto> { KapcsolatCalc(entity) };
        }

        public static List<ProjektKapcsolatDto> Select(ossContext context, string sid,
            int projektKod, bool forUgyfelter = false)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entites = ProjektKapcsolatDal.ReadByProjektKod(context, projektKod);
            var result = new List<ProjektKapcsolatDto>();

            foreach (var entity in entites)
            {
                var dto = KapcsolatCalc(entity);

                if (forUgyfelter)
                {
                    if (entity.Iratkod != null && entity.IratkodNavigation.Irany != "Belső")
                        result.Add(dto);

                    if (entity.Bizonylatkod != null && 
                        (entity.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.DijBekero) |
                        (entity.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.ElolegSzamla) |
                        (entity.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.Szamla))
                        result.Add(dto);
                }
                else
                    result.Add(dto);
            }

            return result;
        }

        public static List<ProjektKapcsolatDto> SelectByBizonylat(ossContext context, string sid,
            int bizonylatKod)
        {
            SessionBll.Check(context, sid);

            var entites = ProjektKapcsolatDal.ReadByBizonylatKod(context, bizonylatKod);
            return ObjectUtils.Convert<Projektkapcsolat, ProjektKapcsolatDto>(entites);
        }

        public static List<ProjektKapcsolatDto> SelectByIrat(ossContext context, string sid, int iratKod)
        {
            SessionBll.Check(context, sid);

            var entites = ProjektKapcsolatDal.ReadByIratKod(context, iratKod);
            return ObjectUtils.Convert<Projektkapcsolat, ProjektKapcsolatDto>(entites);
        }


        public static int AddBizonylatToProjekt(ossContext context, string sid,
            int projektKod, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = new Projektkapcsolat { Projektkod = projektKod, Bizonylatkod = bizonylatKod };
            var result = ProjektKapcsolatDal.Add(context, entity);

            return result;
        }

        public static int AddIratToProjekt(ossContext context, string sid, int projektKod, int iratKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = new Projektkapcsolat { Projektkod = projektKod, Iratkod = iratKod };
            var result = ProjektKapcsolatDal.Add(context, entity);

            return result;
        }

        public static void Delete(ossContext context, string sid, int projektKapcsolatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ProjektKapcsolatDal.Get(context, projektKapcsolatKod);
            ProjektKapcsolatDal.Delete(context, entity);
        }

        public static async Task<int> UjBizonylatToProjektAsync(ossContext context, string sid, int projektKod,
            BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<BizonylatDto, Models.Bizonylat>(dto);
            var bizonylatKod = await BizonylatDal.AddAsync(context, entity);
            return AddBizonylatToProjekt(context, sid, projektKod, bizonylatKod);
        }
    }
}
