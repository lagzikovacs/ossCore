using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    public class FizetesimodBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<FizetesimodDto, Models.Fizetesimod>(dto);
            await FizetesimodDal.ExistsAsync(context, entity);
            return await FizetesimodDal.AddAsync(context, entity);
        }

        public static FizetesimodDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new FizetesimodDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            await FizetesimodDal.CheckReferencesAsync(context, dto.Fizetesimodkod);
            var entity = await FizetesimodDal.GetAsync(context, dto.Fizetesimodkod);
            await FizetesimodDal.DeleteAsync(context, entity);
        }

        public static async Task<FizetesimodDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await FizetesimodDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entity);
        }

        public static async Task<List<FizetesimodDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await FizetesimodDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            var entity = await FizetesimodDal.GetAsync(context, dto.Fizetesimodkod);
            ObjectUtils.Update(dto, entity);
            await FizetesimodDal.ExistsAnotherAsync(context, entity);
            return await FizetesimodDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int fizetesimodKod, string fizetesimod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await FizetesimodDal.ZoomCheckAsync(context, fizetesimodKod, fizetesimod);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Fizetesimodkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Fizetesimod1", Title = "Fizetési mód", Type = ColumnType.STRING },
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
