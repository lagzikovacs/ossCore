using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

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

        public static void Delete(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
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

        public static int Update(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
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

        private static List<ColumnSettings> BaseColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Afakulcskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Afakulcs1", Title = "ÁFA kulcs", Type = ColumnType.STRING },
                new ColumnSettings {Name="Afamerteke", Title = "ÁFA mértéke, %", Type = ColumnType.NUMBER },
            };
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return BaseColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return ColumnSettingsUtil.AddIdobelyeg(BaseColumns());
        }
    }
}
