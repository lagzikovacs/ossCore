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
        public static int Add(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<MennyisegiegysegDto, Mennyisegiegyseg>(dto);
            MennyisegiegysegDal.Exists(context, entity);
            return MennyisegiegysegDal.Add(context, entity);
        }

        public static MennyisegiegysegDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new MennyisegiegysegDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            MennyisegiegysegDal.CheckReferences(context, dto.Mekod);
            var entity = MennyisegiegysegDal.Get(context, dto.Mekod);
            MennyisegiegysegDal.Delete(context, entity);
        }

        public static MennyisegiegysegDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = MennyisegiegysegDal.Get(context, key);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entity);
        }

        public static List<MennyisegiegysegDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = MennyisegiegysegDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            var entity = MennyisegiegysegDal.Get(context, dto.Mekod);
            ObjectUtils.Update(dto, entity);
            MennyisegiegysegDal.ExistsAnother(context, entity);
            return MennyisegiegysegDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int mekod, string me)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            MennyisegiegysegDal.ZoomCheck(context, mekod, me);
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
