using Microsoft.EntityFrameworkCore;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Controllers.Ugyfelkapcsolat;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Kapcsolatihalo
{
    public class KapcsolatihaloBll
    {
        public static async Task<KapcsolatihaloTaskResult> Read(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.KAPCSOLATIHALO);

            var result = new KapcsolatihaloTaskResult
            {
                lstUgyfelkapcsolatDto = new List<UgyfelkapcsolatDto>(),
                lstUgyfelDto = new List<UgyfelDto>()
            };

            // TODO laponként megszerezni, közben esetleg Cancel
            var qry = UgyfelkapcsolatDal.GetQuery(context);
            // var osszesRekord = await qry.CountAsync();
            var entities = await qry.Skip(0).Take(100000).ToListAsync();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Models.Ugyfelkapcsolat, UgyfelkapcsolatDto>(entity);
                result.lstUgyfelkapcsolatDto.Add(dto);

                if (!result.lstUgyfelDto.Any(s => s.Ugyfelkod == dto.Fromugyfelkod))
                    result.lstUgyfelDto.Add(await UgyfelBll.GetAsync(context, dto.Fromugyfelkod));
                if (!result.lstUgyfelDto.Any(s => s.Ugyfelkod == dto.Tougyfelkod))
                    result.lstUgyfelDto.Add(await UgyfelBll.GetAsync(context, dto.Tougyfelkod));
            }

            return result;
        }
    }
}
