using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Globalization;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatBll
    {
        public static int AddIratToBizonylat(ossContext context, string sid, int bizonylatKod, int iratKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = new Models.Bizonylatkapcsolat
            {
                Bizonylatkod = bizonylatKod,
                Iratkod = iratKod
            };
            return BizonylatKapcsolatDal.Add(context, entity);
        }

        public static void Delete(ossContext context, string sid, BizonylatKapcsolatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = BizonylatKapcsolatDal.Get(context, dto.Bizonylatkapcsolatkod);
            BizonylatKapcsolatDal.Delete(context, entity);
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

        public static BizonylatKapcsolatDto Get(ossContext context, string sid, int bizonylatkapcsolatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = BizonylatKapcsolatDal.Get(context, bizonylatkapcsolatKod);
            return Calc(entity);
        }

        public static List<BizonylatKapcsolatDto> Select(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entities = BizonylatKapcsolatDal.Select(context, bizonylatKod);
            var result = new List<BizonylatKapcsolatDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }
    }
}
