using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<HelysegDto, Models.Helyseg>(dto);
            await HelysegDal.ExistsAsync(context, entity);
            return await HelysegDal.AddAsync(context, entity);
        }

        public static async Task<HelysegDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new HelysegDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            await HelysegDal.CheckReferencesAsync(context, dto.Helysegkod);
            var entity = await HelysegDal.GetAsync(context, dto.Helysegkod);
            await HelysegDal.DeleteAsync(context, entity);
        }

        public static async Task<HelysegDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await HelysegDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entity);
        }

        public static async Task<List<HelysegDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await HelysegDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            var entity = await HelysegDal.GetAsync(context, dto.Helysegkod);
            ObjectUtils.Update(dto, entity);
            await HelysegDal.ExistsAnotherAsync(context, entity);
            return await HelysegDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int helysegkod, string helysegnev)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await HelysegDal.ZoomCheckAsync(context, helysegkod, helysegnev);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Helysegkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Helysegnev", Title = "Helységnév", Type = ColumnType.STRING },
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
