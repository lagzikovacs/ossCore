using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Me
{
    public class MennyisegiegysegBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<MennyisegiegysegDto, Mennyisegiegyseg>(dto);
            await MennyisegiegysegDal.ExistsAsync(context, entity);
            return await MennyisegiegysegDal.AddAsync(context, entity);
        }

        public static MennyisegiegysegDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new MennyisegiegysegDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            await MennyisegiegysegDal.CheckReferencesAsync(context, dto.Mekod);
            var entity = await MennyisegiegysegDal.GetAsync(context, dto.Mekod);
            await MennyisegiegysegDal.DeleteAsync(context, entity);
        }

        public static async Task<MennyisegiegysegDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await MennyisegiegysegDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entity);
        }

        public static async Task<List<MennyisegiegysegDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await MennyisegiegysegDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            var entity = await MennyisegiegysegDal.GetAsync(context, dto.Mekod);
            ObjectUtils.Update(dto, entity);
            await MennyisegiegysegDal.ExistsAnotherAsync(context, entity);
            return await MennyisegiegysegDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int mekod, string me)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await MennyisegiegysegDal.ZoomCheckAsync(context, mekod, me);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Mekod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Me", Title = "Mennyiségi egység", Type = ColumnType.STRING },
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
