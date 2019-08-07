using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Penznem
{
    public class PenznemBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<PenznemDto, Models.Penznem>(dto);
            await PenznemDal.ExistsAsync(context, entity);
            return await PenznemDal.AddAsync(context, entity);
        }

        public static PenznemDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new PenznemDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
            await PenznemDal.CheckReferencesAsync(context, dto.Penznemkod);
            var entity = await PenznemDal.GetAsync(context, dto.Penznemkod);
            await PenznemDal.DeleteAsync(context, entity);
        }

        public static async Task<PenznemDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = await PenznemDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Penznem, PenznemDto>(entity);
        }

        public static async Task<List<PenznemDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = await PenznemDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Penznem, PenznemDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
            var entity = await PenznemDal.GetAsync(context, dto.Penznemkod);
            ObjectUtils.Update(dto, entity);
            await PenznemDal.ExistsAnotherAsync(context, entity);
            return await PenznemDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int penznemkod, string penznem)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await PenznemDal.ZoomCheckAsync(context, penznemkod, penznem);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Penznemkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Penznem1", Title = "Pénznem", Type = ColumnType.STRING },
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
