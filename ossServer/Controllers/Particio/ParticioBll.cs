using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Threading.Tasks;

namespace ossServer.Controllers.Particio
{
    public class ParticioBll
    {
        public static async Task<ParticioDto> GetAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PARTICIO);

            var entity = await ParticioDal.GetAsync(context);
            return ObjectUtils.Convert<Models.Particio, ParticioDto>(entity);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, ParticioDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PARTICIO);

            await ParticioDal.Lock(context, dto.Particiokod, dto.Modositva);
            var entity = await ParticioDal.GetAsync(context);
            ObjectUtils.Update(dto, entity);
            return ParticioDal.Update(context, entity);
        }
    }
}
