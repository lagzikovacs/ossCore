using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Teendo
{
    public class TeendoBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<TeendoDto, Models.Teendo>(dto);
            await TeendoDal.ExistsAsync(context, entity);
            return await TeendoDal.AddAsync(context, entity);
        }

        public static TeendoDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new TeendoDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await TeendoDal.Lock(context, dto.Teendokod, dto.Modositva);
            await TeendoDal.CheckReferencesAsync(context, dto.Teendokod);
            var entity = await TeendoDal.GetAsync(context, dto.Teendokod);
            await TeendoDal.DeleteAsync(context, entity);
        }

        public static async Task<TeendoDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await TeendoDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Teendo, TeendoDto>(entity);
        }

        public static async Task<List<TeendoDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await TeendoDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Teendo, TeendoDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await TeendoDal.Lock(context, dto.Teendokod, dto.Modositva);
            var entity = await TeendoDal.GetAsync(context, dto.Teendokod);
            ObjectUtils.Update(dto, entity);
            await TeendoDal.ExistsAnotherAsync(context, entity);
            return await TeendoDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int teendokod, string teendo)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await TeendoDal.ZoomCheckAsync(context, teendokod, teendo);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Teendokod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Teendo1", Title = "Teendő", Type = ColumnType.STRING },
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
