using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatBll
    {
        public static async Task<int> AddIratToBizonylatAsync(ossContext context, string sid, int bizonylatKod, int iratKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = new Models.Bizonylatkapcsolat
            {
                Bizonylatkod = bizonylatKod,
                Iratkod = iratKod
            };
            return await BizonylatKapcsolatDal.AddAsync(context, entity);
        }

        public static async Task DeleteAsync(ossContext context, string sid, BizonylatKapcsolatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = await BizonylatKapcsolatDal.GetAsync(context, dto.Bizonylatkapcsolatkod);
            await BizonylatKapcsolatDal.DeleteAsync(context, entity);
        }

        private static BizonylatKapcsolatDto Calc(Models.Bizonylatkapcsolat entity)
        {
            var dto = ObjectUtils.Convert<Models.Bizonylatkapcsolat, BizonylatKapcsolatDto>(entity);

            dto.Tipus = entity.IratkodNavigation.IrattipuskodNavigation.Irattipus1;
            dto.Azonosito = entity.Iratkod.ToString(CultureInfo.InvariantCulture);
            dto.Keletkezett = entity.IratkodNavigation.Keletkezett;
            dto.Irany = entity.IratkodNavigation.Irany;
            dto.Kuldo = entity.IratkodNavigation.Kuldo;
            dto.Targy = entity.IratkodNavigation.Targy;

            return dto;
        }

        public static async Task<BizonylatKapcsolatDto> GetAsync(ossContext context, string sid, int bizonylatkapcsolatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = await BizonylatKapcsolatDal.GetAsync(context, bizonylatkapcsolatKod);
            return Calc(entity);
        }

        public static async Task<List<BizonylatKapcsolatDto>> SelectAsync(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entities = await BizonylatKapcsolatDal.SelectAsync(context, bizonylatKod);
            var result = new List<BizonylatKapcsolatDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }
    }
}
