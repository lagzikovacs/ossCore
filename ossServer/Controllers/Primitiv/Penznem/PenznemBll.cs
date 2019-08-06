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

        public static async Task DeleteAsync(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
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

        public static async Task<int> UpdateAsync(ossContext context, string sid, PenznemDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await PenznemDal.Lock(context, dto.Penznemkod, dto.Modositva);
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
