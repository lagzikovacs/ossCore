using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<AfakulcsDto, Models.Afakulcs>(dto);
            await AfakulcsDal.ExistsAsync(context, entity);

            return await AfakulcsDal.AddAsync(context, entity);
        }

        public static async Task<AfakulcsDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            return new AfakulcsDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await AfakulcsDal.Lock(context, dto.Afakulcskod, dto.Modositva);
            await AfakulcsDal.CheckReferencesAsync(context, dto.Afakulcskod);
            var entity = await AfakulcsDal.GetAsync(context, dto.Afakulcskod);
            await AfakulcsDal.DeleteAsync(context, entity);
        }

        public static async Task<AfakulcsDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await AfakulcsDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entity);
        }

        public static async Task<List<AfakulcsDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await AfakulcsDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await AfakulcsDal.Lock(context, dto.Afakulcskod, dto.Modositva);
            var entity = await AfakulcsDal.GetAsync(context, dto.Afakulcskod);
            ObjectUtils.Update(dto, entity);
            await AfakulcsDal.ExistsAnotherAsync(context, entity);

            return await AfakulcsDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int afakulcskod, string afakulcs)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);
            await AfakulcsDal.ZoomCheckAsync(context, afakulcskod, afakulcs);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Afakulcskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Afakulcs1", Title = "ÁFA kulcs", Type = ColumnType.STRING },
                new ColumnSettings {Name="Afamerteke", Title = "ÁFA mértéke, %", Type = ColumnType.NUMBER },
            };
        }
        public static List<ColumnSettings> ReszletekColumns()
        {
            return ColumnSettingsUtil.AddIdobelyeg(GridColumns());
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return ReszletekColumns();
        }
    }
}
