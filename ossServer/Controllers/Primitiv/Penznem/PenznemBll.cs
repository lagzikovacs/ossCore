using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Penznem
{
    public class PenznemBll
    {
        public static int Add(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<PenznemDto, Models.Penznem>(dto);
            PenznemDal.Exists(context, entity);
            return PenznemDal.Add(context, entity);
        }

        public static PenznemDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new PenznemDto();
        }

        public static void Delete(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
            PenznemDal.CheckReferences(context, dto.Penznemkod);
            var entity = PenznemDal.Get(context, dto.Penznemkod);
            PenznemDal.Delete(context, entity);
        }

        public static PenznemDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = PenznemDal.Get(context, key);
            return ObjectUtils.Convert<Models.Penznem, PenznemDto>(entity);
        }

        public static List<PenznemDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = PenznemDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Penznem, PenznemDto>(entities);
        }

        public static int Update(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
            var entity = PenznemDal.Get(context, dto.Penznemkod);
            ObjectUtils.Update(dto, entity);
            PenznemDal.ExistsAnother(context, entity);
            return PenznemDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int penznemkod, string penznem)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            PenznemDal.ZoomCheck(context, penznemkod, penznem);
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
