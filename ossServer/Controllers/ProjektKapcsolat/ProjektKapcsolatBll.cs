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

        public static async Task<List<ProjektKapcsolatDto>> GetAsync(ossContext context, string sid, int projektkapcsolatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = await ProjektKapcsolatDal.GetAsync(context, projektkapcsolatKod);
            return new List<ProjektKapcsolatDto> { KapcsolatCalc(entity) };
        }

        public static async Task<List<ProjektKapcsolatDto>> SelectAsync(ossContext context, string sid,
            int projektKod, bool forUgyfelter = false)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entites = await ProjektKapcsolatDal.ReadByProjektKodAsync(context, projektKod);
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

        public static async Task<List<ProjektKapcsolatDto>> SelectByBizonylatAsync(ossContext context, string sid,
            int bizonylatKod)
        {
            SessionBll.Check(context, sid);

            var entites = await ProjektKapcsolatDal.ReadByBizonylatKodAsync(context, bizonylatKod);
            return ObjectUtils.Convert<Projektkapcsolat, ProjektKapcsolatDto>(entites);
        }

        public static async Task<List<ProjektKapcsolatDto>> SelectByIratAsync(ossContext context, string sid, int iratKod)
        {
            SessionBll.Check(context, sid);

            var entites = await ProjektKapcsolatDal.ReadByIratKodAsync(context, iratKod);
            return ObjectUtils.Convert<Projektkapcsolat, ProjektKapcsolatDto>(entites);
        }


        public static async Task<int> AddBizonylatToProjektAsync(ossContext context, string sid,
            int projektKod, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = new Projektkapcsolat { Projektkod = projektKod, Bizonylatkod = bizonylatKod };
            var result = await ProjektKapcsolatDal.AddAsync(context, entity);

            return result;
        }

        public static async Task<int> AddIratToProjektAsync(ossContext context, string sid, int projektKod, int iratKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = new Projektkapcsolat { Projektkod = projektKod, Iratkod = iratKod };
            var result = await ProjektKapcsolatDal.AddAsync(context, entity);

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, int projektKapcsolatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = await ProjektKapcsolatDal.GetAsync(context, projektKapcsolatKod);
            await ProjektKapcsolatDal.DeleteAsync(context, entity);
        }

        public static async Task<int> UjBizonylatToProjektAsync(ossContext context, string sid, int projektKod,
            BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<BizonylatDto, Models.Bizonylat>(dto);
            var bizonylatKod = await BizonylatDal.AddAsync(context, entity);
            return await AddBizonylatToProjektAsync(context, sid, projektKod, bizonylatKod);
        }
    }
}
