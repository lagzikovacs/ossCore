using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogBll
    {
        private static UgyfelterLogDto Calc(Ugyfelterlog entity)
        {
            var result = ObjectUtils.Convert<Ugyfelterlog, UgyfelterLogDto>(entity);
            result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
            result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);

            return result;
        }
        public static List<UgyfelterLogDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELTERLOG);

            var qry = UgyfelterLogDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<UgyfelterLogDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }
    }
}
