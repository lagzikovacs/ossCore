using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Tevekenyseg
{
    public class TevekenysegBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, TevekenysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<TevekenysegDto, Models.Tevekenyseg>(dto);
            await TevekenysegDal.ExistsAsync(context, entity);
            return await TevekenysegDal.AddAsync(context, entity);
        }

        public static async Task<TevekenysegDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            return new TevekenysegDto {};
        }

        public static async Task DeleteAsync(ossContext context, string sid, TevekenysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await TevekenysegDal.Lock(context, dto.Tevekenysegkod, dto.Modositva);
            await TevekenysegDal.CheckReferencesAsync(context, dto.Tevekenysegkod);
            var entity = await TevekenysegDal.GetAsync(context, dto.Tevekenysegkod);
            await TevekenysegDal.DeleteAsync(context, entity);
        }

        public static async Task<TevekenysegDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await TevekenysegDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Tevekenyseg, TevekenysegDto>(entity);
        }

        public static async Task<List<TevekenysegDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await TevekenysegDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Tevekenyseg, TevekenysegDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, TevekenysegDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await TevekenysegDal.Lock(context, dto.Tevekenysegkod, dto.Modositva);
            var entity = await TevekenysegDal.GetAsync(context, dto.Tevekenysegkod);
            ObjectUtils.Update(dto, entity);
            await TevekenysegDal.ExistsAnotherAsync(context, entity);
            return await TevekenysegDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int termekdijkod, string termekdijkt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await TevekenysegDal.ZoomCheckAsync(context, termekdijkod, termekdijkt);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Tevekenysegkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Tevekenyseg1", Title = "Tevékenység", Type = ColumnType.STRING },
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
